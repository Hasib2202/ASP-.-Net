using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRPManagement.DTOs;
using TRPManagement.EF;

namespace TRPManagement.Controllers
{
    public class ChannelController : Controller
    {

        TRPManagementEntities db = new TRPManagementEntities();
        private const int PageSize = 5; // Pagination size


        public static Channel Convert(ChannelDTO p)
        {
            return new Channel()
            {
                ChannelId = p.ChannelId,
                ChannelName = p.ChannelName,
                EstablishedYear = p.EstablishedYear,
                Country = p.Country
            };
        }
        public static ChannelDTO Convert(Channel p)
        {
            return new ChannelDTO()
            {
                ChannelId = p.ChannelId,
                ChannelName = p.ChannelName,
                EstablishedYear = p.EstablishedYear,
                Country = p.Country
            };
        }

        public static List<ChannelDTO> Convert(List<Channel> data)
        {
            var list = new List<ChannelDTO>();
            foreach (var p in data)
            {
                list.Add(Convert(p));
            }
            return list;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ChannelDTO());
        }

        [HttpPost]
        public ActionResult Create(ChannelDTO model)
        {
            // Check for existing channel name
            if (db.Channels.Any(c => c.ChannelName == model.ChannelName))
            {
                ModelState.AddModelError("ChannelName", "This Channel Name is already taken.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var data = new Channel()
                {
                    ChannelName = model.ChannelName,
                    EstablishedYear = model.EstablishedYear,
                    Country = model.Country
                };

                try
                {
                    db.Channels.Add(data);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Channel added successfully.";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the channel: " + ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult List(int page = 1)
        {
            var totalChannels = db.Channels.Count();
            var channels = db.Channels
                .OrderBy(c => c.ChannelId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var channelDTOs = Convert(channels);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalChannels / PageSize);

            return View(channelDTOs);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = db.Channels.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            var channelDTO = Convert(data);
            return View(channelDTO);
        }

        [HttpPost]
        public ActionResult Edit(ChannelDTO c)
        {
            // Check for existing channel name (excluding current channel)
            if (db.Channels.Any(ch => ch.ChannelName == c.ChannelName && ch.ChannelId != c.ChannelId))
            {
                ModelState.AddModelError("ChannelName", "This Channel Name is already taken.");
                return View(c);
            }

            if (ModelState.IsValid)
            {
                var data = db.Channels.Find(c.ChannelId);
                if (data != null)
                {
                    data.ChannelName = c.ChannelName;
                    data.EstablishedYear = c.EstablishedYear;
                    data.Country = c.Country;

                    try
                    {
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Channel updated successfully.";
                        return RedirectToAction("List");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while updating the channel: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Channel not found.");
                }
            }
            return View(c);
        }

        public ActionResult Details(int id)
        {
            var data = db.Channels.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(Convert(data));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }

            // Check if the channel has associated programs
            if (channel.Programs.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete the channel as it has associated programs.";
                return RedirectToAction("List");
            }

            var channelDTO = Convert(channel);
            return View(channelDTO);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var channel = db.Channels.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }

            // Check again before deleting to avoid unexpected issues
            if (channel.Programs.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete the channel as it has associated programs.";
                return RedirectToAction("List");
            }

            try
            {
                db.Channels.Remove(channel);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Channel deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the channel: " + ex.Message;
            }

            return RedirectToAction("List");
        }











    }

}
