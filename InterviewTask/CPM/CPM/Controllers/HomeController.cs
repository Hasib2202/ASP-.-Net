using CPM.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CPM.ViewModels;
using Newtonsoft.Json;

namespace CPM.Controllers
{
    public class HomeController : Controller
    {
        private readonly GTaskEntities _db = new GTaskEntities();

        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/GetCustomers
        [HttpGet]
        public JsonResult GetCustomers(string customerType)
        {
            if (string.IsNullOrEmpty(customerType))
            {
                return Json(new { error = "Customer type is required." }, JsonRequestBehavior.AllowGet);
            }

            if (customerType == "Corporate")
            {
                // Fetch corporate customers from Corporate_Customer_Tbl
                var corporateCustomers = _db.Corporate_Customer_Tbl
                    .Select(c => new
                    {
                        c.CorporateCustomerID,
                        c.ContactPerson
                    }).ToList();

                return Json(corporateCustomers, JsonRequestBehavior.AllowGet);
            }
            else if (customerType == "Individual")
            {
                // Fetch individual customers from Individual_Customer_Tbl
                var individualCustomers = _db.Individual_Customer_Tbl
                    .Select(i => new
                    {
                        i.IndividualCustomerID,
                        i.FullName
                    }).ToList();

                return Json(individualCustomers, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error = "Invalid customer type." }, JsonRequestBehavior.AllowGet);
        }

        // Action to get products/services from the database
        [HttpGet]
        public JsonResult GetProductsService()
        {
            var products = _db.Products_Service_Tbl
                .Select(p => new
                {
                    p.ProductServiceID,
                    p.ProductServiceName,
                    p.Unit
                })
                .ToList();

            // Returning data as JSON
            return Json(products, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveMeetingMinutes(MeetingMinutesViewModel model)
        {
            try
            {
                using (var db = new GTaskEntities())
                {
                    // Validate customer ID based on customer type
                    if (model.CustomerType == "Corporate" && !model.CorporateCustomerID.HasValue)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Corporate Customer ID is required"
                        });
                    }

                    if (model.CustomerType == "Individual" && !model.IndividualCustomerID.HasValue)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Individual Customer ID is required"
                        });
                    }

                    // Ensure customer exists
                    bool customerExists = model.CustomerType == "Corporate"
                        ? db.Corporate_Customer_Tbl.Any(c => c.CorporateCustomerID == model.CorporateCustomerID)
                        : db.Individual_Customer_Tbl.Any(c => c.IndividualCustomerID == model.IndividualCustomerID);

                    if (!customerExists)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Selected customer does not exist"
                        });
                    }

                    // Define SQL parameters for master record
                    var masterParams = new SqlParameter[]
                    {
                        new SqlParameter("@CustomerType", model.CustomerType),
                        new SqlParameter("@CorporateCustomerID", model.CustomerType == "Corporate" ? (object)model.CorporateCustomerID : DBNull.Value),
                        new SqlParameter("@IndividualCustomerID", model.CustomerType == "Individual" ? (object)model.IndividualCustomerID : DBNull.Value),
                        new SqlParameter("@MeetingDate", model.MeetingDate),
                        new SqlParameter("@MeetingTime", model.MeetingTime),
                        new SqlParameter("@MeetingPlace", model.MeetingPlace),
                        new SqlParameter("@ClientAttendees", model.ClientAttendees ?? string.Empty),
                        new SqlParameter("@HostAttendees", model.HostAttendees ?? string.Empty),
                        new SqlParameter("@Agenda", model.Agenda ?? string.Empty),
                        new SqlParameter("@Discussion", model.Discussion ?? string.Empty),
                        new SqlParameter("@Decision", model.Decision ?? string.Empty),
                        new SqlParameter("@MeetingMinutesID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                    };

                    // Execute master stored procedure
                    db.Database.ExecuteSqlCommand(
                        "EXEC @MeetingMinutesID = Meeting_Minutes_Master_Save_SP " +
                        "@CustomerType, @CorporateCustomerID, @IndividualCustomerID, @MeetingDate, @MeetingTime, " +
                        "@MeetingPlace, @ClientAttendees, @HostAttendees, @Agenda, @Discussion, @Decision, @MeetingMinutesID OUT",
                        masterParams);

                    // Retrieve the new MeetingMinutesID
                    int meetingMinutesId = Convert.ToInt32(masterParams.Last().Value);

                    if (meetingMinutesId <= 0)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Failed to save meeting minutes master record."
                        });
                    }

                    // Save details data
                    if (model.Products != null && model.Products.Any())
                    {
                        foreach (var product in model.Products)
                        {
                            // Validate product data
                            if (product.ProductServiceID <= 0)
                            {
                                return Json(new { success = false, message = "Invalid Product/Service ID provided." });
                            }
                            if (product.Quantity <= 0)
                            {
                                return Json(new { success = false, message = "Quantity must be greater than 0." });
                            }
                            if (string.IsNullOrEmpty(product.Unit))
                            {
                                return Json(new { success = false, message = "Unit cannot be empty." });
                            }

                            // Define SQL parameters for detail record
                            var detailParams = new SqlParameter[]
                            {
                                new SqlParameter("@MeetingMinutesID", meetingMinutesId),
                                new SqlParameter("@ProductServiceID", product.ProductServiceID),
                                new SqlParameter("@Quantity", product.Quantity),
                                new SqlParameter("@Unit", product.Unit)
                            };

                            // Execute details stored procedure
                            db.Database.ExecuteSqlCommand(
                                "EXEC Meeting_Minutes_Details_Save_SP @MeetingMinutesID, @ProductServiceID, @Quantity, @Unit",
                                detailParams);
                        }
                    }

                    // Return success response
                    return Json(new
                    {
                        success = true,
                        meetingMinutesId = meetingMinutesId,
                        message = "Meeting minutes saved successfully!"
                    });
                }
            }
            catch (Exception ex)
            {
                // Return error response
                return Json(new
                {
                    success = false,
                    message = "Error saving meeting minutes: " + ex.Message
                });
            }
        }


    }
}