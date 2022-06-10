using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DemoApp.Controllers;

public class ProfilePictureController : Controller
{
    public const string ProfilePictureRouteName = "ProfilePictureRoute";
    private readonly IHostEnvironment _hostEnvironment;

    public ProfilePictureController(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    // GET: ProfilePicture
    [HttpGet]
    [Route("ProfilePicture/{userName}", Name = ProfilePictureRouteName)]
    public ActionResult Index(string userName)
    {
        // #674 Server.MapPath
        var webRoot = Path.Combine(_hostEnvironment.ContentRootPath, "Pics");
        var path = Path.Combine(webRoot, $"{userName}.png");

        if (System.IO.File.Exists(path))
        {
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
        else
        {
            // #660 HttpNotFound
            return NotFound();
        }
    }
}