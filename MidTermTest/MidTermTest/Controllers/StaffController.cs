using MidTermTest.DTOs;
using MidTermTest.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidTermTest.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        MidTermTestEntities db = new MidTermTestEntities();
        private bool IsLoggedIn()
        {
            return Session["info"] != null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private int CurrentUserId
        {
            get
            {
                var userInfo = Session["info"] as User;
                return userInfo?.UserId ?? 0;
            }
        }
        public ActionResult Dashboard()
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            // Only count items belonging to the current user
            //int userId = CurrentUserId;
            //ViewBag.TotalUsers = db.Users.Count();
            //ViewBag.TotalUsers = db.Users.Count(c => c.UserId == userId);
            //ViewBag.TotalNotes = db.Notes.Count(n => n.Contact.UserId == userId);
            //ViewBag.ContactsByCategory = db.Contacts
            //    .Where(c => c.UserId == userId)
            //    .GroupBy(c => c.Category)
            //    .Select(g => new { Category = g.Key, Count = g.Count() })
            //    .ToDictionary(x => x.Category, x => x.Count);

            return View();
        }

        [HttpGet]
        public ActionResult RegisterMember()
        {
            return View(new MemberDTO());  // Pass an empty DTO for new member registration
        }

        [HttpPost]
        public ActionResult RegisterMember(MemberDTO memberDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(memberDTO);  // Return the form with validation messages
            }

            var newMember = ConvertToMember(memberDTO);
            db.Members.Add(newMember);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Member registered successfully.";
            return RedirectToAction("ListMembers");
        }


        [HttpGet]
        public ActionResult RenewMembershipForStaff(int memberId)
        {
            var member = db.Members.FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult RenewMembershipForStaff(int memberId, DateTime newExpiredDate)
        {
            var member = db.Members.FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            member.ExpiredDate = newExpiredDate;
            db.SaveChanges();

            TempData["SuccessMsg"] = "Membership renewed successfully.";
            return RedirectToAction("ListMembers");
        }


    }
}