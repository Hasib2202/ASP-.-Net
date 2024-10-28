using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondClass.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Create()
        {

            return View();
        }
        public ActionResult List()
        {

            return View();
        }
    }
}