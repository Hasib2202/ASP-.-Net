using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPMT.Models
{
    public class CorporateCustomer
    {
        public int CustomerID { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(255)]
        public string ContactName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string ContactEmail { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string ContactPhone { get; set; }
    }
}