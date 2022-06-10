using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Netfx
{
    public static class HtmlHelpers
    {
        public static IHtmlString ProfilePicture(this HtmlHelper helper, string url, string userName, object htmlAttributes = null)
        {
            // #73 TagBuilder
            var builder = new TagBuilder("img");
            builder.GenerateId("profilePicture");
            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", $"Profile picture for {userName}");
            builder.MergeAttribute("width", "200");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}