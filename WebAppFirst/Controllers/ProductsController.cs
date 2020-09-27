using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;
using PagedList;
using WebAppFirst.ViewModels;
using System.Data.Entity.SqlServer;


namespace WebAppFirst.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1, string ProductCategory, string currentProductCategory, int? page, int? pagesize)
        {
            if (Session["UserName"] == null)  
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                ViewBag.CurrentSort = sortOrder;

                ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
                // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
                ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "unitprice_desc" : "UnitPrice";

                if (searchString1 != null)  
                {                           
                    page = 1;
                }
                else
                {
                    searchString1 = currentFilter1;         
                }
                
                ViewBag.currentFilter1 = searchString1;     

                if ((ProductCategory != null) && (ProductCategory != "0"))
	            {
                    page = 1;
                }

                else
                {
                    ProductCategory = currentProductCategory;
                }

                ViewBag.currentProductCategory = ProductCategory;

                northwindEntities1 db = new northwindEntities1();   

                var tuotteet = from p in db.Products            
                               select p;

                if (!string.IsNullOrEmpty(ProductCategory) && (ProductCategory != "0"))
                {
                    int para = int.Parse(ProductCategory);
                    tuotteet = tuotteet.Where(p => p.CategoryID == para);
                }


                if (!String.IsNullOrEmpty(searchString1))  
                {
                    switch (sortOrder)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderByDescending(p => p.ProductName);
                            break;
                        case "UnitPrice":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderBy(p => p.UnitPrice);
                            break;
                        case "unitprice_desc":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderByDescending(p => p.UnitPrice);
                            break;
                        default:
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderBy(p => p.ProductName);
                            break;
                    }   
                }
                else  
                {
                    switch (sortOrder)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.OrderByDescending(p => p.ProductName);
                            break;
                        case "UnitPrice":
                            tuotteet = tuotteet.OrderBy(p => p.UnitPrice);
                            break;
                        case "unitprice_desc":
                            tuotteet = tuotteet.OrderByDescending(p => p.UnitPrice);
                            break;
                        default:
                            tuotteet = tuotteet.OrderBy(p => p.ProductName);
                            break;
                    }
                }

                List<Categories> lstCategories = new List<Categories>();

                var categoryList = from cat in db.Categories
                                   select cat;

                Categories tyhjaCategory = new Categories();
                tyhjaCategory.CategoryID = 0;
                tyhjaCategory.CategoryName = "";
                tyhjaCategory.CategoryIDCategoryName = "";
                lstCategories.Add(tyhjaCategory);

                foreach (Categories category in categoryList)
                {
                    Categories yksiCategory = new Categories();
                    yksiCategory.CategoryID = category.CategoryID;
                    yksiCategory.CategoryName = category.CategoryName;
                    yksiCategory.CategoryIDCategoryName = category.CategoryID.ToString() + " - " + category.CategoryName;
                    lstCategories.Add(yksiCategory);
                }
                ViewBag.CategoryID = new SelectList(lstCategories, "CategoryID", "CategoryIDCategoryName", ProductCategory);

                int pageSize = (pagesize ?? 10);  
                int pageNumber = (page ?? 1);  
                return View(tuotteet.ToPagedList(pageNumber, pageSize));
            }
           

        }

        public ActionResult Index2()
        {
            northwindEntities1 db = new northwindEntities1();   
            List<Products> model = db.Products.ToList();    
            db.Dispose();                                   
            return View(model);                                  

        }

        public ActionResult _ProductSalesPerDate(string productName)
        {
            if (String.IsNullOrEmpty(productName)) productName = "Lakkalikööri";  // debug test

            List<DailyProductSales> dailyproductsaleslist = new List<DailyProductSales>();
            northwindEntities1 db = new northwindEntities1();

            var orderSummary = from pds in db.ProductsDailySales  
                               where pds.ProductName == productName  
                               orderby pds.OrderDate  
                               select new DailyProductSales  
                               {
                                   OrderDate = SqlFunctions.DateName("year", pds.OrderDate) + "." + SqlFunctions.DateName("MM", pds.OrderDate) + "." + SqlFunctions.DateName("day", pds.OrderDate),
                                   DailySales = (float)pds.DailySales,
                                   ProductName = pds.ProductName

                               };
            return Json(orderSummary, JsonRequestBehavior.AllowGet);
        }

    }
}