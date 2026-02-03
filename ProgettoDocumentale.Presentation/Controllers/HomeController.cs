using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoDocumentale.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (User.IsInRole("Admin")) return RedirectToAction("Index", "Admin");
            if (User.IsInRole("CedacriOperator")) return RedirectToAction("Index", "CedacriOperator");
            if (User.IsInRole("BancOperator")) return RedirectToAction("Index", "BankOperator");
            
            return RedirectToAction("AccessDenied", "Account");
        }

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