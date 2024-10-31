using FormSubmission.Models;
//using FromHandle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FromHandle.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        // This method is used to access the value of the input field using Request
        // The value of the input field is stored in the Request object
        // This method is also worked for post and get method

        [HttpGet]
        //public ActionResult Create()
        //{
        //    //ViewBag.Name = Request["Name"];
        //    //ViewBag.Id = Request["Id"];
        //    return View();
        //}

        public ActionResult Create()
        {
            //ViewBag.Name = Request["Name"];
            //ViewBag.Id = Request["Id"];
            return View(new Student());
        }

        // This method is used to access the value of the input field using FormCollection
        // The value of the input field is stored in the FormCollection object
        // This method can be overloded and only use for MVC


        //[HttpPost]
        //public ActionResult Create(FormCollection fc)
        //{
        //    ViewBag.Name = fc["Name"];
        //    ViewBag.Id = fc["Id"];
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(string Name, string Id)
        //{
        //    ViewBag.Name = Name;
        //    ViewBag.Id = Id;
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(Student s)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(s);
        //}
        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid && student.IsValid())
            {
                // Save to database
                //return RedirectToAction("Index", "Home");
                return View(student);

            }
            return View(student);
        }


    }
}