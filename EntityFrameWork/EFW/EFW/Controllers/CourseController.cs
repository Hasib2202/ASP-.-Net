using EFW.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFW.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        StudentModelContext db = new StudentModelContext();

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Course c)
        {
            db.Courses.Add(c);
            db.SaveChanges();

            return RedirectToAction("Lists");
        }
        public ActionResult Lists()
        {
            var data = db.Courses.ToList();
            return View(data);
        }

        public ActionResult Details(int id)
        {
            var data = db.Courses.Find(id);

            if (data == null)
            {
                TempData["Message"] = "Courses with value " + id + " not found";
                return RedirectToAction("Lists");
            }
            return View(data);
        }
    }
}