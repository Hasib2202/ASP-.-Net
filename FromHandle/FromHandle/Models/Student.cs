using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;


namespace FormSubmission.Models
{
    //public class Student
    //{
    //    [Required(ErrorMessage = "Please provide name")]
    //    [StringLength(10)]
    //    public string Name { get; set; }
    //    [Required]
    //    [Range(1, 40, ErrorMessage = "ID must be 1<=id<=40")]
    //    public int ID { get; set; }
    //    [Required]
    //    [EmailAddress]
    //    public string Email { get; set; }
    //    [Required]
    //    public string Phone { get; set; }
    //}

    //public class Student
    //{
    //    [Required(ErrorMessage = "Name is required")]
    //    [RegularExpression(@"^[a-zA-Z._]+$", ErrorMessage = "Name can only contain alphabets, dots, and underscores")]
    //    public string Name { get; set; }


    //    [Required(ErrorMessage = "ID is required")]
    //    [RegularExpression(@"^[0-9]{2}-[0-9]{5}-[1-3]$", ErrorMessage = "ID must be in format: xx-xxxxx-x where xx and xxxxx are digits (0-9), and last x is a digit (1-3)")]
    //    public string ID { get; set; }

    //    [Required(ErrorMessage = "Date of Birth is required")]
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime DateOfBirth { get; set; }

    //    [Required(ErrorMessage = "Email is required")]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    public bool IsValid()
    //    {
    //        // Age validation
    //        var age = DateTime.Today.Year - DateOfBirth.Year;
    //        if (DateOfBirth > DateTime.Today.AddYears(-age)) age--;
    //        if (age < 20) return false;

    //        // Email validation
    //        if (!Email.Equals($"{ID}@student.aiub.edu", StringComparison.OrdinalIgnoreCase))
    //            return false;

    //        return true;
    //    }
    //}

    public class Student
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z._]+$", ErrorMessage = "Name can only contain alphabets, dots, and underscores")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ID is required")]
        [RegularExpression(@"^[0-9]{2}-[0-9]{5}-[1-3]$", ErrorMessage = "ID must be in format: xx-xxxxx-x where xx and xxxxx are digits (0-9), and last x is a digit (1-3)")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(Student), nameof(ValidateAge))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [CustomValidation(typeof(Student), nameof(ValidateEmail))]
        public string Email { get; set; }

        // Custom validation method for age
        public static ValidationResult ValidateAge(DateTime dateOfBirth, ValidationContext context)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;

            if (age < 20)
            {
                return new ValidationResult("Age must be 20 years or older");
            }

            return ValidationResult.Success;
        }

        // Custom validation method for email
        public static ValidationResult ValidateEmail(string email, ValidationContext context)
        {
            var student = context.ObjectInstance as Student;

            // Check if student instance, ID, or email is null
            if (student == null || string.IsNullOrEmpty(student.ID) || string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success; // Skips validation if ID or Email is missing
            }

            string expectedEmail = $"{student.ID}@student.aiub.edu";

            if (!email.Equals(expectedEmail, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult($"Email must be in format: {student.ID}@student.aiub.edu");
            }

            return ValidationResult.Success;
        }


        public bool IsValid()
        {
            var validationContext = new ValidationContext(this);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

            if (!isValid)
            {
                // You can access validation messages through validationResults if needed
                return false;
            }

            return true;
        }
    }
}