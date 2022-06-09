using Netfx.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Netfx.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Signin
        [AllowAnonymous]
        public ActionResult Signin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/SignIn

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SigninModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    // #680 FormsAuthentication
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/SignOut
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            // #680 FormsAuthentication
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private static bool ValidateUser(string userName, string password)
        {
            // TODO : Validate the username and hashed password against a database
            //        or, better, use an actual identity framework like ASP.NET identity.
            return userName.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}