using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WebApplicationtest.Models.Employee
{
    public class siplDBFactory
    {
        public static string siplAppPath = HttpContext.Current.Request.PhysicalApplicationPath;
        public static string siplTextErrorLogFilePath = siplAppPath + "ErrorLog.txt";
        public static int _nCommandTimeout = 300;
        private string _DBProviderName;
        private string _ConnectionString;
        private DbProviderFactory _dpf;
        private DbProviderFactory GetDBProviderFactory()
        {
            return _dpf;
        }

        public siplDBFactory()
        {
            _DBProviderName = ConfigurationManager.ConnectionStrings["DefaultConnection"].ProviderName;

            _ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString ;

            if (DbProviderFactories.GetFactoryClasses().Select("InvariantName='" + _DBProviderName + "'").Length == 0)
            {
                throw new Exception("Invalid .NET Data Provider specification: " + _DBProviderName);
            }
            _dpf = DbProviderFactories.GetFactory(_DBProviderName);
        }

        internal DbDataReader siplOpenReader(object conn)
        {
            throw new NotImplementedException();
        }

        public siplDBFactory(bool bHIS = false)
        {

            _DBProviderName = HttpContext.Current.Session["HISDBProvider"].ToString();
            _ConnectionString = HttpContext.Current.Session["HISConnectionString"].ToString();

            if (DbProviderFactories.GetFactoryClasses().Select("InvariantName='" + _DBProviderName + "'").Length == 0)
            {
                throw new Exception("Invalid .NET Data Provider specification: " + _DBProviderName);
            }
            _dpf = DbProviderFactories.GetFactory(_DBProviderName);
        }

        public siplDBFactory(string ProviderName, string ConnectionString)
        {
            _DBProviderName = ProviderName;
            _ConnectionString = ConnectionString;
            if (DbProviderFactories.GetFactoryClasses().Select("InvariantName='" + _DBProviderName + "'").Length == 0)
            {
                throw new Exception("Invalid .NET Data Provider specification: " + _DBProviderName);
            }
            _dpf = DbProviderFactories.GetFactory(_DBProviderName);
        }


        private DbConnection GetDBConnection()
        {
            DbConnection dbConn = GetDBProviderFactory().CreateConnection();
            dbConn.ConnectionString = _ConnectionString;
            return dbConn;
        }

        private DbCommand GetDBCommand()
        {
            DbCommand dbCmd = GetDBProviderFactory().CreateCommand();
            return dbCmd;
        }

        private DbDataAdapter GetDBDataAdapter()
        {
            DbDataAdapter dbAdap = GetDBProviderFactory().CreateDataAdapter();
            return dbAdap;
        }

        #region "DATABASE OPERATIONS"

        public DbConnection siplOpenDB()
        {
            DbConnection sipladConnection;
            try
            {
                sipladConnection = GetDBConnection();
                if (sipladConnection.State == ConnectionState.Open)
                {
                    sipladConnection.Close();
                }
                sipladConnection.Open();
                return sipladConnection;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
        }

        public string siplExecuteScalar(string sSQL, DbConnection adConnection)
        {

            DbCommand cmdTemp = default(DbCommand);
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                return cmdTemp.ExecuteScalar() + "";
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public string siplExecuteScalar(string sSQL, DbConnection adConnection, DbTransaction objTrans)
        {
            DbCommand cmdTemp = default(DbCommand);
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                if (objTrans != null)
                {
                    cmdTemp.Transaction = objTrans;
                }
                return cmdTemp.ExecuteScalar() + "";
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }

        }

        public int siplExecute(string sSQL, DbConnection adConnection)
        {
            DbCommand cmdTemp = default(DbCommand);
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                iAffected = cmdTemp.ExecuteNonQuery();
                return iAffected;
            }
            //catch (Exception ex)
            //{
            //    //siplInsertErrorLog(ex);
            //    return 0;
            //}
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public int siplExecute(string sSQL, DbConnection adConnection, DbTransaction objTrans, object strOBJ, string parameterName)
        {
            DbCommand cmdTemp = null;
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                //byte[] b = new byte[16];
                ////   b = ;
                //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                //rng.GetBytes(b);

                //createa parameter and fill it
                DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                ParamTemp.ParameterName = parameterName;
                ParamTemp.DbType = DbType.Binary;
                if (strOBJ != null)
                {
                    ParamTemp.Value = strOBJ;
                }
                else
                {
                    ParamTemp.Value = DBNull.Value;
                }
                cmdTemp.Parameters.Add(ParamTemp);
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                cmdTemp.Transaction = objTrans;
                iAffected = cmdTemp.ExecuteNonQuery();
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public int siplExecute(string sSQL, DbConnection adConnection, object strOBJ, string parameterName, DbType dbtype)
        {
            DbCommand cmdTemp = null;
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();

                //createa parameter and fill it
                DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                ParamTemp.ParameterName = parameterName;
                ParamTemp.DbType = dbtype;
                if (strOBJ != null)
                {
                    ParamTemp.Value = strOBJ;
                }
                else
                {
                    ParamTemp.Value = DBNull.Value;
                }
                cmdTemp.Parameters.Add(ParamTemp);
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                iAffected = cmdTemp.ExecuteNonQuery();
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public int siplExecute(string sSQL, DbConnection adConnection, object[] strOBJ, string[] parameterName)
        {
            DbCommand cmdTemp = null;
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                for (int i = 0; i < strOBJ.Length; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = parameterName[i];
                    ParamTemp.DbType = DbType.Binary;
                    if (strOBJ[i] != null)
                    {
                        ParamTemp.Value = strOBJ[i];
                    }
                    else
                    {
                        ParamTemp.Value = DBNull.Value;
                    }
                    cmdTemp.Parameters.Add(ParamTemp);
                }

                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                iAffected = cmdTemp.ExecuteNonQuery();
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
            }
        }

        public int siplExecute(string sSQL, DbConnection adConnection, DbTransaction adTrasection, object[] strOBJ, string[] parameterName)
        {
            DbCommand cmdTemp = null;
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                for (int i = 0; i < strOBJ.Length; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = parameterName[i];
                    ParamTemp.DbType = DbType.Binary;
                    if (strOBJ[i] != null)
                    {
                        ParamTemp.Value = strOBJ[i];
                    }
                    else
                    {
                        ParamTemp.Value = DBNull.Value;
                    }
                    cmdTemp.Parameters.Add(ParamTemp);
                }

                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                cmdTemp.Transaction = adTrasection;
                iAffected = cmdTemp.ExecuteNonQuery();
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }
        public int siplExecute(string sSQL, DbConnection adConnection, DbTransaction objTrans)
        {
            DbCommand cmdTemp = null;
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.Connection = adConnection;
                cmdTemp.Transaction = objTrans;
                iAffected = cmdTemp.ExecuteNonQuery();
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public DbDataReader siplOpenReader(string sSQL, DbConnection conn)
        {
            DbDataReader dr = default(DbDataReader);
            DbCommand cmdTemp = default(DbCommand);
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.Connection = conn;
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.CommandTimeout = _nCommandTimeout;
                dr = cmdTemp.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public DbDataReader siplOpenReader(string sSQL, DbConnection con, DbTransaction objTransaction)
        {
            DbDataReader dr = default(DbDataReader);
            DbCommand cmdTemp = default(DbCommand);
            //siplFunctions objSIPL = new siplFunctions();
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.Connection = con;
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.CommandTimeout = _nCommandTimeout;
                if (objTransaction != null)
                {
                    cmdTemp.Transaction = objTransaction;
                }

                dr = cmdTemp.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                //dr.Close()
                //objSIPL.siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //objSIPL = null;
                //siplDisposeObject(ref cmdTemp);
            }
        }

        public DbDataReader siplOpenReader(string sSQL, ref DbConnection adConnection, ref DbParameter[] dbparams)
        {
            //siplFunctions objsipl = new siplFunctions();
            try
            {
                DbCommand cmdTemp = GetDBCommand();
                cmdTemp.Connection = adConnection;
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;

                if (dbparams != null)
                {
                    foreach (DbParameter param in dbparams)
                    {
                        if (param.Value == null)
                        {
                            param.Value = DBNull.Value;
                        }
                        cmdTemp.Parameters.Add(param);
                    }
                }

                // Important: CommandBehavior.CloseConnection ensures connection closes when reader is closed
                return cmdTemp.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                //siplInsertErrorLog(ex);
                throw;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw;
            }
        }
        public DataSet siplOpenDataSet(string sSQL, DbConnection con)
        {
            DataSet ds = null;
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmd = default(DbCommand);
            try
            {
                ds = new DataSet();
                da = GetDBDataAdapter();
                cmd = GetDBCommand();
                cmd.CommandText = sSQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
            //    siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmd);
                //siplDisposeObject(ref da);
            }
        }

        public DataSet siplOpenDataSet(string sSQL, DbConnection con, DbTransaction objTransaction)
        {
            DataSet ds = null;
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmd = default(DbCommand);
            try
            {
                ds = new DataSet();
                da = GetDBDataAdapter();
                cmd = GetDBCommand();
                cmd.CommandText = sSQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                cmd.Transaction = objTransaction;
                da.SelectCommand = cmd;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmd);
                //siplDisposeObject(ref da);
            }
        }

        public DataTable siplOpenDataTable(string sSQL, DbConnection conn)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DataTable dt = null;
            DbCommand cmdTemp = default(DbCommand);
            try
            {
                dt = new DataTable();
                cmdTemp = GetDBCommand();
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.Connection = conn;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {

                //siplDisposeObject(ref cmdTemp);
                //siplDisposeObject(ref da);
            }

        }

        public DataTable siplOpenDataTable(string sSQL, DbConnection conn, DbTransaction Trans)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DataTable dt = null;
            DbCommand cmdTemp = default(DbCommand);
            try
            {
                dt = new DataTable();
                cmdTemp = GetDBCommand();
                cmdTemp.CommandType = CommandType.Text;
                cmdTemp.CommandText = sSQL;
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.Connection = conn;
                if (Trans != null)
                {
                    cmdTemp.Transaction = Trans;
                }

                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;

            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
                //siplDisposeObject(ref da);
            }
        }

        public DataTable siplOpenDataTable(string sSQL, ref DbConnection adConnection, ref DbParameter[] dbparams)
        {
            //siplFunctions objsipl = new siplFunctions();
            DataTable dt = new DataTable();

            try
            {
                using (DbCommand cmdTemp = GetDBCommand())
                {
                    using (DbDataAdapter da = GetDBDataAdapter())
                    {
                        cmdTemp.CommandType = CommandType.Text;
                        cmdTemp.CommandText = sSQL;
                        cmdTemp.Connection = adConnection;
                        if (dbparams != null)
                        {
                            foreach (DbParameter param in dbparams)
                            {
                                if (param.Value == null)
                                {
                                    param.Value = DBNull.Value;
                                }
                                cmdTemp.Parameters.Add(param);
                            }
                        }
                        da.SelectCommand = cmdTemp;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (SqlException ex)
            {
                //objsipl.siplInsertErrorLog(ex);
                throw;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw;
            }
        }

        public DataTable siplOpenDataTable(string sSQL, ref DbConnection adConnection, ref DbTransaction adTransaction, ref DbParameter[] dbparams)
        {
            //siplFunctions objsipl = new siplFunctions();
            DataTable dt = new DataTable();

            try
            {
                using (DbCommand cmdTemp = GetDBCommand())
                {
                    using (DbDataAdapter da = GetDBDataAdapter())
                    {
                        cmdTemp.CommandType = CommandType.Text;
                        cmdTemp.CommandText = sSQL;
                        cmdTemp.Connection = adConnection;
                        cmdTemp.Transaction = adTransaction;
                        if (dbparams != null)
                        {
                            foreach (DbParameter param in dbparams)
                            {
                                if (param.Value == null)
                                {
                                    param.Value = DBNull.Value;
                                }
                                cmdTemp.Parameters.Add(param);
                            }
                        }
                        da.SelectCommand = cmdTemp;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (SqlException ex)
            {
                //objsipl.siplInsertErrorLog(ex);
                throw;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw;
            }
        }
        public void siplFillComboFromDB(string sSQL, DbConnection conn, ref DropDownList cbo, bool bItemData = false, bool bFirstRowBlank = false, bool bSelectIfSingleItem = false)
        {
            DataSet ds = null;
            try
            {
                cbo.SelectedIndex = -1;
                ds = siplOpenDataSet(sSQL, conn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbo.DataSource = ds;
                    cbo.DataTextField = ds.Tables[0].Columns[0].ToString();

                    if (bItemData)
                    {
                        cbo.DataValueField = ds.Tables[0].Columns[1].ToString();
                    }
                    cbo.DataBind();
                }

                if (bFirstRowBlank)
                {
                    ListItem itm = new ListItem();
                    itm.Text = "";
                    itm.Value = "";
                    cbo.Items.Insert(0, itm);
                }

                if (bSelectIfSingleItem & bFirstRowBlank)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        cbo.SelectedIndex = 1;
                    }
                }
                else if (bSelectIfSingleItem & bFirstRowBlank == false)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        cbo.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
            }

        }

        public void siplFillComboFromDB(DataSet ds, ref DropDownList cbo, bool bItemData, bool bFirstRowBlank)
        {
            try
            {
                if (ds == null)
                    return;
                cbo.DataSource = ds;
                cbo.DataTextField = ds.Tables[0].Columns[0].ToString();

                if (bItemData)
                {
                    cbo.DataValueField = ds.Tables[0].Columns[1].ToString();
                }

                if (bFirstRowBlank)
                {
                    DataRow dRow = ds.Tables[0].NewRow();
                    dRow[0] = "";
                    ds.Tables[0].Rows.InsertAt(dRow, 0);
                }

                cbo.DataBind();
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
            }
        }

        public void siplFillComboFromDB(DataTable dt, ref DropDownList cbo, bool bItemData, bool bFirstRowBlank)
        {
            try
            {
                if (dt == null)
                    return;
                cbo.SelectedIndex = -1;
                cbo.DataSource = dt;
                cbo.DataTextField = dt.Columns[0].ToString();

                if (bItemData)
                {
                    cbo.DataValueField = dt.Columns[1].ToString();
                }

                if (bFirstRowBlank)
                {
                    DataRow dRow = dt.NewRow();
                    dRow[0] = "";

                    dt.Rows.InsertAt(dRow, 0);
                }
                cbo.DataBind();
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
            }
        }

        public void siplFillComboWithDistinct(DropDownList cbo, DbConnection conn, string sTableName, string sFieldName)
        {

            DataTable dt = null;
            DataTable dtTemp = null;
            DataRow dr = null;
            short i = 0;
            string sSQL = null;
            //siplFunctions objSIPL = new siplFunctions();
            // cbo = CType(cbo, DropDownList)
            try
            {
                sSQL = "SELECT DISTINCT " + sFieldName + " FROM " + sTableName + " WHERE CAST(" + sFieldName + " AS varchar) <> ''";
                dt = siplOpenDataTable(sSQL, conn);
                dt.TableName = sFieldName;

                if (cbo.Items.Count > 0)
                {
                    for (i = 0; i <= cbo.Items.Count - 1; i++)
                    {
                        dt.Rows.Add(new object[] { cbo.Items[i].Text });
                    }
                }

                //dtTemp = objSIPL.siplSelectDistinctFromDataTable(dt.TableName, dt, sFieldName);

                // Need to replace in C# 
                //cbo.ValueMember = "GetDisplayValue";

                cbo.Items.Clear();
                foreach (DataRow dr_loopVariable in dtTemp.Rows)
                {
                    dr = dr_loopVariable;
                    cbo.Items.Add(dr[sFieldName].ToString());
                }
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
            }
            finally
            {
                //siplDisposeObject(ref dt);
                //siplDisposeObject(ref dtTemp);
                //objSIPL = null;
            }
        }

        public List<SelectListItem> siplFillComboFromDataTable(DataTable dtTemp, string sFilter = "", string sSortBy = "", bool bNoItemData = false, bool bSelectIfOnlyOne = false)
        {
            List<SelectListItem> objSelectListItem = new List<SelectListItem>();
            try
            {

                if (dtTemp == null)
                {
                    return objSelectListItem;
                }
                if (dtTemp.Rows.Count == 0)
                {
                    return objSelectListItem;
                }
                dtTemp.DefaultView.RowFilter = "";
                if (!string.IsNullOrEmpty((sFilter + "").Trim()))
                {
                    dtTemp.DefaultView.RowFilter = sFilter;
                }
                dtTemp.DefaultView.Sort = "";
                if (!string.IsNullOrEmpty((sSortBy + "").Trim()))
                {
                    dtTemp.DefaultView.Sort = sSortBy;
                }

                for (int i = 0; i <= dtTemp.DefaultView.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(dtTemp.DefaultView[i][0].ToString() + ""))
                    {
                        // Cbo.Items.Add(dtTemp.DefaultView.Item(i).Item(0) + "");

                        objSelectListItem.Add(new SelectListItem
                        {
                            Value = dtTemp.DefaultView[i][0].ToString(),
                            Text = dtTemp.DefaultView[i][0].ToString()
                        });
                    }
                }
                if (bSelectIfOnlyOne == true)
                {
                    //If Only one item is present then select the same
                    //if (Cbo.Items.Count == 1)
                    //{
                    //    Cbo.SelectedIndex = 0;
                    //}
                    //else
                    //{
                    //    Cbo.SelectedIndex = -1;
                    //}
                }
                else
                {
                    //Cbo.SelectedIndex = -1;
                }
                return objSelectListItem;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
            }
            return objSelectListItem;
        }

        public void siplFillCheckListBoxFromDB(string sSQL, DbConnection conn, ref CheckBoxList CheckListBox, bool bItemData = false)
        {
            DataSet ds = new System.Data.DataSet();
            try
            {
                ds = siplOpenDataSet(sSQL, conn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CheckListBox.DataSource = ds;
                    CheckListBox.DataTextField = ds.Tables[0].Columns[0].ToString();

                    if (bItemData)
                    {
                        CheckListBox.DataValueField = ds.Tables[0].Columns[1].ToString();
                    }
                    CheckListBox.DataBind();
                }
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
            }
        }

        //This Function return the no of rows affected .
        public int siplExecuteStoreProc(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params, ref DbTransaction siplTransaction)
        {
            DbCommand cmdTemp = default(DbCommand);
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;

                cmdTemp.Transaction = siplTransaction;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.Direction = Params[i].Direction;
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                iAffected = cmdTemp.ExecuteNonQuery();

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        //This Function return the no of rows affected .
        public int siplExecuteStoreProc(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params)
        {
            DbCommand cmdTemp = default(DbCommand);
            int iAffected = 0;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.Direction = Params[i].Direction;
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                iAffected = cmdTemp.ExecuteNonQuery();
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                return iAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        //This Function return a scalar value .
        public string siplExecuteStoreProcScalar(string sStoreProc, ref DbConnection sipladConnection)
        {
            DbCommand cmdTemp = default(DbCommand);
            string sAffected = string.Empty;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                cmdTemp.Connection = sipladConnection;
                sAffected = Convert.ToString(cmdTemp.ExecuteScalar());
                return sAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        //This Function return a scalar value .
        public string siplExecuteStoreProcScalar(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params)
        {
            DbCommand cmdTemp = default(DbCommand);
            string sAffected = string.Empty;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.Direction = Params[i].Direction;
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                sAffected = Convert.ToString(cmdTemp.ExecuteScalar());
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                return sAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        //This Function return a scalar value .
        public string siplExecuteStoreProcScalar(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params, ref DbTransaction siplTransaction)
        {
            DbCommand cmdTemp = default(DbCommand);
            string sAffected = string.Empty;
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.Transaction = siplTransaction;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.Direction = Params[i].Direction;
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                sAffected = Convert.ToString(cmdTemp.ExecuteScalar());
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                return sAffected;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        //Excutes strored procdure and Returns datareader 
        public DbDataReader siplGetExecuteStoreProc(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params)
        {
            DbCommand cmdTemp = default(DbCommand);
            DbDataReader drTemp = default(DbDataReader);
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.Direction = Params[i].Direction;
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                drTemp = cmdTemp.ExecuteReader();
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }

                return drTemp;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //siplDisposeObject(ref cmdTemp);
            }
        }

        //Excutes strored procdure and Returns datareader 
        public DbDataReader siplGetExecuteStoreProc(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params, ref DbTransaction siplTransaction)
        {
            DbCommand cmdTemp = default(DbCommand);
            DbDataReader drTemp = default(DbDataReader);
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.Transaction = siplTransaction;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    //ParamTemp.Value = Params(i).Value
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.Direction = Params[i].Direction;
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                drTemp = cmdTemp.ExecuteReader();
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                return drTemp;
            }
            catch (Exception ex)
            {
                //siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                //    siplDisposeObject(ref cmdTemp);
                //}
            }
        }

        public DbDataReader siplGetExecuteStoreProc(string sStoreProc, ref DbConnection sipladConnection)
        {
            DbCommand cmdTemp = default(DbCommand);
            DbDataReader drTemp = default(DbDataReader);
            try
            {
                cmdTemp = GetDBCommand();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                cmdTemp.Connection = sipladConnection;
                drTemp = cmdTemp.ExecuteReader();
                return drTemp;
            }
            catch (Exception ex)
            {
                siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
            }
        }

        //Excutes strored procdure and Returns Dataset
        public DataSet siplGetExecuteStoreProcDataSet(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmdTemp = default(DbCommand);
            DataSet dstTemp = null;
            try
            {
                cmdTemp = GetDBCommand();
                dstTemp = new DataSet();
                cmdTemp.CommandTimeout = _nCommandTimeout;

                //.Transaction = siplTransaction
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    ParamTemp.Direction = Params[i].Direction;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                da.Fill(dstTemp);
                return dstTemp;
            }
            catch (Exception ex)
            {
                siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
            }
        }

        public DataSet siplGetExecuteStoreProcDataSet(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params, ref DbTransaction sipladTrasaction)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmdTemp = default(DbCommand);
            DataSet dstTemp = null;
            try
            {
                cmdTemp = GetDBCommand();
                dstTemp = new DataSet();
                cmdTemp.CommandTimeout = _nCommandTimeout;

                //.Transaction = siplTransaction
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    ParamTemp.Direction = Params[i].Direction;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                cmdTemp.Transaction = sipladTrasaction;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                da.Fill(dstTemp);
                return dstTemp;
            }
            catch (Exception ex)
            {
                siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
            }
        }

        public DataSet siplGetExecuteStoreProcDataSet(string sStoreProc, ref DbConnection sipladConnection)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmdTemp = default(DbCommand);
            DataSet dstTemp = null;
            try
            {
                cmdTemp = GetDBCommand();
                dstTemp = new DataSet();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                cmdTemp.Connection = sipladConnection;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                da.Fill(dstTemp);
                return dstTemp;
            }
            catch (Exception ex)
            {
                siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
            }
        }

        public DataTable siplGetExecuteStoreProcDataTable(string sStoreProc, ref DbConnection sipladConnection)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmdTemp = default(DbCommand);
            DataTable dtTemp = null;
            try
            {
                cmdTemp = GetDBCommand();
                dtTemp = new DataTable();
                cmdTemp.CommandTimeout = _nCommandTimeout;
                //.Transaction = siplTransaction
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;
                cmdTemp.Connection = sipladConnection;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                da.Fill(dtTemp);
                return dtTemp;
            }
            catch (Exception ex)
            {
                siplInsertErrorLog(ex);
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
                siplDisposeObject(ref dtTemp);

            }

        }

        public DataTable siplGetExecuteStoreProcDataTable(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmdTemp = default(DbCommand);
            DataTable dtTemp = null;
            try
            {
                cmdTemp = GetDBCommand();
                dtTemp = new DataTable();
                cmdTemp.CommandTimeout = _nCommandTimeout;

                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    ParamTemp.Direction = Params[i].Direction;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                da.Fill(dtTemp);
                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
                siplDisposeObject(ref dtTemp);
            }

        }

        public DataTable siplGetExecuteStoreProcDataTable(string sStoreProc, ref DbConnection sipladConnection, ref DbParameter[] Params, ref DbTransaction siplTransaction)
        {
            DbDataAdapter da = default(DbDataAdapter);
            DbCommand cmdTemp = default(DbCommand);
            DataTable dtTemp = null;
            try
            {
                cmdTemp = GetDBCommand();
                dtTemp = new DataTable();
                cmdTemp.CommandTimeout = _nCommandTimeout;

                cmdTemp.Transaction = siplTransaction;
                cmdTemp.CommandType = CommandType.StoredProcedure;
                cmdTemp.CommandText = sStoreProc;

                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    DbParameter ParamTemp = GetDBProviderFactory().CreateParameter();
                    ParamTemp.ParameterName = Params[i].ParameterName;
                    ParamTemp.Direction = Params[i].Direction;
                    if (!(ParamTemp.Direction == ParameterDirection.Output | ParamTemp.Direction == ParameterDirection.ReturnValue))
                    {
                        ParamTemp.Value = Params[i].Value;
                    }
                    ParamTemp.DbType = Params[i].DbType;
                    cmdTemp.Parameters.Add(ParamTemp);
                }
                cmdTemp.Connection = sipladConnection;
                da = GetDBDataAdapter();
                da.SelectCommand = cmdTemp;
                da.SelectCommand.CommandTimeout = _nCommandTimeout;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    if ((Params[i].Direction == ParameterDirection.Output | Params[i].Direction == ParameterDirection.InputOutput | Params[i].Direction == ParameterDirection.ReturnValue))
                    {
                        Params[i].Value = cmdTemp.Parameters[i].Value;
                    }
                }
                da.Fill(dtTemp);
                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                siplDisposeObject(ref cmdTemp);
                siplDisposeObject(ref dtTemp);
            }

        }

        public int siplGenerateSequence(string sSequenceName, DbConnection con, DbTransaction objTransc = null)
        {
            StringBuilder sSQL = new StringBuilder();
            DbDataReader dr = default(DbDataReader);
            try
            {
                sSQL.Remove(0, sSQL.Length);
                sSQL.Append(" SELECT " + sSequenceName + ".NEXTVAL As NO FROM DUAL");
                if (objTransc != null)
                {
                    dr = siplOpenReader(sSQL.ToString(), con, objTransc);

                }
                else
                {
                    dr = siplOpenReader(sSQL.ToString(), con);
                }
                if (dr.Read())
                {
                    return Convert.ToInt32(dr["NO"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                siplInsertErrorLog(ex);
                return 0;
            }
        }

        //public int siplGenerateKey(DbConnection con, string sCode, string sTableName, ref string[] sWhereClauseColumnName, ref string[] sWhereClauseValue)
        //{
        //    int nKey = 0;
        //    string objValue = null;
        //    //siplFunctions objSIPL = new siplFunctions();
        //    string sSQL = null;
        //    sSQL = "SELECT MAX(to_number(" + sCode + ")) AS MAXCODE";
        //    sSQL += " FROM " + sTableName;

        //    if (sWhereClauseColumnName != null)
        //    {
        //        for (int i = 0; i <= (sWhereClauseColumnName).Length - 1; i++)
        //        {
        //            if (!string.IsNullOrEmpty(sWhereClauseColumnName[i].Trim()))
        //            {

        //                if (sSQL.Contains(" WHERE "))
        //                {
        //                    sSQL += " AND Upper(" + sWhereClauseColumnName[i] + ") = '" + objSIPL.siplReplaceSingleQuotes(sWhereClauseValue[i]).ToString().ToUpper() + "'";
        //                }
        //                else
        //                {
        //                    sSQL += " WHERE Upper(" + sWhereClauseColumnName[i] + ") = '" + objSIPL.siplReplaceSingleQuotes(sWhereClauseValue[i]).ToString().ToUpper() + "'";
        //                }
        //            }
        //        }
        //    }

        //    //objSIPL = null;

        //    try
        //    {

        //        objValue = siplExecuteScalar(sSQL, con);
        //        if (System.DBNull.Value.Equals(objValue))
        //        {
        //            nKey = 0;
        //        }
        //        else
        //        {
        //            nKey = Convert.ToInt32(objValue);

        //        }

        //        nKey = nKey + 1;
        //        return nKey;
        //    }
        //    catch (Exception ex)
        //    {
        //        siplInsertErrorLog(ex);
        //        throw ex;
        //    }
        //}

        //public int siplGenerateKey(DbConnection con, DbTransaction objTrans, string sCode, string sTableName, ref string[] sWhereClauseColumnName, ref string[] sWhereClauseValue)
        //{
        //    int nKey = 0;
        //    string objValue = null;
        //    //siplFunctions objSIPL = new siplFunctions();
        //    string sSQL = null;
        //    sSQL = "SELECT MAX(to_number(" + sCode + ")) AS MAXCODE";
        //    sSQL += " FROM " + sTableName;

        //    if (sWhereClauseColumnName != null)
        //    {
        //        for (int i = 0; i <= (sWhereClauseColumnName).Length - 1; i++)
        //        {
        //            if (!string.IsNullOrEmpty(sWhereClauseColumnName[i].Trim()))
        //            {

        //                if (sSQL.Contains(" WHERE "))
        //                {
        //                    sSQL += " AND Upper(" + sWhereClauseColumnName[i] + ") = '" + objSIPL.siplReplaceSingleQuotes(sWhereClauseValue[i]).ToString().ToUpper() + "'";
        //                }
        //                else
        //                {
        //                    sSQL += " WHERE Upper(" + sWhereClauseColumnName[i] + ") = '" + objSIPL.siplReplaceSingleQuotes(sWhereClauseValue[i]).ToString().ToUpper() + "'";
        //                }
        //            }
        //        }
        //    }

            //objSIPL = null;

        //    try
        //    {

        //        objValue = siplExecuteScalar(sSQL, con, objTrans);
        //        if (System.DBNull.Value.Equals(objValue))
        //        {
        //            nKey = 0;
        //        }
        //        else
        //        {
        //            nKey = Convert.ToInt32(objValue);

        //        }

        //        nKey = nKey + 1;
        //        return nKey;
        //    }
        //    catch (Exception ex)
        //    {
        //        siplInsertErrorLog(ex);
        //        throw ex;
        //    }
        //}

        //public int siplGenerateKey(DbConnection con, DbTransaction objTrans, string sCode, string sTableName, string[] sWhereClauseColumnName = null, string[] sWhereClauseValue = null)
        //{
        //    int nKey = 0;
        //    string objValue = string.Empty;
        //    //siplFunctions objSIPL = new siplFunctions();
        //    string sSQL = string.Empty;
        //    try
        //    {
        //        if /*(siplFunctions.siplBackEnd == siplTypeDef.enBackEnd.BE_ORACLE)*/
        //        {
        //            sSQL = "SELECT MAX(to_number(" + sCode + ")) AS MAXCODE FROM " + sTableName;
        //        }
        //        else
        //        {
        //            sSQL = "SELECT MAX(CONVERT(NUMERIC, " + sCode + ")) AS MaxCode FROM " + sTableName;
        //        }


        //        if (sWhereClauseColumnName != null)
        //        {
        //            for (int i = 0; i <= (sWhereClauseColumnName).Length - 1; i++)
        //            {
        //                if (!string.IsNullOrEmpty(sWhereClauseColumnName[i].Trim()))
        //                {

        //                    if (sSQL.Contains(" WHERE "))
        //                    {
        //                        sSQL += " AND Upper(" + sWhereClauseColumnName[i] + ") = '" + objSIPL.siplReplaceSingleQuotes(sWhereClauseValue[i]).ToString().ToUpper() + "'";
        //                    }
        //                    else
        //                    {
        //                        sSQL += " WHERE Upper(" + sWhereClauseColumnName[i] + ") = '" + objSIPL.siplReplaceSingleQuotes(sWhereClauseValue[i]).ToString().ToUpper() + "'";
        //                    }
        //                }
        //            }
        //        }

        //        //objSIPL = null;
        //        objValue = siplExecuteScalar(sSQL, con, objTrans);
        //        if (System.DBNull.Value.Equals(objValue) || objValue == "")
        //        {
        //            nKey = 0;
        //        }
        //        else
        //        {
        //            nKey = Convert.ToInt32(objValue);

        //        }

        //        nKey = nKey + 1;
        //        return nKey;
        //    }
        //    catch (Exception ex)
        //    {
        //        //objSIPL.siplInsertErrorLog(ex);
        //        throw ex;
        //    }
        //}

        #endregion

        public static void siplInsertErrorLog(Exception ex)
        {
            string sFilePath = null;
            sFilePath = siplTextErrorLogFilePath;
            Stream st = File.Open(sFilePath, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(st);
            string strLine = "";
            string sHash = " # ";
            string sProductVersion = Assembly.GetCallingAssembly().GetName().Version.ToString();
            sw.WriteLine(sHash + "Date/Time : " + DateTime.Now + sHash + "Version : " + sProductVersion);
            if ((ex.TargetSite != null))
            {
                sw.WriteLine("# Source : " + ex.TargetSite.ToString() + sHash);
            }
            sw.WriteLine("# Error Type : " + ex.Message + sHash);
            sw.WriteLine("# Error Details : " + "Computer Name : " + HttpContext.Current.Request.UserHostName + ex.StackTrace + sHash);
            sw.WriteLine(strLine.PadRight(95, '*'));
            sw.WriteLine();
            sw.Close();
            st.Close();
        }

        #region "Dispose Object"


        public void siplDisposeObject(ref DbCommand obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        public void siplDisposeObject(ref DbDataAdapter obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }
        public void siplDisposeObject(ref DataSet obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }
        public void siplDisposeObject(ref DataTable obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        public void siplDisposeObject(ref DbDataReader obj)
        {
            if (obj != null)
            {
                obj.Close();
                obj.Dispose();
                obj = null;
            }
        }

        public void siplDisposeObject(ref DbConnection obj)
        {
            if (obj != null)
            {
                if (obj.State == ConnectionState.Open)
                {
                    obj.Close();
                }
                obj.Dispose();
                obj = null;
            }
        }

        public void siplDisposeObject(ref DbTransaction obj)
        {
            if (obj != null)
            {
                if (obj.Connection != null)
                {
                    obj.Rollback();
                }
                obj.Dispose();
                obj = null;
            }
        }


        #endregion
    }
}
