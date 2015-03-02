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
using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 输出html的head
    /// </summary>
    /// <remarks>
    /// 在head中自动呈现Title、Meta、ShortcutIcon、引入的js/css、注册的js/css代码块
    /// </remarks>
    public class MvcHead : IDisposable
    {

        private bool _disposed;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;
        private readonly IPageResourceManager pageResourceManager;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="title"></param>
        /// <param name="disableClientCache"></param>
        /// <param name="charset"></param>
        public MvcHead(ViewContext viewContext, string title = null, bool disableClientCache = false, string charset = "UTF-8")
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            _viewContext = viewContext;
            _writer = viewContext.Writer;
            pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();

            TagBuilder tagBuilder = new TagBuilder("head");
            _writer.WriteLine(tagBuilder.ToString(TagRenderMode.StartTag));
            _writer.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", charset);

            if (string.IsNullOrEmpty(title))
            {
                if (pageResourceManager.IsAppendSiteName)
                {
                    pageResourceManager.AppendTitleParts(DIContainer.Resolve<ISettingsManager<SiteSettings>>().Get().SiteName);
                }
                title = pageResourceManager.GenerateTitle();
            }

            if (!string.IsNullOrEmpty(title))
                _writer.WriteLine("<title>{0}</title>", title);

            _writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/ico\" href=\"{0}\" />", pageResourceManager.ShortcutIcon);

            if (disableClientCache)
            {
                _writer.WriteLine("<meta http-equiv=\"Pragma\" content=\"no-cache\" />\n");
                _writer.WriteLine("<meta http-equiv=\"no-cache\" />\n");
                _writer.WriteLine("<meta http-equiv=\"Expires\" content=\"-1\" />\n");
                _writer.WriteLine("<meta http-equiv=\"Cache-Control\" content=\"no-cache\" />\n");
            }
            IList<MetaEntry> metas = pageResourceManager.GetRegisteredMetas();
            foreach (var meta in metas)
            {
                _writer.WriteLine(meta.GetRenderingTag());
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                //呈现引入的css
                IList<string> includeStyles = pageResourceManager.GetIncludedStyleHtmls();
                foreach (var includeStyle in includeStyles)
                {
                    _writer.WriteLine(includeStyle);
                }
                pageResourceManager.ClearIncludedStyles();

                //呈现注册的css代码块
                IList<string> styleBlocks = pageResourceManager.GetRegisteredStyleBlocks();
                _writer.WriteLine("<style type=\"text/css\">");
                foreach (var styleBlock in styleBlocks)
                {
                    _writer.WriteLine(styleBlock);
                }
                _writer.WriteLine("</style>");
                pageResourceManager.ClearRegisteredStyleBlocks();

                _writer.WriteLine("</head>");
            }
        }

        /// <summary>
        /// 结束Head
        /// </summary>
        public void EndHead()
        {
            Dispose(true);
        }

    }
}
