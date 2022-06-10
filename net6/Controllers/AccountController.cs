using DemoApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace DemoApp.Controllers
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
        public async Task<ActionResult> SignInAsync(SigninModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    // #680 FormsAuthentication
                    // NOTE: that this also requires adding builder.Services.AddAuthentication and app.UseAuthentication
                    // calls in program.cs. Given all the possible permutations of this code and the fact that it's security-related,
                    // if might be best to not auto-replace this one and just redirect folks using FormsAuthentication to
                    // https://docs.microsoft.com/aspnet/core/security/authentication/cookie or some similar resource.
                    var claimsIdentity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, model.UserName)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), null);

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
        public async Task<ActionResult> SignOutAsync()
        {
            // #680 FormsAuthentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

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