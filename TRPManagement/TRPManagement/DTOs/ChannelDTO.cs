using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TRPManagement.DTOs
{
    public class ChannelDTO
    {
        public int ChannelId { get; set; }

        [Required(ErrorMessage = "Channel Name is required")]
        [StringLength(100, ErrorMessage = "Channel Name cannot exceed 100 characters")]
        public string ChannelName { get; set; }

        [Required(ErrorMessage = "Established Year is required")]
        [Range(1900, 2024, ErrorMessage = "Established Year must be between 1900 and current year")]
        public int EstablishedYear { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; }
    }
}