using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace photoGallery.Utility
{
    public static class Utility4photoGallery
    {

        public static MvcHtmlString bootstrapActionLink(this HtmlHelper htmlHelper, string linkText, string Title, string actionName, string controllerName, string glyphicon, object routeValues = null, object htmlAttributes = null)
        {
            //Exemple of result:
            //<a href="@Url.Action("Edit", new { id = Model.id_rod })">
            //  <i class="glyphicon glyphicon-pencil"></i>
            //  <span>Edit</span>
            //</a>

            var builderI = new TagBuilder("i");
            builderI.MergeAttribute("class", "glyphicon " + glyphicon);
            if (!string.IsNullOrEmpty(Title))
            {
                builderI.MergeAttribute("Title", Title);
            }
            string iTag = builderI.ToString(TagRenderMode.Normal);

            string spanTag = "";
            if (!string.IsNullOrEmpty(linkText))
            {
               
                var builderSPAN = new TagBuilder("span");
                builderSPAN.InnerHtml = " " + linkText;
                spanTag = builderSPAN.ToString(TagRenderMode.Normal);
            }

            //Create the "a" tag that wraps
            var builderA = new TagBuilder("a");

            var requestContext = HttpContext.Current.Request.RequestContext;
            var uh = new UrlHelper(requestContext);

            builderA.MergeAttribute("href", uh.Action(actionName, controllerName, routeValues));

            if (htmlAttributes != null)
            {
                IDictionary<string, object> attributes = new RouteValueDictionary(htmlAttributes);
                builderA.MergeAttributes(attributes);
            }

            builderA.InnerHtml = iTag + spanTag;

            return new MvcHtmlString(builderA.ToString(TagRenderMode.Normal));
        }
    }
}