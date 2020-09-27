using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;
using WebAppFirst.ViewModels;
using PagedList;

namespace WebAppFirst.Controllers
{
    public class Orders2Controller : Controller
    {
        private northwindEntities1 db = new northwindEntities1();

        // GET: Orders2
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers);
                return View(orders.ToList());
            }
            
        }

        // GET: Orders2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders2/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName");
            return View();
        }

        // POST: Orders2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // GET: Orders2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // POST: Orders2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return PartialView("_ModalEdit", orders);
        }

        // GET: Orders2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Ordersummary()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";

                var orderSummary = from o in db.Orders
                                   join od in db.Order_Details on o.OrderID equals od.OrderID
                                   join p in db.Products on od.ProductID equals p.ProductID
                                   join c in db.Categories on p.CategoryID equals c.CategoryID
                                   select new OrderSummaryData
                                   {
                                       OrderID = o.OrderID,
                                       CustomerID = o.CustomerID,
                                       EmployeeID = (int)o.EmployeeID,
                                       OrderDate = (DateTime)o.OrderDate,
                                       RequiredDate = (DateTime)o.RequiredDate,
                                       ShippedDate = (DateTime)o.ShippedDate,
                                       ShipVia = (int)o.ShipVia,
                                       Freight = (float)o.Freight,
                                       ShipName = o.ShipName,
                                       ShipAddress = o.ShipAddress,
                                       ShipCity = o.ShipCity,
                                       ShipRegion = o.ShipRegion,
                                       ShipPostalCode = o.ShipPostalCode,
                                       ShipCountry = o.ShipCountry,
                                       ProductID = p.ProductID,
                                       UnitPrice = (float)p.UnitPrice,
                                       Quantity = (int)od.Quantity,
                                       Discount = (float)od.Discount,
                                       ProductName = p.ProductName,
                                       SupplierID = (int)p.SupplierID,
                                       CategoryID = (int)c.CategoryID,
                                       QuantityPerUnit = p.QuantityPerUnit,
                                       UnitsInStock = (int)p.UnitsInStock,
                                       UnitsOnOrder = (int)p.UnitsOnOrder,
                                       ReorderLevel = (int)p.ReorderLevel,
                                       Discontinued = p.Discontinued,
                                       ImageLink = p.ImageLink,
                                       CategoryName = c.CategoryName,
                                       Description = c.Description,

                                   };
                return View(orderSummary);
            }

           
        }

        public ActionResult TilausOtsikot(string searchString1, string searchString2, string searchString3, string currentFilter1, string currentFilter2, string currentFilter3, int? page, int? pagesize)
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {   
                ViewBag.LoggedStatus = "Logged in";

                if (searchString1 != null && searchString2 !=null && searchString3 !=null)  
                {                           
                    page = 1;
                }
                else
                {
                    searchString1 = currentFilter1;         
                    searchString2 = currentFilter2;
                    searchString3 = currentFilter3;
                }

                ViewBag.currentFilter1 = searchString1;      
                ViewBag.currentFilter2 = searchString2;
                ViewBag.currentFilter3 = searchString3;

                var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers);
                
                if (!String.IsNullOrEmpty(searchString1))
                {
                    orders = orders.Where(c => c.Customers.CompanyName.Contains(searchString1));
                }
                if (!String.IsNullOrEmpty(searchString2))
                {
                    orders = orders.Where(c => c.Customers.City.Contains(searchString2));
                }
                if (!String.IsNullOrEmpty(searchString3))
                {
                    orders = orders.Where(s => s.Shippers.CompanyName.Contains(searchString3));
                }


                int pageSize = (pagesize ?? 10);  
                int pageNumber = (page ?? 1);  
                return View(orders.OrderBy(i => i.OrderID).ToPagedList(pageNumber, pageSize));
            }
            
        }

        public ActionResult _TilausRivit(int? orderid)
        {
            var orderRowsList = from od in db.Order_Details
                                join p in db.Products on od.ProductID equals p.ProductID
                                join c in db.Categories on p.CategoryID equals c.CategoryID
                                where od.OrderID == orderid
                                select new OrderRows 
                                {
                                    OrderID = od.OrderID,
                                    ProductID = p.ProductID,
                                    UnitPrice = (float)p.UnitPrice,
                                    Quantity = (int)od.Quantity,
                                    Discount = (float)od.Discount,
                                    ProductName = p.ProductName,
                                    SupplierID = (int)p.SupplierID,
                                    CategoryID = (int)c.CategoryID,
                                    QuantityPerUnit = p.QuantityPerUnit,
                                    UnitsInStock = (int)p.UnitsInStock,
                                    UnitsOnOrder = (int)p.UnitsOnOrder,
                                    ReorderLevel = (int)p.ReorderLevel,
                                    Discontinued = p.Discontinued,
                                    ImageLink = p.ImageLink,
                                    CategoryName = c.CategoryName,
                                    Description = c.Description,
                                    //Picture = (Image)c.Picture
                                };
            return PartialView(orderRowsList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
