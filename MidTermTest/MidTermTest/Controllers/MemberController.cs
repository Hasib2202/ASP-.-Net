using MidTermTest.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidTermTest.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member

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

        [HttpPost]
        public ActionResult ToggleMembershipStatus(int memberId)
        {
            var member = db.Members.FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            member.IsActive = !member.IsActive;  // Toggle the active status
            db.SaveChanges();

            TempData["SuccessMsg"] = "Membership status updated.";
            return RedirectToAction("ListMembers");
        }

    }
}