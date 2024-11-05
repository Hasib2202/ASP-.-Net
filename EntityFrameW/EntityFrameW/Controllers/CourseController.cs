using EntityFrameW.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFrameW.Controllers
{
    public class CourseController : Controller
    {

        ASPEntities1 db = new ASPEntities1();

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
            return RedirectToAction("CourseList");
        }

        public ActionResult CourseList()
        {
            var courses = db.Courses.ToList();
            return View(courses);
        }

        public ActionResult CourseDetails(int id)
        {
            var data = db.Courses.Find(id);
            if(data == null)
            {
                TempData["Message"] = "Course with id "+ id+ " not found!";
                return RedirectToAction("CourseList");
            }
            return View(data);
        }
    }
}