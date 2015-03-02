//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using Tunynet.Utilities;
using Spacebuilder;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Tunynet.Common;
using Tunynet;
using Spacebuilder.Common;
using Spacebuilder.CMS.Metadata;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 栏目联动下拉列表
    /// </summary>
    public static class HtmlHelperFolderDropDownListExtensions
    {
        /// <summary>
        /// 栏目下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">选择实体中栏目属性的lamda表达式</param>
        /// <param name="contentTypeKey">数据模型Id</param>
        /// <param name="exceptFolderId">需要去掉的ID</param>
        /// <param name="folderLevel">栏目层级(默认取站点配置）</param>
        public static MvcHtmlString FolderDropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int>> expression, string contentTypeKey = null, int exceptFolderId = 0, bool? onlyModerated = null, int? folderLevel = null)
        {
            string getChildFoldersUrl = SiteUrls.Instance().GetChildContentFolders(contentTypeKey, exceptFolderId, onlyModerated);
            FolderServiceHelper folderServiceHelper = new FolderServiceHelper(contentTypeKey, exceptFolderId, onlyModerated);
            if (folderLevel == null)
            {
                folderLevel = 4;
            }
            return htmlHelper.LinkageDropDownListFor<TModel, int>(expression, 0, folderLevel.Value, folderServiceHelper.GetRootFolderDictionary(), folderServiceHelper.GetParentId, folderServiceHelper.GetChildrenDictionary, getChildFoldersUrl);
        }

        /// <summary>
        /// 栏目下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">控件name属性</param>
        /// <param name="value">选中的栏目Id</param>
        /// <param name="contentTypeKey">数据模型Id</param>
        /// <param name="exceptFolderId">需要去掉的ID</param>
        /// <param name="folderLevel">栏目层级(默认取站点配置）</param>
        public static MvcHtmlString FolderDropDownList(this HtmlHelper htmlHelper, string name, int value, string contentTypeKey = null, int exceptFolderId = 0, bool? onlyModerated = null, int? folderLevel = null, string optionLabel = "请选择")
        {
            string getChildFoldersUrl = SiteUrls.Instance().GetChildContentFolders(contentTypeKey, exceptFolderId, onlyModerated);

            FolderServiceHelper folderServiceHelper = new FolderServiceHelper(contentTypeKey, exceptFolderId, onlyModerated);
            if (folderLevel == null)
            {
                folderLevel = 4;
            }
            return htmlHelper.LinkageDropDownList<int>(name, value, 0, folderLevel.Value, folderServiceHelper.GetRootFolderDictionary(), folderServiceHelper.GetParentId, folderServiceHelper.GetChildrenDictionary, getChildFoldersUrl, optionLabel);
        }
    }

    /// <summary>
    /// 栏目业务逻辑扩展类
    /// </summary>
    internal class FolderServiceHelper
    {
        private string contentTypeKey = null;
        private int exceptFolderId = 0;
        private bool? onlyModerated = null;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="contentTypeKey"></param>
        /// <param name="exceptFolderId"></param>
        public FolderServiceHelper(string contentTypeKey, int exceptFolderId, bool? onlyModerated)
        {
            this.contentTypeKey = contentTypeKey;
            this.exceptFolderId = exceptFolderId;
            this.onlyModerated = onlyModerated;
        }
        ContentFolderService contentFolderService = new ContentFolderService();

        /// <summary>
        /// 获取父栏目Id
        /// </summary>
        public int GetParentId(int folderId)
        {
            ContentFolder folder = contentFolderService.Get(folderId);
            if (folder != null)
                return folder.ParentId;
            return 0;
        }

        /// <summary>
        /// 获取子栏目
        /// </summary>
        public Dictionary<int, string> GetChildrenDictionary(int folderId)
        {
            ContentFolder folder = contentFolderService.Get(folderId);
            if (folder == null)
                return null;
            var folders = folder.Children;
            if (!string.IsNullOrEmpty(contentTypeKey))
                folders = folders.Where(n => n.ContentTypeKeys.Split(',').Contains(contentTypeKey));
            if (onlyModerated.HasValue && onlyModerated.Value)
            {
                var authorizer = DIContainer.Resolve<Authorizer>();
                folders = folders.Where(n => authorizer.CMS_ManageContentFolder(n));
            }
            return folders.Where(n => n.ContentFolderId != exceptFolderId && !n.IsLink).ToDictionary(n => n.ContentFolderId, n => StringUtility.Trim(n.FolderName, 7));
        }

        /// <summary>
        /// 获取一级栏目
        /// </summary>
        public Dictionary<int, string> GetRootFolderDictionary()
        {
            var folders = contentFolderService.GetRootFolders();
            if (folders == null)
                return null;
            if (!string.IsNullOrEmpty(contentTypeKey))
                folders = folders.Where(n => n.ContentTypeKeys.Split(',').Contains(contentTypeKey));
            if (onlyModerated.HasValue && onlyModerated.Value)
            {
                var authorizer = DIContainer.Resolve<Authorizer>();
                folders = folders.Where(n => authorizer.CMS_ManageContentFolder(n));
            }
            return folders.Where(n => n.ContentFolderId != exceptFolderId && !n.IsLink)
                        .ToDictionary(n => n.ContentFolderId, n => StringUtility.Trim(n.FolderName, 7));
        }
    }
}