﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;
using WebAppFirst.ViewModels;

namespace WebAppFirst.Controllers
{
    public class StatisticsController : Controller
    {
        private northwindEntities1 db = new northwindEntities1();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult CategorySales()
		{
			string categoryNameList;
			string categorySalesList;
			List<CategorySalesClass> CategorySalesList = new List<CategorySalesClass>();


			var categorySalesData = from cs in db.Category_Sales_for_1997
							select cs;

			foreach (Category_Sales_for_1997 salesfor1997 in categorySalesData)
			{
				CategorySalesClass OneSalesRow = new CategorySalesClass();   
				OneSalesRow.CategoryName = salesfor1997.CategoryName;        
				OneSalesRow.CategorySales = (int)salesfor1997.CategorySales;  
				CategorySalesList.Add(OneSalesRow);   
			}

			categoryNameList = "'" + string.Join("','", CategorySalesList.Select(n => n.CategoryName).ToList()) + "'";
			categorySalesList = string.Join(",", CategorySalesList.Select(n => n.CategorySales).ToList());

			ViewBag.categoryName = categoryNameList;
			ViewBag.categorySales = categorySalesList;

			return View();
		}
	}
}