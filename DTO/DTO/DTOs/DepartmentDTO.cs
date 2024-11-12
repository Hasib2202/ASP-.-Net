using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DTO.DTOs
{
    public class DepartmentDTO
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(15, ErrorMessage = "Name cannot be longer than 15 characters")]
        public string name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(15, ErrorMessage = "Location cannot be longer than 15 characters")]
        public string location { get; set; }
    }
}