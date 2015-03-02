//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Web;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;
using Tunynet.Utilities;


namespace Spacebuilder.CMS
{
    /// <summary>
    /// 检测文件附件下载权限，如未购买，则先购买
    /// </summary>
    public class ContentAttachmentAuthorizeHandler : DownloadFileHandlerBase
    {
        private IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();

        public override void ProcessRequest(HttpContext context)
        {
            long attachmentId = context.Request.QueryString.Get<long>("attachmentId", 0);
            if (attachmentId <= 0)
            {
                WebUtility.Return404(context);
                return;
            }

            ContentAttachmentService contentAttachmentService = new ContentAttachmentService();
            ContentAttachment attachment = contentAttachmentService.Get(attachmentId);
            if (attachment == null)
            {
                WebUtility.Return404(context);
                return;
            }

            IUser currentUser = UserContext.CurrentUser;

            //下载计数
            CountService countService = new CountService(TenantTypeIds.Instance().ContentAttachment());
            countService.ChangeCount(CountTypes.Instance().DownloadCount(), attachment.AttachmentId, attachment.UserId, 1, false);

            bool enableCaching = context.Request.QueryString.GetBool("enableCaching", true);

            context.Response.Status = "302 Object Moved";
            context.Response.StatusCode = 302;

            LinktimelinessSettings linktimelinessSettings = DIContainer.Resolve<ISettingsManager<LinktimelinessSettings>>().Get();
            string token = Utility.EncryptTokenForAttachmentDownload(linktimelinessSettings.Highlinktimeliness, attachmentId);
            context.Response.Redirect(SiteUrls.Instance().ContentAttachmentTempUrl(attachment.AttachmentId, token, enableCaching), true);
            context.Response.Flush();
            context.Response.End();
        }


    }
}