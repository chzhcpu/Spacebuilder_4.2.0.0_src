//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Spacebuilder.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 创建栏目的类
    /// </summary>
    [Serializable]
    public class FolderEditModel
    {
        /// <summary>
        /// ContentFolderId
        /// </summary>
        public int ContentFolderId { get; set; }

        private string folderName = string.Empty;
        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "栏目名称")]
        [Required(ErrorMessage = "请输入栏目名称")]
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, ErrorMessage = "栏目名称过长")]
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        }

        private string description = string.Empty;
        /// <summary>
        /// 栏目描述
        /// </summary>
        [Display(Name = "栏目描述")]
        [StringLength(TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH, ErrorMessage = "栏目描述过长")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string editor = string.Empty;
        /// <summary>
        /// 栏目编辑
        /// </summary>
        [Display(Name = "责任编辑")]
        [StringLength(TextLengthSettings.TEXT_NAME_MAXLENGTH, ErrorMessage = "责任编辑名称过长")]
        public string Editor
        {
            get { return editor; }
            set { editor = value; }
        }


        private int? parentId;
        /// <summary>
        /// ParentId
        /// </summary>
        public int? ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        /// 父栏目名称
        /// </summary>
        public string ParentName
        {
            get
            {
                if (this.ParentId != null && this.ParentId.Value > 0)
                {
                    var parentFolder = new ContentFolderService().Get(this.ParentId.Value);
                    if (parentFolder != null)
                        return parentFolder.FolderName;
                }
                return string.Empty;
            }
        }

        private bool isEnabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        private bool needAuditing = true;
        /// <summary>
        /// 是否审核
        /// </summary>
        [Display(Name = "是否审核")]
        public bool NeedAuditing
        {
            get { return needAuditing; }
            set { needAuditing = value; }
        }

        private List<string> contentTypeKeys = new List<string>();
        /// <summary>
        /// 内容模型Id
        /// </summary>
        [Display(Name = "内容模型")]
        [Required(ErrorMessage = "请选择模型")]
        public List<string> ContentTypeKeys
        {
            get { return contentTypeKeys; }
            set { contentTypeKeys = value; }
        }

        private string metaTitle = string.Empty;
        /// <summary>
        /// meta标题
        /// </summary>
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, ErrorMessage = "meta标题过长")]
        public string METATitle
        {
            get { return metaTitle; }
            set { metaTitle = value; }
        }

        private string metaKeywords = string.Empty;
        /// <summary>
        /// meta关键词
        /// </summary>
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, ErrorMessage = "meta关键词过长")]
        public string METAKeywords
        {
            get { return metaKeywords; }
            set { metaKeywords = value; }
        }

        private string metaDescription = string.Empty;
        /// <summary>
        /// meta描述
        /// </summary>
        [StringLength(TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH, ErrorMessage = "meta描述过长")]
        public string METADescription
        {
            get { return metaDescription; }
            set { metaDescription = value; }
        }

        private bool isAsNavigation = true;
        /// <summary>
        /// 是否作为导航显示
        /// </summary>
        public bool IsAsNavigation
        {
            get { return isAsNavigation; }
            set { isAsNavigation = value; }
        }

        private bool isLink;
        /// <summary>
        /// 是否外部链接
        /// </summary>
        public bool IsLink
        {
            get { return isLink; }
            set { isLink = value; }
        }

        private string linkUrl = string.Empty;
        /// <summary>
        /// 链接地址(支持 ~/)
        /// </summary>
        [Display(Name = "链接地址")]
        [Required(ErrorMessage = "请输入链接地址")]
        [StringLength(255, ErrorMessage = "链接地址过长")]
        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }

        private bool isLinkToNewWindow = true;
        /// <summary>
        /// 是否在新窗口打开链接
        /// </summary>
        [Display(Name = "新窗口打开")]
        public bool IsLinkToNewWindow
        {
            get { return isLinkToNewWindow; }
            set { isLinkToNewWindow = value; }
        }

        private string page_List = string.Empty;
        /// <summary>
        /// 默认内容列表页面
        /// </summary>
        [Required(ErrorMessage = "请输入栏目描述")]
        public string Page_List
        {
            get { return page_List; }
            set { page_List = value; }
        }

        private string page_Detail = string.Empty;
        /// <summary>
        /// 默认详细显示页面
        /// </summary>
        [Required(ErrorMessage = "请输入栏目描述")]
        public string Page_Detail
        {
            get { return page_Detail; }
            set { page_Detail = value; }
        }

        private string currentNavigationId = string.Empty;
        /// <summary>
        /// 导航Id
        /// </summary>
        public string CurrentNavigationId
        {
            get { return currentNavigationId; }
            set { currentNavigationId = value; }
        }

        private string icon = string.Empty;
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }


        public ContentFolder AsContentFolder()
        {
            ContentFolder contentFolder = new ContentFolder();

            if (this.ContentFolderId > 0)
                contentFolder = new ContentFolderService().Get(this.ContentFolderId);

            contentFolder.FolderName = this.FolderName;

            contentFolder.Description = this.Description ?? string.Empty;

            contentFolder.IsEnabled = this.IsEnabled;

            contentFolder.IsAsNavigation = this.IsAsNavigation;

            contentFolder.ParentId = this.ParentId.HasValue ? this.ParentId.Value : 0;

            contentFolder.Page_Detail = this.Page_Detail ?? string.Empty;

            contentFolder.Page_List = this.Page_List ?? string.Empty;

            contentFolder.IsLinkToNewWindow = this.IsLinkToNewWindow;

            contentFolder.LinkUrl = this.LinkUrl ?? string.Empty;

            contentFolder.IsLink = this.IsLink;

            contentFolder.ContentTypeKeys = string.Join(",", this.ContentTypeKeys);

            contentFolder.Icon = this.icon ?? string.Empty;

            contentFolder.METATitle = this.METATitle ?? string.Empty;
            contentFolder.METAKeywords = this.METAKeywords ?? string.Empty;
            contentFolder.METADescription = this.METADescription ?? string.Empty;
            contentFolder.NeedAuditing = this.NeedAuditing;
            contentFolder.Editor = this.Editor ?? string.Empty;
            return contentFolder;
        }
    }

    /// <summary>
    /// 栏目的扩展方法
    /// </summary>
    public static class ContentFolderExtensions
    {
        /// <summary>
        /// 转换成AsEditModel
        /// </summary>
        public static FolderEditModel AsEditModel(this ContentFolder contentFolder)
        {
            return new FolderEditModel
            {
                ContentFolderId = contentFolder.ContentFolderId,

                FolderName = contentFolder.FolderName,

                Description = contentFolder.Description,

                IsEnabled = contentFolder.IsEnabled,

                IsAsNavigation = contentFolder.IsAsNavigation,

                ParentId = contentFolder.ParentId,

                Page_Detail = contentFolder.Page_Detail,

                Page_List = contentFolder.Page_List,

                IsLinkToNewWindow = contentFolder.IsLinkToNewWindow,

                LinkUrl = contentFolder.LinkUrl,

                IsLink = contentFolder.IsLink,

                ContentTypeKeys = contentFolder.ContentTypeKeys.Split(',').ToList(),

                Icon = contentFolder.Icon,

                Editor = contentFolder.Editor,

                NeedAuditing = contentFolder.NeedAuditing,

                METATitle = contentFolder.METATitle,

                METAKeywords = contentFolder.METAKeywords,

                METADescription = contentFolder.METADescription
            };
        }
    }
}
