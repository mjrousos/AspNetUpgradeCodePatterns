using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoApp.Controllers;

public class ProfilePictureController : Controller
{
    public const string ProfilePictureRouteName = "ProfilePictureRoute";

    // GET: ProfilePicture
    [HttpGet]
    [Route("ProfilePicture/{userName}", Name = ProfilePictureRouteName)]
    public ActionResult Index(string userName)
    {
        // #674 Server.MapPath
        var webRoot = Server.MapPath("~/Pics");
        var path = Path.Combine(webRoot, $"{userName}.png");

        if (System.IO.File.Exists(path))
        {
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
        else
        {
            // #660 HttpNotFound
            return HttpNotFound();
        }
    }
}