using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;

namespace WebAppFirst.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index(string ShipperID, string currentFilter1)  
        {
            northwindEntities1 db = new northwindEntities1();
            var orders = from p in db.Orders
                         select p;

            if (!string.IsNullOrEmpty(ShipperID) && (ShipperID != "0"))  
            {
                int para = int.Parse(ShipperID);
                orders = orders.Where(p => p.ShipVia == para);     
            }

            List<Shippers> shipperit = new List<Shippers>();

            var shipperList = from ship in db.Shippers
                               select ship;

            Shippers tyhjaCategory = new Shippers();
            tyhjaCategory.ShipperID = 0;
            tyhjaCategory.CompanyName = "";
            shipperit.Add(tyhjaCategory);

            foreach (Shippers shipper in shipperList)
            {
                Shippers yksiCategory = new Shippers();
                yksiCategory.ShipperID = shipper.ShipperID;
                yksiCategory.CompanyName = shipper.CompanyName;
                shipperit.Add(yksiCategory);
            }
            ViewBag.ShipperID = new SelectList(shipperit, "ShipperID", "CompanyName", ShipperID); 

            return View(orders);
        }
    }
}