using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DemoApp.Controllers
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
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            var widget = _service.GetWidget(id.Value, HttpContext);

            if (widget is null)
            {
                // #660 HttpNotFound
                return NotFound();
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
        public ActionResult Create([Bind("Name", "Price")] Widget widget)
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
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            var widget = _service.GetWidget(id.Value, HttpContext);

            if (widget is null)
            {
                // #660 HttpNotFound
                return NotFound();
            }

            return View(widget);
        }

        // POST: Widget/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // #682 BindAttribute (Exclude)
        public async Task<ActionResult> Edit(int id, [Bind("Name", "Price")] Widget updatedWidget)
        {
            if (ModelState.IsValid)
            {
                var widget = _service.GetWidget(id, HttpContext);

                // #684 TryUpdateModel
                // This is a useful transformaion, but if it's too complicated, just transforming overloads that don't specify property names
                // would also be useful.
                await TryUpdateModelAsync(widget, string.Empty, w => w.Name, w => w.Price);

                _service.UpdateWidget(widget, HttpContext);
                return RedirectToAction("Index");
            }

            return View(updatedWidget);
        }

        // GET: Widget/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id is null)
            {
                // #672 StatusCodeResult
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            var widget = _service.GetWidget(id.Value, HttpContext);

            if (widget is null)
            {
                // #660 HttpNotFound
                return NotFound();
            }

            return View(widget);
        }

        // POST: Widget/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        // #683 FormCollection
        public ActionResult DeleteConfirmed(int id, IFormCollection collection)
        {
            if (!_service.DeleteWidget(id, HttpContext))
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }
    }
}
