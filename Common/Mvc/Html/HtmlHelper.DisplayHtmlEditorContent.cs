//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using Tunynet.Common.Configuration;
using Spacebuilder.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperDisplayHtmlEditorContentExtensions
    {

        /// <summary>
        /// 输出html编辑器产生的内容，根据显示区域的宽度自动调整图片尺寸，并引入js脚本
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="content">html编辑器的内容</param>
        /// <param name="imageWidth">显示区域的图片宽度</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayHtmlEditorContent(this HtmlHelper htmlHelper, string tenantTypeId, string content, int imageWidth)
        {
            if (string.IsNullOrEmpty(content))
            {
                return MvcHtmlString.Create(string.Empty);
            }
            htmlHelper.Style("~/Bundle/Styles/CodeHighlighter");
            htmlHelper.Script("~/Bundle/Scripts/CodeHighlighter");

            htmlHelper.Style("~/Bundle/Styles/FancyBox");
            htmlHelper.Script("~/Bundle/Scripts/FancyBox");

            htmlHelper.Script("~/Scripts/tunynet/body.js");

            htmlHelper.Script("~/Scripts/tunynet/insertMedia.js");

            TenantAttachmentSettings tenantAttachmentSettings = TenantAttachmentSettings.GetRegisteredSettings(tenantTypeId);

            content = content.Replace("width=\"" + tenantAttachmentSettings.InlinedImageWidth + "\"", "width=\"" + imageWidth + "\"");

            return MvcHtmlString.Create(content);
        }
    }
}