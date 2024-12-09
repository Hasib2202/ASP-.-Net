using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPMT.Models
{
    public class ProductService
    {
        public int ProductServiceID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductServiceName { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}