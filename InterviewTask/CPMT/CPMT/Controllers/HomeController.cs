using CPMT.DAL;
using CPMT.EF;
using CPMT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CPMT.Controllers
{
    public class HomeController : Controller
    {
        private readonly GTwoTaskEntities db = new GTwoTaskEntities();

        public ActionResult Index()
        {
            // Fetch corporate customers
            var corporateCustomers = db.Corporate_Customer_Tbl
                .Select(c => new { c.CustomerID, c.CompanyName })
                .ToList();

            // Fetch individual customers
            var individualCustomers = db.Individual_Customer_Tbl
                .Select(i => new { i.CustomerID, FullName = i.FirstName + " " + i.LastName })
                .ToList();

            // Pass data to ViewBag
            ViewBag.CorporateCustomers = corporateCustomers;
            ViewBag.IndividualCustomers = individualCustomers;

            return View();
        }
    }
}