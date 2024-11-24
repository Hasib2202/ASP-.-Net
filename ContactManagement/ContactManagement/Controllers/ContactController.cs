using ContactManagement.DTOs;
using ContactManagement.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ContactManagement.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactManagementEntities db = new ContactManagementEntities();

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
            int userId = CurrentUserId;
            ViewBag.TotalContacts = db.Contacts.Count(c => c.UserId == userId);
            ViewBag.TotalNotes = db.Notes.Count(n => n.Contact.UserId == userId);
            ViewBag.ContactsByCategory = db.Contacts
                .Where(c => c.UserId == userId)
                .GroupBy(c => c.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Category, x => x.Count);

            return View();
        }

        // GET: Contact/Index
        [HttpGet]
        public ActionResult Index(string searchTerm = "", string category = "", string sortBy = "name")
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var userId = CurrentUserId;
                var query = db.Contacts.Where(c => c.UserId == userId);

                // Apply search filters
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(c =>
                        c.Name.Contains(searchTerm) ||
                        c.Email.Contains(searchTerm) ||
                        c.Phone.Contains(searchTerm)
                    );
                }

                // Apply category filter
                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(c => c.Category == category);
                }

                // Apply sorting
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(c => c.Name);
                        break;
                    case "email":
                        query = query.OrderBy(c => c.Email);
                        break;
                    case "category":
                        query = query.OrderBy(c => c.Category);
                        break;
                    case "birthday":
                        query = query.OrderBy(c => c.Birthday);
                        break;
                }

                var contacts = query.ToList();

                // Store search parameters for the view
                ViewBag.SearchTerm = searchTerm;
                ViewBag.Category = category;
                ViewBag.SortBy = sortBy;
                ViewBag.Categories = db.Contacts
                    .Where(c => c.UserId == userId)
                    .Select(c => c.Category)
                    .Distinct()
                    .ToList();

                return View(contacts);
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error retrieving contacts: " + ex.Message;
                return View(new List<Contact>());
            }
        }

        // GET: Contact/Details/{id}
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var contact = db.Contacts
                    .Include(c => c.Notes)
                    .FirstOrDefault(c => c.ContactId == id && c.UserId == CurrentUserId);

                if (contact == null)
                {
                    TempData["msg"] = "Contact not found or access denied.";
                    return RedirectToAction("Index");
                }

                return View(contact);
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error retrieving contact details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Contact/Create
        [HttpGet]
        public ActionResult Create()
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Categories = GetCategorySelectList();
            return View(new AddContactDTO());
        }

        // POST: Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddContactDTO dto)
        {

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategorySelectList(dto.Category); // Pass the selected category
                return View(dto);
            }

            try
            {
                // Check for duplicate email within user's contacts
                if (!string.IsNullOrEmpty(dto.Email) &&
                    db.Contacts.Any(c => c.UserId == CurrentUserId && c.Email == dto.Email))
                {
                    ModelState.AddModelError("Email", "You already have a contact with this email address.");
                    ViewBag.Categories = GetCategorySelectList();
                    return View(dto);
                }

                var contact = new Contact
                {
                    UserId = CurrentUserId,
                    Name = dto.Name.Trim(),
                    Email = dto.Email?.Trim(),
                    Phone = dto.Phone?.Trim(),
                    Address = dto.Address?.Trim(),
                    Category = dto.Category,
                    Birthday = dto.Birthday,
                    //CreatedAt = DateTime.Now
                };

                db.Contacts.Add(contact);
                db.SaveChanges();

                TempData["SuccessMsg"] = "Contact created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error creating contact: " + ex.Message;
                ViewBag.Categories = GetCategorySelectList();
                return View(dto);
            }
        }

        // GET: Contact/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var contact = db.Contacts.FirstOrDefault(c => c.ContactId == id && c.UserId == CurrentUserId);

                if (contact == null)
                {
                    TempData["msg"] = "Contact not found or access denied.";
                    return RedirectToAction("Index");
                }

                var dto = new AddContactDTO
                {
                    ContactId = contact.ContactId,
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Address = contact.Address,
                    Category = contact.Category,
                    Birthday = contact.Birthday

                };

                ViewBag.Categories = GetCategorySelectList(contact.Category);
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error retrieving contact: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Contact/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddContactDTO dto)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetCategorySelectList(dto.Category);
                return View(dto);
            }

            try
            {
                var contact = db.Contacts.FirstOrDefault(c =>
                    c.ContactId == dto.ContactId && c.UserId == CurrentUserId);

                if (contact == null)
                {
                    TempData["msg"] = "Contact not found or access denied.";
                    return RedirectToAction("Index");
                }

                // Check for duplicate email, excluding current contact
                if (!string.IsNullOrEmpty(dto.Email) &&
                    db.Contacts.Any(c => c.UserId == CurrentUserId &&
                                       c.Email == dto.Email &&
                                       c.ContactId != dto.ContactId))
                {
                    ModelState.AddModelError("Email", "You already have another contact with this email address.");
                    ViewBag.Categories = GetCategorySelectList(dto.Category);
                    return View(dto);
                }

                contact.Name = dto.Name.Trim();
                contact.Email = dto.Email?.Trim();
                contact.Phone = dto.Phone?.Trim();
                contact.Address = dto.Address?.Trim();
                contact.Category = dto.Category;
                contact.Birthday = dto.Birthday;
                //contact.LastUpdated = DateTime.Now;

                db.SaveChanges();

                TempData["SuccessMsg"] = "Contact updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error updating contact: " + ex.Message;
                ViewBag.Categories = GetCategorySelectList(dto.Category);
                return View(dto);
            }
        }

        // POST: Contact/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var contact = db.Contacts
                    .Include(c => c.Notes)
                    .FirstOrDefault(c => c.ContactId == id && c.UserId == CurrentUserId);

                if (contact == null)
                {
                    TempData["msg"] = "Contact not found or access denied.";
                    return RedirectToAction("Index");
                }

                // Remove associated notes first
                db.Notes.RemoveRange(contact.Notes);
                db.Contacts.Remove(contact);
                db.SaveChanges();

                TempData["SuccessMsg"] = "Contact and associated notes deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error deleting contact: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        private SelectList GetCategorySelectList(string selectedCategory = null)
        {
            var categories = new[]
            {
                new { Value = "Family", Text = "Family" },
                new { Value = "Friend", Text = "Friend" },
                new { Value = "Work", Text = "Work" }
            };

            return new SelectList(categories, "Value", "Text", selectedCategory);
        }


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
    }
}