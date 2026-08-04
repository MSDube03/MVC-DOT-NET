using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text;

namespace WebApplicationtest.Models.Employee
{
    public class Employee
    {
        public string Emp_Name { get; set; }

        //  [Required]
        public int Age { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public int salary { get; set; }
        public int id { get; set; }
        public string Contact { get; set; }

        public string sOperation { get; set; }



        //public List<Employee> DisplayRecords()
        //{
        //    StringBuilder sSQL = new StringBuilder();
        //    DbConnection conn = null;
        //    siplDBFactory factory = new siplDBFactory();
        //    DbDataReader reader = null;
        //    List<Employee> emplist = new List<Employee>();

        //    try
        //    {
        //        sSQL.Clear();
        //        sSQL.Append("select EmployeeID,Emp_Name, Email, PhoneNumber, Department , Salary from Employees");
        //        conn = factory.siplOpenDB();
        //        reader = factory.siplOpenReader(sSQL.ToString(), conn);

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                Employee obj = new Employee();
        //                obj.salary = reader["EmployeeID"] == null ? 0 : Convert.ToInt32(reader["EmployeeID"].ToString());
        //                obj.Name = reader["Emp_Name"] == null ? string.Empty : reader["Emp_Name"].ToString();
        //                obj.Email = reader["Email"] == null ? string.Empty : reader["Email"].ToString();
        //                obj.Contact = reader["PhoneNumber"] == null ? string.Empty : reader["PhoneNumber"].ToString();
        //                obj.Department = reader["Department"] == null ? string.Empty : reader["Department"].ToString();
        //                obj.salary = reader["Salary"] == null ? 0 : Convert.ToInt32(reader["Salary"].ToString());
        //                emplist.Add(obj);


        //            }

        //        }

        //    }

        //    catch (Exception)
        //    {


        //        throw;
        //    }

        //    finally
        //    {

        //        factory.siplDisposeObject(ref reader);
        //        factory.siplDisposeObject(ref reader);
        //        factory = null;
        //        sSQL = null;

        //    }

        //    return emplist;
        //}


        //THIS IS THE CODE TO INSERT OR UPDATE THE EMPLOYEE DATA INTO DATABASE
        //public int mInsertUpdateDatabase()
        //   {
        //    StringBuilder sSQL = new StringBuilder();
        //    DbConnection conn = null;
        //    siplDBFactory factory = new siplDBFactory();

        //    try
        //    {
        //        if (sOperation == "Add")
        //        {
        //            sSQL.Clear();
        //            sSQL.Append("INSERT INTO Employees ");
        //            sSQL.Append("(Emp_Name, Email, PhoneNumber, Department, Salary) ");
        //            sSQL.Append("VALUES (");
        //            //sSQL.Append("'" + id.ToString().Trim() + "',");
        //            sSQL.Append("'" + Name.ToString().Trim() + "',");
        //            sSQL.Append("'" + Email.ToString().Trim() + "',");
        //            sSQL.Append("'" + Contact.ToString().Trim() + "',");
        //            sSQL.Append("'" + Department.ToString().Trim() + "',");
        //            sSQL.Append(salary);
        //            sSQL.Append(")");
        //        }
        //        else
        //        {
        //            sSQL.Clear();
        //            sSQL.Append("UPDATE Employees SET ");
        //            sSQL.Append("Emp_Name = '" + Name.ToString().Trim() + "', ");
        //            sSQL.Append("Email = '" + Email.ToString().Trim() + "', ");
        //            sSQL.Append("PhoneNumber = '" + Contact.ToString().Trim() + "', ");
        //            sSQL.Append("Department = '" + Department.ToString().Trim() + "', ");
        //            sSQL.Append("Salary = " + salary + " ");
        //            sSQL.Append("WHERE EmployeeID = '" + id.ToString().Trim() + "'");
        //        }
        //         conn = factory.siplOpenDB();
        //        int iRowsAffected = factory.siplExecute(sSQL.ToString(), conn);
        //        return iRowsAffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        factory.siplDisposeObject(ref conn);
        //        factory = null;
        //        sSQL = null;
        //    }
        //}
    }
}

    