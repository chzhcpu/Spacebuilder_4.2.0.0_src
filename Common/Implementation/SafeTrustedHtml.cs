//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Logging;
using Tunynet.Utilities;
using Tunynet.Common;
using System.Web;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 当前操作者信息获取器
    /// </summary>
    public class SafeTrustedHtml : TrustedHtml
    {
        #region 构造器
        private bool _encodeHtml = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SafeTrustedHtml()
            : this(false)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="encodeHtml">是否需要htmlencode</param>
        public SafeTrustedHtml(bool encodeHtml)
            : base(encodeHtml)
        {
            this._encodeHtml = encodeHtml;
        }

        #endregion

        public override bool IsSafeAttribute(string tag, string attr, string attrVal)
        {
            if (attr.ToLowerInvariant() == "src" || attr.ToLowerInvariant() == "href")
                attrVal = Microsoft.Security.Application.Encoder.UrlEncode(attrVal);
            return base.IsSafeAttribute(tag, attr, attrVal);
        }

        /// <summary>
        /// 编辑器中受信任的标签
        /// </summary>
        public override TrustedHtml HtmlEditor()
        {
            if (!addedRules.ContainsKey(TrustedHtmlLevel.HtmlEditor))
            {
                addedRules[TrustedHtmlLevel.HtmlEditor] = new SafeTrustedHtml(_encodeHtml)
                           .AddTags("h1", "h2", "h3", "h4", "h5", "h6", "h7", "strong", "em", "u", "b", "i",
                                   "strike", "sub", "sup", "font", "blockquote", "ul", "ol", "li", "p",
                                   "address", "div", "hr", "br", "a", "span", "img", "table", "tbody", "th",
                                   "td", "tr", "pre", "code", "xmp", "object", "param", "embed")

                           .AddGlobalAttributes("align", "id", "style")

                           .AddAttributes("font", "size", "color", "face")
                           .AddAttributes("blockquote", "dir")
                           .AddAttributes("p", "dir")
                           .AddAttributes("em", "rel")
                           .AddAttributes("a", "href", "title", "name", "target", "rel")
                           .AddAttributes("img", "src", "alt", "title", "border", "width", "height")
                           .AddAttributes("table", "border", "cellpadding", "cellspacing", "bgcorlor", "width")
                           .AddAttributes("th", "bgcolor", "width")
                           .AddAttributes("td", "rowspan", "colspan", "bgcolor", "width")
                           .AddAttributes("pre", "name", "class")
                           .AddAttributes("object", "classid", "codebase", "width", "height", "data", "type")
                           .AddAttributes("param", "name", "value")
                           .AddAttributes("embed", "name", "value", "type", "src", "width", "height", "quality", "wmode", "scale",
                                          "bgcolor", "vspace", "hspace", "base", "flashvars", "swliveconnect", "allowfullscreen")
                           .AddProtocols("a", "href", "ftp", "http", "https", "mailto")
                           .AddProtocols("img", "src", "http", "https")
                           .AddProtocols("blockquote", "cite", "http", "https")
                           .AddProtocols("cite", "cite", "http", "https");
            }

            return addedRules[TrustedHtmlLevel.HtmlEditor];
        }

    }
}