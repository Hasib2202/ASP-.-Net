using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DTO.DTOs
{
    public class StudentDTO
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Name is required")]

        public string name { get; set; }

        [Required(ErrorMessage = "Cgpa is required")]

        public string cgpa { get; set; }

        [Required(ErrorMessage = "Father name is required")]

        public string fathername { get; set; }
        [Required(ErrorMessage = "Mother name is required")]

        public string mothername { get; set; }
        [Required(ErrorMessage = "Religion is required")]

        public string religion { get; set; }
        [Required(ErrorMessage = "Gender is required")]

        public string gender { get; set; }
        [Required(ErrorMessage = "Interset is required")]

        public string studentinterest { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]

        public System.DateTime dateofbirth { get; set; }
    }
}