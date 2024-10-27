using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstClass.Controllers
{
    public class PortfolioController : Controller
    {
        // GET: Portfolio
        public ActionResult Education()
        {
            return View();
        }

        public ActionResult Bio()
        {
            return View();
        }


    }
}