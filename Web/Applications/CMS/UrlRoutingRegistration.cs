//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-11-07</createdate>
//<author>jiangshl</author>
//<email>jiangshl@tunynet.com</email>
//<log date="2012-11-07" version="0.5">创建</log>
//----------------------

using System.Configuration;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet;
using Spacebuilder.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯路由设置
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "CMS"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;


            #region Channel

            //资讯频道-资讯首页
            context.MapRoute(
              "Channel_CMS_Home", // Route name
              "CMS" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "Home", CurrentNavigationId = "10101502" } // Parameter defaults
            );

            //资讯频道-栏目列表
            context.MapRoute(
              "Channel_CMS_FolderDetail", // Route name
              "CMS/f-{contentFolderId}" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "FolderDetail", CurrentNavigationId = "10101501" } // Parameter defaults
            );

            //资讯频道-资讯详情页
            context.MapRoute(
              "Channel_CMS_ContentItemDetail", // Route name
              "CMS/c-{contentItemId}" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "ContentItemDetail", CurrentNavigationId = "10101501" } // Parameter defaults
            );

            //资讯频道-标签详情页
            context.MapRoute(
              "Channel_CMS_TagDetail", // Route name
              "CMS/t-{tagName}" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "TagDetail", CurrentNavigationId = "10101501" } // Parameter defaults
            );

            //资讯频道-我的资讯
            context.MapRoute(
              "Channel_CMS_User", // Route name
              "CMS/u/{SpaceKey}" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "CmsUser", CurrentNavigationId = "10101506" } // Parameter defaults
            );

            //资讯频道-我的资讯
            context.MapRoute(
              "Channel_CMS_My", // Route name
              "CMS/My" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "CmsUser", CurrentNavigationId = "10101506" } // Parameter defaults
            );

            //资讯频道-投稿
            context.MapRoute(
              "Channel_CMS_Contribute", // Route name
              "CMS/Contribute" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelCms", action = "Contribute", CurrentNavigationId = "10101501" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_CMS_Common", // Route name
                "CMS/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelCms", action = "Home" } // Parameter defaults
            );

            #endregion

            #region 动态列表控件路由

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateCmsComment", TenantTypeIds.Instance().Comment()), // Route name
                "CmsActivity/CreateComment/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelCms", action = "_CreateCmsComment" } // Parameter defaults
            );

            #endregion


            #region ControlPanel
            context.MapRoute(
                "ControlPanel_CMS_Home", // Route name
                "ControlPanel/Content/CMS/ManageContentItems" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelCms", action = "ManageContentItems", CurrentNavigationId = "20101501" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_CMS_Common", // Route name
                "ControlPanel/Content/CMS/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelCms", CurrentNavigationId = "20101501" } // Parameter defaults
            );

            #endregion

            #region Handler
            context.Routes.MapHttpHandler<ContentAttachmentAuthorizeHandler>("ContentAttachmentAuthorize", "Handlers/ContentAttachmentAuthorize.ashx");
            context.Routes.MapHttpHandler<ContentAttachmentDownloadHandler>("ContentAttachment", "Handlers/ContentAttachment.ashx");
            #endregion

        }
    }
}