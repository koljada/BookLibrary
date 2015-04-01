using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace BookStore.HtmlHelpers
{
    public static class AjaxImageLink
    {
        public static bool StartWithNumber(this string str)
        {
            if (str.StartsWith("1") || str.StartsWith("0") || str.StartsWith("2") || str.StartsWith("3") || str.StartsWith("4") || str.StartsWith("5") || str.StartsWith("6") || str.StartsWith("7") || str.StartsWith("8") || str.StartsWith("9") || str.StartsWith("0"))
                return true;
            else return false;
        }
        public static IHtmlString ImageActionLink(this AjaxHelper helper, string actionName, string img_url, int bookID, AjaxOptions ajaxOptions)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", img_url);
            builder.MergeAttribute("title", "Save Image");
            builder.MergeAttribute("class", "thumbnail");
            var link = helper.ActionLink("[replaceme]", actionName, new { image_url = img_url, bookID }, ajaxOptions).ToHtmlString();
            return new MvcHtmlString(link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
        }
    }
}
