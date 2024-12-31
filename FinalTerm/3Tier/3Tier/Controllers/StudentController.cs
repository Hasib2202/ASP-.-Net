using BLL.DTOs;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _3Tier.Controllers
{
    public class StudentController : ApiController
    {
        // GET api/student
        [HttpGet]
        [Route("api/students/all")]
        public HttpResponseMessage Get()
        {
            var data = StudentService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        // POST api/student
        [HttpPost]
        [Route("api/students/create")]
        public HttpResponseMessage Create(StudentDTO s)
        {
            StudentService.Create(s);
            return Request.CreateResponse(HttpStatusCode.OK,"Student successfully created");
        }
    }
}
