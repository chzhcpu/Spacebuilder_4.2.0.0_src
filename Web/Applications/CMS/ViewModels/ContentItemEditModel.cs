//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Tunynet.Utilities;
using System.Collections.Generic;
using Tunynet.Common;
using System;
using Tunynet;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using Spacebuilder.CMS.Metadata;
using Spacebuilder.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯实体
    /// </summary>
    [Serializable]
    public class ContentItemEditModel
    {
        /// <summary>
        /// 扩展属性
        /// </summary>
        [AllowHtml]
        public IDictionary<string, object> AdditionalProperties { get; set; }

        /// <summary>
        /// 栏目id
        /// </summary>
        [Required(ErrorMessage = "请选择栏目")]
        public int ContentFolderId { get; set; }

        /// <summary>
        /// 内容项id
        /// </summary>
        public long ContentItemId { get; set; }

        /// <summary>
        /// 内容模型id
        /// </summary>
        public int ContentTypeId { get; set; }
        /// <summary>
        /// 作者UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        /// 图片土建id
        /// </summary>
        public long FeaturedImageAttachmentId { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string FeaturedImage { get; set; }
        /// <summary>
        /// 是否用户投稿
        /// </summary>
        public bool IsContributed { get; set; }

        /// <summary>
        /// 是否精华
        /// </summary>
        public bool IsEssential { get; set; }
        /// <summary>
        /// 是否整站置顶
        /// </summary>
        [Display(Name = "整站置顶")]
        public bool IsGlobalSticky { get; set; }
        /// <summary>
        /// 是否栏目置顶
        /// </summary>
        [Display(Name = "栏目置顶")]
        public bool IsFolderSticky { get; set; }

        /// <summary>
        /// 整站置顶时间
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime GlobalStickyDate { get; set; }
        /// <summary>
        /// 栏目置顶时间
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime FolderStickyDate { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// 设置第一张图片为标题图
        /// </summary>
        public bool FirstAsTitleImage { get; set; }
        /// <summary>
        /// 自动分页
        /// </summary>
        public bool AutoPage { get; set; }
        /// <summary>
        /// 是否截取内容前200字作为摘要
        /// </summary>
        public bool TrimBodyAsSummary { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [WaterMark(Content = "在此输入链接地址")]
        [Required(ErrorMessage = "请输入链接地址")]
        [DataType(DataType.Url)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [WaterMark(Content = "在此输入资讯标题")]
        [Required(ErrorMessage = "请输入资讯标题")]
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, MinimumLength = TextLengthSettings.TEXT_SUBJECT_MINLENGTH, ErrorMessage = "最多可以输入{1}个字")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        [Display(Name = "摘要")]
        [StringLength(TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH, ErrorMessage = "最多可以输入{1}个字")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<string> TagNames { get; set; }

        /// <summary>
        /// 获取栏目
        /// </summary>
        public ContentFolder ContentFolder
        {
            get
            {
                var contentFolder = new ContentFolderService().Get(this.ContentFolderId);
                if (contentFolder == null)
                    contentFolder = new ContentFolder();
                return contentFolder;
            }
        }

        /// <summary>
        /// 将EditModel转换成数据库实体
        /// </summary>
        public ContentItem AsContentItem(System.Web.HttpRequestBase Request)
        {
            ContentItem contentItem = null;
            if (this.ContentItemId > 0)
            {
                contentItem = new ContentItemService().Get(this.ContentItemId);
            }
            else
            {
                contentItem = new ContentItem();
                contentItem.Author = UserContext.CurrentUser == null ? "" : UserContext.CurrentUser.DisplayName;
                contentItem.IP = WebUtility.GetIP();
                contentItem.UserId = UserContext.CurrentUser == null ? 0 : UserContext.CurrentUser.UserId;
                contentItem.ContentTypeId = this.ContentTypeId;
            }

            if (this.ContentFolderId > 0)
            {
                ContentFolder folder = new ContentFolderService().Get(this.ContentFolderId);
                if (folder != null)
                {
                    contentItem.ContentFolderId = this.ContentFolderId;
                    if (this.AdditionalProperties == null)
                        this.AdditionalProperties = new Dictionary<string, object>();
                    IEnumerable<ContentTypeColumnDefinition> contentTypeColumnDefinitions = new MetadataService().GetColumnsByContentTypeId(this.ContentTypeId);
                    foreach (var item in contentTypeColumnDefinitions)
                    {
                        object value = null;


                        switch (item.DataType)
                        {
                            case "int":
                            case "long":
                                value = Request.Form.Get<long>(item.ColumnName, 0);
                                break;
                            case "datetime":
                                value = Request.Form.Get<DateTime>(item.ColumnName, DateTime.MinValue);
                                break;

                            case "bool":
                                value = Request.Form.Get<bool>(item.ColumnName, false);
                                break;

                            default:
                                value = Request.Form.Get<string>(item.ColumnName, string.Empty);
                                break;
                        }

                        if (this.AdditionalProperties.ContainsKey(item.ColumnName))
                            this.AdditionalProperties[item.ColumnName] = value;
                        else
                            this.AdditionalProperties.Add(item.ColumnName, value);
                    }
                }
            }
            contentItem.AdditionalProperties = this.AdditionalProperties;
            contentItem.IsEssential = false;
            contentItem.IsLocked = false;

            contentItem.IsGlobalSticky = this.IsGlobalSticky;
            if (this.GlobalStickyDate.CompareTo(DateTime.MinValue) > 0)
                contentItem.GlobalStickyDate = this.GlobalStickyDate;
            contentItem.IsFolderSticky = this.IsFolderSticky;
            if (this.FolderStickyDate.CompareTo(DateTime.MinValue) > 0)
                contentItem.FolderStickyDate = this.FolderStickyDate;
            contentItem.DisplayOrder = this.DisplayOrder;
            contentItem.FeaturedImageAttachmentId = this.FeaturedImageAttachmentId;
            contentItem.LastModified = DateTime.UtcNow;
            contentItem.Title = this.Title;
            if (this.ReleaseDate.CompareTo(DateTime.MinValue) > 0)
                contentItem.ReleaseDate = this.ReleaseDate.ToUniversalTime();
            //摘要
            contentItem.Summary = this.Summary ?? string.Empty;
            if (this.TrimBodyAsSummary)
            {
                string summary = HtmlUtility.TrimHtml(Request.Form.Get<string>("Body", string.Empty), TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH);
                contentItem.Summary = summary;
            }

            if (contentItem.AdditionalProperties.ContainsKey("AutoPage"))
            {
                bool autoPage = Request.Form.Get<bool>("AutoPage", false);
                int pageLength = Request.Form.Get<int>("PageLength", 1000);
                string body = contentItem.AdditionalProperties.Get<string>("Body", string.Empty);
                if (autoPage)
                {
                    pageLength = pageLength > 0 ? pageLength : 1000;
                    if (!string.IsNullOrEmpty(body))
                    {
                        body = body.Replace(ContentPages.PageSeparator, "");
                        contentItem.AdditionalProperties["Body"] = string.Join(ContentPages.PageSeparator, ContentPages.GetPageContentForStorage(body, pageLength, true).ToArray());
                    }
                }
                else
                {
                    pageLength = 0;
                    body = body.Replace(ContentPages.PageSeparator, "");
                    contentItem.AdditionalProperties["Body"] = body;
                    contentItem.AdditionalProperties["pageLength"] = pageLength;
                }
            }

            if (contentItem.AdditionalProperties.ContainsKey("Editor"))
            {
                string editor = contentItem.AdditionalProperties.Get<string>("Editor", string.Empty);
                if (string.IsNullOrEmpty(editor))
                    contentItem.AdditionalProperties["Editor"] = contentItem.ContentFolder.Editor;
            }

            if (contentItem.FeaturedImageAttachmentId > 0)
            {
                AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(TenantTypeIds.Instance().ContentItem());
                Attachment attachment = attachmentService.Get(contentItem.FeaturedImageAttachmentId);
                if (attachment != null)
                {
                    contentItem.FeaturedImage = attachment.GetRelativePath() + "\\" + attachment.FileName;
                }
                else
                {
                    contentItem.FeaturedImageAttachmentId = 0;
                }
            }
            else
            {
                contentItem.FeaturedImage = string.Empty;
            }
            return contentItem;
        }
    }

    /// <summary>
    /// 资讯扩展类
    /// </summary>
    public static class ContentItemExtensions
    {
        /// <summary>
        /// 将数据库实体转换成EditModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ContentItemEditModel AsEditModel(this ContentItem item)
        {
            return new ContentItemEditModel
            {
                AdditionalProperties = item.AdditionalProperties,
                ContentTypeId = item.ContentTypeId,
                ContentFolderId = item.ContentFolderId,
                ContentItemId = item.ContentItemId,
                DisplayOrder = item.DisplayOrder,
                FeaturedImageAttachmentId = item.FeaturedImageAttachmentId,
                IsGlobalSticky = item.IsGlobalSticky,
                GlobalStickyDate = item.GlobalStickyDate,
                IsFolderSticky = item.IsFolderSticky,
                FolderStickyDate = item.FolderStickyDate,
                ReleaseDate = item.ReleaseDate.ToLocalTime(),
                Title = item.Title,
                Summary = item.Summary,
                FeaturedImage = item.FeaturedImage

            };
        }
    }
}