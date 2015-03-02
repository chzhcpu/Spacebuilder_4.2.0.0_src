//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Common;


namespace Spacebuilder.CMS
{
    /// <summary>
    /// 对HtmlEditorOptions进行扩展
    /// </summary>
    public static class HtmlEditorOptionsExtension
    {
        /// <summary>
        /// 增加上传文件按钮
        /// </summary>
        /// <returns></returns>
        public static HtmlEditorOptions AddUploadFileButton(this HtmlEditorOptions htmlEditorOptions)
        {
            if (htmlEditorOptions.CustomButtons == null)
                htmlEditorOptions.CustomButtons = new Dictionary<string, string>();

            string url = SiteUrls.Instance()._ContentAttachmentManage();
            htmlEditorOptions.CustomButtons["fileButton"] = url;

            return htmlEditorOptions;
        }
    }
}