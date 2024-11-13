using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProductCustomer.DTOs
{
    public class ProductDTO
    {
        public int id { get; set; }  // Primary Key – auto-generated

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double price { get; set; }

        [Required(ErrorMessage = "Stock Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity must be a positive integer.")]
        public int stock_quantity { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [MaxLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        [Category] // Custom validation attribute for predefined categories
        public string category { get; set; }

        // Inline custom validation attribute for Category
        public class CategoryAttribute : ValidationAttribute
        {
            private readonly string[] _categories = new[] { "electronics", "clothing", "furniture", "books" }; // Add more categories as needed

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || !_categories.Contains(value.ToString().ToLower()))
                {
                    return new ValidationResult("Category must be one of the predefined categories: " + string.Join(", ", _categories));
                }
                return ValidationResult.Success;
            }
        }
    }
}
