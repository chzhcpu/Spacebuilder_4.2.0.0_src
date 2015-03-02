//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-09-18</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-09-18" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Utilities;


namespace Spacebuilder.Bar
{
    /// <summary>
    /// 处理特殊的BarUrl
    /// </summary>
    public class BarUrlHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string url = string.Empty;
            long anchorPostId = context.Request.QueryString.Get<long>("anchorPostId");

            BarPostService barPostService = new BarPostService();
            BarPost post = barPostService.Get(anchorPostId);
            if (post == null)
                WebUtility.Return404(context);

            IBarUrlGetter urlGetter = BarUrlGetterFactory.Get(post.TenantTypeId);

            if (post != null)
            {
                int? childPostIndex = 0;
                if (post.ParentId != 0)
                {
                    childPostIndex = barPostService.GetPageIndexForChildrenPost(post.ParentId, post.PostId);
                }

                int pageIndex = barPostService.GetPageIndexForPostInThread(post.ThreadId, anchorPostId);

                url = urlGetter.ThreadDetail(post.ThreadId, pageIndex: pageIndex, anchorPostId: anchorPostId, childPostIndex: childPostIndex);

            }


            context.Response.RedirectPermanent(url);
        }
    }
}
