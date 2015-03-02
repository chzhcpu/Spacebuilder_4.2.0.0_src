//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet.UI;
using System.Collections.Generic;
using System;

namespace Spacebuilder.CMS
{
    [Serializable]
    public class CmsApplication : ApplicationBase
    {
        protected CmsApplication(ApplicationModel model)
            : base(model)
        { }

        /// <summary>
        /// 删除用户在应用中的数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="takeOverUserName">用于接管删除用户时不能删除的内容(例如：用户创建的群组)</param>
        /// <param name="isTakeOver">是否接管被删除用户可被接管的内容</param>
        protected override void DeleteUser(long userId, string takeOverUserName, bool isTakeOver)
        {
            ContentItemService contentItemService = new ContentItemService();
            contentItemService.DeleteUser(userId, takeOverUserName, isTakeOver);
        }

        protected override bool Install(string presentAreaKey, long ownerId)
        {
            return true;
        }

        protected override bool UnInstall(string presentAreaKey, long ownerId)
        {
            return true;
        }

        protected override IEnumerable<Navigation> GetDynamicNavigations(string presentAreaKey, long ownerId = 0)
        {
            List<Navigation> navigations = new List<Navigation>();

            if (presentAreaKey != PresentAreaKeysOfBuiltIn.Channel)
                return navigations;

            ContentFolderService contentFolderService = new ContentFolderService();
            IEnumerable<ContentFolder> contentFolders = contentFolderService.GetRootFolders();

            if (contentFolders != null)
            {
                foreach (var contentFolder in contentFolders)
                {
                    if (!contentFolder.IsEnabled)
                        continue;
                    string url = SiteUrls.Instance().FolderDetail(contentFolder.ContentFolderId);
                    if (contentFolder.IsLink)
                        url = contentFolder.LinkUrl;

                    int navigationId = NavigationService.GenerateDynamicNavigationId(contentFolder.ContentFolderId);
                    Navigation navigation = new Navigation()
                    {
                        ApplicationId = ApplicationId,
                        Depth = 1,
                        NavigationId = navigationId,
                        NavigationText = contentFolder.FolderName,
                        ParentNavigationId = 10101501,
                        IsEnabled = true,
                        NavigationTarget = "_self",
                        NavigationUrl = url,
                        PresentAreaKey = PresentAreaKeysOfBuiltIn.Channel,
                        DisplayOrder = (int)contentFolder.DisplayOrder + 90000000
                    };
                    if (contentFolder.IsLink && contentFolder.IsLinkToNewWindow)
                        navigation.NavigationTarget = "_blank";
                    navigations.Add(navigation);
                }
            }

            return navigations;
        }

    }
}