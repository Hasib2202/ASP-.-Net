using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRPManagement.DTOs;
using TRPManagement.EF;

namespace TRPManagement.Controllers
{
    public class ProgramController : Controller
    {
        private TRPManagementEntities db = new TRPManagementEntities();
        private const int PageSize = 5;

        // Conversion methods
        public static Program Convert(ProgramDTO dto)
        {
            return new Program
            {
                ProgramId = dto.ProgramId,
                ProgramName = dto.ProgramName,
                TRPScore = dto.TRPScore,
                ChannelId = dto.ChannelId,
                AirTime = dto.AirTime,
                //AirTime = TimeSpan.Parse(dto.AirTime) // Convert string to TimeSpan
            };
        }

        public static ProgramDTO Convert(Program entity)
        {
            return new ProgramDTO
            {
                ProgramId = entity.ProgramId,
                ProgramName = entity.ProgramName,
                TRPScore = entity.TRPScore,
                ChannelId = entity.ChannelId,
                AirTime = entity.AirTime,
                //AirTime = entity.AirTime.ToString(@"hh\:mm\:ss"),
                ChannelName = entity.Channel.ChannelName
            };
        }

        public static List<ProgramDTO> Convert(List<Program> data)
        {
            var list = new List<ProgramDTO>();
            foreach (var p in data)
            {
                list.Add(Convert(p));
            }
            return list;
        }

        // GET: Program/Create
        public ActionResult Create()
        {
            // Populate Channel Dropdown
            ViewBag.Channels = new SelectList(db.Channels, "ChannelId", "ChannelName");
            return View(new ProgramDTO());
        }

        [HttpPost]
        public ActionResult Create(ProgramDTO model)
        {
            // Check for unique program name within the channel
            bool programExists = db.Programs.Any(p =>
                p.ProgramName == model.ProgramName &&
                p.ChannelId == model.ChannelId);

            if (programExists)
            {
                ModelState.AddModelError("ProgramName", "Program Name must be unique within the channel.");
            }

            if (ModelState.IsValid)
            {
                var program = Convert(model);

                try
                {
                    db.Programs.Add(program);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Program added successfully.";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving program: " + ex.Message);
                }
            }

            // Repopulate Channel Dropdown
            ViewBag.Channels = new SelectList(db.Channels, "ChannelId", "ChannelName", model.ChannelId);
            return View(model);
        }

        // GET: Program/List
        public ActionResult List(int page = 1, string searchTerm = "", int? channelId = null, decimal? minTRPScore = null)
        {
            // LINQ query for filtering and searching
            var query = db.Programs.AsQueryable();

            // Search by name or filter by channel or TRP score
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProgramName.Contains(searchTerm));
            }

            if (channelId.HasValue)
            {
                query = query.Where(p => p.ChannelId == channelId.Value);
            }

            if (minTRPScore.HasValue)
            {
                query = query.Where(p => p.TRPScore >= minTRPScore.Value);
            }

            // Group by channel
            var programsGrouped = query
                .OrderBy(p => p.Channel.ChannelName)
                .ThenBy(p => p.ProgramName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(Convert)
                .ToList();

            // Pagination and filter ViewBags
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);
            ViewBag.Channels = new SelectList(db.Channels, "ChannelId", "ChannelName", channelId);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedChannelId = channelId;
            ViewBag.MinTRPScore = minTRPScore;

            return View(programsGrouped);
        }






        [HttpGet]
        public ActionResult Edit(int id)
        {
            var program = db.Programs.Find(id);
            if (program == null)
            {
                return HttpNotFound("Program not found.");
            }

            // Convert entity to DTO
            var model = Convert(program);

            // Populate Channels dropdown
            ViewBag.Channels = new SelectList(db.Channels, "ChannelId", "ChannelName", model.ChannelId);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProgramDTO model)
        {
            if (ModelState.IsValid)
            {
                var program = db.Programs.Find(model.ProgramId);

                if (program == null)
                {
                    return HttpNotFound("Program not found.");
                }

                // Check for unique program name within the same channel
                bool programExists = db.Programs.Any(p =>
                    p.ProgramId != model.ProgramId &&
                    p.ProgramName == model.ProgramName &&
                    p.ChannelId == model.ChannelId);

                if (programExists)
                {
                    ModelState.AddModelError("ProgramName", "Program Name must be unique within the channel.");
                    ViewBag.Channels = new SelectList(db.Channels, "ChannelId", "ChannelName", model.ChannelId);
                    return View(model);
                }

                // Update fields
                program.ProgramName = model.ProgramName;
                program.TRPScore = model.TRPScore;
                program.ChannelId = model.ChannelId;
                program.AirTime = model.AirTime;

                try
                {
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Program updated successfully.";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating program: " + ex.Message);
                }
            }

            // Repopulate Channels dropdown on validation error
            ViewBag.Channels = new SelectList(db.Channels, "ChannelId", "ChannelName", model.ChannelId);
            return View(model);
        }



        public ActionResult Details(int id)
        {
            var data = db.Programs.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(Convert(data));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.Programs.Find(id);
            if (data == null)
            {
                TempData["ErrorMessage"] = "Program not found.";
                return RedirectToAction("List");
            }

            return View(Convert(data));
        }


        [HttpPost]
        public ActionResult Delete(int id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var data = db.Programs.Find(id);
                if (data == null)
                {
                    TempData["ErrorMessage"] = "Program not found.";
                    return RedirectToAction("List");
                }

                try
                {
                    db.Programs.Remove(data);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Program deleted successfully.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the program: " + ex.Message;
                }
            }
            return RedirectToAction("List");
        }


    }
}