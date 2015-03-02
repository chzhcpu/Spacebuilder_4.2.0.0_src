//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展对js控件的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperAjaxFormExtensions
    {
        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxForm(this HtmlHelper htmlHelper, AjaxFormOptions options)
        {
            return FormHelper(htmlHelper, null /* formAction */ , FormMethod.Post, options, new RouteValueDictionary());
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method, AjaxFormOptions options)
        {
            return BeginAjaxForm(htmlHelper, actionName, controllerName, null /* values */, method, options, null /* htmlAttributes */);
        }
        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, FormMethod method, AjaxFormOptions options)
        {
            return BeginAjaxForm(htmlHelper, actionName, controllerName, routeValues, method, options, null /* htmlAttributes */);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <param name="htmlAttributes">表单html属性集合</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, FormMethod method, AjaxFormOptions options, object htmlAttributes)
        {
            RouteValueDictionary newValues = new RouteValueDictionary(routeValues);
            RouteValueDictionary newAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return BeginAjaxForm(htmlHelper, actionName, controllerName, newValues, method, options, newAttributes);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxForm(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, FormMethod method, AjaxFormOptions options)
        {
            return BeginAjaxForm(htmlHelper, actionName, controllerName, routeValues, method, options, null /* htmlAttributes */);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <param name="htmlAttributes">表单html属性集合</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxForm(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, FormMethod method, AjaxFormOptions options, IDictionary<string, object> htmlAttributes)
        {
            // get target URL
            string formAction = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues ?? new RouteValueDictionary(), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true /* includeImplicitMvcValues */);
            return FormHelper(htmlHelper, formAction, method, options, htmlAttributes);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="routeName"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxRouteForm(this HtmlHelper htmlHelper, string routeName, FormMethod method, AjaxFormOptions options)
        {
            return BeginAjaxRouteForm(htmlHelper, routeName, null /* routeValues */, method, options, null /* htmlAttributes */);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxRouteForm(this HtmlHelper htmlHelper, string routeName, object routeValues, FormMethod method, AjaxFormOptions options)
        {
            return BeginAjaxRouteForm(htmlHelper, routeName, (object)routeValues, method, options, null /* htmlAttributes */);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <param name="htmlAttributes">表单html属性集合</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxRouteForm(this HtmlHelper htmlHelper, string routeName, object routeValues, FormMethod method, AjaxFormOptions options, object htmlAttributes)
        {
            RouteValueDictionary newAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return BeginAjaxRouteForm(htmlHelper, routeName, new RouteValueDictionary(routeValues), method, options, newAttributes);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxRouteForm(this HtmlHelper htmlHelper, string routeName, RouteValueDictionary routeValues, FormMethod method, AjaxFormOptions options)
        {
            return BeginAjaxRouteForm(htmlHelper, routeName, routeValues, method, options, null /* htmlAttributes */);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <param name="htmlAttributes">表单html属性集合</param>
        /// <returns>MvcForm</returns>
        public static MvcForm BeginAjaxRouteForm(this HtmlHelper htmlHelper, string routeName, RouteValueDictionary routeValues, FormMethod method, AjaxFormOptions options, IDictionary<string, object> htmlAttributes)
        {
            string formAction = UrlHelper.GenerateUrl(routeName, null /* actionName */, null /* controllerName */, routeValues ?? new RouteValueDictionary(), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, false /* includeImplicitMvcValues */);
            return FormHelper(htmlHelper, formAction, method, options, htmlAttributes);
        }

        /// <summary>
        /// 输出AjaxForm表单
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="formAction"></param>
        /// <param name="method">表单请求方式</param>
        /// <param name="options">异步提交表单选项</param>
        /// <param name="htmlAttributes">表单html属性集合</param>
        /// <returns>MvcForm</returns>
        private static MvcForm FormHelper(this HtmlHelper htmlHelper, string formAction, FormMethod method, AjaxFormOptions options, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("form");
            builder.MergeAttributes(htmlAttributes);
            if (!string.IsNullOrEmpty(formAction))
                builder.MergeAttribute("action", formAction);
            builder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
            builder.MergeAttributes(options.ToHtmlAttributes());
            htmlHelper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag) + htmlHelper.AntiForgeryToken());
            MvcForm theForm = new MvcForm(htmlHelper.ViewContext);
            return theForm;
        }
    }
}
