using DTO.DTOs;
using DTO.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTO.Controllers
{
    public class LoginController : Controller
    {

        ASPEntities1 db = new ASPEntities1();


        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



        // Post: Login
        [HttpPost]
        public ActionResult Login(LoginDTO log)
        {
            // Add validation here

            var user = (from u in db.Users
                        where u.uname.Equals(log.uname) &&
                        u.password.Equals(log.password)
                        select u).SingleOrDefault();
            if (user != null)
            {
                Session["user"] = user; //boxing
                return RedirectToAction("List", "Department");
            }
            TempData["Msg"] = "User not found";
            return View();
        }
    }
}