using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactManagement.DTOs
{
    public class AddNoteDTO
    {
        [Required]
        public int NoteId { get; set; }

        [Required]
        public int ContactId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
        public string Content { get; set; }


        [Required]
        public System.DateTime CreatedAt { get; set; }
    }
}