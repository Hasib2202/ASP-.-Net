using CFA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CFA2.Controllers
{
    public class HomeController : Controller
    {
        StudentContext db = new StudentContext();

        public ActionResult Index()
        {
            var data = db.Students.ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student s)
        {
            if (ModelState.IsValid == true)
            {
                db.Students.Add(s);
                int a = db.SaveChanges();

                if (a > 0)
                {
                    //ViewBag.InsertMessage = "<script>alert('Data Inserted')</script>";
                    //TempData["InsertMessage"] = "<script>alert('Data Inserted')</script>";
                    TempData["InsertMessage"] = "Data Inserted Successfully";
                    return RedirectToAction("Index");
                    //ModelState.Clear();
                }
                else
                {
                    ViewBag.InsertMessage = "<script>alert('Failed to Insert Data')</script>";
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var row = db.Students.Where(model => model.Id == id).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult Edit(Student s)
        {
            if (ModelState.IsValid == true)
            {
                db.Entry(s).State = EntityState.Modified;
                int a = db.SaveChanges();

                if (a > 0)
                {
                    TempData["UpdateMessage"] = "Data Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.UpdateMessage = "<script>alert('Failed to Update Data')</script>";
                }

            }
            return View();
        }

        // Delete using HttpGet and HttpPost with conformation

        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    var row = db.Students.Where(model => model.Id == id).FirstOrDefault();
        //    return View(row);
        //}

        //[HttpPost]
        //public ActionResult Delete(Student s)
        //{
        //    if (ModelState.IsValid == true)
        //    {
        //        db.Entry(s).State = EntityState.Deleted;
        //        int a = db.SaveChanges();
        //        if (a > 0)
        //        {
        //            TempData["DeleteMessage"] = "Data Deleted Successfully";
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ViewBag.DeleteMessage = "<script>alert('Failed to Delete Data')</script>";
        //        }
        //    }
        //    return View();
        //}


        // Delete using HttpGet without conformation
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var row = db.Students.Where(model => model.Id == id).FirstOrDefault();
                if (row != null)
                {
                    db.Entry(row).State = EntityState.Deleted;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMessage"] = "Data Deleted Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.DeleteMessage = "<script>alert('Failed to Delete Data')</script>";
                    }
                }
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            var row = db.Students.Where(model => model.Id == id).FirstOrDefault();
            return View(row);
        }
    }
}