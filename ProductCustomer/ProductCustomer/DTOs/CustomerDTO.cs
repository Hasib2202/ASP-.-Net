using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCustomer.DTOs
{
    public class CustomerDTO
    {
        // CustomerId: Primary Key – auto-generated
        public int id { get; set; }

        // FirstName: Required, maximum length of 50 characters.
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string fname { get; set; }

        // LastName: Required, maximum length of 50 characters.
        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string lname { get; set; }

        // Email: Required, must be a valid email format.
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string email { get; set; }

        // Phone: Optional, must be a valid phone number format (10-15 digits).
        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits.")]
        public string phone { get; set; }

        // Address: Optional, maximum length of 250 characters.
        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string address { get; set; }

        // DateJoined: Required, cannot be a future date.
        [Required(ErrorMessage = "Date Joined is required.")]
        [DataType(DataType.Date)]
        [CustomDateValidation(ErrorMessage = "Date Joined cannot be a future date.")]
        public DateTime date_joined { get; set; }
    }

    // Custom validation attribute to ensure date_joined is not in the future
    public class CustomDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date > DateTime.Now)
            {
                return new ValidationResult(ErrorMessage ?? "The date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
