using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class StudentDTO
    {
        [Required]
        public int StudentID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Student Name must be between 1 and 100 characters")]
        public string StudentName { get; set; }
    }
}
