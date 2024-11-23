using System;
using System.Linq;
using System.Web.Mvc;
using LearningManagement.DTOs;
using LearningManagement.EF;

namespace LearningManagement.Controllers
{
    public class AdminController : Controller
    {
        private LearningManagementEntities db = new LearningManagementEntities();

        // Conversion methods
        private static User ConvertToUser(UserDTO userDTO)
        {
            return new User
            {
                UserId = userDTO.UserId,
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = userDTO.Role
            };
        }

        private static UserDTO ConvertToUserDTO(User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
        }

        private static Course ConvertToCourse(CourseDTO courseDTO)
        {
            return new Course
            {
                CourseId = courseDTO.CourseId,
                Name = courseDTO.Name,
                InstructorId = courseDTO.InstructorId,
                Duration = courseDTO.Duration
            };
        }

        private static CourseDTO ConvertToCourseDTO(Course course)
        {
            return new CourseDTO
            {
                CourseId = course.CourseId,
                Name = course.Name,
                InstructorId = course.InstructorId,
                Duration = course.Duration
            };
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalCourses = db.Courses.Count();
            ViewBag.TotalEnrollments = db.Enrollments.Count();

            return View();
        }

        // USER MANAGEMENT SECTION

        // GET: Admin/ListUsers
        public ActionResult ListUsers()
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            var users = db.Users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            }).ToList();

            return View(users);
        }

        // GET: Admin/CreateUser
        [HttpGet]
        public ActionResult CreateUser()
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        public ActionResult CreateUser(UserDTO userDTO)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Validation
            if (string.IsNullOrEmpty(userDTO.Name) ||
                string.IsNullOrEmpty(userDTO.Email) ||
                string.IsNullOrEmpty(userDTO.Password) ||
                string.IsNullOrEmpty(userDTO.Role))
            {
                TempData["msg"] = "All fields are required.";
                ViewBag.Name = userDTO.Name;
                ViewBag.Email = userDTO.Email;
                ViewBag.Password = userDTO.Password;
                ViewBag.Role = userDTO.Role;

                return View(userDTO);
            }

            // Check if email already exists
            var emailExists = db.Users.Any(u => u.Email == userDTO.Email);
            if (emailExists)
            {
                TempData["msg"] = "Email already registered.";
                ViewBag.Name = userDTO.Name;
                ViewBag.Email = userDTO.Email;
                ViewBag.Password = userDTO.Password;
                ViewBag.Role = userDTO.Role;
                return View(userDTO);
            }

            // Create user
            var newUser = ConvertToUser(userDTO);
            db.Users.Add(newUser);
            db.SaveChanges();

            TempData["SuccessMsg"] = "User created successfully.";
            return RedirectToAction("ListUsers");
        }


        // Generic Edit method for User
        [HttpGet]
        public ActionResult EditUser(int? id)
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

            var user = db.Users.Find(id.Value);
            if (user == null)
            {
                TempData["msg"] = "User not found.";
                return RedirectToAction("ListUsers");
            }

            var userDTO = ConvertToUserDTO(user);
            return View(userDTO);
        }

        // Generic Edit POST method for User
        [HttpPost]
        public ActionResult EditUser(UserDTO userDTO)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Validation
            if (string.IsNullOrEmpty(userDTO.Name) ||
                string.IsNullOrEmpty(userDTO.Email) ||
                string.IsNullOrEmpty(userDTO.Role))
            {
                TempData["msg"] = "All fields are required.";
                return View(userDTO);
            }

            var existingUser = db.Users.Find(userDTO.UserId);
            if (existingUser == null)
            {
                TempData["msg"] = "User not found.";
                return RedirectToAction("ListUsers");
            }

            // Update user details
            existingUser.Name = userDTO.Name;
            existingUser.Email = userDTO.Email;
            existingUser.Role = userDTO.Role;



            db.SaveChanges();

            TempData["SuccessMsg"] = "User updated successfully.";
            return RedirectToAction("ListUsers");
        }

        //// Generic Delete method for User
        //[HttpPost]
        //public ActionResult DeleteUser(int? id)
        //{
        //    if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
        //    {
        //        TempData["msg"] = "Unauthorized access. Please log in as an admin.";
        //        return RedirectToAction("Index", "Login");
        //    }

        //    // Check if id is provided
        //    if (!id.HasValue)
        //    {
        //        TempData["msg"] = "Invalid user ID.";
        //        return RedirectToAction("ListUsers");
        //    }

        //    var user = db.Users.Find(id.Value);
        //    if (user == null)
        //    {
        //        TempData["msg"] = "User not found.";
        //        return RedirectToAction("ListUsers");
        //    }

        //    // Prevent deleting the current admin user
        //    var currentUser = Session["info"] as User;
        //    if (currentUser.UserId == user.UserId)
        //    {
        //        TempData["msg"] = "You cannot delete your own account.";
        //        return RedirectToAction("ListUsers");
        //    }

        //    // Remove related enrollments and courses
        //    var relatedEnrollments = db.Enrollments
        //        .Where(e => e.StudentId == id.Value || e.Course.InstructorId == id.Value)
        //        .ToList();
        //    db.Enrollments.RemoveRange(relatedEnrollments);

        //    var relatedCourses = db.Courses
        //        .Where(c => c.InstructorId == id.Value)
        //        .ToList();
        //    db.Courses.RemoveRange(relatedCourses);

        //    // Remove student progress records
        //    var relatedProgress = db.StudentProgresses
        //        .Where(p => p.StudentId == id.Value)
        //        .ToList();
        //    db.StudentProgresses.RemoveRange(relatedProgress);

        //    db.Users.Remove(user);
        //    db.SaveChanges();

        //    TempData["SuccessMsg"] = "User deleted successfully.";
        //    return RedirectToAction("ListUsers");
        //}


        // GET: Admin/DeleteUser
        [HttpGet]
        public ActionResult DeleteUser(int? id)
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

            var user = db.Users.Find(id.Value);
            if (user == null)
            {
                TempData["msg"] = "User not found.";
                return RedirectToAction("ListUsers");
            }

            // Pass user details to the view
            return View(user);
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        public ActionResult ConfirmDelete(int? id)
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

            var user = db.Users.Find(id.Value);
            if (user == null)
            {
                TempData["msg"] = "User not found.";
                return RedirectToAction("ListUsers");
            }

            // Prevent deleting the current admin user
            var currentUser = Session["info"] as User;
            if (currentUser.UserId == user.UserId)
            {
                TempData["msg"] = "You cannot delete your own account.";
                return RedirectToAction("ListUsers");
            }

            // Remove related enrollments, courses, and progress
            var relatedEnrollments = db.Enrollments
                .Where(e => e.StudentId == id.Value || e.Course.InstructorId == id.Value)
                .ToList();
            db.Enrollments.RemoveRange(relatedEnrollments);

            var relatedCourses = db.Courses
                .Where(c => c.InstructorId == id.Value)
                .ToList();
            db.Courses.RemoveRange(relatedCourses);

            var relatedProgress = db.StudentProgresses
                .Where(p => p.StudentId == id.Value)
                .ToList();
            db.StudentProgresses.RemoveRange(relatedProgress);

            db.Users.Remove(user);
            db.SaveChanges();

            TempData["SuccessMsg"] = "User deleted successfully.";
            return RedirectToAction("ListUsers");
        }


        // COURSE MANAGEMENT SECTION

        // Similar CRUD operations for Course will follow the same pattern as User CRUD
        // I'll implement those in the next iteration if you'd like

        // Add these methods to the existing AdminController

        // GET: Admin/ListCourses
        public ActionResult ListCourses()
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Use LINQ to get courses with instructor details
            var courses = db.Courses.Select(c => new CourseDTO
            {
                CourseId = c.CourseId,
                Name = c.Name,
                InstructorId = c.InstructorId,
                Duration = c.Duration
            }).ToList();

            return View(courses);
        }

        // GET: Admin/CreateCourse
        [HttpGet]
        public ActionResult CreateCourse()
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Get list of instructors for dropdown
            ViewBag.Instructors = db.Users
                .Where(u => u.Role == "Instructor")
                .Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = u.Name
                })
                .ToList();

            return View();
        }

        // POST: Admin/CreateCourse
        [HttpPost]
        public ActionResult CreateCourse(CourseDTO courseDTO)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            // Validation
            if (string.IsNullOrEmpty(courseDTO.Name) ||
                courseDTO.InstructorId == 0 ||
                courseDTO.Duration <= 0)
            {
                TempData["msg"] = "All fields are required.";
                return View(courseDTO);
            }

            // Check if instructor exists
            var instructorExists = db.Users
                .Any(u => u.UserId == courseDTO.InstructorId && u.Role == "Instructor");

            if (!instructorExists)
            {
                TempData["msg"] = "Invalid instructor selected.";
                return View(courseDTO);
            }

            // Check if course name already exists
            var courseExists = db.Courses
                .Any(c => c.Name == courseDTO.Name);

            if (courseExists)
            {
                TempData["msg"] = "A course with this name already exists.";
                return View(courseDTO);
            }

            // Create course
            var newCourse = ConvertToCourse(courseDTO);
            db.Courses.Add(newCourse);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Course created successfully.";
            return RedirectToAction("ListCourses");
        }

        

        [HttpGet]
        public ActionResult EditCourse(int? id)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            if (!id.HasValue)
            {
                TempData["msg"] = "Course ID is missing.";
                return RedirectToAction("ListCourses");
            }

            var course = db.Courses.Find(id.Value);
            if (course == null)
            {
                TempData["msg"] = "Course not found.";
                return RedirectToAction("ListCourses");
            }

            // Populate instructors dropdown
            ViewBag.Instructors = db.Users
                .Where(u => u.Role == "Instructor")
                .Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = u.Name,
                    Selected = u.UserId == course.InstructorId
                })
                .ToList();

            var courseDTO = ConvertToCourseDTO(course);
            return View(courseDTO);
        }

        [HttpPost]
        public ActionResult EditCourse(CourseDTO courseDTO)
        {
            if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            {
                TempData["msg"] = "Unauthorized access. Please log in as an admin.";
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid || courseDTO.CourseId <= 0)
            {
                TempData["msg"] = "Invalid data. Please check all required fields.";
                return View(courseDTO);
            }

            var existingCourse = db.Courses.Find(courseDTO.CourseId);
            if (existingCourse == null)
            {
                TempData["msg"] = "Course not found.";
                return RedirectToAction("ListCourses");
            }

            // Check if instructor exists
            var instructorExists = db.Users
                .Any(u => u.UserId == courseDTO.InstructorId && u.Role == "Instructor");

            if (!instructorExists)
            {
                TempData["msg"] = "Invalid instructor selected.";
                return View(courseDTO);
            }

            // Update course details
            existingCourse.Name = courseDTO.Name;
            existingCourse.InstructorId = courseDTO.InstructorId;
            existingCourse.Duration = courseDTO.Duration;

            db.SaveChanges();

            TempData["SuccessMsg"] = "Course updated successfully.";
            return RedirectToAction("ListCourses");
        }

        [HttpGet]
        public ActionResult DeleteCourse(int? id)
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

            var course = db.Courses.Find(id.Value);
            if (course == null)
            {
                TempData["msg"] = "Course not found.";
                return RedirectToAction("ListCourses");
            }

            // Pass user details to the view
            return View(course);

        }

        // POST: Admin/DeleteUser
        [HttpPost]
        public ActionResult ConfirmDeleteCourse(int? id)
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

            var course = db.Courses.Find(id.Value);
            if (course == null)
            {
                TempData["msg"] = "Courses not found.";
                return RedirectToAction("ListCourses");
            }


            //Remove related enrollments and progress records
            var relatedEnrollments = db.Enrollments.Where(e => e.CourseId == id.Value);
            db.Enrollments.RemoveRange(relatedEnrollments);

            var relatedProgress = db.StudentProgresses.Where(p => p.CourseId == id.Value);
            db.StudentProgresses.RemoveRange(relatedProgress);

            db.Courses.Remove(course);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Course deleted successfully.";
            return RedirectToAction("ListCourses");
        }



        //[HttpPost]
        //public ActionResult DeleteCourse(int? id)
        //{
        //    if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
        //    {
        //        TempData["msg"] = "Unauthorized access. Please log in as an admin.";
        //        return RedirectToAction("Index", "Login");
        //    }

        //    if (!id.HasValue)
        //    {
        //        TempData["msg"] = "Course ID is missing.";
        //        return RedirectToAction("ListCourses");
        //    }

        //    var course = db.Courses.Find(id.Value);
        //    if (course == null)
        //    {
        //        TempData["msg"] = "Course not found.";
        //        return RedirectToAction("ListCourses");
        //    }

        //    // Remove related enrollments and progress records
        //    var relatedEnrollments = db.Enrollments.Where(e => e.CourseId == id.Value);
        //    db.Enrollments.RemoveRange(relatedEnrollments);

        //    var relatedProgress = db.StudentProgresses.Where(p => p.CourseId == id.Value);
        //    db.StudentProgresses.RemoveRange(relatedProgress);

        //    db.Courses.Remove(course);
        //    db.SaveChanges();

        //    TempData["SuccessMsg"] = "Course deleted successfully.";
        //    return RedirectToAction("ListCourses");
        //}


        





    }
}