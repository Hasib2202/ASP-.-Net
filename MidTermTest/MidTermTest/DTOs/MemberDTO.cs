using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MidTermTest.DTOs
{
    public class MemberDTO
    {
        //[Required]
        //public int MemberId { get; set; }


        //[Required(ErrorMessage = "Name is required.")]
        //[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        //public string Name { get; set; }

        //[Required(ErrorMessage = "Membership type is required.")]
        //[StringLength(50, ErrorMessage = "Membership type cannot exceed 50 characters.")]
        //public string MemberType { get; set; }


        //[Required(ErrorMessage = "Expiration date is required.")]
        //[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        //public DateTime ExpiredDate { get; set; }


        //public int UserId { get; set; }
        //public string UserEmail { get; set; }

        //[Required]
        //public int MemberId { get; set; } // Unique identifier for the member

        //[Required(ErrorMessage = "Name is required.")]
        //[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        //public string Name { get; set; } // Member's name

        //[Required(ErrorMessage = "Membership type is required.")]
        //[StringLength(50, ErrorMessage = "Membership type cannot exceed 50 characters.")]
        //public string MemberType { get; set; } // Type of membership (e.g., Gold, Silver)

        //[Required(ErrorMessage = "Expiration date is required.")]
        //[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        //public DateTime ExpiredDate { get; set; } // Membership expiration date


        public int MemberId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Member Type is required")]
        [RegularExpression("^(Regular|VIP)$", ErrorMessage = "Invalid Member Type")]
        public string MemberType { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Expiration Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiredDate { get; set; }

        // Additional property to display user email
        public string UserEmail { get; set; }
    }
}