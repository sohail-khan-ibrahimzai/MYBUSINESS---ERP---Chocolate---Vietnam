using Azure.Core;
using MYBUSINESS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYBUSINESS.CustomClasses
{
    public class DAL
    {
        private BusinessContext db = new BusinessContext();
        //public static List<Customer> dbCustomers()
        //{
        //    return db.Customers.Where(x => x.Status != "D").ToList<Customer>();
        //}

        //private List<Supplier> supplier;
        public IQueryable<Supplier> dbSuppliers
        {
            get { return db.Suppliers.Where(x => x.Status != "D"); }
            //set { customers = value; }
        }

        private IQueryable<Customer> customers;
        public IQueryable<Customer> dbCustomers
        {
            get{return db.Customers.Where(x => x.Status != "D");}
            set { customers = value; }
        }

        //private List<Product> products;
        public IQueryable<Product> dbProducts
        {
            get { return db.Products.Where(x => x.Status != "D"); }
            //set { products = value; }
        }

        //private List<Store> stores;
        public IQueryable<Store> dbStore
        {
            get { return db.Stores.Where(x => x.Status != "D"); }
            //set { products = value; }
        } 
        public IQueryable<DailyBalanceVnd> dbVndBalance
        {
            //get { return db.DailyBalanceVnds.Where(x => x.Status != "D"); }
            get { return db.DailyBalanceVnds; }
            //set { products = value; }
        }
    }
}