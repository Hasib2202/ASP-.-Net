using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPMT.Models
{
    public class MeetingMinutesDetail
    {
        public int DetailID { get; set; }

        [Required]
        public int MeetingID { get; set; }

        [Required]
        public int ProductServiceID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
    }
}