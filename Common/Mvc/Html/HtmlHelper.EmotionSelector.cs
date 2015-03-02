//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Spacebuilder.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展对Html编辑器的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperSmileySelectorExtensions
    {
        /// <summary>
        /// 表情选择器
        /// </summary> 
        /// <param name="htmlHelper">被扩展对象</param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString EmotionSelector(this HtmlHelper htmlHelper)
        {
            //htmlHelper.Script("~/Scripts/tunynet/emotionSelector.js");
            return MvcHtmlString.Create(htmlHelper.Icon(IconTypes.Emotion, "表情", new { ntype = "emotion" }).ToHtmlString() + htmlHelper.Hidden("list-emotions", SiteUrls.Instance()._EmotionSelector()).ToHtmlString());
        }
    }
}
