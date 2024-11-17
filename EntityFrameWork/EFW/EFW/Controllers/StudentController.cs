using EFW.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EFW.Controllers
{
    public class StudentController : Controller
    {
        StudentModelContext1 db = new StudentModelContext1();

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student s)
        {
            // Server-side validation for name
            var namePattern = @"^[a-zA-Z. ]+$"; // Allows letters, dots, and spaces only
            if (!Regex.IsMatch(s.name, namePattern))
            {
                ModelState.AddModelError("name", "Name can only contain letters, dots (.), and spaces, with no numbers or other special characters.");
            }


            // Server-side validation for CGPA
            if (!float.TryParse(s.cgpa, out float cgpaValue))
            {
                ModelState.AddModelError("cgpa", "CGPA must be a valid numeric value.");
            }

            // Server-side validation for father's name
            if (string.IsNullOrWhiteSpace(s.fathername))
            {
                ModelState.AddModelError("fathername", "Father's name is required.");
            }

            // Server-side validation for mother's name
            if (string.IsNullOrWhiteSpace(s.mothername))
            {
                ModelState.AddModelError("mothername", "Mother's name is required.");
            }

            // Server-side validation for religion
            if (string.IsNullOrWhiteSpace(s.religion))
            {
                ModelState.AddModelError("religion", "Religion is required.");
            }

            // Server-side validation for gender
            if (string.IsNullOrWhiteSpace(s.gender))
            {
                ModelState.AddModelError("gender", "Gender selection is required.");
            }

            // Server-side validation for student interest
            if (string.IsNullOrWhiteSpace(s.studentinterest))
            {
                ModelState.AddModelError("studentinterest", "Student interest selection is required.");
            }

            // Server-side validation for date of birth
            if (s.dateofbirth == default(DateTime))
            {
                ModelState.AddModelError("dateofbirth", "Date of birth is required.");
            }

            // Check if ModelState is valid before saving
            if (!ModelState.IsValid)
            {
                return View(s); // Return the form with validation errors
            }

            try
            {
                db.Students.Add(s);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log error
                ModelState.AddModelError("", "An error occurred while saving data.");
                return View(s);
            }
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

        // GET: Edit
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
            // Validate Name
            if (string.IsNullOrWhiteSpace(s.name))
            {
                ModelState.AddModelError("Name", "Name field cannot be empty.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(s.name, @"^[a-zA-Z.\s]+$"))
            {
                ModelState.AddModelError("Name", "Name can only contain letters, dots (.), and spaces.");
            }

            // Validate FatherName
            if (string.IsNullOrWhiteSpace(s.fathername))
            {
                ModelState.AddModelError("FatherName", "Father's Name cannot be empty.");
            }

            // Validate MotherName
            if (string.IsNullOrWhiteSpace(s.mothername))
            {
                ModelState.AddModelError("MotherName", "Mother's Name cannot be empty.");
            }

            // Validate Religion
            if (string.IsNullOrWhiteSpace(s.religion))
            {
                ModelState.AddModelError("Religion", "Religion cannot be empty.");
            }

            // Validate CGPA
            //if (s.cgpa < 0 || s.cgpa > 4.0)
            //{
            //    ModelState.AddModelError("CGPA", "CGPA must be between 0.0 and 4.0.");
            //}

            // Validate Gender
            if (string.IsNullOrEmpty(s.gender))
            {
                ModelState.AddModelError("Gender", "Gender must be selected.");
            }

            // Validate StudentInterest
            if (string.IsNullOrEmpty(s.studentinterest))
            {
                ModelState.AddModelError("StudentInterest", "Interest must be selected.");
            }

            // Validate DateOfBirth (should not be in the future)
            if (s.dateofbirth > DateTime.Now)
            {
                ModelState.AddModelError("DateOfBirth", "Date of Birth cannot be in the future.");
            }

            if (!ModelState.IsValid)
            {
                return View(s); // Return the form with error messages if validation fails
            }

            var data = db.Students.Find(s.id);
            if (data == null)
            {
                TempData["Message"] = "Id with value " + s.id + " not found";
                return RedirectToAction("List");
            }

            // Update the student's fields
            data.name = s.name;
            data.fathername = s.fathername;
            data.mothername = s.mothername;
            data.religion = s.religion;
            //data.CGPA = s.CGPA;
            data.gender = s.gender;
            data.studentinterest = s.studentinterest;
            data.dateofbirth = s.dateofbirth;
            db.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.Students.Find(id);
            if (data == null)
            {
                TempData["Message"] = "Id with value " + id + " not found";
                return RedirectToAction("List");
            }
            return View(data); // This will display the delete confirmation view
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var data = db.Students.Find(id);
            if (data != null)
            {
                db.Students.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }


    }
}