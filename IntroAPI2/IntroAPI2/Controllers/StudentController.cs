using IntroAPI2.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IntroAPI2.Controllers
{
    public class StudentController : ApiController
    {
        // Database context
        Context db = new Context();

        [HttpPost]
        [Route("api/student/add")]
        public HttpResponseMessage CreateStudent([FromBody] Student s)
        {
            if (s == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Student data is null.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data. Please check all fields.");
            }

            db.Students.Add(s);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, s);
        }

        [HttpGet]
        [Route("api/student/getAllStudents")]
        public HttpResponseMessage GetAllStudents()
        {
            var students = db.Students.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, students);
        }

        [HttpGet]
        [Route("api/student/getStudentById/{id}")]
        public HttpResponseMessage GetStudentById(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, student);
        }

        [HttpPut]
        [Route("api/student/updateStudent/{id}")]
        public HttpResponseMessage UpdateStudent(int id, [FromBody] Student s)
        {
            if (s == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Student data is null.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data. Please check all fields.");
            }

            var student = db.Students.Find(id);
            if (student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            student.SName = s.SName;
            student.SCgpa = s.SCgpa;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, student);
        }

        [HttpDelete]
        [Route("api/student/deleteStudent/{id}")]
        public HttpResponseMessage DeleteStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            db.Students.Remove(student);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Student deleted successfully.");
        }

        [HttpPost]
        [Route("api/student/getStudentsByCgpa")]
        public HttpResponseMessage GetStudentsByCgpa([FromBody] float cgpa)
        {
            var tolerance = 0.01f;
            var students = db.Students
                .Where(s => Math.Abs(s.SCgpa - cgpa) <= tolerance)
                .ToList();

            if (students == null || !students.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No students found with the specified CGPA.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, students);
        }


        public class CgpaRangeRequest
        {
            public float MinCgpa { get; set; }
            public float MaxCgpa { get; set; }
        }

        [HttpPost]
        [Route("api/student/getStudentsByCgpaRange")]
        public HttpResponseMessage GetStudentsByCgpaRange([FromBody] CgpaRangeRequest request)
        {
            if (request.MinCgpa > request.MaxCgpa)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Minimum CGPA cannot be greater than maximum CGPA.");
            }

            var students = db.Students
                .Where(s => s.SCgpa >= request.MinCgpa && s.SCgpa <= request.MaxCgpa)
                .ToList();

            if (students == null || !students.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No students found in the specified CGPA range.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, students);
        }


    }
}
