using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationtest.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFoundError()
        {
            Response.StatusCode = 404;   
            return View();
        }

    }
}