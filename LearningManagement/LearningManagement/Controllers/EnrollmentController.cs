using LearningManagement.DTOs;
using LearningManagement.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LearningManagement.Controllers
{
    public class EnrollmentController : Controller
    {
        // Database connection
        LearningManagementEntities db = new LearningManagementEntities();


        public ActionResult ListEnrollment()
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Fetch enrollments with detailed information using LINQ
            var enrollmentList = from e in db.Enrollments
                                 join s in db.Users on e.StudentId equals s.UserId
                                 join c in db.Courses on e.CourseId equals c.CourseId
                                 join i in db.Users on c.InstructorId equals i.UserId
                                 select new EnrollmentDTO
                                 {
                                     EnrollmentId = e.EnrollmentId,
                                     StudentId = e.StudentId,
                                     CourseId = e.CourseId,
                                     StudentName = s.Name,
                                     StudentEmail = s.Email,
                                     CourseName = c.Name,
                                     CourseDuration = c.Duration,
                                     InstructorName = i.Name
                                 };

            return View(enrollmentList.ToList());
        }


        // Filter Enrollments
        public ActionResult FilterEnrollments(int? studentId, int? courseId)
        {
            // Admin authorization check
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Base query for enrollments
            var query = from e in db.Enrollments
                        join s in db.Users on e.StudentId equals s.UserId
                        join c in db.Courses on e.CourseId equals c.CourseId
                        join i in db.Users on c.InstructorId equals i.UserId
                        join p in db.StudentProgresses on new { e.StudentId, e.CourseId }
                            equals new { StudentId = p.StudentId, CourseId = p.CourseId } into progressGroup
                        from progress in progressGroup.DefaultIfEmpty()
                        select new EnrollmentDTO
                        {
                            EnrollmentId = e.EnrollmentId,
                            StudentId = e.StudentId,
                            CourseId = e.CourseId,
                            StudentName = s.Name,
                            StudentEmail = s.Email,
                            CourseName = c.Name,
                            CourseDuration = c.Duration,
                            InstructorName = i.Name,
                            Progress = progress != null ? progress.Progress : 0
                        };

            // Apply filters
            if (studentId.HasValue)
                query = query.Where(e => e.StudentId == studentId);

            if (courseId.HasValue)
                query = query.Where(e => e.CourseId == courseId);

            // Populate dropdown lists for filtering
            ViewBag.Students = new SelectList(
                db.Users.Where(u => u.Role == "Student"),
                "UserId", "Name");

            ViewBag.Courses = new SelectList(
                db.Courses,
                "CourseId", "Name");

            return View(query.ToList());
        }

        // Student Progress Report
        public ActionResult StudentProgressReport()
        {
            // Admin authorization check
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Detailed progress query
            var progressReport = from p in db.StudentProgresses
                                 join s in db.Users on p.StudentId equals s.UserId
                                 join c in db.Courses on p.CourseId equals c.CourseId
                                 select new StudentProgressDTO
                                 {
                                     StudentId = s.UserId,
                                     StudentName = s.Name,
                                     CourseName = c.Name,
                                     Progress = p.Progress
                                 };

            return View(progressReport.ToList());
        }

        // Export Progress as CSV (Basic implementation)
        public ActionResult ExportProgressReport()
        {
            var progressReport = from p in db.StudentProgresses
                                 join s in db.Users on p.StudentId equals s.UserId
                                 join c in db.Courses on p.CourseId equals c.CourseId
                                 select new
                                 {
                                     StudentName = s.Name,
                                     CourseName = c.Name,
                                     Progress = p.Progress
                                 };

            // Convert to CSV
            var csv = new StringBuilder();
            csv.AppendLine("Student Name,Course Name,Progress");
            foreach (var item in progressReport)
            {
                csv.AppendLine($"{item.StudentName},{item.CourseName},{item.Progress}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()),
                "text/csv",
                "StudentProgressReport.csv");

        }

        [HttpGet]
        public ActionResult DeleteEnrollment(int? id)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Check if id is provided
            if (!id.HasValue)
            {
                TempData["msg"] = "Invalid user ID.";
                return RedirectToAction("ListUsers");
            }

            var enrollment = db.Enrollments.Find(id.Value);
            if (enrollment == null)
            {
                TempData["msg"] = "Enrollment not found.";
                return RedirectToAction("ListEnrollment");
            }

            // Pass user details to the view
            return View(enrollment);

        }

        // POST: Admin/DeleteUser
        [HttpPost]
        public ActionResult ConfirmEnrollment(int? id)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Check if id is provided
            if (!id.HasValue)
            {
                TempData["msg"] = "Invalid user ID.";
                return RedirectToAction("ListUsers");
            }

            var enrollment = db.Enrollments.Find(id.Value);
            if (enrollment == null)
            {
                TempData["msg"] = "Courses not found.";
                return RedirectToAction("ListEnrollment");
            }


            //Remove related enrollments and progress records
            var relatedEnrollments = db.Enrollments.Where(e => e.CourseId == id.Value);
            db.Enrollments.RemoveRange(relatedEnrollments);

            var relatedProgress = db.StudentProgresses.Where(p => p.CourseId == id.Value);
            db.StudentProgresses.RemoveRange(relatedProgress);

            db.Enrollments.Remove(enrollment);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Enrollments deleted successfully.";
            return RedirectToAction("ListEnrollment");
        }


    }
}