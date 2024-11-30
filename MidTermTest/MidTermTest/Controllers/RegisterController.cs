using MidTermTest.DTOs;
using MidTermTest.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidTermTest.Controllers
{
    public class RegisterController : Controller
    {
       MidTermTestEntities db = new MidTermTestEntities();


        // Convert RegisterDTO to User entity
        private static User Convert(RegisterDTO registerDTO)
        {
            return new User()
            {
                UserId = registerDTO.UserId,
                Name = registerDTO.Name,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Role = "Member" // Default role is Student
            };
        }

        // GET: Register/Create
        [HttpGet]
        public ActionResult Index()
        {
            return View(new RegisterDTO());
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Index(RegisterDTO registerDTO)
        {
            // Validate input fields
            if (string.IsNullOrEmpty(registerDTO.Name) || string.IsNullOrEmpty(registerDTO.Email) ||
                string.IsNullOrEmpty(registerDTO.Password))
            {
                TempData["msg"] = "All fields are required.";
                ViewBag.Name = registerDTO.Name;
                ViewBag.Email = registerDTO.Email;
                ViewBag.Password = registerDTO.Password;
                return View();
            }

            // Check if the email already exists using LINQ
            var isEmailExists = (from u in db.Users
                                 where u.Email == registerDTO.Email
                                 select u).Any();

            if (isEmailExists)
            {
                TempData["msg"] = "This email is already registered.";
                ViewBag.Name = registerDTO.Name;
                ViewBag.Email = registerDTO.Email;
                ViewBag.Password = registerDTO.Password;
                return View();
            }

            // Ensure the password is at least 6 characters long
            if (registerDTO.Password.Length < 6)
            {
                TempData["msg"] = "Password must be at least 6 characters long.";
                ViewBag.Name = registerDTO.Name;
                ViewBag.Email = registerDTO.Email;
                ViewBag.Password = registerDTO.Password;
                return View();
            }

            // Save the user to the database with default role "Student"
            var newUser = Convert(registerDTO);
            db.Users.Add(newUser);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Registration successful! You can now log in.";
            return RedirectToAction("Index", "Login");
        }

    }
}