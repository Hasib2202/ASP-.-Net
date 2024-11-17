using DTO.DTOs;
using DTO.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTO.Controllers
{
    public class DepartmentController : Controller
    {
        ASPEntities1 db = new ASPEntities1();


        // Convert the EF model to DTO
        public static Department Convert(DepartmentDTO d)
        {
            return new Department()
            {
                id = d.id,
                name = d.name,
                location = d.location
            };
        }

        // Convert the DTO to EF model
        public static DepartmentDTO Convert(Department d)
        {
            return new DepartmentDTO()
            {
                id = d.id,
                name = d.name,
                location = d.location
            };
        }

        // Convert the list of EF model to DTO
        public static List<DepartmentDTO>Convert(List<Department> data)
        {
            var list = new List<DepartmentDTO>();
            foreach (var i in data)
            {
                list.Add(Convert(i));
            }
            return list;
        }

        // GET: Department
        [HttpGet]
        public ActionResult Create()
        {
            
            return View(new DepartmentDTO());
        }

        [HttpPost]
        public ActionResult Create(DepartmentDTO d)
        {
            if (ModelState.IsValid)
            {
                var efcreatedata = new Department()
                {
                    name = d.name,
                    location = d.location
                };
                db.Departments.Add(efcreatedata);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(d);

        }
        // List
        public ActionResult List()
        {
            var efcreatedata = db.Departments.ToList();
            return View(efcreatedata);
        }


        // Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var efcreatedata = db.Departments.Find(id);
            if (efcreatedata == null)
            {
                return HttpNotFound();
            }
            var departmentDTO = Convert(efcreatedata);
            return View(departmentDTO);
        }

        [HttpPost]
        public ActionResult Edit(DepartmentDTO d)
        {
            if (ModelState.IsValid)
            {
                var data = db.Departments.Find(d.id);
                if (data != null)
                {
                    data.name = d.name;
                    data.location = d.location;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            return View(d);
        }


        // Details
        public ActionResult Details(int id)
        {
            var data = db.Departments.Find(id);
            var test = data.Students;
            return View(Convert(data));
        }

        // Delete 
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.Departments.Find(id);
            return View(Convert(data));
        }

        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var data = db.Departments.Find(Id);
                db.Departments.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}