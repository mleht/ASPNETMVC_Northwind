using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;
using WebAppFirst.ViewModels;

namespace WebAppFirst.Controllers
{
    public class ShippersController : Controller
    {
        northwindEntities1 db = new northwindEntities1();   
        // GET: Shippers
        [HttpGet]
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
                var shippers = db.Shippers.Include(s => s.Region);
                return View(shippers.ToList());
            }
            
        }

        public ActionResult Edit(int? id)       

        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  

                Shippers shippers = db.Shippers.Find(id);
                if (shippers == null) return HttpNotFound();  
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription", shippers.RegionID);                                                                                                  
                return View(shippers);                       
            }
            


        }

        [HttpPost] // kun tulee submit niin se nappaa HttpPostin. Http verbi GET ja Post samannimisille edit-metoidelle
        [ValidateAntiForgeryToken] //Katso https://go.microsoft.com/fwlink/?LinkId=317598 tietoturvaan liittyvä
        public ActionResult Edit([Bind(Include = "ShipperID,CompanyName,Phone,RegionID")] Shippers shipper)
        // parametrina tuee Bind(Include = "sarakenimet")] Shippers tyyppinen shipper niminen olio
        // lomake palauttaa shipperin ja nuo sarakkeet/kentät kaivetaan ja shipper viedään tietokantaan alla olevalla koodilla
        {

            if (ModelState.IsValid)   // jos ModelStare on validi eli ns. kaikki kunnossa
            {
                db.Entry(shipper).State = EntityState.Modified;   // using system.data.entity käyttöön, jotta entitystate on tunnistettu
                // formista tuleva shipper olio tuodaan tähän Entry(shipper). db.Entry pävittää Staten ja sitten tallennetaan muutokset
                db.SaveChanges();
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription");  // tämä tuli tähän koska RegionID foreignkey lisättiin ja region joinattiin
                return RedirectToAction("Index");
            }
            return View(shipper);
        }


        public ActionResult Create()      // Tätä metodia kutsutaan listanäkymästä ja tämä metodi näyttää luontinäytön: Create.cshtml
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription");  // tämä tuli tähän koska RegionID foreignkey lisättiin ja region joinattiin ja halutaa se näkyviin myös uuden luontiin
                return View();
            }

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShipperID,CompanyName,Phone,RegionID")] Shippers shipper)
        {

            if (ModelState.IsValid)
            {
                db.Shippers.Add(shipper);
                db.SaveChanges();
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription");  // tämä tuli tähän koska RegionID foreignkey lisättiin ja region joinattiin
                return RedirectToAction("Index");
            }
            return View(shipper);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Shippers shippers = db.Shippers.Find(id);
                if (shippers == null) return HttpNotFound();
                return View(shippers);
            }

            
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shippers shippers = db.Shippers.Find(id);
            db.Shippers.Remove(shippers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShipRegIndex() 
        {
            var innerJoin = from shipper in db.Shippers
                            join region in db.Region on shipper.RegionID equals region.RegionID
                            select shipper;
                           
            return View(innerJoin);
        }

        public ActionResult ShipRegIndex2()
        {
            var innerJoin = from shipper in db.Shippers
                            join region in db.Region on shipper.RegionID equals region.RegionID
                            select new ShippersRegions 
                            { ShipperID = shipper.ShipperID, 
                              CompanyName = shipper.CompanyName,
                              Phone = shipper.Phone,
                              RegionDescription = region.RegionDescription,
                            };

            return View(innerJoin);
        }
    }
}