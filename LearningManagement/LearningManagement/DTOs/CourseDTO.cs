using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningManagement.DTOs
{
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int InstructorId { get; set; }
        public int Duration { get; set; }
        public bool IsEnrolled { get; set; }

        public string InstructorName { get; set; }
    }
}