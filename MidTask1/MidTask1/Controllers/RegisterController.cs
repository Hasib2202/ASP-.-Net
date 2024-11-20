using MidTask1.DTOs;
using MidTask1.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MidTask1.Controllers
{
    public class RegisterController : Controller
    {
        // Database context
        private readonly ASP1Entities db = new ASP1Entities();

        // Convert RegisterDTO to User entity
        private static User Convert(RegisterDTO registerDTO)
        {
            return new User()
            {
                ID = registerDTO.ID,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Password = registerDTO.Password,
                Email = registerDTO.Email,
                Phone = registerDTO.Phone,
                Role = registerDTO.Role,
                Gender = registerDTO.Gender,
            };
        }

        // GET: Register/Create
        [HttpGet]
        public ActionResult Create()
        {
            // Return an empty form
            return View(new RegisterDTO());
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error}");
                }
                return View(registerDTO);
            }

            try
            {
                // Convert DTO to entity
                var user = Convert(registerDTO);

                // Add user to the database
                db.Users.Add(user);

                // Save changes to the database
                db.SaveChanges();

                // Redirect to the login page
                TempData["SuccessMessage"] = "User registered successfully!";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                // Log exception details for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                // Show a user-friendly error message
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again!");
                return View(registerDTO);
            }
        }
    }
}
