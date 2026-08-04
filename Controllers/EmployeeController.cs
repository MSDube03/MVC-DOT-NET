using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationtest.Models.Employee;
namespace WebApplicationtest.Controllers
{

    [HandleError]
    public class EmployeeController : Controller
    {

        //    private static List<Employee> employee = new List<Employee>() {
        //    new Employee { Name = "Meera",   Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Akshata", Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Shraddha",Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Sakshi",  Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Sneha",   Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Sayali",  Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Bhumika", Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 },
        //    new Employee { Name = "Priya",   Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 50000 }


        //};
        // GET: Employee

        [OutputCache(Duration = 30)]


        public ActionResult Index()
        {
            // Employee objEmp = new Models.Employee.Employee() { Name = "Meera", Age = 20, Department = "ENTC", Email = "meera@softlink.com", salary = 40000, id = 403, Contact = "rtrrrt" };
            ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");


            //Test_MeeraEntities3 db = new Test_MeeraEntities3();
            //var empList =(from emp in db.Employees1 select emp).ToList();

           using (var ctx = new Test_MeeraEntities3())
            {
                var temp = ctx.Employees1.ToList();
                return View(temp);
            }




            //Employee objEmployee = new Employee ();
            //List<Employee> emplist = objEmployee.DisplayRecords();
            //return View(emplist);

            // This is the code to filter the data by using where 
            //var filteredEmployees =
            //from emp in emplist
            //where emp.Department == "ENTC"
            //orderby emp.Name descending
            //select emp;
            //Employee[] employeeArray = emplist.ToArray();


            //var filteredEmployees =
            //from emp in emplist
            ////where emp.Department == "ENTC"
            //orderby emp.Name 
            //select emp;



            // //Employee[] employeeArray =
            //(from emp in emplist
            // where emp.Department == "ENTC"
            // orderby emp.Name descending
            // select emp).ToArray();




            //return View(filteredEmployees.ToList());
        }





        //public ActionResult Create()
        //{
        //    Employee emp = new Employee();
        //    //List<Employee> emplist = objEmployee.DisplayRecords();

        //    emp.sOperation = "Add";
        //    return View(emp);
        //    //return PartialView();
        //}



        public ActionResult Delete()
        {

            return PartialView();

        }
        // GET: Employee/Edit/5
        public ActionResult Edit(int EmployeeId)
        {
            using (var ctx = new Test_MeeraEntities3())
            {
                var emp = ctx.Employees1.Find(EmployeeId);

                if (emp == null)
                {
                    return HttpNotFound();
                }

                return PartialView(emp);   
            }
        }

        // POST: Employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee1 employee)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new Test_MeeraEntities3())
                {
                    var emp = ctx.Employees1.Find(employee.EmployeeId);

                    if (emp == null)
                    {
                        return HttpNotFound();
                    }

                    emp.Emp_Name = employee.Emp_Name;
                    emp.Age = employee.Age;
                    emp.Department = employee.Department;
                    emp.Email = employee.Email;
                    emp.Salary = employee.Salary;
                    emp.PhoneNumber = employee.PhoneNumber;

                    ctx.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return PartialView(employee);
        }


        // public ActionResult Edit()
        // {
        //     return View();
        //}


        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Create(Employee emp)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        //employees.id = employee.Any() ? employee.Max(x => x.id) + 1 : 1;

        //        //employee.Add(employees);
        //        int result = emp.mInsertUpdateDatabase();

        //        TempData["emp"] = "Employee Saved Successfully";

        //        return RedirectToAction("Index");
        //    }

        //    return PartialView("Create", emp);
        //}



        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee1 employee)
       {
            if (ModelState.IsValid)
           {
                using (var ctx = new Test_MeeraEntities3())
               {
                   ctx.Employees1.Add(employee);
                   ctx.SaveChanges();
               }

                return RedirectToAction("Index");
          }

            return View(employee);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Employee1 employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            using (var ctx = new Test_MeeraEntities3())
        //            {
        //                ctx.Employees1.Add(employee);
        //                ctx.SaveChanges();
        //            }

        //            return RedirectToAction("Index");
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //        {
        //            foreach (var eve in ex.EntityValidationErrors)
        //            {
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    System.Diagnostics.Debug.WriteLine(
        //                        $"Property: {ve.PropertyName} Error: {ve.ErrorMessage}");
        //                }
        //            }

        //            throw;
        //        }
        //    }

        //    return View(employee);
        //}





        //[HandleError(ExceptionType = typeof(NullReferenceException), View ="NullReference")]
        //  public ActionResult TestMethod1()
        //       {

        //           throw new NullReferenceException();
        //       }

        //[HandleError(ExceptionType = typeof(DivideByZeroException), View = "DivideByZeroException")]
        //       public ActionResult TestMethod2()
        //       {
        //           throw new DivideByZeroException();
        //       }

    }
}
