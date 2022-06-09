using Netfx.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Netfx.Services
{
    public class WidgetService
    {
        private ConcurrentDictionary<int, Widget> _widgets;
        private int _lastId;

        private ConcurrentDictionary<int, Widget> Widgets
        {
            get
            {
                if (_widgets is null)
                {
                    // #675 HostingEnvironment.ApplicationPhysicalPath
                    var widgetFilePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Widgets.csv");
                    var widgets = File.ReadAllLines(widgetFilePath)
                        .Select(ParseWidget)
                        .Where(x => x != null);

                    _widgets = new ConcurrentDictionary<int, Widget>(widgets.ToDictionary(w => w.Id));
                    _lastId = _widgets.Select(w => w.Key).Max() + 1;
                }

                return _widgets;
            }
        }

        public IEnumerable<Widget> GetAllWidgets(HttpContextBase context) => FilterForUser(Widgets, context).Values;

        public Widget GetWidget(int id, HttpContextBase context) => FilterForUser(Widgets, context)[id];

        public Widget AddWidget(Widget newWidget)
        {
            newWidget.Id = Interlocked.Increment(ref _lastId);
            if (!Widgets.TryAdd(newWidget.Id, newWidget))
            {
                throw new InvalidOperationException("Failed to get widget ID");
            }

            return newWidget;
        }

        public bool DeleteWidget(int id, HttpContextBase context)
        {
            if (GetWidget(id, context) is null)
            {
                return false;
            }
            else
            {
                return Widgets.TryRemove(id, out _);
            }
        }

        public bool UpdateWidget(Widget widget, HttpContextBase context)
        {
            if (GetWidget(widget.Id, context) is null)
            {
                return false;
            }
            else
            {
                Widgets[widget.Id] = widget;
                return true;
            }
        }

        private Widget ParseWidget(string s)
        {
            var split = s?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (split is null
                || split.Length != 4
                || !int.TryParse(split[0], out var id)
                || !double.TryParse(split[2], out var price)
                || !bool.TryParse(split[3], out var adminOnly))
            {
                return null;
            }

            return new Widget(id, split[1], price, adminOnly);
        }

        // #681 HttpContextBase
        private IDictionary<int, Widget> FilterForUser(IDictionary<int, Widget> widgets, HttpContextBase context)
        {
            // #500 HttpRequest.IsAuthenticated
            if (!context.Request.IsAuthenticated)
            {
                return widgets.Where(w => !w.Value.AdminOnly).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            else
            {
                return widgets;
            }
        }
    }
}