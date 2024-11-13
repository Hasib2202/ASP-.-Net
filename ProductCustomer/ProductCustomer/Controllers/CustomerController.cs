using ProductCustomer.DTOs;
using ProductCustomer.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductCustomer.Controllers
{
    public class CustomerController : Controller
    {
        PCEntities db = new PCEntities();


        // Convert the DTO model to EF model
        public static Customer Convert(CustomerDTO p)
        {
            return new Customer()
            {
                fname = p.fname,
                lname = p.lname,
                email = p.email,
                phone = p.phone,
                address = p.address,
                date_joined = p.date_joined

            };
        }

        // Convert the EF Model to DTO Model
        public static CustomerDTO Convert(Customer p)
        {
            return new CustomerDTO()
            {
                fname = p.fname,
                lname = p.lname,
                email = p.email,
                phone = p.phone,
                address = p.address,
                date_joined = p.date_joined
            };
        }

        // Convert the list of EF model to DTO
        public static List<CustomerDTO> Convert(List<Customer> data)
        {
            var list = new List<CustomerDTO>();
            foreach (var i in data)
            {
                list.Add(Convert(i));
            }
            return list;
        }

        // GET: Product
        [HttpGet]
        public ActionResult Create()
        {

            return View(new CustomerDTO());
        }

        [HttpPost]
        public ActionResult Create(CustomerDTO p)
        {
            if (ModelState.IsValid)
            {
                var efcreatedata = new Customer()
                {
                    fname = p.fname,
                    lname = p.lname,
                    email = p.email,
                    phone = p.phone,
                    address = p.address,
                    date_joined = p.date_joined
                };
                db.Customers.Add(efcreatedata);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(p);

        }

        // List
        public ActionResult List()
        {
            var efcreatedata = db.Customers.ToList();
            return View(efcreatedata);
        }

        // Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var efcreatedata = db.Customers.Find(id);
            if (efcreatedata == null)
            {
                return HttpNotFound();
            }
            var productDTO = Convert(efcreatedata);
            return View(productDTO);
        }

        [HttpPost]
        public ActionResult Edit(CustomerDTO d)
        {
            if (ModelState.IsValid)
            {
                var data = db.Customers.Find(d.id);
                if (data != null)
                {
                    data.fname = d.fname;
                    data.lname = d.lname;
                    data.email = d.email;
                    data.phone = d.phone;
                    data.address = d.address;
                    data.date_joined = d.date_joined;

                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Customer not found.");
                }
            }
            else
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }
            return View(d);
        }



        // Details
        public ActionResult Details(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        // Delete 
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.Customers.Find(id);
            return View(Convert(data));
        }

        [HttpPost]
        public ActionResult Delete(int id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var data = db.Customers.Find(id);
                db.Customers.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}