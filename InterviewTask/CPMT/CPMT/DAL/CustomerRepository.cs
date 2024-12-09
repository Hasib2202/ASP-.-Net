using CPMT.EF;
using CPMT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPMT.DAL
{
    public class CustomerRepository
    {
        private readonly GTwoTaskEntities _context;

        public CustomerRepository(GTwoTaskEntities context)
        {
            _context = context;
        }

        public IEnumerable<Corporate_Customer_Tbl> GetCorporateCustomers()
        {
            return _context.Corporate_Customer_Tbl.ToList();
        }

        public IEnumerable<Individual_Customer_Tbl> GetIndividualCustomers()
        {
            return _context.Individual_Customer_Tbl.ToList();
        }
    }


}