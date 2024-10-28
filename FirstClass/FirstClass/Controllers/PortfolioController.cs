using FirstClass.Models;
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
   
        public ActionResult Bio()
        {
            return View();
        }

        public ActionResult Education()
        {
            Degree D1 = new Degree();
            D1.Title = "HSC";
            D1.Ins = "Govt. Science College";
            D1.Year = "2019";
            D1.Result = "4.67";

            Degree D2 = new Degree();
            D2.Title = "SSC";
            D2.Ins = "Govt. High School";
            D2.Year = "2017";
            D2.Result = "5.00";

            Degree D3 = new Degree();
            D3.Title = "JSC";
            D3.Ins = "Govt. High School";
            D3.Year = "2015";
            D3.Result = "5.00";

            Degree[] degrees = new Degree[] { D1, D2, D3 };
            ViewBag.Degrees = degrees;

            return View();
        }

        public ActionResult Qualifications()
        {
            // Suppose has Qualifications
            bool hasQualifications = false;
            //
            //
            //
            if(hasQualifications)
            {
                return View();
            }
            else
            {
                TempData["Message"] = "No Qualifications Found Visit Education insted!";
                return RedirectToAction("Education", "Portfolio");

            }
           
        }


    }
}