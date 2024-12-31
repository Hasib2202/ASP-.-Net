using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntroAPI2.EF
{
    public class Teacher
    {
        [Key]
        public int TId { get; set; }

        public string TName { get; set; }

        public string TDesignation { get; set; }
    }
}