using Microsoft.Ajax.Utilities;
using MidTermTest.DTOs;
using MidTermTest.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MidTermTest.Controllers
{
    public class LoginController : Controller
    {
        MidTermTestEntities db = new MidTermTestEntities();

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            TempData["logout"] = "You have been successfully logged out.";
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Check if TempData["logout"] is set
            if (TempData["logout"] != null)
            {
                ViewBag.LogoutMessage = TempData["logout"];
            }
            else
            {
                ViewBag.LogoutMessage = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginDTO user)
        {

            // Validate the email and password
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                TempData["msg"] = "Email and Password are required.";
                ViewBag.Email = user.Email;  // Store entered email in ViewBag
                ViewBag.Password = user.Password;  // Store entered password in ViewBag
                return View();
            }

            // Validate email format
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(user.Email))
            {
                TempData["msg"] = "Please enter a valid email address.";
                ViewBag.Email = user.Email;
                ViewBag.Password = user.Password;
                return View();
            }

            // Check if the user exists in the database
            var info = (from u in db.Users
                        where u.Email.Equals(user.Email) && u.Password.Equals(user.Password)
                        select u).SingleOrDefault();

            // If user is found
            if (info != null)
            {
                // Store user session
                Session["info"] = info;

                // Fetch the user's name
                string userName = info.Name;  // This gets the user's name from the User table

                // You can use the userName variable to pass the name to the view or for other logic
                // For example, passing it to ViewBag to display it in the view
                ViewBag.UserName = userName;

                // Redirect based on the user's role
                if (info.Role == "Admin")
                {
                    TempData["SuccessMsg"] = "Welcome Admin!";
                    return RedirectToAction("Dashboard", "Admin");  // Admin dashboard
                }

                //Defaut role = Member[Role]
                else if (info.Role == "Member")
                {
                    TempData["SuccessMsg"] = "Welcome Member!";
                    return RedirectToAction("Dashboard", "Member");  // Student dashboard
                }
                else if (info.Role == "Staff")
                {
                    TempData["SuccessMsg"] = "Welcome Staff!";
                    return RedirectToAction("Dashboard", "Staff");  // Instructor dashboard
                }
                else
                {
                    // If the role is unrecognized, return to the login page
                    TempData["msg"] = "Unknown user role.";
                    return View();
                }
            }
            else
            {
                // If login fails, store the error message in TempData
                TempData["msg"] = "Invalid Email or Password";
                ViewBag.Email = user.Email;
                ViewBag.Password = user.Password;
                return View();
            }
        }
    }
           
}
