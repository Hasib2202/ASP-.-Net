using EFW.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFW.Controllers
{
    public class StudentController : Controller
    {
        StudentModelContext db = new StudentModelContext();

        // GET: Student

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student s)
        {
            db.Students.Add(s);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {   
            var data = db.Students.ToList();
            return View(data);
        }

        public ActionResult Details(int id)
        {
            var data = db.Students.Find(id);

            if(data == null)
            {
                TempData["Message"] = "Id with value " + id + " not found";
                return RedirectToAction("List");
            }
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = db.Students.Find(id);

            if (data == null)
            {
                TempData["Message"] = "Id with value " + id + " not found";
                return RedirectToAction("List");
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Student s)
        {

        }


        }
}