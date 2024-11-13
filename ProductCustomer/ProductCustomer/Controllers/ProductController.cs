using ProductCustomer.DTOs;
using ProductCustomer.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductCustomer.Controllers
{
    public class ProductController : Controller
    {
        PCEntities db = new PCEntities();


        // Convert the DTO model to EF model
        public static Product Convert(ProductDTO p)
        {
            return new Product()
            {
                name = p.name,
                description = p.description,
                price = p.price,
                stock_quantity = p.stock_quantity,
                category = p.category
            };
        }

        // Convert the EF Model to DTO Model
        public static ProductDTO Convert(Product p)
        {
            return new ProductDTO()
            {
                name = p.name,
                description = p.description,
                price = p.price,
                stock_quantity = p.stock_quantity,
                category = p.category
            };
        }

        // Convert the list of EF model to DTO
        public static List<ProductDTO> Convert(List<Product> data)
        {
            var list = new List<ProductDTO>();
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

            return View(new ProductDTO());
        }

        [HttpPost]
        public ActionResult Create(ProductDTO p)
        {
            if (ModelState.IsValid)
            {
                var efcreatedata = new Product()
                {
                    name = p.name,
                    description = p.description,
                    price = p.price,
                    stock_quantity = p.stock_quantity,
                    category = p.category
                };
                db.Products.Add(efcreatedata);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(p);

        }

        // List
        public ActionResult List()
        {
            var efcreatedata = db.Products.ToList();
            return View(efcreatedata);
        }

        // Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var efcreatedata = db.Products.Find(id);
            if (efcreatedata == null)
            {
                return HttpNotFound();
            }
            var productDTO = Convert(efcreatedata);
            return View(productDTO);
        }

        [HttpPost]
        public ActionResult Edit(ProductDTO d)
        {
            if (ModelState.IsValid)
            {
                var data = db.Products.Find(d.id);
                if (data != null)
                {
                    data.name = d.name;
                    data.description = d.description;
                    data.price = d.price;
                    data.stock_quantity = d.stock_quantity;
                    data.category = d.category;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Product not found.");
                }
            }
            return View(d);
        }




        // Details
        public ActionResult Details(int id)
        {
            var customer = db.Products.Find(id);
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
            var data = db.Products.Find(id);
            return View(Convert(data));
        }

        [HttpPost]
        public ActionResult Delete(int id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var data = db.Products.Find(id);
                db.Products.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }








    }
}