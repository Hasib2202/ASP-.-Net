using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalTaskOne.Models
{
    public class TokenModel
    {
        // Global token number (used for all counters)
        public int GlobalTokenNumber { get; set; } = 1;

        // Max tokens for each counter
        public int MaxTokensPerCounter { get; set; } = 25;

        // Current token counts for each counter
        public int MedicalTokensIssued { get; set; } = 0;
        public int TouristTokensIssued { get; set; } = 0;
        public int BusinessTokensIssued { get; set; } = 0;
        public int GovtOfficialTokensIssued { get; set; } = 0;

        // Max requests per day and the total requests made today
        public int MaxRequestsPerDay { get; set; } = 100;
        public int RequestsMadeToday { get; set; } = 0;
    }
}