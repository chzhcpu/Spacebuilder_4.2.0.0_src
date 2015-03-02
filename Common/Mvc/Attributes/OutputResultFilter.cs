//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tunynet;
using Tunynet.UI;

namespace Spacebuilder.Common
{
    public class OutputResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            if (filterContext.HttpContext.Request.AcceptTypes != null)
                if (filterContext.HttpContext.Request.AcceptTypes.Contains("application/json"))
                    return;

            string token = filterContext.HttpContext.Request.Form.Get<string>("CurrentUserIdToken", string.Empty);

            if (!string.IsNullOrEmpty(token))
                return;

            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            //输出Script
            IList<string> scriptHtmls = pageResourceManager.GetIncludedScriptHtmls();
            if (scriptHtmls.Count > 0)
            {
                var scripts = MvcHtmlString.Create(string.Join(System.Environment.NewLine, scriptHtmls));
                filterContext.RequestContext.HttpContext.Response.Write(scripts);
                pageResourceManager.ClearIncludedScripts();
            }

            //输出Style
            IList<string> styleHtmls = pageResourceManager.GetIncludedStyleHtmls();
            if (styleHtmls.Count > 0)
            {
                var styles = MvcHtmlString.Create(string.Join(System.Environment.NewLine, styleHtmls));
                filterContext.RequestContext.HttpContext.Response.Write(styles);
                pageResourceManager.ClearIncludedStyles();
            }

            //输出ScriptBlock
            IList<string> scriptBlocks = pageResourceManager.GetRegisteredScriptBlocks();
            if (scriptBlocks.Count > 0)
            {
                var scripts = MvcHtmlString.Create(string.Join(System.Environment.NewLine, scriptBlocks));
                filterContext.RequestContext.HttpContext.Response.Write(scripts);
                pageResourceManager.ClearRegisteredScriptBlocks();
            }
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }
}
