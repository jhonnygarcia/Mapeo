using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using GestorMapeos.App_Start;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GestorMapeos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (!WebAppAccount.ValidatePassword(username, password))
            { 
                return View("Login");
            }

            var account = new WebAppAccount { UserName = username };
            HttpContext.GetOwinContext()
                       .Authentication
                       .SignIn(new AuthenticationProperties
                               {
                                   ExpiresUtc = DateTime.UtcNow.AddMinutes(120)
                               }, 
                               account.GetUserIdentity(DefaultAuthenticationTypes.ApplicationCookie));
            return RedirectToAction("Index"); 
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}