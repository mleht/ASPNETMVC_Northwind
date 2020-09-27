using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;
using WebAppFirst.ViewModels;

namespace WebAppFirst.Controllers
{
    public class ProdsuppController : Controller
    {
        // GET: Prodsupp
        public ActionResult Index()
        {
            northwindEntities1 db = new northwindEntities1();
            var test = from p in db.Products
                       join s in db.Suppliers on p.SupplierID equals s.SupplierID
                       select new Prodsuppliers
                       { ProductID = p.ProductID,
                           ProductName = p.ProductName,
                       SupplierID = (int)p.SupplierID,
                       CategoryID = (int)p.CategoryID,
                       QuantityPerUnit = p.QuantityPerUnit,
                       UnitPrice = (decimal)p.UnitPrice,
                       UnitsInStock = (short)p.UnitsInStock,
                       UnitsOnOrder = (short)p.UnitsOnOrder,
                       ReorderLevel = (short)p.ReorderLevel,
                       Discontinued = p.Discontinued,
                       ImageLink = p.ImageLink,
                       CompanyName = s.CompanyName,
                       ContactName = s.ContactName,
                       ContactTitle = s.ContactTitle,
                       Address = s.Address,
                       City = s.City,
                       Region = s.Region,
                       PostalCode = s.PostalCode,
                       Country = s.Country,
                       Phone = s.Phone,
                       Fax = s.Fax,
                       HomePage = s.HomePage,
                       };
            return View(test);
        }
    }
}