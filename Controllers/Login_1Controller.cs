using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationtest.Models;
namespace WebApplicationtest.Controllers
{
    public class Login_1Controller : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public ActionResult Register(string username, string password, string role)
        {
            bool result = UserStore.Register(username, password, role);

            if (result)
            {
                TempData["Message"] = "Registration Successful";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Username already exists.";
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            WebApplicationtest. Models.User user = UserStore.Login(username, password);

            if (user != null)
            {
                Session["Username"] = user.Username;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Employee");
            }

            ViewBag.Error = "Invalid Username or Password";
            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}