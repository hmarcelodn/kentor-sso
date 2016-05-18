using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epiq.KentorSSO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Insecured()
        {
            ViewBag.Title = "Insecured";

            return View();
        }

        [Authorize]
        public ActionResult Secured()
        {
            ViewBag.Title = "Secured";

            return View();
        }
    }
}
