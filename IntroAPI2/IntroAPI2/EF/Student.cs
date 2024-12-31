using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntroAPI2.EF
{
    public class Student
    {
        [Key]
        public int SId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name can't exceed 50 characters.")]
        public string SName { get; set; }

        [Range(0.0, 4.0, ErrorMessage = "CGPA must be between 0.0 and 4.0.")]
        public float SCgpa { get; set; }
    }
}