using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Ninject;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebFramework.App_Start;
using WebFramework.Helpers;
namespace WebFramework.Controllers
{
    public class SecurityController : BaseController
    {
        private readonly ILog _log;

        public SecurityController([Named(WebFramework.Helpers.Constants.ServerLoggerName)] ILog log)
            : base(log)
        {
            _log = log;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Code = ConfigManager.Get<string>("SecurityCode");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string code, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Code = ConfigManager.Get<string>("SecurityCode");

            var noOfTries = GetNoOfTries();
            if (noOfTries >= 5)
            {
                DateTime? nextCommentAt = GetNextTryAt();
                var laterOn = nextCommentAt.HasValue ? " after " + nextCommentAt.Value.ToString() : string.Empty;
                ModelState.AddModelError("", "Too many tries. Take a break and comeback" + laterOn + ".");
                return View();
            }

            if (ModelState.IsValid && 
               (ConfigManager.Get<string>("SecurityCode") == code || ConfigManager.Get<string>("SecretSecurityCode") == code))
            {
                var ident = new ClaimsIdentity(
                  new[] { 
                          // adding following 2 claim just for supporting default antiforgery provider
                          new Claim(ClaimTypes.NameIdentifier, "ValidatedUser"),
                          new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", 
                                    "ASP.NET Identity", 
                                    "http://www.w3.org/2001/XMLSchema#string"),

                          new Claim(ClaimTypes.Name, "ValidatedUser"),

                          new Claim(ClaimTypes.UserData, code),

                          // optionally you could add roles if any
                          new Claim(ClaimTypes.Role, "RoleName"),
                          new Claim(ClaimTypes.Role, "AnotherRole"),

                      },
                  DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                    new AuthenticationProperties 
                            {
                                IsPersistent = false, 
                                IssuedUtc = DateTime.UtcNow,
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(5) 
                            }, 
                            ident);

                return RedirectToLocal(returnUrl);
            }
            else
            {
                IncrementNoOfTries();

                ModelState.AddModelError("", "Security failed.");
            }

            return View();
        }

        [SecurityCode]
        public ActionResult Logout(string returnUrl)
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToLocal(returnUrl);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private int GetNoOfTries()
        {
            var cookie = Request.Cookies["NO_OF_TRIES"];
            if (cookie != null)
            {
                var noOfTries = 0;
                var value = cookie.Value ?? string.Empty;
                if (int.TryParse(value, out noOfTries))
                {
                    return noOfTries;
                }
            }

            return 0;
        }

        private DateTime? GetNextTryAt()
        {
            var cookie = Request.Cookies["NEXT_TRY_AT"];
            if (cookie != null)
            {
                DateTime nextTryAt;
                var value = cookie.Value ?? string.Empty;
                if (DateTime.TryParse(value, out nextTryAt))
                {
                    return nextTryAt;
                }
            }

            return null;
        }

        private void IncrementNoOfTries()
        {
            var noOfTries = GetNoOfTries();
            var nextTryAt = DateTime.Now.Add(TimeSpan.FromMinutes(30));

            var cookie = new HttpCookie("NO_OF_TRIES", (++noOfTries).ToString());
            cookie.Expires = nextTryAt;
            Response.Cookies.Add(cookie);

            var expiry = new HttpCookie("NEXT_TRY_AT", nextTryAt.ToString());
            expiry.Expires = nextTryAt;
            Response.Cookies.Add(expiry);
        }
    }
}