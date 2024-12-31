using IntroAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IntroAPI.Controllers
{
    public class StudentController : ApiController
    {
        [HttpGet]
        [Route("api/student/allStudents")]
        public HttpResponseMessage GetStudent()
        {
            var students = new List<Student>();
            for (int i = 1; i <= 10; i++)
            {
                students.Add(new Student()
                {
                    SId = i,
                    SName = "Student " + i
                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, students);
        }

        [HttpGet]
        [Route("api/student/{id}")]
        public HttpResponseMessage Get(int id)
        {
            //
            //
            return Request.CreateResponse(HttpStatusCode.OK, new Student() { SId = id });
        }

        [HttpPost]
        [Route("api/student/create")]
        public HttpResponseMessage Create(Student s)
        {
            //
            //
            s.SId = 1;
            return Request.CreateResponse(HttpStatusCode.OK, s);
        }
    }
}
