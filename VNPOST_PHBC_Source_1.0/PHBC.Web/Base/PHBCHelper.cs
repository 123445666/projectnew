using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Web.WebPages;
using System.Linq;
using System.Web.Mvc;
using PHBC.Web.Permission;
using PHBC.DAO.Common;
namespace System.Web.Mvc.Html
{
    public static class PHBCHelper
    {
        public static IHtmlString StringHelper(this HtmlHelper htmlHelper, string value, int length = 30)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new HtmlString("");
            if (value.Length > length)
                value = value.Remove(length) + "...";
            return new HtmlString(value);
        }
        public static IHtmlString StringHelper<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int length = 30)
        {
            TValue _value = expression.Compile()(html.ViewData.Model);
            string value = _value.ToString();
            return PHBCHelper.StringHelper(html, value, length);
        }
        public static IHtmlString RemoveXss(this HtmlHelper htmlHelper, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new HtmlString("");  
            else
            {
                value = value.Replace("'", "\\'");
                value = value.Replace("\"", "\\\"");
            }
            return new HtmlString(value);
        }
        public static IHtmlString RemoveXss<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TValue _value = expression.Compile()(html.ViewData.Model);
            string value = _value.ToString();
            return PHBCHelper.RemoveXss(html, value);
        }
        public static string replaceUnicode(this HtmlHelper htmlHelper, string vaule)
        {
            return Utils.GetFriendlyTitle(vaule, true, int.MaxValue);
        }

        

        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string text, string action, string controller)
        {
            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }
            li.InnerHtml = htmlHelper.ActionLink(text, action, controller).ToHtmlString();
            return MvcHtmlString.Create(li.ToString());
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Func<object, HelperResult> startHtml)
        {
            return LabelFor(html, expression, startHtml, null, new RouteValueDictionary("new {}"));
        }

        /// <summary>Creates a Label with custom Html before the label text.  Starting Html and a single Html attribute is provided.</summary>
        /// <param name="startHtml">Html to preempt the label text.</param>
        /// <param name="htmlAttributes">A single Html attribute to include.</param>
        /// <returns>MVC Html for the Label</returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Func<object, HelperResult> startHtml, object htmlAttributes)
        {
            return LabelFor(html, expression, startHtml, null, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>Creates a Label with custom Html before the label text.  Starting Html and a collection of Html attributes are provided.</summary>
        /// <param name="startHtml">Html to preempt the label text.</param>
        /// <param name="htmlAttributes">A collection of Html attributes to include.</param>
        /// <returns>MVC Html for the Label</returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, Func<object, HelperResult> startHtml, IDictionary<string, object> htmlAttributes)
        {
            return LabelFor(html, expression, startHtml, null, htmlAttributes);
        }

        /// <summary>Creates a Label with custom Html before and after the label text.  Starting Html and ending Html are provided.</summary>
        /// <param name="startHtml">Html to preempt the label text.</param>
        /// <param name="endHtml">Html to follow the label text.</param>
        /// <returns>MVC Html for the Label</returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Func<object, HelperResult> startHtml, Func<object, HelperResult> endHtml)
        {
            return LabelFor(html, expression, startHtml, endHtml, new RouteValueDictionary("new {}"));
        }

        /// <summary>Creates a Label with custom Html before and after the label text.  Starting Html, ending Html, and a single Html attribute are provided.</summary>
        /// <param name="startHtml">Html to preempt the label text.</param>
        /// <param name="endHtml">Html to follow the label text.</param>
        /// <param name="htmlAttributes">A single Html attribute to include.</param>
        /// <returns>MVC Html for the Label</returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Func<object, HelperResult> startHtml, Func<object, HelperResult> endHtml, object htmlAttributes)
        {
            return LabelFor(html, expression, startHtml, endHtml, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>Creates a Label with custom Html before and after the label text.  Starting Html, ending Html, and a collection of Html attributes are provided.</summary>
        /// <param name="startHtml">Html to preempt the label text.</param>
        /// <param name="endHtml">Html to follow the label text.</param>
        /// <param name="htmlAttributes">A collection of Html attributes to include.</param>
        /// <returns>MVC Html for the Label</returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, Func<object, HelperResult> startHtml, Func<object, HelperResult> endHtml, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            //Use the DisplayName or PropertyName for the metadata if available.  Otherwise default to the htmlFieldName provided by the user.
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            //Create the new label.
            TagBuilder tag = new TagBuilder("label");

            //Add the specified Html attributes
            tag.MergeAttributes(htmlAttributes);

            //Specify what property the label is tied to.
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            //Run through the various iterations of null starting or ending Html text.
            if (startHtml == null && endHtml == null) tag.InnerHtml = labelText;
            else if (startHtml != null && endHtml == null) tag.InnerHtml = string.Format("{0}{1}", startHtml(null).ToHtmlString(), labelText);
            else if (startHtml == null && endHtml != null) tag.InnerHtml = string.Format("{0}{1}", labelText, endHtml(null).ToHtmlString());
            else tag.InnerHtml = string.Format("{0}{1}{2}", startHtml(null).ToHtmlString(), labelText, endHtml(null).ToHtmlString());

            return MvcHtmlString.Create(tag.ToString());
        }


        public static MvcHtmlString CustomLink(this HtmlHelper htmlHelper, string labelText, string action, object htmlAttributes = null, string icon = "", string toltip = "")
        {
            if (string.IsNullOrWhiteSpace(labelText) && string.IsNullOrWhiteSpace(icon))
                return new MvcHtmlString("");
            var anchor = new TagBuilder("a");
            if(!string.IsNullOrWhiteSpace(toltip))
            {
                anchor.Attributes.Add("data-toggle", "tooltip");
                anchor.Attributes.Add("title", toltip);
            }
            if (htmlAttributes != null)
                anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var routeData = htmlHelper.ViewContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            anchor.Attributes["href"] = urlHelper.Action(action, controller);
            anchor.InnerHtml = icon + labelText;
            return MvcHtmlString.Create(anchor.ToString());
        }

        public static MvcHtmlString CustomLink(this HtmlHelper htmlHelper, string labelText, string action, object routerValues = null, object htmlAttributes = null, string icon = "", string toltip = "")
        {
            if (string.IsNullOrWhiteSpace(labelText) && string.IsNullOrWhiteSpace(icon))
                return new MvcHtmlString("");
            var anchor = new TagBuilder("a");
            if (!string.IsNullOrWhiteSpace(toltip))
            {
                anchor.Attributes.Add("data-toggle", "tooltip");
                anchor.Attributes.Add("title", toltip);
            }
            if (htmlAttributes != null)
                anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var routeData = htmlHelper.ViewContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            anchor.Attributes["href"] = urlHelper.Action(action, controller, routerValues);
            anchor.InnerHtml = icon + labelText;
            return MvcHtmlString.Create(anchor.ToString());
        }

        public static MvcHtmlString CustomLink(this HtmlHelper htmlHelper, string labelText, string action, PermissonController permisson, object routerValues = null, object htmlAttributes = null, string icon = "", string toltip = "")
        {
            if (!permisson.hasPermisson(action) || (string.IsNullOrWhiteSpace(labelText) && string.IsNullOrWhiteSpace(icon)))
                return new MvcHtmlString("");
            var anchor = new TagBuilder("a");
            if (!string.IsNullOrWhiteSpace(toltip))
            {
                anchor.Attributes.Add("data-toggle", "tooltip");
                anchor.Attributes.Add("title", toltip);
            }
            if (htmlAttributes != null)
                anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var routeData = htmlHelper.ViewContext.RouteData;
            var controller = routeData.GetRequiredString("controller");
            anchor.Attributes["href"] = urlHelper.Action(action, controller, routerValues);
            anchor.InnerHtml = icon + labelText;
            return MvcHtmlString.Create(anchor.ToString());
        }

        public static MvcHtmlString CustomLink(this HtmlHelper htmlHelper, string labelText, string action, string controller, PermissonController permisson, object routerValues = null, object htmlAttributes = null, string icon = "", string toltip = "")
        {
            RouteValueDictionary routerVal = routerValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routerValues);
            string areaName = "";
            if (routerVal.ContainsKey("area"))
            {
                areaName = routerVal["area"].ToString();
            }
            if (!permisson.hasPermisson(areaName, controller, action) || (string.IsNullOrWhiteSpace(labelText) && string.IsNullOrWhiteSpace(icon)))
                return new MvcHtmlString("");
            var anchor = new TagBuilder("a");
            if (!string.IsNullOrWhiteSpace(toltip))
            {
                anchor.Attributes.Add("data-toggle", "tooltip");
                anchor.Attributes.Add("title", toltip);
            }
            if (htmlAttributes != null)
                anchor.MergeAttributes(routerVal);
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            anchor.Attributes["href"] = urlHelper.Action(action, controller, routerValues);
            anchor.InnerHtml = icon + labelText;
            return MvcHtmlString.Create(anchor.ToString());
        }



        public static MvcHtmlString CustomRouterLink(this HtmlHelper htmlHelper, PermissonController permisson, string routerName, object routerValues, string labelText, object htmlAttributes = null, string icon = "", string toltip = "")
        {
            bool hasPermisson = true;

            string areaName = "";
            string controllerName = "";
            string action = "";
            RouteValueDictionary routerVal = routerValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routerValues);
            var router = RouteTable.Routes[routerName] as Route;
            if (router != null)
            {
                action = (routerVal["action"] as string ?? router.Defaults["action"] as string) ?? string.Empty;
                controllerName = (routerVal["controller"] as string ?? router.Defaults["controller"] as string) ?? string.Empty;
                areaName = (routerVal["area"] as string ?? router.DataTokens["area"] as string) ?? string.Empty;
            }
            else if (routerValues != null)
            {
                if (routerVal.ContainsKey("action"))
                {
                    action = routerVal["action"].ToString();
                }
                if (routerVal.ContainsKey("controller"))
                {
                    controllerName = routerVal["controller"].ToString();
                }
                if (routerVal.ContainsKey("area"))
                {
                    areaName = routerVal["area"].ToString();
                }
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                hasPermisson = permisson.hasPermisson(action);
            }else
            {
                hasPermisson = permisson.hasPermisson(areaName, controllerName, action);
            }
            if (!hasPermisson || (string.IsNullOrWhiteSpace(labelText) && string.IsNullOrWhiteSpace(icon)))
                return new MvcHtmlString("");
            var anchor = new TagBuilder("a");
            if (!string.IsNullOrWhiteSpace(toltip))
            {
                anchor.Attributes.Add("data-toggle", "tooltip");
                anchor.Attributes.Add("title", toltip);
            }
            if (htmlAttributes != null)
                anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            anchor.Attributes["href"] = urlHelper.RouteUrl(routerName, routerValues);
            anchor.InnerHtml = icon + labelText;
            return MvcHtmlString.Create(anchor.ToString());
        }
        public static MvcHtmlString LabelForEdit<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes=null)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            //Use the DisplayName or PropertyName for the metadata if available.  Otherwise default to the htmlFieldName provided by the user.
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }


            //Create the new label.
            TagBuilder tag = new TagBuilder("label");

            //Add the specified Html attributes
            if(htmlAttributes!=null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            //Specify what property the label is tied to.
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            if (metadata.IsRequired)
            {
                tag.InnerHtml = labelText + " (<span class=\"text-danger\">*</span>)";
            }
            else
                tag.InnerHtml = labelText;
            return MvcHtmlString.Create(tag.ToString());
        }
        public static MvcHtmlString LabelForView<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes=null)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            //Use the DisplayName or PropertyName for the metadata if available.  Otherwise default to the htmlFieldName provided by the user.
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            //Create the new label.
            TagBuilder tag = new TagBuilder("label");

            //Add the specified Html attributes
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            //Specify what property the label is tied to.
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.InnerHtml = labelText;
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}