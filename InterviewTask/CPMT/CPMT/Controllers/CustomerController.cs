using CPMT.DAL;
using CPMT.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPMT.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController()
        {
            var context = new GTwoTaskEntities();
            _customerRepository = new CustomerRepository(context);
        }

        public JsonResult GetCorporateCustomers()
        {
            var customers = _customerRepository.GetCorporateCustomers();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndividualCustomers()
        {
            var customers = _customerRepository.GetIndividualCustomers();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
    }
}