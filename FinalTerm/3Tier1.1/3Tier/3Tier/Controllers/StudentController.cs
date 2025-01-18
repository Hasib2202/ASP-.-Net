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
        //// GET api/student
        //[HttpGet]
        //[Route("api/students/all")]
        //public HttpResponseMessage Get()
        //{
        //    var data = StudentService.Get();
        //    return Request.CreateResponse(HttpStatusCode.OK, data,"All Students");
        //}

        //// POST api/student
        //[HttpPost]
        //[Route("api/students/create")]
        //public HttpResponseMessage Create(StudentDTO s)
        //{
        //    StudentService.Create(s);
        //    return Request.CreateResponse(HttpStatusCode.OK,"Student successfully created");
        //}

        // GET api/students/all
        [HttpGet]
        [Route("api/students/all")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var data = StudentService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/students/{id}
        [HttpGet]
        [Route("api/students/{id}")]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                var data = StudentService.Get(id);
                if (data == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found");

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/students/create
        [HttpPost]
        [Route("api/students/create")]
        public HttpResponseMessage Create(StudentDTO s)
        {
            if (string.IsNullOrWhiteSpace(s.StudentName))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Student Name is required");
            }

            try
            {
                StudentService.Create(s);
                return Request.CreateResponse(HttpStatusCode.Created, "Student successfully created");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/students/update/{id}
        [HttpPut]
        [Route("api/students/update/{id}")]
        public HttpResponseMessage Update(int id, StudentDTO s)
        {
            if (id <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Student ID in the URL");
            }

            if (id != s.StudentID)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Student ID in the URL and body must match");
            }

            if (string.IsNullOrWhiteSpace(s.StudentName))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Student Name is required");
            }

            try
            {
                var existingStudent = StudentService.Get(id);
                if (existingStudent == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found");
                }

                StudentService.Update(s);
                return Request.CreateResponse(HttpStatusCode.OK, "Student successfully updated");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        // DELETE api/students/delete/{id}
        [HttpDelete]
        [Route("api/students/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                StudentService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Student successfully deleted");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
