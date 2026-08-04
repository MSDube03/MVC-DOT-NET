using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationtest.Models;


namespace WebApplicationtest.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()

        //{ 
        //    Class1 STUDENT = new Class1();
       
        //    return View(STUDENT);
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}