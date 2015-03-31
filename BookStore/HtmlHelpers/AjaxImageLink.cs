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
