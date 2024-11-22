using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningManagement.DTOs
{
    public class StudentProgressDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public decimal Progress { get; set; }

        public int ProgressId { get; set; }
        public int CourseId { get; set; }
        //public string CourseName { get; set; }
        //public decimal Progress { get; set; }
    }
}