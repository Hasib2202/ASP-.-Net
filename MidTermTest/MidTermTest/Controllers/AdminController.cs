using MidTermTest.DTOs;
using MidTermTest.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidTermTest.Controllers
{
    public class AdminController : Controller
    {

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


        public ActionResult Dashboard()
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            // Only count items belonging to the current user
            int userId = CurrentUserId;
            ViewBag.TotalUsers = db.Users.Count();
            //ViewBag.TotalUsers = db.Users.Count(c => c.UserId == userId);
            //ViewBag.TotalNotes = db.Notes.Count(n => n.Contact.UserId == userId);
            //ViewBag.ContactsByCategory = db.Contacts
            //    .Where(c => c.UserId == userId)
            //    .GroupBy(c => c.Category)
            //    .Select(g => new { Category = g.Key, Count = g.Count() })
            //    .ToDictionary(x => x.Category, x => x.Count);

            return View();
        }


        // GET: Admin/ListUsers
        [HttpGet]
        public ActionResult ListUsers()
        {
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
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
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        public ActionResult CreateUser(UserDTO userDTO)
        {
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
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
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
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
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
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

        // GET: Admin/DeleteUser
        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
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
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
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
            //var relatedEnrollments = db.Enrollments
            //    .Where(e => e.StudentId == id.Value || e.Course.InstructorId == id.Value)
            //    .ToList();
            //db.Enrollments.RemoveRange(relatedEnrollments);

            //var relatedCourses = db.Courses
            //    .Where(c => c.InstructorId == id.Value)
            //    .ToList();
            //db.Courses.RemoveRange(relatedCourses);

            //var relatedProgress = db.StudentProgresses
            //    .Where(p => p.StudentId == id.Value)
            //    .ToList();
            //db.StudentProgresses.RemoveRange(relatedProgress);

            db.Users.Remove(user);
            db.SaveChanges();

            TempData["SuccessMsg"] = "User deleted successfully.";
            return RedirectToAction("ListUsers");
        }

        // GET: Contact/Details/{id}
        [HttpGet]
        public ActionResult Details(int id)
        {
            //if (Session["info"] == null || (Session["info"] as User)?.Role != "Admin")
            //{
            //    TempData["msg"] = "Unauthorized access. Please log in as an admin.";
            //    return RedirectToAction("Index", "Login");
            //}

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            try
            {


                var data = db.Users.Find(id);
                //return View(Convert(data));
                

                if (data == null)
                {
                    TempData["msg"] = "Contact not found or access denied.";
                    return RedirectToAction("Index");
                }

                return View(ConvertToUserDTO(data));
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error retrieving user details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        // Conversion methods for DTO and Entity
        private static Member ConvertToMember(MemberDTO memberDTO)
        {
            return new Member
            {
                MemberId = memberDTO.MemberId,
                Name = memberDTO.Name,
                MemberType = memberDTO.MemberType,
                ExpiredDate = memberDTO.ExpiredDate,

                UserId = memberDTO.UserId
            };
        }

        private static MemberDTO ConvertToMemberDTO(Member member)
        {
            return new MemberDTO
            {
                MemberId = member.MemberId,
                Name = member.Name,
                MemberType = member.MemberType,
                ExpiredDate = member.ExpiredDate,
                UserId = member.UserId
            };
        }

        // Helper method to check if user is logged in
        //private bool IsLoggedIn()
        //{
        //    return Session["info"] != null;
        //}

        // Helper method to get current user
        private User GetCurrentUser()
        {
            return Session["info"] as User;
        }

        [HttpGet]
        public ActionResult ListMemberss()
        {
            var members = db.Members.ToList();
            return View(members);
        }


        //// Dispose method
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        // GET: Member/ListMembers
        //[HttpGet]
        //public ActionResult ListMembers()
        //{
        //    // Check login and admin/staff role
        //    if (!IsLoggedIn())
        //    {
        //        TempData["msg"] = "Unauthorized access. Please log in.";
        //        return RedirectToAction("Index", "Login");
        //    }

        //    var currentUser = GetCurrentUser();
        //    if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
        //    {
        //        TempData["msg"] = "You do not have permission to view members.";
        //        return RedirectToAction("Dashboard", "Home");
        //    }

        //    // Fetch all members with their associated user information
        //    var members = db.Members
        //        .Select(m => new MemberDTO
        //        {
        //            MemberId = m.MemberId,
        //            Name = m.Name,
        //            MemberType = m.MemberType,
        //            ExpiredDate = m.ExpiredDate,
        //            UserId = m.UserId,
        //            UserEmail = m.User.Email // Assuming there's a navigation property to User
        //        })
        //        .ToList();

        //    return View(members);
        //}

        [HttpGet]
        public ActionResult RenewMembership(int memberId)
        {
            var member = db.Members.FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            return View(member);  // Pass the member data to the view for renewal
        }

        [HttpPost]
        public ActionResult RenewMembership(int memberId, DateTime newExpiredDate)
        {
            var member = db.Members.FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            member.ExpiredDate = newExpiredDate;  // Update the expiration date
            db.SaveChanges();

            TempData["SuccessMsg"] = "Membership renewed successfully.";
            return RedirectToAction("ListMembers");
        }


        public ActionResult ListMembers()
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
            {
                TempData["msg"] = "You do not have permission to view members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Use LINQ to get courses with instructor details
            var members = db.Members
                .Select(m => new MemberDTO
                {
                    MemberId = m.MemberId,
                    Name = m.Name,
                    MemberType = m.MemberType,
                    ExpiredDate = m.ExpiredDate,
                    UserId = m.UserId,
                    UserEmail = m.User.Email // Assuming there's a navigation property to User
                })
                .ToList();

            return View(members);
        }


        [HttpGet]
        public ActionResult CreateMember()
        {
            // Check login and admin/staff role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
            {
                TempData["msg"] = "You do not have permission to create members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Populate User dropdown for selection
            ViewBag.Users = new SelectList(
                db.Users.Where(u => u.Role == "Member").ToList(),
                "UserId",    // Value field
                "Email"      // Text field to display
            );

            return View(new MemberDTO()); // Pass a new empty DTO
        }


        // POST: Member/CreateMember
        [HttpPost]
        public ActionResult CreateMember(MemberDTO memberDTO)
        {
            // Check login and admin/staff role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(
                    db.Users.Where(u => u.Role == "Member").ToList(),
                    "UserId",
                    "Email"
                );
                return View(memberDTO);
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
            {
                TempData["msg"] = "You do not have permission to create members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Validation
            if (string.IsNullOrEmpty(memberDTO.Name) ||
                string.IsNullOrEmpty(memberDTO.MemberType) ||
                memberDTO.UserId == 0 )
            {
                TempData["msg"] = "All fields are required.";
                return View(memberDTO);
            }

            // Check if member already exists for this user
            var existingMember = db.Members.Any(m => m.UserId == memberDTO.UserId);
            if (existingMember)
            {
                TempData["msg"] = "A membership already exists for this user.";
                return View(memberDTO);
            }

            // Create new member
            var newMember = ConvertToMember(memberDTO);
            db.Members.Add(newMember);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Member created successfully.";
            return RedirectToAction("ListMembers");
        }



        // GET: Member/EditMember/{id}
        [HttpGet]
        public ActionResult EditMember(int? id)
        {
            // Check login and admin/staff role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
            {
                TempData["msg"] = "You do not have permission to edit members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Validate ID
            if (!id.HasValue)
            {
                TempData["msg"] = "Invalid member ID.";
                return RedirectToAction("ListMembers");
            }

            var member = db.Members.Find(id.Value);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            var memberDTO = ConvertToMemberDTO(member);
            return View(memberDTO);
        }

        // POST: Member/EditMember
        [HttpPost]
        public ActionResult EditMember(MemberDTO memberDTO)
        {
            // Check login and admin/staff role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
            {
                TempData["msg"] = "You do not have permission to edit members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Validation
            if (string.IsNullOrEmpty(memberDTO.Name) ||
                string.IsNullOrEmpty(memberDTO.MemberType) ||
                memberDTO.ExpiredDate == null)
            {
                TempData["msg"] = "All fields are required.";
                return View(memberDTO);
            }

            var existingMember = db.Members.Find(memberDTO.MemberId);
            if (existingMember == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            // Update member details
            existingMember.Name = memberDTO.Name;
            existingMember.MemberType = memberDTO.MemberType;
            existingMember.ExpiredDate = memberDTO.ExpiredDate;

            db.SaveChanges();

            TempData["SuccessMsg"] = "Member updated successfully.";
            return RedirectToAction("ListMembers");
        }

        // GET: Member/DeleteMember/{id}
        [HttpGet]
        public ActionResult DeleteMember(int? id)
        {
            // Check login and admin/staff role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin")
            {
                TempData["msg"] = "Only admins can delete members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Validate ID
            if (!id.HasValue)
            {
                TempData["msg"] = "Invalid member ID.";
                return RedirectToAction("ListMembers");
            }

            var member = db.Members.Find(id.Value);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            return View(member);
        }

        // POST: Member/ConfirmDeleteMember
        [HttpPost]
        public ActionResult ConfirmDeleteMember(int? id)
        {
            // Check login and admin role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin")
            {
                TempData["msg"] = "Only admins can delete members.";
                return RedirectToAction("Dashboard", "Home");
            }

            // Validate ID
            if (!id.HasValue)
            {
                TempData["msg"] = "Invalid member ID.";
                return RedirectToAction("ListMembers");
            }

            var member = db.Members.Find(id.Value);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            // Remove the member
            db.Members.Remove(member);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Member deleted successfully.";
            return RedirectToAction("ListMembers");
        }

        // GET: Member/Details/{id}
        [HttpGet]
        public ActionResult MemberDetails(int id)
        {
            // Check login and admin/staff role
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role != "Admin" && currentUser.Role != "Staff")
            {
                TempData["msg"] = "You do not have permission to view member details.";
                return RedirectToAction("Dashboard", "Home");
            }

            var member = db.Members.Find(id);
            if (member == null)
            {
                TempData["msg"] = "Member not found.";
                return RedirectToAction("ListMembers");
            }

            return View(ConvertToMemberDTO(member));
        }




    }
}