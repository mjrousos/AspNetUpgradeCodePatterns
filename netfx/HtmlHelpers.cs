using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DemoApp
{
    public static class HtmlHelpers
    {
        // #1158 IHtmlString
        public static IHtmlString ProfilePicture(this HtmlHelper helper, string url, string userName, object htmlAttributes = null)
        {
            // #73 TagBuilder
            var builder = new TagBuilder("img");

            // #73 TagBuilder.GenerateId
            builder.GenerateId("profilePicture");
            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", $"Profile picture for {helper.Encode(userName)}");
            builder.MergeAttribute("width", "200");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // #73 TagBuilder.ToString
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}