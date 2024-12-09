using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CPM.ViewModels
{
    public class MeetingMinutesViewModel
    {
        public MeetingMinutesViewModel()
        {
            Products = new List<ProductServiceViewModel>();
        }

        [Required(ErrorMessage = "Customer Type is required")]
        [RegularExpression("^(Corporate|Individual)$", ErrorMessage = "Invalid Customer Type")]
        public string CustomerType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a Corporate Customer")]
        public int? CorporateCustomerID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an Individual Customer")]
        public int? IndividualCustomerID { get; set; }

        [Required(ErrorMessage = "Meeting Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime MeetingDate { get; set; }

        [Required(ErrorMessage = "Meeting Time is required")]
        [DataType(DataType.Time)]
        public TimeSpan MeetingTime { get; set; }

        [Required(ErrorMessage = "Meeting Place is required")]
        [StringLength(255, ErrorMessage = "Meeting Place cannot exceed 255 characters")]
        public string MeetingPlace { get; set; }

        [StringLength(1000, ErrorMessage = "Client Attendees cannot exceed 1000 characters")]
        public string ClientAttendees { get; set; }

        [StringLength(1000, ErrorMessage = "Host Attendees cannot exceed 1000 characters")]
        public string HostAttendees { get; set; }

        [StringLength(2000, ErrorMessage = "Agenda cannot exceed 2000 characters")]
        public string Agenda { get; set; }

        [StringLength(4000, ErrorMessage = "Discussion cannot exceed 4000 characters")]
        public string Discussion { get; set; }

        [StringLength(2000, ErrorMessage = "Decision cannot exceed 2000 characters")]
        public string Decision { get; set; }

        public List<ProductServiceViewModel> Products { get; set; }

        public bool IsValid()
        {
            if (CustomerType == "Corporate" && !CorporateCustomerID.HasValue)
            {
                return false;
            }

            if (CustomerType == "Individual" && !IndividualCustomerID.HasValue)
            {
                return false;
            }

            if (MeetingDate.Date < DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }

    public class ProductServiceViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Product/Service")]
        public int ProductServiceID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters")]
        public string Unit { get; set; }
    }
}