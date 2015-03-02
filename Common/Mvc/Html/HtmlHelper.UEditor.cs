//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2014-09-11</createdate>
//<author>zhaoyx</author>
//<email>zhaoyx@tunynet.com</email>
//<log date="2014-09-11" version="0.1">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using Newtonsoft.Json;

namespace Spacebuilder.Common
{
    /// <summary>
    /// UEditor的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperUEditorExtensions
    {
        /// <summary>
        /// 输出UEditor编辑器
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="associateId"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditor(this HtmlHelper htmlHelper, string name, string tenantTypeId, long associateId = 0, string value = null, Dictionary<string, object> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("参数名不能为空", "argsname is null");
            }
            htmlHelper.Script("~/Bundle/Scripts/UEditor");
            htmlHelper.Script("~/Bundle/Scripts/AtUser");

            string emotionHtml = htmlHelper.Hidden("list-emotions", SiteUrls.Instance()._EmotionSelector()).ToHtmlString();
            string fileHtml = htmlHelper.Hidden("list-files", SiteUrls.Instance()._AttachmentManage(tenantTypeId, associateId)).ToHtmlString();
            string atUserHtml = htmlHelper.Hidden("list-atuser", SiteUrls.Instance()._AtUsers()).ToHtmlString();
            string musicHtml = htmlHelper.Hidden("list-music", SiteUrls.Instance()._AddMusic()).ToHtmlString();
            string imageHtml = htmlHelper.Hidden("list-images", SiteUrls.Instance()._ImageManage(tenantTypeId, associateId)).ToHtmlString();

            TagBuilder builder = new TagBuilder("span");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            data.Add("tenantTypeId", tenantTypeId);
            data.Add("associateId", associateId);
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
            htmlAttrs.Add("plugin", "ueditor");
            builder.InnerHtml = htmlHelper.TextArea(name, value ?? string.Empty, htmlAttrs).ToString();
            return MvcHtmlString.Create(builder.ToString() + emotionHtml + fileHtml + atUserHtml + musicHtml + imageHtml);
        }

        /// <summary>
        /// 利用ViewModel输出UEditor编辑器
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="associateId"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string tenantTypeId, long associateId = 0, Dictionary<string, object> htmlAttributes = null)
        {
            htmlHelper.Script("~/Bundle/Scripts/UEditor");
            htmlHelper.Script("~/Bundle/Scripts/AtUser");

            string emotionHtml = htmlHelper.Hidden("list-emotions", SiteUrls.Instance()._EmotionSelector()).ToHtmlString();
            string fileHtml = htmlHelper.Hidden("list-files", SiteUrls.Instance()._AttachmentManage(tenantTypeId, associateId)).ToHtmlString();
            string imageHtml = htmlHelper.Hidden("list-images", SiteUrls.Instance()._ImageManage(tenantTypeId, associateId)).ToHtmlString();
            string atUserHtml = htmlHelper.Hidden("list-atuser", SiteUrls.Instance()._AtUsers()).ToHtmlString();
            string musicHtml = htmlHelper.Hidden("list-music", SiteUrls.Instance()._AddMusic()).ToHtmlString();

            TagBuilder builder = new TagBuilder("span");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            data.Add("tenantTypeId", tenantTypeId);
            data.Add("associateId", associateId);
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
            htmlAttrs.Add("plugin", "ueditor");
            builder.InnerHtml = htmlHelper.TextAreaFor(expression, htmlAttrs).ToString();
            return MvcHtmlString.Create(builder.ToString() + emotionHtml + fileHtml + atUserHtml + musicHtml + imageHtml);
        }
    }
}
