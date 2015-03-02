//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Routing;
using Spacebuilder.CMS;
using Tunynet.Common;

using Tunynet.Utilities;
using System.Collections.Generic;
using Spacebuilder.Common;
using System;
using Tunynet.FileStore;
using Tunynet;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯链接管理
    /// </summary>
    public static class SiteUrlsExtension
    {
        private static readonly string CmsAreaName = CmsConfig.Instance().ApplicationKey;

        #region 媒体库

        /// <summary>
        /// 上传附件列表
        /// </summary>
        public static string _ListContentAttachments(this SiteUrls siteUrls, string keyword = null, DateTime? startDate = null, DateTime? endDate = null, MediaType? mediaType = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary() { { "t", new Random().Next(1, 100).ToString() } };
            if (!string.IsNullOrEmpty(keyword))
                dic["keyword"] = WebUtility.UrlEncode(keyword);
            if (startDate != null)
                dic["startDate"] = startDate;
            if (endDate != null)
                dic["endDate"] = endDate;
            if (mediaType != null)
                dic["mediaType"] = mediaType;
            return CachedUrlHelper.Action("_ListContentAttachments", "ChannelCms", CmsAreaName, dic);
        }

        /// <summary>
        /// 附件上传管理
        /// </summary>  
        /// <returns></returns>
        public static string _ContentAttachmentManage(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_ContentAttachmentManage", "ChannelCms", CmsAreaName, new RouteValueDictionary() { { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <returns></returns>
        public static string _EditContentAttachment(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_EditContentAttachment", "ChannelCms", CmsAreaName, new RouteValueDictionary() { { "t", new Random().Next(1, 100).ToString() } });
        }

        /// <summary>
        /// 批量/单个删除附件
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="attachmentId">资讯ID</param>
        public static string _DeleteContentAttachments(this SiteUrls siteUrls, long? attachmentId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (attachmentId != null)
                routeValueDictionary.Add("attachmentIds", attachmentId);

            return CachedUrlHelper.Action("_DeleteContentAttachments", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 获取附件下载的URL
        /// </summary>
        /// <param name="attachmentId">附件Id</param>
        /// <param name="enableCaching">是否缓存</param>
        public static string ContentAttachmentUrl(this SiteUrls siteUrls, long attachmentId, bool enableCaching = true)
        {
            return WebUtility.ResolveUrl(string.Format("~/Handlers/ContentAttachmentAuthorize.ashx?attachmentId={0}&enableCaching={1}", attachmentId, enableCaching));
        }

        /// <summary>
        /// 附件下载的临时地址，根据设定的时间自动过期
        /// </summary>
        /// <param name="attachmentId">附件id</param>
        /// <param name="token">加密串</param>
        /// <param name="enableCaching">是否缓存</param>
        /// <returns></returns>
        public static string ContentAttachmentTempUrl(this SiteUrls siteUrls, long attachmentId, string token, bool enableCaching = true)
        {
            return WebUtility.ResolveUrl(string.Format("~/Handlers/ContentAttachment.ashx?attachmentId={0}&token={1}&enableCaching={2}", attachmentId, token, enableCaching));
        }

        /// <summary>
        /// 附件下载的直连地址
        /// </summary>
        /// <param name="attachment">附件实体</param>
        /// <param name="enableClientCaching">是否缓存</param>
        /// <returns></returns>
        public static string ContentAttachmentDirectUrl(this SiteUrls siteUrls, ContentAttachment attachment, bool enableCaching = true)
        {
            if (attachment == null)
            {
                return string.Empty;
            }
            IStoreProvider storeProvider = DIContainer.Resolve<IStoreProvider>();
            string attachmentPath = attachment.GetRelativePath() + "/" + attachment.FileName;
            if (enableCaching)
            {
                return storeProvider.GetDirectlyUrl(attachmentPath);
            }
            else
            {
                return storeProvider.GetDirectlyUrl(attachmentPath, DateTime.Now);
            }
        }

        /// <summary>
        /// 上传资讯附件
        /// </summary>
        public static string UploadContentAttachment(this SiteUrls siteUrls, string CurrentUserIdToken)
        {
            return CachedUrlHelper.Action("UploadContentAttachment", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "CurrentUserIdToken", CurrentUserIdToken } });
        }

        /// <summary>
        /// 上传资讯附件
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="timeliness">时间限制</param>
        /// <returns></returns>
        public static string UploadContentAttachment(this SiteUrls siteUrls, long userId, double timeliness = 0.1)
        {
            return siteUrls.UploadContentAttachment(Utility.EncryptTokenForUploadfile(timeliness, userId));
        }

        /// <summary>
        /// 预览视频
        /// </summary>
        /// <returns></returns>
        public static string _PreviewVideo(this SiteUrls siteUrls, long attachmentId)
        {
            return CachedUrlHelper.Action("_PreviewVideo", "ChannelCms", CmsAreaName, new RouteValueDictionary() { { "attachmentId", attachmentId }, { "t", new Random().Next(1, 100).ToString() } });
        }

        #endregion

        #region 频道资讯

        /// <summary>
        /// 资讯首页
        /// </summary>
        public static string CmsHome(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Home", "ChannelCms", CmsAreaName);
        }

        /// <summary>
        /// 资讯详细显示页
        /// </summary>
        public static string ContentItemDetail(this SiteUrls siteUrls, long contentItemId)
        {
            var contentItemService = new ContentItemService();
            var item = contentItemService.Get(contentItemId);
            if (item == null)
                return string.Empty;
            if (item.ContentType != null && item.ContentType.ContentTypeKey == ContentTypeKeys.Instance().NewsLink())
                return item.AdditionalProperties.Get<string>("LinkUrl", string.Empty);

            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("contentItemId", contentItemId);

            return CachedUrlHelper.Action("ContentItemDetail", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 栏目详细显示页
        /// </summary>
        public static string FolderDetail(this SiteUrls siteUrls, long contentFolderId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("contentFolderId", contentFolderId);

            return CachedUrlHelper.Action("FolderDetail", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除投稿
        /// </summary>
        public static string _DeleteContribute(this SiteUrls siteUrls, long contentItemId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("contentItemId", contentItemId);

            return CachedUrlHelper.Action("_DeleteContribute", "ChannelCms", CmsAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 投稿
        /// </summary>
        public static string Contribute(this SiteUrls siteUrls, long contentItemId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("contentItemId", contentItemId);

            return CachedUrlHelper.Action("Contribute", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 标签详细
        /// </summary>
        public static string CmsTagDetail(this SiteUrls siteUrls, string tagName)
        {
            return CachedUrlHelper.Action("TagDetail", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "tagName", tagName.TrimEnd('.') } });
        }

        /// <summary>
        /// 我的资讯
        /// </summary>
        public static string CmsUser(this SiteUrls siteUrls, long userId)
        {
            return CachedUrlHelper.Action("CmsUser", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "spaceKey", UserIdToUserNameDictionary.GetUserName(userId) } });
        }

        /// <summary>
        /// 资讯评论
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="contentItemId"></param>
        /// <returns></returns>
        public static string Comments(this SiteUrls siteUrls, long contentItemId, long? commentId = null)
        {
            return CachedUrlHelper.Action("Comments", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "contentItemId", contentItemId } }) + (commentId.HasValue ? "#" + commentId.Value : string.Empty);
        }

        /// <summary>
        /// 最新资讯列表
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string _LastestItems(this SiteUrls siteUrls, int? pageSize = null, int? pageIndex = null)
        {
            return CachedUrlHelper.Action("_LastestItems", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "pageSize", pageSize }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 获取子级栏目
        /// </summary>
        public static string GetChildContentFolders(this SiteUrls siteUrls, string contentTypeKey = null, int exceptFolderId = 0, bool? onlyModerated = null)
        {
            return CachedUrlHelper.Action("GetChildContentFolders", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "contentTypeKey", contentTypeKey }, { "exceptFolderId", exceptFolderId }, { "onlyModerated", onlyModerated } });
        }

        /// <summary>
        /// 标签控件
        /// </summary>
        public static string _TagSelector(this SiteUrls siteUrls, long contentItemId, int contentFolderId)
        {
            return CachedUrlHelper.Action("_TagSelector", "ChannelCms", CmsAreaName, new RouteValueDictionary { { "contentItemId", contentItemId }, { "contentFolderId", contentFolderId } });
        }

        #endregion


        #region 资讯前台管理
        /// <summary>
        /// 资讯前台管理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="folderId"></param>
        public static string ChannelManageContentItems(this SiteUrls siteUrls, AuditStatus? auditStatus = null, int? folderId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (auditStatus.HasValue)
            {
                dic.Add("auditStatus", auditStatus);
            }
            if (folderId.HasValue && folderId.Value > 0)
            {
                dic.Add("folderId", folderId);
            }
            return CachedUrlHelper.Action("ManageContentItems", "ChannelCms", CmsAreaName, dic);
        }

        /// <summary>
        /// 批量/单个删除资讯
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="contentItemId">资讯ID</param>
        public static string _DeleteContentItems(this SiteUrls siteUrls, long? contentItemId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (contentItemId != null)
                routeValueDictionary.Add("contentItemIds", contentItemId);

            return CachedUrlHelper.Action("_DeleteContentItems", "ChannelCms", CmsAreaName, routeValueDictionary);
        }


        /// <summary>
        /// 前台置顶
        /// </summary>
        /// <param name="siteurls"></param>
        /// <param name="contentItemId">资讯Id</param>
        /// <param name="isGlobalSticky">是否整站置顶</param>
        /// <param name="isFolderSticky">是否栏目置顶</param>
        /// <param name="globalStickyDate">整站置顶时间</param>
        /// <param name="folderStickyDate">栏目置顶时间</param>
        /// <returns></returns>
        public static string _CmsSetSticky(this SiteUrls siteurls, bool isGlobalSticky, bool isFolderSticky, DateTime? globalStickyDate = null, DateTime? folderStickyDate = null, long? contentItemId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (contentItemId.HasValue)
            {
                routeValueDictionary.Add("contentItemId", contentItemId);

            }
            routeValueDictionary.Add("isFolderSticky", isFolderSticky);
            routeValueDictionary.Add("isGlobalSticky", isGlobalSticky);
            routeValueDictionary.Add("globalStickyDate", globalStickyDate);
            routeValueDictionary.Add("folderStickyDate", folderStickyDate);
            return CachedUrlHelper.Action("_CmsSetSticky", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 设置置顶时间
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="contentItemId">资讯Id</param>
        /// <returns></returns>
        public static string _SetStickyDate(this SiteUrls siteUrls, long? contentItemId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("contentItemIds", contentItemId);
            return CachedUrlHelper.Action("_SetStickyDate", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量更新资讯的审核状态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApprove">审核状态</param>
        public static string CmsUpdateAuditStatus(this SiteUrls siteUrls, bool isApproved = true, long? contentItemId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (contentItemId.HasValue)
            {
                routeValueDictionary.Add("contentItemIds", contentItemId);
            }
            routeValueDictionary.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("_UpdateAuditStatus", "ChannelCms", CmsAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 设置资讯栏目
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="contentItemId">被设置的单条栏目</param>
        /// <returns></returns>
        public static string _SetContentFolder(this SiteUrls urls, long? contentItemId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (contentItemId.HasValue)
                dic.Add("contentItemIds", contentItemId);
            return CachedUrlHelper.Action("_SetContentFolder", "ChannelCms", CmsAreaName, dic);
        }

        #endregion

        #region 资讯搜索
        /// <summary>
        /// 资讯全局搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string CmsGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "ChannelCms", CmsAreaName);
        }

        /// <summary>
        /// 资讯快捷搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string CmsQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "ChannelCms", CmsAreaName);
        }

        /// <summary>
        /// 资讯搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string CmsPageSearch(this SiteUrls siteUrls, string keyword = "")
        {
            keyword = WebUtility.UrlEncode(keyword);
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            return CachedUrlHelper.Action("Search", "ChannelCms", CmsAreaName, dic);
        }

        /// <summary>
        /// 资讯搜索自动完成
        /// </summary>
        public static string CmsSearchAutoComplete(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SearchAutoComplete", "ChannelCms", CmsAreaName);
        }

        #endregion

        #region 资讯后台管理
        /// <summary>
        /// 资讯后台管理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="auditStatus">审核状态</param>
        public static string CmsControlPanelManage(this SiteUrls siteUrls, AuditStatus? auditStatus = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (auditStatus.HasValue)
            {
                dic.Add("auditStatus", auditStatus);
            }
            return CachedUrlHelper.Action("ManageContentItems", "ControlPanelCms", CmsAreaName, dic);
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="contentTypeId"></param>
        /// <param name="contentItemId">审核状态</param>
        public static string EditContentItem(this SiteUrls siteUrls, long? contentItemId = null, int? contentTypeId = null)
        {

            RouteValueDictionary dic = new RouteValueDictionary();
            if (contentItemId.HasValue)
            {
                dic.Add("contentItemId", contentItemId);
            }
            if (contentTypeId.HasValue)
            {
                dic.Add("contentTypeId", contentTypeId);
            }
            return CachedUrlHelper.Action("EditContentItem", "ControlPanelCms", CmsAreaName, dic);
        }


        #region 资讯栏目管理
        /// <summary>
        /// 资讯栏目管理
        /// </summary>
        /// <returns></returns>
        public static string ManageContentFolders(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("ManageContentFolders", "ControlPanelCms", CmsAreaName);
        }

        /// <summary>
        /// 更改显示顺序
        /// </summary>
        /// <returns></returns>
        public static string ChangeContentFolderOrder(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("ChangeContentFolderOrder", "ControlPanelCms", CmsAreaName);
        }

        /// <summary>
        /// 删除资讯栏目
        /// </summary>
        /// <param name="contentFolderId">栏目ID</param>
        /// <returns></returns>
        public static string DeleteContentFolder(this SiteUrls siteUrls, int contentFolderId)
        {
            return CachedUrlHelper.Action("DeleteContentFolder", "ControlPanelCms", CmsAreaName, new RouteValueDictionary() { { "contentFolderId", contentFolderId } });
        }

        /// <summary>
        /// 添加编辑类别页
        /// </summary>
        public static string _EditContentFolderLink(this SiteUrls siteUrls, int contentFolderId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            if (contentFolderId != 0)
            {
                dic.Add("contentFolderId", contentFolderId);
            }

            return CachedUrlHelper.Action("_EditContentFolderLink", "ControlPanelCms", CmsAreaName, dic);
        }


        /// <summary>
        /// 添加编辑类别页
        /// </summary>
        public static string EditContentFolder(this SiteUrls siteUrls, int contentFolderId = 0, int parentId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            if (contentFolderId != 0)
            {
                dic.Add("contentFolderId", contentFolderId);
            }

            if (parentId != 0)
            {
                dic.Add("parentId", parentId);
            }

            return CachedUrlHelper.Action("EditContentFolder", "ControlPanelCms", CmsAreaName, dic);
        }

        /// <summary>
        /// 合并移动资讯栏目页
        /// </summary>
        public static string _MoveContentFolder(this SiteUrls siteUrls, int fromContentFolderId = 0, string option = "move")
        {
            return CachedUrlHelper.Action("_MoveContentFolder", "ControlPanelCms", CmsAreaName, new RouteValueDictionary { { "fromContentFolderId", fromContentFolderId }, { "option", option } });
        }

        /// <summary>
        /// 合并移动资讯栏目
        /// </summary>
        public static string _MoveContentFolder(this SiteUrls siteUrls, int fromContentFolderId = 0, int contentFolderId = 0, string option = "move")
        {
            return CachedUrlHelper.Action("_MoveContentFolder", "ControlPanelCms", CmsAreaName, new RouteValueDictionary { { "fromContentFolderId", fromContentFolderId }, { "contentFolderId", contentFolderId }, { "option", option } });
        }

        #endregion

        /// <summary>
        /// 资讯后台管理
        /// </summary>
        /// <param name="siteUrls"></param>
        public static string ManageContentAttachments(this SiteUrls siteUrls)
        {            
            return CachedUrlHelper.Action("ManageContentAttachments", "ControlPanelCms", CmsAreaName);
        }


        #endregion




    }
}
