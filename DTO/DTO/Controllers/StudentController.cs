using DTO.DTOs;
using DTO.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTO.Controllers
{
    public class StudentController : Controller
    {
        // Data base

        ASPEntities1 db = new ASPEntities1();

        // Convert the EF model to DTO
        public static Student Convert(StudentDTO s)
        {
            return new Student()
            {
                id = s.id,
                name = s.name,
                cgpa = s.cgpa,
                fathername = s.fathername,
                mothername = s.mothername,
                religion = s.religion,
                gender = s.gender,
                studentinterest = s.studentinterest,
                dateofbirth = s.dateofbirth
            };
        }

        // Convert the DTO to EF model
        public static StudentDTO Convert(Student s)
        {
            return new StudentDTO()
            {
                id = s.id,
                name = s.name,
                cgpa = s.cgpa,
                fathername = s.fathername,
                mothername = s.mothername,
                religion = s.religion,
                gender = s.gender,
                studentinterest = s.studentinterest,
                dateofbirth = s.dateofbirth
            };
        }

        // Convert the list of EF model to DTO

        public static List<StudentDTO> Convert(List<Student> data)
        {
            var studentlist = new List<StudentDTO>();
            foreach (var i in data)
            {
                studentlist.Add(Convert(i));
            }
            return studentlist;
        }


        // GET: Student
        [HttpGet]
        public ActionResult Create()
        {
            return View(new StudentDTO());
        }

        [HttpPost]
        public ActionResult Create(StudentDTO s)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = new Student()
                    {
                        id = s.id,
                        name = s.name,
                        cgpa = s.cgpa,
                        fathername = s.fathername,
                        mothername = s.mothername,
                        religion = s.religion,
                        gender = s.gender,
                        studentinterest = s.studentinterest,
                        dateofbirth = s.dateofbirth
                    };
                    db.Students.Add(data);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(s);
        }


        // List 
        public ActionResult List()
        {
            var studentlist = db.Students.ToList();
            return View(studentlist);
        }

        // Edit 

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var efcreatedata = db.Students.Find(id);
            if (efcreatedata == null)
            {
                return HttpNotFound();
            }
            var studentDTO = Convert(efcreatedata);
            return View(studentDTO);
        }

        [HttpPost]
        public ActionResult Edit(StudentDTO s)
        {
            if (ModelState.IsValid)
            {
                var data = db.Students.Find(s.id);
                if (data != null)
                {
                    data.name = s.name;
                    data.cgpa = s.cgpa;
                    data.fathername = s.fathername;
                    data.mothername = s.mothername;
                    data.religion = s.religion;
                    data.gender = s.gender;
                    data.studentinterest = s.studentinterest;
                    data.dateofbirth = s.dateofbirth;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            return View(s);
        }

        // Details
        public ActionResult Details(int id)
        {
            var data = db.Students.Find(id);
            return View(Convert(data));
        }

        // Delete 
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.Students.Find(id);
            return View(Convert(data));
        }

        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var data = db.Students.Find(Id);
                db.Students.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

    }
}