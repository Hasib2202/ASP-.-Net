using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningManagement.DTOs
{
    public class EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        // New properties for display
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string CourseName { get; set; }
        public double CourseDuration { get; set; }
        public string InstructorName { get; set; }
        public decimal Progress { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }

}