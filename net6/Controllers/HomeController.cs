using DemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // #275 AuthorizeAttribute namespace (now comes from Microsoft.AspNetCore.Authorization)
        [Authorize]
        public ActionResult UserInfo()
        {
            // #679 Session
            // NOTE : This also requires adding Session services and middleware in program.cs
            var count = (HttpContext.Session.GetInt32("Counter") is int previousCount
                ? previousCount
                : 0) + 1;

            // #679 Session
            HttpContext.Session.SetInt32("Counter", count);

            // #495 HttpCookieCollection
            var idCookie = Request.Cookies["UniqueId"];
            var id = idCookie;
            if (id is null)
            {
                id = Guid.NewGuid().ToString();

                // #495 HttpCookieCollection
                // #496 HttpCookie
                Response.Cookies.Append("UniqueId", id);
            }

            return View(new UserInfo
            {
                // #676 UserAgent
                UserAgent = Request.Headers["User-Agent"].ToString(),

                // #677 RawUrl
                // This one's a bit tricky because we may often want GetEncodedUrl depending on the scenario
                CurrentUrl = Request.GetDisplayUrl(),

                UserName = User.Identity.Name,
                IncrementCount = count,
                UniqueId = id,
                ProfilePictureUrl = GetProfilePictureUrl(User.Identity.Name)
            });
        }

        private string GetProfilePictureUrl(string name) =>
            // #1159 HttpRequest.Url.Scheme
            Url.RouteUrl(ProfilePictureController.ProfilePictureRouteName, new { userName = name.ToLowerInvariant() }, Request.Scheme);
    }
}