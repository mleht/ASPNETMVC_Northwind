using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;

namespace WebAppFirst.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return View();
            }
            else
            {
                Session.Abandon();
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("Index", "Home"); 
            }
                
            
        }

        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            northwindEntities1 db = new northwindEntities1();
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "Logged in";
                ViewBag.LoginError = 0; 
                Session["UserName"] = LoggedUser.UserName;
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoggedStatus = "Logged out";
                ViewBag.LoginError = 1; 
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); 
        }


        public ActionResult Index()
        {
            ViewBag.LoginError = 0;

            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
            }
            else ViewBag.LoggedStatus = "Logged in";
            return View();
        }

        public ActionResult About()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                ViewBag.Message = "Päivitämme sivujamme.";
                return View();
            }
           
        }

        public ActionResult Contact()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                ViewBag.Message = "Address";
                return View();
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                ViewBag.Message = "Address";
                return View();
            }
            
        }

        public ActionResult Map()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                ViewBag.Message = "Map";
                return View();
            }
            else
            {
                ViewBag.LoggedStatus = "Logged in";
                ViewBag.Message = "Map";
                return View();
            }


        }  
    }
}