using Netfx.Models;
using Netfx.Services;
using System.Net;
using System.Web.Mvc;

namespace Netfx.Controllers
{
    public class WidgetController : Controller
    {
        private readonly WidgetService _service;

        public WidgetController(WidgetService service)
        {
            _service = service;
        }

        // GET: Widget
        public ActionResult Index()
        {
            return View(_service.GetAllWidgets(HttpContext));
        }

        // GET: Widget/Details/5
        public ActionResult Details(int? id)
        {
            if (id is null)
            {
                // #672 StatusCodeResult
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var widget = _service.GetWidget(id.Value, HttpContext);

            if (widget is null)
            {
                // #660 HttpNotFound
                return HttpNotFound();
            }

            return View(widget);
        }

        // GET: Widget/Create
        [Authorize]
        public ActionResult Create()
        {
            return View(new Widget());
        }

        // POST: Widget/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // #673 BindAttribute (Include)
        public ActionResult Create([Bind(Include = "Name,Price")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                _service.AddWidget(widget);
                return RedirectToAction("Index");
            }
            return View(widget);
        }

        // GET: Widget/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id is null)
            {
                // #672 StatusCodeResult
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var widget = _service.GetWidget(id.Value, HttpContext);

            if (widget is null)
            {
                // #660 HttpNotFound
                return HttpNotFound();
            }

            return View(widget);
        }

        // POST: Widget/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // #682 BindAttribute (Exclude)
        public ActionResult Edit(int id, [Bind(Exclude = "Id")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateWidget(widget, HttpContext);
                return RedirectToAction("Index");
            }

            return View(widget);
        }

        // GET: Widget/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id is null)
            {
                // #672 StatusCodeResult
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var widget = _service.GetWidget(id.Value, HttpContext);

            if (widget is null)
            {
                // #660 HttpNotFound
                return HttpNotFound();
            }

            return View(widget);
        }

        // POST: Widget/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        // #683 - FormCollection
        public ActionResult DeleteConfirmed(int id, FormCollection collection)
        {
            if (!_service.DeleteWidget(id, HttpContext))
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }
    }
}
