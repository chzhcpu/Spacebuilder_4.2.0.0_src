//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using System;
using System.Linq;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯权限验证
    /// </summary>
    public static class AuthorizerExtension
    {

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="authorizer"></param>
        /// <returns></returns>
        public static bool CMS_Contribute(this Authorizer authorizer)
        {
            string errorMessage = string.Empty;
            return authorizer.CMS_Contribute(out errorMessage);
        }

        /// <summary>
        /// 是否具有前台管理资讯的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <returns></returns>
        public static bool CMS_ManageContentItemsForChannel(this Authorizer authorizer)
        {
            IUser currentUser = UserContext.CurrentUser;

            if (currentUser == null)
                return false;

            if (authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                return true;
            ContentFolderModeratorService contentFolderModeratorService = new ContentFolderModeratorService();
            var folderIds = contentFolderModeratorService.GetModeratedFolderIds(currentUser.UserId);
            if (folderIds.Count() > 0)
                return true;
            return false;
        }


        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="errorMessage"></param>
        public static bool CMS_Contribute(this Authorizer authorizer, out string errorMessage)
        {
            errorMessage = "没有权限投稿";
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                errorMessage = "您需要先登录，才能投稿";
                return false;
            }

            bool result = authorizer.AuthorizationService.Check(currentUser, PermissionItemKeys.Instance().CMS_ContentItem());
            if (!result && currentUser.IsModerated)
                errorMessage = Resources.Resource.Description_ModeratedUser_CreateNewsDenied;
            return result;
        }

        /// <summary>
        /// 发布内容项
        /// </summary>
        /// <param name="folder">栏目</param>
        /// <param name="authorizer"></param>
        public static bool CMS_CreateContentItem(this Authorizer authorizer, ContentFolder folder)
        {
            if (authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
            {
                return true;
            }
            return authorizer.CMS_ManageContentFolder(folder);
        }

        /// <summary>
        /// 编辑内容项
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="contentItem"></param>
        public static bool CMS_EditContentItem(this Authorizer authorizer, ContentItem contentItem)
        {
            if (authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                return true;

            if (authorizer.CMS_ManageContentFolder(contentItem.ContentFolder))
                return true;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return false;
            }
            if (contentItem.AuditStatus != AuditStatus.Success && contentItem.UserId == currentUser.UserId)
                return true;
            return false;
        }
        /// <summary>
        /// 删除资讯
        /// </summary>
        public static bool CMS_DeleteContentItem(this Authorizer authorizer, ContentItem contentItem)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return false;
            }

            if (authorizer.CMS_ManageContentFolder(contentItem.ContentFolder))
                return true;

            if (contentItem.AuditStatus != AuditStatus.Success && contentItem.UserId == currentUser.UserId)
                return true;

            return false;
        }

        /// <summary>
        /// 刪除附件
        /// </summary>
        public static bool CMS_DeleteContentAttachment(this Authorizer authorizer, ContentAttachment contentItem)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return false;
            }

            if (authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                return true;

            if (currentUser.UserId == contentItem.UserId)
                return true;

            return false;
        }

        /// <summary>
        /// 管理栏目
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="folder">栏目</param>
        public static bool CMS_ManageContentFolder(this Authorizer authorizer, ContentFolder folder)
        {
            IUser currentUser = UserContext.CurrentUser;

            if (currentUser == null)
                return false;

            if (authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                return true;

            if (folder.Moderators.Any(n => n.UserId == currentUser.UserId))
                return true;

            if (folder.Parents.Any(n => n.Moderators != null && n.Moderators.Any(m => m.UserId == currentUser.UserId)))
                return true;

            return false;
        }
    }
}