using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPMT.Models
{
    public class MeetingMinutesMaster
    {
        public int MeetingID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerType { get; set; }

        [Required]
        public DateTime MeetingDate { get; set; }

        [Required]
        [StringLength(10)]
        public string MeetingTime { get; set; }

        [Required]
        [StringLength(255)]
        public string MeetingPlace { get; set; }

        [StringLength(255)]
        public string AttendsFromClientSide { get; set; }

        [StringLength(255)]
        public string AttendsFromHostSide { get; set; }

        [Required]

        public string MeetingAgenda { get; set; }

        [Required]

        public string MeetingDiscussion { get; set; }

        [Required]

        public string MeetingDecision { get; set; }
    
}
}