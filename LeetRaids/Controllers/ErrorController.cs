using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace LeetRaids.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        //This is for exceptions, and other generic errors
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult NotFound()
        {
            return View();
        }


    }
}
