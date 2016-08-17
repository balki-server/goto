using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
//using static UrlShortenerWebAPI.ApplicationUserManager;
using UrlShortenerWebAPI.Models;
using UrlShortenerModels.Models;
using System.Web.Security;
using System;
using System.Security.Claims;
using System.Configuration;

namespace UrlShortenerWebAPI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var adminUserName = ConfigurationManager.AppSettings["AdminUserName"];
            var adminPassword = ConfigurationManager.AppSettings["AdminPassword"];

            if (model.Email.ToLower().Trim().Equals(adminUserName) == false || model.Password.Equals(adminPassword) == false)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);

            }
            var ident = new ClaimsIdentity(
        new[]
        {
            // adding following 2 claim just for supporting default antiforgery provider
            new Claim(ClaimTypes.NameIdentifier,adminUserName),
            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

            new Claim(ClaimTypes.Name, adminUserName),
            new Claim(ClaimTypes.Role, "Admin")
         },
         DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
               new AuthenticationProperties { IsPersistent = false }, ident);


            return RedirectToLocal(returnUrl); ;
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("ShortenUrl", "Home");
        }
        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
    }
}