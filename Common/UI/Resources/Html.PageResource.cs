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
using System.IO;
using Tunynet.UI;
using System.Web.WebPages;
using System.Web;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// PageResource在HtmlHelper的扩展方法
    /// </summary>
    public static class PageResourceExtensions
    {
        /// <summary>
        /// 输出&lt;head&gt;及预置的StyleSheet
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHead BeginHead(this HtmlHelper htmlHelper)
        {
            return BeginHead(htmlHelper, title: null);
        }

        /// <summary>
        /// 输出&lt;head&gt;及预置的StyleSheet
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title">标题</param>
        /// <param name="disableClientCache">是否禁用客户端缓存</param>
        /// <param name="charset">字符集</param>
        /// <returns>返回MvcHead对象</returns>
        public static MvcHead BeginHead(this HtmlHelper htmlHelper, string title = null, bool disableClientCache = false, string charset = "UTF-8")
        {
            return new MvcHead(htmlHelper.ViewContext, title, disableClientCache, charset);
        }

        /// <summary>
        /// 引入js文件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="scriptUrl">脚本文件地址（比如：~/Scripts/Site.js）</param>
        /// <returns>空字符串，只有在调用RenderScripts方法时，才会统一输出</returns>
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, string scriptUrl)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.IncludeScript(scriptUrl);
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 批量输出js文件引用
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            IList<string> scripts = pageResourceManager.GetIncludedScriptHtmls();
            pageResourceManager.ClearIncludedScripts();
            return MvcHtmlString.Create(string.Join(Environment.NewLine, scripts));
        }

        /// <summary>
        /// 引入js代码块
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="template">代码块</param>
        /// <returns></returns>
        public static MvcHtmlString ScriptBlock(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.RegisterScriptBlock(template(null).ToHtmlString());
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 批量输出js代码块
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderScriptBlocks(this HtmlHelper htmlHelper)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            IList<string> scriptBlocks = pageResourceManager.GetRegisteredScriptBlocks();
            pageResourceManager.ClearRegisteredScriptBlocks();
            return MvcHtmlString.Create(string.Join(Environment.NewLine, scriptBlocks));
        }

        /// <summary>
        /// 引入css文件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="styleUrl">样式文件地址（比如：~/Styles/Site.css）</param>
        /// <returns>空字符串，只有在调用RenderStyles方法时，才会统一输出</returns>
        public static MvcHtmlString Style(this HtmlHelper htmlHelper, string styleUrl)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.IncludeStyle(styleUrl);
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// 批量输出css文件引用
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderStyles(this HtmlHelper htmlHelper)
        {
            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            IList<string> styles = pageResourceManager.GetIncludedStyleHtmls();
            pageResourceManager.ClearIncludedStyles();
            return MvcHtmlString.Create(string.Join(Environment.NewLine, styles));
        }
    }
}
