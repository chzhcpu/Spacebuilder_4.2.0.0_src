//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;

using Tunynet.UI;
using Tunynet.Utilities;
using Spacebuilder.Search;
using Tunynet.Search;
using Spacebuilder.CMS.Metadata;
using System;
using System.Web;
using Tunynet.Common.Configuration;
using System.IO;
using DevTrends.MvcDonutCaching;

namespace Spacebuilder.CMS.Controllers
{
    /// <summary>
    /// 资讯控制器
    /// </summary>
    [TitleFilter(TitlePart = "资讯", IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [AnonymousBrowseCheck]
    public class ChannelCmsController : Controller
    {
        public Authorizer authorizer { get; set; }
        public IPageResourceManager pageResourceManager { get; set; }
        public ContentItemService contentItemService { get; set; }
        public ContentFolderService contentFolderService { get; set; }
        public IUserService userService { get; set; }
        public CommentService commentService { get; set; }
        private TagService tagService = new TagService(TenantTypeIds.Instance().ContentItem());

        /// <summary>
        /// 资讯首页
        /// </summary>
        public ActionResult Home()
        {
            pageResourceManager.InsertTitlePart("资讯首页");
            return View();
        }

        #region 媒体库

        /// <summary>
        /// 附件上传的管理
        /// </summary>
        public ActionResult _ContentAttachmentManage()
        {
            return View();
        }

        /// <summary>
        /// 附件上传编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _EditContentAttachment()
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }
            return View();
        }

        /// <summary>
        /// 附件列表
        /// </summary>
        public ActionResult _ListContentAttachments(string keyword = null, DateTime? startDate = null, DateTime? endDate = null, MediaType? mediaType = null, int pageIndex = 1)
        {
            IUser user = UserContext.CurrentUser;
            if (user == null)
            {
                return new EmptyResult();
            }

            long? userId = null;
            if (!authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                userId = user.UserId;
            keyword = WebUtility.UrlDecode(keyword);
            PagingDataSet<ContentAttachment> attachments = new ContentAttachmentService().Gets(userId, keyword, startDate, endDate, mediaType, 12, pageIndex);
            return View(attachments);
        }


        /// <summary>
        /// 附件上传编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _PreviewVideo(long attachmentId)
        {
            ContentAttachment contentAttachment = new ContentAttachmentService().Get(attachmentId);
            if (contentAttachment == null)
                return new EmptyResult();
            return View(contentAttachment);
        }


        /// <summary>
        /// 上传资讯附件
        /// </summary>
        [HttpPost]
        public ActionResult UploadContentAttachment()
        {
            IUser user = UserContext.CurrentUser;

            if (user == null)
            {
                return new EmptyResult();
            }

            ContentAttachmentService attachementService = new ContentAttachmentService();
            long userId = user.UserId;
            string userDisplayName = user.DisplayName;
            long attachmentId = 0;
            if (Request.Files.Count > 0 && Request.Files["Filedata"] != null)
            {
                HttpPostedFileBase postFile = Request.Files["Filedata"];
                string fileName = postFile.FileName;

                string contentType = MimeTypeConfiguration.GetMimeType(postFile.FileName);
                ContentAttachment attachment = new ContentAttachment(postFile, contentType);
                attachment.UserId = userId;
                attachment.UserDisplayName = userDisplayName;

                using (Stream stream = postFile.InputStream)
                {
                    attachementService.Create(attachment, stream);
                }

                attachmentId = attachment.AttachmentId;
            }
            return Json(new { AttachmentId = attachmentId });
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachmentIds"></param>
        [HttpPost]
        public ActionResult _DeleteContentAttachments(List<long> attachmentIds)
        {
            if (attachmentIds == null || attachmentIds.Count <= 0)
                return Json(new StatusMessageData(StatusMessageType.Success, "没有找到需要删除的附件"));

            var ContentAttachmentService = new ContentAttachmentService();
            foreach (long attachmentId in attachmentIds)
            {
                ContentAttachment contentAttachment = ContentAttachmentService.Get(attachmentId);
                if (contentAttachment == null)
                    continue;

                if (authorizer.CMS_DeleteContentAttachment(contentAttachment))
                    ContentAttachmentService.Delete(contentAttachment);
                else
                    return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除此附件的权限"));
            }

            return Json(new StatusMessageData(StatusMessageType.Success, "成功删除了附件"));
        }

        #endregion

        #region 动态内容块
        /// <summary>
        /// 创建资讯评论的动态块
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        //[DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _CreateCmsComment(long ActivityId)
        {
            Activity activity = new ActivityService().Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);

            ContentItem contentItem = contentItemService.Get(activity.ReferenceId);
            if (contentItem == null)
                return Content(string.Empty);
            IEnumerable<Attachment> attachments = new AttachmentService(TenantTypeIds.Instance().ContentItem()).GetsByAssociateId(contentItem.ContentItemId);
            if (attachments != null && attachments.Count() > 0)
            {
                IEnumerable<Attachment> attachmentImages = attachments.Where(n => n.MediaType == MediaType.Image);
                if (attachmentImages != null && attachmentImages.Count() > 0)
                    ViewData["Attachments"] = attachmentImages.FirstOrDefault();
            }

            Comment comment = commentService.Get(activity.SourceId);
            if (comment == null)
                return Content(string.Empty);

            ViewData["contentItem"] = contentItem;
            ViewData["ActivityId"] = ActivityId;
            return View(comment);
        }

        #endregion

        #region 局部页面

        /// <summary>
        /// 最新资讯列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult _LastestItems(int pageSize = 10, int pageIndex = 1)
        {
            var items = contentItemService.GetContentItemsSortBy(ContentItemSortBy.ReleaseDate_Desc, pageSize: pageSize, pageIndex: pageIndex);
            return View(items);
        }

        /// <summary>
        /// 推荐资讯
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendContentItems(string recommendTypeId = null, int topNumber = 8)
        {
            IEnumerable<RecommendItem> recommendItems = new RecommendService().GetTops(topNumber, recommendTypeId);
            ViewData["recommendTypeId"] = recommendTypeId;
            return View(recommendItems);
        }

        /// <summary>
        /// 推荐评论（精彩点评）
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="recommendTypeId"></param>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendComments(string recommendTypeId = null, int topNumber = 5)
        {
            if (CmsConfig.Instance().EnableSocialComment)
                return new EmptyResult();
            IEnumerable<RecommendItem> recommendItems = new RecommendService().GetTops(topNumber, recommendTypeId);
            ViewData["recommendTypeId"] = recommendTypeId;
            return View(recommendItems);
        }

        /// <summary>
        /// 资讯TopN内容块
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="contentFolderId"></param>
        /// <param name="viewName"></param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _Tops(ContentItemSortBy sortBy = ContentItemSortBy.ReleaseDate_Desc, int? contentFolderId = null, string viewName = null, int topNumber = 5)
        {
            var items = contentItemService.GetTops(5, contentFolderId, sortBy);
            if (CmsConfig.Instance().EnableSocialComment && (sortBy == ContentItemSortBy.StageCommentCount || sortBy == ContentItemSortBy.CommentCount))
                return new EmptyResult();
            ViewData["sortBy"] = sortBy;
            ViewData["contentFolderId"] = contentFolderId;
            return View(viewName ?? "_Tops", items);
        }


        /// <summary>
        /// 他的投稿
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _HisContribute(long userId, int topNumber = 5)
        {
            User user = userService.GetFullUser(userId);
            ViewData["user"] = user;
            IEnumerable<ContentItem> items = contentItemService.GetUserContentItems(userId, pageSize: topNumber, pageIndex: 1);
            return View(items);
        }

        /// <summary>
        /// 侧边栏-浏览过本文的人还看过
        /// </summary>
        /// <param name="contentItemId">资讯Id</param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _VisitorAlsoVisited(long contentItemId, int topNumber = 10)
        {
            var items = contentItemService.GetVisitorAlsoVisitedItems(contentItemId, topNumber);
            return View(items);
        }

        #endregion

        #region 资讯详情页
        /// <summary>
        /// 资讯详情页
        /// </summary>
        public ActionResult ContentItemDetail(long contentItemId)
        {
            ContentItem contentItem = contentItemService.Get(contentItemId);
            if (contentItem == null || contentItem.User == null)
            {
                return HttpNotFound();
            }

            //验证是否通过审核
            long currentSpaceUserId = UserIdToUserNameDictionary.GetUserId(contentItem.User.UserName);
            if (!authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId) && contentItem.UserId != currentSpaceUserId
                && (int)contentItem.AuditStatus < (int)(new AuditService().GetPubliclyAuditStatus(CmsConfig.Instance().ApplicationId)))
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Title = "尚未通过审核",
                    Body = "由于当前资讯尚未通过审核，您无法浏览当前内容。",
                    StatusMessageType = StatusMessageType.Hint
                }));

            AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(TenantTypeIds.Instance().ContentItem());


            //更新浏览计数
            CountService countService = new CountService(TenantTypeIds.Instance().ContentItem());
            countService.ChangeCount(CountTypes.Instance().HitTimes(), contentItem.ContentItemId, contentItem.UserId, 1, true);
            if (UserContext.CurrentUser != null)
            {
                //创建访客记录
                VisitService visitService = new VisitService(TenantTypeIds.Instance().ContentItem());
                visitService.CreateVisit(UserContext.CurrentUser.UserId, UserContext.CurrentUser.DisplayName, contentItem.ContentItemId, contentItem.Title);
            }
            //设置SEO信息
            pageResourceManager.InsertTitlePart(contentItem.Title);
            List<string> keywords = new List<string>();
            keywords.AddRange(contentItem.TagNames);
            string keyword = string.Join(" ", keywords.Distinct());
            keyword += " " + string.Join(" ", ClauseScrubber.TitleToKeywords(contentItem.Title));
            pageResourceManager.SetMetaOfKeywords(keyword);
            pageResourceManager.SetMetaOfDescription(contentItem.Summary);
            return View(contentItem);
        }

        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="contentItemId"></param>
        /// <returns></returns>
        public ActionResult Comments(long contentItemId)
        {
            ContentItem item = contentItemService.Get(contentItemId);
            if (item == null)
                return HttpNotFound();
            //设置路由数据中的当前导航Id
            RouteData.Values["CurrentNavigationId"] = "10101501";
            return View(item);
        }

        #endregion

        #region 我的资讯

        /// <summary>
        /// 他的资讯/我的资讯
        /// </summary>
        public ActionResult CmsUser(string spaceKey, int? contentFolderId = null, AuditStatus? auditStatus = null, int pageSize = 50, int pageIndex = 1)
        {
            IUser user = null;
            if (string.IsNullOrEmpty(spaceKey))
            {
                user = UserContext.CurrentUser;
                if (user == null)
                {
                    return Redirect(SiteUrls.Instance().Login(true));
                }
                pageResourceManager.InsertTitlePart("我的资讯");
            }
            else
            {
                user = userService.GetFullUser(spaceKey);
                if (user == null)
                {
                    return HttpNotFound();
                }

                if (!new PrivacyService().Validate(user.UserId, UserContext.CurrentUser != null ? UserContext.CurrentUser.UserId : 0, PrivacyItemKeys.Instance().VisitUserSpace()))
                {
                    if (UserContext.CurrentUser == null)
                        return Redirect(SiteUrls.Instance().Login(true));
                    else
                        return Redirect(SiteUrls.Instance().PrivacyHome(user.UserName));
                }

                if (UserContext.CurrentUser != null && user.UserId == UserContext.CurrentUser.UserId)
                {
                    pageResourceManager.InsertTitlePart("我的资讯");
                }
                else
                {
                    pageResourceManager.InsertTitlePart(user.DisplayName + "的资讯");
                }
            }
            ViewData["user"] = user;
            bool hasManagePermission = UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == user.UserId;
            if (authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                hasManagePermission = true;
            PubliclyAuditStatus? publiclyAuditStatus = null;
            if (hasManagePermission)
            {
                if (auditStatus.HasValue)
                    switch (auditStatus.Value)
                    {
                        case AuditStatus.Again:
                            publiclyAuditStatus = PubliclyAuditStatus.Again;
                            break;
                        case AuditStatus.Fail:
                            publiclyAuditStatus = PubliclyAuditStatus.Fail;
                            break;
                        case AuditStatus.Success:
                            publiclyAuditStatus = PubliclyAuditStatus.Success;
                            break;
                        case AuditStatus.Pending:
                        default:
                            publiclyAuditStatus = PubliclyAuditStatus.Pending;
                            break;
                    }
            }
            else
                publiclyAuditStatus = new AuditService().GetPubliclyAuditStatus(CmsConfig.Instance().ApplicationId);

            PagingDataSet<ContentItem> contentItems = contentItemService.GetUserContentItems(user.UserId, contentFolderId, publiclyAuditStatus, pageSize, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_UserContentItems", contentItems);
            ViewData["hasManagePermission"] = hasManagePermission;
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            ViewData["contributeCount"] = ownerDataService.GetLong(user.UserId, OwnerDataKeys.Instance().ContributeCount());

            return View(contentItems);
        }

        /// <summary>
        /// 删除投稿
        /// </summary>
        /// <param name="contentItemId"></param>
        [HttpPost]
        public ActionResult _DeleteContribute(long contentItemId)
        {
            var contentItemService = new ContentItemService();
            ContentItem contentItem = contentItemService.Get(contentItemId);
            if (contentItem == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到准备删除的投稿"));
            if (authorizer.CMS_DeleteContentItem(contentItem))
                contentItemService.Delete(contentItem);
            else
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除此投稿的权限"));
            return Json(new StatusMessageData(StatusMessageType.Success, "成功删除了投稿"));
        }

        #endregion

        #region 资讯列表

        /// <summary>
        /// 栏目详细页
        /// </summary>
        public ActionResult FolderDetail(int contentFolderId, int pageSize = 15, int pageIndex = 1)
        {
            var contentFolderService = new ContentFolderService();
            var folder = contentFolderService.Get(contentFolderId);
            if (folder == null || !folder.IsEnabled)
            {
                return HttpNotFound();
            }
            pageResourceManager.InsertTitlePart(folder.FolderName);
            pageResourceManager.SetMeta(new MetaEntry("Title", folder.METATitle));
            pageResourceManager.SetMetaOfKeywords(folder.METAKeywords);
            pageResourceManager.SetMetaOfDescription(folder.METADescription);

            PagingDataSet<ContentItem> contentItems = contentItemService.GetContentItemsSortBy(ContentItemSortBy.ReleaseDate_Desc, null, contentFolderId, true, null, pageSize, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", contentItems);
            ViewData["currentFolder"] = folder;

            ViewData["allParentContentFolders"] = folder.Parents;
            //设置路由数据中的当前导航Id
            RouteData.Values["CurrentNavigationId"] = NavigationService.GenerateDynamicNavigationId(folder.Parents.FirstOrDefault() == null ? contentFolderId : folder.Parents.First().ContentFolderId);

            return View(contentItems);
        }

        /// <summary>
        /// 资讯标签详情页
        /// </summary>
        public ActionResult TagDetail(string tagName, int pageSize = 15, int pageIndex = 1)
        {
            //tagName = WebUtility.UrlDecode(tagName);

            var tag = tagService.Get(tagName);

            if (tag == null)
            {
                return HttpNotFound();
            }

            PagingDataSet<ContentItem> contentItems = contentItemService.GetContentItemsSortBy(ContentItemSortBy.ReleaseDate_Desc, null, null, false, tag.TagName, pageSize, pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", contentItems);


            //设置title和meta
            pageResourceManager.InsertTitlePart(tag.TagName);
            if (!string.IsNullOrEmpty(tag.Description))
            {
                pageResourceManager.SetMetaOfDescription(HtmlUtility.TrimHtml(tag.Description, 100));
            }
            ViewData["tag"] = tag;
            return View(contentItems);
        }

        #endregion

        #region 投稿

        /// <summary>
        /// 投稿
        /// </summary>
        /// <param name="contentItemId"></param>
        /// <returns></returns>
        public ActionResult Contribute(long? contentItemId = null)
        {
            pageResourceManager.InsertTitlePart("编辑资讯");
            var metadataService = new MetadataService();
            ContentTypeDefinition contentType = metadataService.GetContentType(ContentTypeKeys.Instance().News());
            if (contentType == null)
                return HttpNotFound();
            if (UserContext.CurrentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));
            ContentItemEditModel model = TempData.Get<ContentItemEditModel>("ContentItemEditModel", null);
            if (model == null)
            {
                ContentItem item = null;
                string errorMessage = string.Empty;

                if (contentItemId.HasValue && contentItemId.Value > 0)
                {
                    item = new ContentItemService().Get(contentItemId.Value);
                    if (item == null)
                        return HttpNotFound();
                    model = item.AsEditModel();

                    if (!authorizer.CMS_EditContentItem(item))
                    {
                        errorMessage = "没有权限编辑资讯：" + item.Title;

                        return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                        {
                            Body = errorMessage,
                            Title = "没有权限",
                            StatusMessageType = StatusMessageType.Hint
                        }, Request.RawUrl));
                    }
                }
                else
                {
                    model = new ContentItemEditModel { ContentTypeId = contentType.ContentTypeId };
                    if (!authorizer.CMS_Contribute(out errorMessage))
                    {
                        return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                        {
                            Body = errorMessage,
                            Title = "没有权限",
                            StatusMessageType = StatusMessageType.Hint
                        }, Request.RawUrl));
                    }
                }
            }
            else
            {
                TempData.Remove("ContentItemEditModel");
            }
            return View(contentItemId.HasValue ? contentType.Page_Edit : contentType.Page_New, model);
        }

        /// <summary>
        /// 编辑内容项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Contribute(ContentItemEditModel model)
        {
            ContentItem item = model.AsContentItem(Request);
            var contentItemService = new ContentItemService();

            string errorMessage = string.Empty;
            if (item.ContentItemId > 0)
            {
                if (!authorizer.CMS_EditContentItem(item))
                {
                    errorMessage = "没有权限编辑资讯：" + item.Title;

                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = errorMessage,
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }, Request.RawUrl));
                }
            }
            else
            {
                if (!authorizer.CMS_Contribute(out errorMessage))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = errorMessage,
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }, Request.RawUrl));
                }
            }
            try
            {
                if (item.ContentItemId > 0)
                {
                    contentItemService.Update(item);
                    TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新成功");
                    //清除标签
                    tagService.ClearTagsFromItem(item.ContentItemId, item.UserId);
                }
                else
                {
                    if (!authorizer.CMS_ManageContentFolder(item.ContentFolder))
                        item.IsContributed = true;
                    item.ReleaseDate = DateTime.UtcNow;
                    contentItemService.Create(item);
                    TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "创建成功");
                }
            }
            catch
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "创建或者更新失败，请重试");
                TempData["ContentItemEditModel"] = model;
                return Redirect(SiteUrls.Instance().Contribute(item.ContentItemId));
            }

            string tags = string.Join(",", model.TagNames);
            if (!string.IsNullOrEmpty(tags))
            {
                tagService.AddTagsToItem(tags, item.UserId, item.ContentItemId);
            }
            return Redirect(SiteUrls.Instance().ContentItemDetail(item.ContentItemId));
        }

        #endregion

        #region 前台管理资讯

        /// <summary>
        /// 管理资讯
        /// </summary>
        [HttpGet]
        public ActionResult ManageContentItems(AuditStatus? auditStatus = null, int? folderId = null, string subjectKeyWord = null, string tagNameKeyword = null, string userId = null, DateTime? minDate = null, DateTime? maxDate = null, int pageSize = 20, int pageIndex = 1)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));
            long? id = null;
            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.TrimStart(',').TrimEnd(',');
                if (!string.IsNullOrEmpty(userId))
                {
                    id = long.Parse(userId);
                }
            }
            ViewData["userId"] = id;
            var contentFolderService = new ContentFolderService();
            if (folderId.HasValue && folderId.Value > 0)
            {
                ContentFolder contentFolder = contentFolderService.Get(folderId.Value);
                if (!authorizer.CMS_ManageContentFolder(contentFolder))
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = string.Format("您没有权限管理 {0} ！", contentFolder == null ? "" : contentFolder.FolderName),
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }
            }
            else
            {
                //判断用户是否为栏目管理员
                if (!authorizer.CMS_ManageContentItemsForChannel())
                {
                    return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                    {
                        Body = "您没有权限在前台管理资讯",
                        Title = "没有权限",
                        StatusMessageType = StatusMessageType.Hint
                    }));
                }
            }

            var contentTypes = new MetadataService().GetContentTypes(true);
            ViewData["ContentTypes"] = contentTypes;
            long? moderatorUserId = null;
            if (!authorizer.IsAdministrator(CmsConfig.Instance().ApplicationId))
                moderatorUserId = currentUser.UserId;
            PagingDataSet<ContentItem> items = contentItemService.GetContentItemsForAdmin(auditStatus, subjectKeyWord, folderId, true, tagNameKeyword: tagNameKeyword, userId: id, moderatorUserId: moderatorUserId, minDate: minDate, maxDate: maxDate, pageSize: pageSize, pageIndex: pageIndex);
            pageResourceManager.InsertTitlePart("资讯管理");
            RouteData.Values["CurrentNavigationId"] = "10101501";
            return View(items);
        }

        /// <summary>
        /// 删除资讯
        /// </summary>
        /// <param name="contentItemIds">资讯id列表</param>
        [HttpPost]
        public JsonResult _DeleteContentItems(List<long> contentItemIds)
        {
            if (contentItemIds == null || contentItemIds.Count <= 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到需要删除的资讯"));
            foreach (var itemId in contentItemIds)
            {
                var item = contentItemService.Get(itemId);
                if (item == null)
                    continue;
                if (authorizer.CMS_DeleteContentItem(item))
                    contentItemService.Delete(itemId);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "删除资讯成功！"));
        }


        /// <summary>
        /// 更新审核状态
        /// </summary>
        /// <param name="contentItemIds">被操作项的Id集合</param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _UpdateAuditStatus(List<long> contentItemIds, bool isApproved)
        {
            if (contentItemIds == null || contentItemIds.Count <= 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到需要更新审核状态的资讯"));
            foreach (var itemId in contentItemIds)
            {
                var item = contentItemService.Get(itemId);
                if (item == null)
                    continue;
                if (authorizer.CMS_ManageContentFolder(item.ContentFolder))
                    contentItemService.UpdateAuditStatus(itemId, isApproved);
            }
            return Json(new StatusMessageData(StatusMessageType.Success, "更新审核状态成功！"));
        }

        /// <summary>
        /// 设置置顶时间的局部页面
        /// </summary>
        /// <param name="contentItemIds">资讯Id</param>
        /// <returns>设置置顶时间的局部页面</returns>
        [HttpGet]
        public ActionResult _SetStickyDate(List<long> contentItemIds)
        {
            if (contentItemIds == null || contentItemIds.Count <= 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到需要置顶的资讯"));
            contentItemIds = contentItemIds.Where(n => n > 0).ToList();
            if (contentItemIds.Count == 1)
            {
                ViewData["contentItem"] = contentItemService.Get(contentItemIds.First());
            }
            return View(contentItemIds);
        }

        /// <summary>
        /// 批量置顶
        /// </summary>
        /// <param name="contentItemIds"></param>
        /// <param name="isGlobalSticky"></param>
        /// <param name="isFolderSticky"></param>
        /// <param name="globalStickyDate"></param>
        /// <param name="folderStickyDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CmsSetSticky(List<long> contentItemIds, bool isGlobalSticky, bool isFolderSticky, DateTime globalStickyDate, DateTime folderStickyDate)
        {
            if (contentItemIds == null || contentItemIds.Count <= 0)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有找到需要置顶的资讯"));
            if (!isGlobalSticky)
                globalStickyDate = ValueUtility.GetSafeSqlDateTime(DateTime.MinValue);
            if (!isFolderSticky)
                folderStickyDate = ValueUtility.GetSafeSqlDateTime(DateTime.MinValue);
            foreach (var contentItemId in contentItemIds)
            {
                var item = contentItemService.Get(contentItemId);
                if (item == null)
                    continue;
                if (authorizer.CMS_ManageContentFolder(item.ContentFolder))
                    contentItemService.SetSticky(contentItemId, isGlobalSticky, globalStickyDate, isFolderSticky, folderStickyDate);
            }

            string message = "操作成功";
            return Json(new StatusMessageData(StatusMessageType.Success, message));
        }

        /// <summary>
        /// 设置资讯的栏目
        /// </summary>
        /// <param name="contentItemIds">被移动的资讯id集合</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _SetContentFolder(List<long> contentItemIds)
        {
            return View(contentItemIds);
        }

        /// <summary>
        /// 设置资讯栏目
        /// </summary>
        /// <param name="contentItemIds">准备移动的资讯id</param>
        /// <param name="toContentFolderId">接收的栏目</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SetContentFolder(List<long> contentItemIds, int toContentFolderId)
        {
            if (contentItemIds == null)
                return Json(new StatusMessageData(StatusMessageType.Hint, "没有找到需要移动的资讯"));
            var toContentFolder = contentFolderService.Get(toContentFolderId);
            if (toContentFolder == null)
                return Json(new StatusMessageData(StatusMessageType.Hint, "没有找到接收的栏目"));
            if (!authorizer.CMS_ManageContentFolder(toContentFolder))
                return Json(new StatusMessageData(StatusMessageType.Hint, string.Format("没有管理{0}的权限", toContentFolder.FolderName)));
            var itemIds = new List<long>();
            foreach (var contentItemId in contentItemIds)
            {
                var item = contentItemService.Get(contentItemId);
                if (item == null)
                    continue;
                if (authorizer.CMS_ManageContentFolder(item.ContentFolder))
                    itemIds.Add(contentItemId);
            }
            contentItemService.Move(contentItemIds, toContentFolderId);
            return Json(new StatusMessageData(StatusMessageType.Success, "移动成功"));
        }

        #endregion

        #region 全文检索

        /// <summary>
        /// 资讯搜索
        /// </summary>
        public ActionResult Search(CmsFullTextQuery query)
        {
            query.Keyword = WebUtility.UrlDecode(query.Keyword);
            query.PageSize = 20;//每页记录数

            //调用搜索器进行搜索
            CmsSearcher cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
            PagingDataSet<ContentItem> contentItems = cmsSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    SearchHistoryService searchHistoryService = new SearchHistoryService();
                    searchHistoryService.SearchTerm(CurrentUser.UserId, CmsSearcher.CODE, query.Keyword);
                }
            }

            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(CmsSearcher.CODE, query.Keyword);
            }

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("资讯搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart(query.Keyword + "的相关资讯");//设置页面Title
            }

            return View(contentItems);
        }

        /// <summary>
        /// 资讯全局搜索
        /// </summary>
        public ActionResult _GlobalSearch(CmsFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;

            //调用搜索器进行搜索
            CmsSearcher cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
            PagingDataSet<ContentItem> cmsThreads = cmsSearcher.Search(query);

            return PartialView(cmsThreads);
        }

        /// <summary>
        /// 资讯快捷搜索
        /// </summary>
        public ActionResult _QuickSearch(CmsFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.Range = CmsSearchRange.TITLE;
            query.Keyword = Server.UrlDecode(query.Keyword);

            //调用搜索器进行搜索
            CmsSearcher cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
            PagingDataSet<ContentItem> cmsThreads = cmsSearcher.Search(query);

            return PartialView(cmsThreads);
        }

        /// <summary>
        /// 资讯搜索自动完成
        /// </summary>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {
            //调用搜索器进行搜索
            CmsSearcher cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
            IEnumerable<string> terms = cmsSearcher.AutoCompleteSearch(keyword, topNumber);

            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        /// <summary>
        /// 用户数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public ActionResult _UserData(long userId)
        {
            User user = userService.GetFullUser(userId);
            if (user == null)
                return HttpNotFound();
            ViewData["user"] = user;
            return PartialView("_UserData", user);
        }
        /// <summary>
        /// 评论详细页中的用户数据（显示用户评论数）
        /// </summary>
        /// <returns></returns>
        public ActionResult _UserComment()
        {
            IUser currentUser = UserContext.CurrentUser;
            CommentService commentService = new CommentService();
            long userCommentCount = commentService.GetUserCommentCount(currentUser.UserId, TenantTypeIds.Instance().ContentItem());
            ViewData["userCommentCount"] = userCommentCount;
            return View();
        }

        /// <summary>
        /// 获取子级栏目
        /// </summary>
        /// <returns></returns>
        public JsonResult GetChildContentFolders(string contentTypeKey = null, int exceptFolderId = 0, bool? onlyModerated = null)
        {
            int parentId = Request.QueryString.Get<int>("Id", 0);

            var folder = new ContentFolderService().Get(parentId);
            if (folder == null || folder.Children == null)
                return Json(new { }, JsonRequestBehavior.AllowGet);
            var folders = folder.Children;
            if (!string.IsNullOrEmpty(contentTypeKey))
                folders = folders.Where(n => n.ContentTypeKeys.Split(',').Contains(contentTypeKey));
            if (onlyModerated.HasValue && onlyModerated.Value)
            {
                folders = folders.Where(n => authorizer.CMS_ManageContentFolder(n));
            }
            return Json(folders.Where(n => n.ContentFolderId != exceptFolderId && !n.IsLink).Select(n => new { id = n.ContentFolderId, name = n.FolderName }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 资讯标签选择器（支持自动提取）
        /// </summary>
        /// <param name="contentFolderId"></param>
        /// <param name="contentItemId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public ActionResult _TagSelector(long contentItemId, int contentFolderId, string title = null)
        {
            IEnumerable<string> selectedTagNames = new List<string>();
            if (string.IsNullOrEmpty(title))
            {
                var contentItem = contentItemService.Get(contentItemId);
                if (contentItem != null)
                    selectedTagNames = contentItem.TagNames;
                ViewData["selectedTagNames"] = selectedTagNames;
                return View();
            }
            string filteredPhrase = ClauseScrubber.LuceneKeywordsScrub(title);
            if (!string.IsNullOrEmpty(filteredPhrase))
            {
                string[] nameSegments = ClauseScrubber.SegmentForPhraseQuery(filteredPhrase);
                selectedTagNames = nameSegments.Where(n => n.Length > 1).Take(5).ToList();
                ViewData["selectedTagNames"] = selectedTagNames;
            }
            return View();
        }
    }
}