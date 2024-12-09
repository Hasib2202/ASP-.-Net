using CPMT.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPMT.DAL
{
    public class MeetingMinutesRepository
    {
        public class ProductServiceRepository
        {
            private readonly GTwoTaskEntities _context;

            public ProductServiceRepository(GTwoTaskEntities context)
            {
                _context = context;
            }

            public IEnumerable<Products_Service_Tbl> GetProductServices()
            {
                return _context.Products_Service_Tbl.ToList();
            }
        }
    }
}