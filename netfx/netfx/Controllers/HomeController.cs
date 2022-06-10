using DemoApp.Models;
using System;
using System.Web;
using System.Web.Mvc;

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

        // #275 AuthorizeAttribute namespace
        [Authorize]
        public ActionResult UserInfo()
        {
            // #679 Session
            var count = (Session["Counter"] is int previousCount
                ? previousCount
                : 0) + 1;

            // #679 Session
            Session["Counter"] = count;

            // #495 HttpCookieCollection
            var idCookie = Request.Cookies["UniqueId"];
            var id = idCookie?.Value;
            if (id is null)
            {
                id = Guid.NewGuid().ToString();

                // #495 HttpCookieCollection
                // #496 HttpCookie
                Response.Cookies.Add(new HttpCookie("UniqueId", id));
            }

            return View(new UserInfo
            {
                // #676 UserAgent
                UserAgent = Request.UserAgent,

                // #677 RawUrl
                CurrentUrl = Request.RawUrl,

                UserName = User.Identity.Name,
                IncrementCount = count,
                UniqueId = id,
                ProfilePictureUrl = GetProfilePictureUrl(User.Identity.Name)
            });
        }

        private string GetProfilePictureUrl(string name) =>
            Url.RouteUrl(ProfilePictureController.ProfilePictureRouteName, new { userName = name.ToLowerInvariant() }, Request.Url.Scheme);
    }
}