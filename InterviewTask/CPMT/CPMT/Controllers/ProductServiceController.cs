//using CPMT.EF;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace CPMT.Controllers
//{
//    public class ProductServiceController : Controller
//    {
//        private readonly ProductServiceRepository _productServiceRepository;

//        public ProductServiceController()
//        {
//            var context = new GTwoTaskEntities();
//            _productServiceRepository = new ProductServiceRepository(context);
//        }

//        public JsonResult GetProductServices()
//        {
//            var productServices = _productServiceRepository.GetProductServices();
//            return Json(productServices, JsonRequestBehavior.AllowGet);
//        }
//    }
//}