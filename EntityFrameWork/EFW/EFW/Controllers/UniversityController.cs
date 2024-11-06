using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFW.Controllers
{
    public class UniversityController : Controller
    {
        // GET: University
        public ActionResult All()
        {
            return View();
        }
    }
}