using ContactManagement.DTOs;
using ContactManagement.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ContactManagement.Controllers
{
    public class NoteController : Controller
    {
        private readonly ContactManagementEntities db = new ContactManagementEntities();

        // Converts Note entity to AddNoteDTO
        private static AddNoteDTO ConvertToDTO(Note note)
        {
            return new AddNoteDTO
            {
                NoteId = note.NoteId,
                ContactId = note.ContactId,
                Content = note.Content,
                CreatedAt = note.CreatedAt
            };
        }

        // Converts AddNoteDTO to Note entity
        private static Note ConvertToEntity(AddNoteDTO dto)
        {
            return new Note
            {
                NoteId = dto.NoteId,
                ContactId = dto.ContactId,
                Content = dto.Content,
                CreatedAt = dto.CreatedAt
            };
        }

        // Verifies if a user is logged in
        private bool IsLoggedIn()
        {
            return Session["info"] != null;
        }

        // Checks if the logged-in user owns the contact
        private bool IsAuthorized(int contactId)
        {
            var userId = ((User)Session["info"])?.UserId;
            return db.Contacts.Any(c => c.ContactId == contactId && c.UserId == userId);
        }

        [HttpGet]
        public ActionResult Create(int contactId)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            if (!IsAuthorized(contactId))
            {
                TempData["msg"] = "Access denied. You do not own this contact.";
                return RedirectToAction("Index", "Contact");
            }

            var note = new AddNoteDTO { ContactId = contactId };
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddNoteDTO note)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            if (!IsAuthorized(note.ContactId))
            {
                TempData["msg"] = "Access denied. You do not own this contact.";
                return RedirectToAction("Index", "Contact");
            }

            if (ModelState.IsValid)
            {
                note.CreatedAt = DateTime.Now;
                db.Notes.Add(ConvertToEntity(note));
                db.SaveChanges();

                TempData["SuccessMsg"] = "Note added successfully.";
                return RedirectToAction("Index", "Contact", new { id = note.ContactId });
            }

            TempData["msg"] = "Error adding note. Please check the input and try again.";
            return View(note);
        }

        [HttpGet]
        public ActionResult Edit(int noteId)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            var note = db.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null || !IsAuthorized(note.ContactId))
            {
                TempData["msg"] = "Access denied. You do not own this note.";
                return RedirectToAction("Index", "Contact");
            }

            return View(ConvertToDTO(note));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddNoteDTO note)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            if (!IsAuthorized(note.ContactId))
            {
                TempData["msg"] = "Access denied. You do not own this note.";
                return RedirectToAction("Index", "Contact");
            }

            if (ModelState.IsValid)
            {
                var dbNote = db.Notes.FirstOrDefault(n => n.NoteId == note.NoteId);
                if (dbNote == null)
                {
                    TempData["msg"] = "Note not found.";
                    return RedirectToAction("Index", "Contact");
                }

                dbNote.Content = note.Content;
                db.SaveChanges();

                TempData["SuccessMsg"] = "Note updated successfully.";
                return RedirectToAction("Index", "Contact", new { id = note.ContactId });
            }

            TempData["msg"] = "Error updating note. Please check the input and try again.";
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int noteId)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            var note = db.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null || !IsAuthorized(note.ContactId))
            {
                TempData["msg"] = "Access denied. You do not own this note.";
                return RedirectToAction("Index", "Contact");
            }

            db.Notes.Remove(note);
            db.SaveChanges();

            TempData["SuccessMsg"] = "Note deleted successfully.";
            return RedirectToAction("Index", "Contact", new { id = note.ContactId });
        }

        [HttpGet]
        public ActionResult Index(int? contactId)
        {
            if (!contactId.HasValue)
            {
                TempData["msg"] = "Contact ID is required to view notes.";
                return RedirectToAction("Index", "Contact"); // Redirect to a relevant page
            }

            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            if (!IsAuthorized(contactId.Value))
            {
                TempData["msg"] = "Access denied. You do not own this contact.";
                return RedirectToAction("Index", "Note");
            }

            var notes = db.Notes
                .Where(n => n.ContactId == contactId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .ToList()
                .Select(n => ConvertToDTO(n));

            ViewBag.ContactId = contactId.Value;
            ViewBag.ContactName = db.Contacts
                .Where(c => c.ContactId == contactId.Value)
                .Select(c => c.Name)
                .FirstOrDefault();

            return View(notes);
        }

        [HttpGet]
        public ActionResult Details(int noteId)
        {
            if (!IsLoggedIn())
            {
                TempData["msg"] = "Unauthorized access. Please log in.";
                return RedirectToAction("Login", "User");
            }

            var note = db.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null || !IsAuthorized(note.ContactId))
            {
                TempData["msg"] = "Access denied. You do not own this note.";
                return RedirectToAction("Index", "Contact");
            }

            return View(ConvertToDTO(note));

        }

    }
}
