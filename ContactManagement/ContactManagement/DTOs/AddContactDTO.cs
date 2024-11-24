using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactManagement.DTOs
{
    public class AddContactDTO
    {
        public int ContactId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters")]
        [RegularExpression(@"^\+?[\d\s-]+$", ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime Birthday { get; set; }


    }
}