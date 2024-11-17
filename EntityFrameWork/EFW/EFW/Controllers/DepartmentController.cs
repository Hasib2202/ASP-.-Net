using EFW.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFW.Controllers
{
    public class DepartmentController : Controller
    {
        StudentModelContext1 db = new StudentModelContext1();

        // GET: Department
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Department d)
        {
            db.Departments.Add(d);
            db.SaveChanges();

            return RedirectToAction("Depts");
        }

        public ActionResult Depts()
        {
            var data = db.Departments.ToList();
            return View(data);
        }

        public ActionResult Details(int id)
        {
            var data = db.Departments.Find(id);

            if (data == null)
            {
                TempData["Message"] = "Department with value " + id + " not found";
                return RedirectToAction("Depts");
            }
            return View(data);
        }
    }
}