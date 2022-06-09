using System.Web.Mvc;

namespace Netfx.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();

        // #275 AuthorizeAttribute namespace
        [Authorize]
        public ActionResult UserInfo() => View();

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}