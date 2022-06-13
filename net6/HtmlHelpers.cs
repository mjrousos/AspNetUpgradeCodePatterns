using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoApp
{
    public static class HtmlHelpers
    {
        // #1158 IHtmlString
        public static IHtmlContent ProfilePicture(this IHtmlHelper helper, string url, string userName, object htmlAttributes = null)
        {
            // #73 TagBuilder
            var builder = new TagBuilder("img");

            // #73 TagBuilder.GenerateId
            builder.GenerateId("profilePicture", "_");
            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", $"Profile picture for {helper.Encode(userName)}");
            builder.MergeAttribute("width", "200");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // #73 TagBuilder.ToString
            return builder.RenderSelfClosingTag();
        }
    }
}