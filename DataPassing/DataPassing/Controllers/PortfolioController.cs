using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntroMVC.Models;

namespace IntroMVC.Controllers
{
    public class PortfolioController : Controller
    {
        public ActionResult Bio()
        {
            return View();
        }
        public ActionResult Education()
        {
            Degree d1 = new Degree()
            {
                Title = "SSC",
                Ins = "In1",
                Year = "2018",
                Result = "5.00"
            };
            Degree d2 = new Degree()
            {
                Title = "HSC",
                Ins = "In2",
                Year = "2020",
                Result = "5.00"
            };
            Degree d3 = new Degree()
            {
                Title = "BSc",
                Ins = "AIUB",
                Year = "2024",
                Result = "3.00"
            };

            Degree[] degrees = new Degree[] { d1, d2, d3 };
            ViewBag.Degrees = degrees;
            return View();
        }
        public ActionResult Qualifications()
        {
            bool hasQualifications = false;
            //
            //
            //
            //
            if (hasQualifications)
            {
                return View();
            }
            else
            {
                TempData["Msg"] = "No extra qualifications, visit education instead";
                return RedirectToAction("Education", "Portfolio");
            }

        }
        public ActionResult References()
        {
            Referee r1 = new Referee()
            {
                Name = "N1",
                Email = "E1",
                Desig = "De1"
            };
            Referee r2 = new Referee()
            {
                Name = "N2",
                Email = "E1",
                Desig = "De1"
            };
            Referee r3 = new Referee()
            {
                Name = "N3",
                Email = "E1",
                Desig = "De1"
            };
            Referee[] refs = new Referee[] { r1, r2, r3 };

            return View(refs);
        }
    }
}