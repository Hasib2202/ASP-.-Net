using LearningManagement.DTOs;
using LearningManagement.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningManagement.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        private readonly LearningManagementEntities db = new LearningManagementEntities();

        // Dashboard Action
        [HttpGet]
        public ActionResult Dashboard()
        {
            

            if (Session["info"] == null )
            {
                TempData["msg"] = "Unauthorized access. Please log in.";

                return RedirectToAction("Index", "Login");
            }


            var user = (User)Session["info"];
            if (user.Role != "Student")
            {
                TempData["msg"] = "Only students can view enrollments.";
                return RedirectToAction("Dashboard", "Login");
            }

            var enrollments = db.Enrollments
                .Where(e => e.StudentId == user.UserId)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    StudentName = e.User.Name,
                    StudentEmail = e.User.Email,
                    CourseId = e.CourseId,
                    CourseName = e.Course.Name,
                    CourseDuration = e.Course.Duration,
                    InstructorName = e.Course.User.Name, // Assuming Course.User is the Instructor
                    //Progress = e.StudentProgresses.FirstOrDefault()?.Progress ?? 0, // If progress exists
                    //EnrollmentDate = e.EnrollmentDate
                })
                .ToList();

            return View(enrollments);
        }


        [HttpGet]
        public ActionResult AvailableCourses()
        {
            if (Session["info"] is User loggedInUser && loggedInUser.Role == "Student")
            {
                var availableCourses = (from c in db.Courses
                                        where !(from e in db.Enrollments
                                                where e.StudentId == loggedInUser.UserId
                                                select e.CourseId).Contains(c.CourseId)
                                        select c).ToList();

                ViewBag.AvailableCourses = availableCourses;
                return View();
            }

            TempData["msg"] = "Please log in as a Student.";
            return RedirectToAction("Index", "Login");
        }


        [HttpGet]
        public ActionResult MyEnrollments()
        {
            if (Session["info"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (User)Session["info"];
            if (user.Role != "Student")
            {
                TempData["msg"] = "Only students can view enrollments.";
                return RedirectToAction("Dashboard", "Login");
            }

            var enrollments = db.Enrollments
                .Where(e => e.StudentId == user.UserId)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    StudentName = e.User.Name,
                    StudentEmail = e.User.Email,
                    CourseId = e.CourseId,
                    CourseName = e.Course.Name,
                    CourseDuration = e.Course.Duration,
                    InstructorName = e.Course.User.Name, // Assuming Course.User is the Instructor
                    //Progress = e.StudentProgresses.FirstOrDefault()?.Progress ?? 0, // If progress exists
                    //EnrollmentDate = e.EnrollmentDate
                })
                .ToList();

            return View(enrollments);
        }

        [HttpGet]
        public ActionResult CourseDetails(int courseId)
        {
            if (Session["info"] == null)
            {
                TempData["msg"] = "Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var user = (User)Session["info"];
            if (user.Role != "Student")
            {
                TempData["msg"] = "Only students can view course details.";
                return RedirectToAction("Dashboard", "Login");
            }

            var course = db.Courses.SingleOrDefault(c => c.CourseId == courseId);
            if (course == null)
            {
                TempData["msg"] = "Course not found.";
                return RedirectToAction("AvailableCourses");
            }

            var enrollments = db.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    StudentName = e.User.Name,
                    StudentEmail = e.User.Email,
                    CourseId = e.CourseId,
                    CourseName = e.Course.Name,
                    InstructorName = e.Course.User.Name,
                    CourseDuration = e.Course.Duration,
                    //Progress = e.StudentProgresses.FirstOrDefault()?.Progress ?? 0,
                    //EnrollmentDate = e.EnrollmentDate
                })
                .ToList();

            ViewBag.CourseName = course.Name;
            ViewBag.InstructorName = course.User.Name;
            ViewBag.Duration = course.Duration;

            return View(enrollments);
        }


       

        [HttpPost]
        public ActionResult Enroll(int courseId)
        {
            if (Session["info"] == null)
            {
                TempData["msg"] = "Please log in to enroll.";
                return RedirectToAction("Index", "Login");
            }

            var user = (User)Session["info"];
            if (user.Role != "Student")
            {
                TempData["msg"] = "Only students can enroll in courses.";
                return RedirectToAction("Dashboard", "Login");
            }

            var existingEnrollment = db.Enrollments
                .FirstOrDefault(e => e.StudentId == user.UserId && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                TempData["msg"] = "Already enrolled in this course.";
                return RedirectToAction("AvailableCourses");
            }

            try
            {
                db.Enrollments.Add(new Enrollment
                {
                    StudentId = user.UserId,
                    CourseId = courseId
                });

                db.SaveChanges();
                TempData["msg"] = "Successfully enrolled!";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error while enrolling: " + ex.Message;
            }

            return RedirectToAction("AvailableCourses");
        }



    }
}