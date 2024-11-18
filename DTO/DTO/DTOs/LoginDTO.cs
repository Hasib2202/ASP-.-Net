using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DTO.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string uname { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string password { get; set; }

    }
}