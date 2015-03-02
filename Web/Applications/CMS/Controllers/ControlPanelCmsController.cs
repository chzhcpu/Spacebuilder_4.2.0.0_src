//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;

using Tunynet.UI;
using Spacebuilder.CMS.Metadata;
using Tunynet.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace Spacebuilder.CMS.Controllers
{
    /// <summary>
    /// 资讯管理控制器
    /// </summary>
    [ManageAuthorize(CheckCookie = false)]
    [TitleFilter(TitlePart = "资讯管理", IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.ControlPanel, IsApplication = true)]
    public class ControlPanelCmsController : Controller
    {
        public IPageResourceManager pageResourceManager { get; set; }
        public ContentFolderService contentFolderService { get; set; }
        public ContentItemService contentItemService { get; set; }
        private TagService tagService = new TagService(TenantTypeIds.Instance().ContentItem());

        /// <summary>
        /// 管理资讯
        /// </summary>
        [HttpGet]
        public ActionResult ManageContentItems(AuditStatus? auditStatus = null, int? folderId = null, string subjectKeyWord = null, string tagNameKeyword = null, string userId = null, DateTime? minDate = null, DateTime? maxDate = null, int pageSize = 20, int pageIndex = 1)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Trim(',');
                if (!string.IsNullOrEmpty(userId))
                {
                    id = long.Parse(userId);
                }
            }
            ViewData["userId"] = id;
            ContentFolder contentFolder = null;
            if (folderId.HasValue && folderId.Value > 0)
                contentFolder = contentFolderService.Get(folderId.Value);
            ViewData["ContentFolder"] = contentFolder;
            var contentTypes = new MetadataService().GetContentTypes(true);
            ViewData["ContentTypes"] = contentTypes;
            PagingDataSet<ContentItem> items = contentItemService.GetContentItemsForAdmin(auditStatus, subjectKeyWord, folderId, true, tagNameKeyword: tagNameKeyword, userId: id, minDate: minDate, maxDate: maxDate, pageSize: pageSize, pageIndex: pageIndex);
            pageResourceManager.InsertTitlePart("资讯管理");
            return View(items);
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        /// <param name="contentItemId">内容项id</param>
        /// <param name="contentTypeId">内容模板id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditContentItem(long? contentItemId = null, int? contentTypeId = null)
        {
            pageResourceManager.InsertTitlePart("编辑资讯");
            var metadataService = new MetadataService();
            ContentTypeDefinition contentType = null;
            ContentItem item = null;
            if (contentItemId.HasValue && contentItemId.Value > 0)
            {
                item = contentItemService.Get(contentItemId.Value);
                if (item == null)
                    return HttpNotFound();
                contentType = item.ContentType;
            }
            else
            {
                if (contentTypeId.HasValue && contentTypeId.Value > 0)
                    contentType = metadataService.GetContentType(contentTypeId.Value);
                else
                    contentType = metadataService.GetContentTypes(true).FirstOrDefault();
                if (contentType == null)
                    return HttpNotFound();
            }
            ContentItemEditModel model = TempData.Get<ContentItemEditModel>("ContentItemEditModel", null);
            if (model == null)
            {
                if (item != null)
                {
                    model = item.AsEditModel();
                }
                else
                {
                    model = new ContentItemEditModel { ContentTypeId = contentType.ContentTypeId, ReleaseDate = DateTime.Now };
                }
            }
            else
            {
                TempData.Remove("ContentItemEditModel");
            }
            ViewData["contentItem"] = item;
            return View(contentItemId.HasValue ? contentType.Page_Edit : contentType.Page_New, model);
        }

        /// <summary>
        /// 编辑内容项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditContentItem(ContentItemEditModel model)
        {
            ContentItem item = model.AsContentItem(Request);

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
                    item.GlobalStickyDate = ValueUtility.GetSafeSqlDateTime(DateTime.MinValue);
                    item.FolderStickyDate = ValueUtility.GetSafeSqlDateTime(DateTime.MinValue);

                    contentItemService.Create(item);
                    TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "创建成功");
                }
            }
            catch (Exception)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "创建或者更新失败，请重试");
                TempData["ContentItemEditModel"] = model;
                return Redirect(SiteUrls.Instance().EditContentItem(item.ContentItemId));
            }
            if (model.TagNames != null)
            {
                string tags = string.Join(",", model.TagNames);
                if (!string.IsNullOrEmpty(tags))
                {
                    tagService.AddTagsToItem(tags, item.UserId, item.ContentItemId);
                }
            }
            if (item.ContentType != null && item.ContentType.ContentTypeKey == ContentTypeKeys.Instance().NewsLink())
                return Redirect(SiteUrls.Instance().FolderDetail(item.ContentFolderId));
            return Redirect(SiteUrls.Instance().ContentItemDetail(item.ContentItemId));
        }

        #region 栏目管理
        /// <summary>
        /// 管理栏目
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageContentFolders()
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return HttpNotFound();

            pageResourceManager.InsertTitlePart("栏目管理");

            IEnumerable<ContentFolder> folders = contentFolderService.GetIndentedFolders();
            return View(folders);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="contentFolderId">栏目ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteContentFolder(int contentFolderId)
        {
            var contentFolder = contentFolderService.Get(contentFolderId);
            if (contentFolder == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到栏目！"));
            //删除栏目
            contentFolderService.Delete(contentFolder);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功！"));
        }
        /// <summary>
        /// 添加编辑链接
        /// </summary>
        /// <param name="contentFolderId">栏目ID</param>
        /// <param name="parentId">父栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditContentFolderLink(int contentFolderId = 0)
        {
            FolderEditModel folderEditModel = new FolderEditModel();
            //编辑    
            if (contentFolderId > 0)
            {
                ContentFolder contentFolder = contentFolderService.Get(contentFolderId);
                if (contentFolder == null)
                    return HttpNotFound();
                folderEditModel = contentFolder.AsEditModel();
                pageResourceManager.InsertTitlePart("编辑链接");
            }
            else
            {
                pageResourceManager.InsertTitlePart("创建链接");
                folderEditModel.ContentFolderId = 0;
            }

            return View(folderEditModel);
        }

        /// <summary>
        /// 编辑添加栏目
        /// </summary>
        /// <param name="folderEditModel">栏目实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditContentFolderLink(FolderEditModel folderEditModel)
        {
            ContentFolder contentFolder = folderEditModel.AsContentFolder();
            contentFolder.IsLink = true;
            //编辑
            if (folderEditModel.ContentFolderId > 0)
            {
                contentFolderService.Update(contentFolder);
                return Json(new StatusMessageData(StatusMessageType.Success, "编辑成功！"));
            }
            //添加
            else
            {
                contentFolderService.Create(contentFolder);
                return Json(new StatusMessageData(StatusMessageType.Success, "添加成功！"));
            }
        }

        /// <summary>
        /// 添加编辑栏目
        /// </summary>
        /// <param name="contentFolderId">栏目ID</param>
        /// <param name="parentId">父栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditContentFolder(int contentFolderId = 0, int parentId = 0)
        {
            FolderEditModel folderEditModel = new FolderEditModel();
            ContentFolder contentFolder = null;
            //编辑
            if (contentFolderId > 0)
            {
                contentFolder = contentFolderService.Get(contentFolderId);
                if (contentFolder == null)
                    return HttpNotFound();
                folderEditModel = contentFolder.AsEditModel();
                ViewData["moderatorUserIds"] = contentFolder.Moderators.Select(n => n.UserId);
                pageResourceManager.InsertTitlePart("编辑栏目");
            }
            else
            {
                if (parentId > 0)
                {
                    contentFolder = contentFolderService.Get(parentId);
                    folderEditModel.ParentId = parentId;
                    pageResourceManager.InsertTitlePart("创建子栏目");
                }
                else
                    pageResourceManager.InsertTitlePart("创建栏目");
            }

            return View(folderEditModel);
        }

        /// <summary>
        /// 编辑添加栏目
        /// </summary>
        /// <param name="folderEditModel">栏目实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditContentFolder(FolderEditModel folderEditModel)
        {
            ContentFolder contentFolder = folderEditModel.AsContentFolder();
            //编辑
            if (folderEditModel.ContentFolderId > 0)
            {
                contentFolderService.Update(contentFolder);
            }
            //添加
            else
            {
                contentFolderService.Create(contentFolder);
            }

            //设置栏目管理员
            IEnumerable<long> moderatorUserIds = Request.Form.Gets<long>("ModeratorUserIds");
            if (moderatorUserIds != null)
                new ContentFolderModeratorService().SetModeratorBaseFolder(contentFolder.ContentFolderId, moderatorUserIds);

            //编辑
            if (folderEditModel.ContentFolderId > 0)
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "编辑成功！");
            }
            //添加
            else
            {
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "添加成功！");
            }
            return Redirect(SiteUrls.Instance().ManageContentFolders());
        }

        /// <summary>
        /// 合并移动栏目页
        /// </summary>
        /// <param name="fromContentFolderId"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _MoveContentFolder(int fromContentFolderId = 0, string option = "move")
        {
            FolderEditModel folderEditModel = new FolderEditModel();
            int maxDepth = 0;
            if (fromContentFolderId != 0)
            {
                ContentFolder contentFolder = contentFolderService.Get(fromContentFolderId);
                folderEditModel = contentFolder.AsEditModel();
                maxDepth = contentFolder.Depth;
            }
            ViewData["option"] = option;
            ViewData["maxDepth"] = maxDepth;
            return View(folderEditModel);
        }

        /// <summary>
        /// 合并移动栏目
        /// </summary>
        /// <param name="fromContentFolderId"></param>
        /// <param name="contentFolderId"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _MoveContentFolder(int fromContentFolderId = 0, int contentFolderId = 0, string option = "move")
        {
            string errormsg = "请选择要移动或合并的项";
            ContentFolder contentFolder = contentFolderService.Get(contentFolderId);
            ContentFolder fromContentFolder = contentFolderService.Get(fromContentFolderId);
            if (fromContentFolderId != 0 && contentFolderId != 0)
            {
                if (option == "merge")
                {
                    try
                    {
                        contentFolderService.Merge(fromContentFolderId, contentFolderId);
                        return Json(new StatusMessageData(StatusMessageType.Success, "合并成功"));
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                    }
                }
                if (option == "move")
                {
                    try
                    {

                        contentFolderService.Move(fromContentFolderId, contentFolderId);
                        return Json(new StatusMessageData(StatusMessageType.Success, "移动成功"));
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                    }
                }
            }
            return Json(new StatusMessageData(StatusMessageType.Error, errormsg));
        }

        /// <summary>
        /// 更改栏目显示顺序
        /// </summary>
        /// <returns></returns>
        public JsonResult ChangeContentFolderOrder(int fromContentFolderId, int toContentFolderId)
        {
            ContentFolder fromContentFolder = contentFolderService.Get(fromContentFolderId);
            ContentFolder toContentFolder = contentFolderService.Get(toContentFolderId);

            int temp = fromContentFolder.DisplayOrder;

            fromContentFolder.DisplayOrder = toContentFolder.DisplayOrder;
            contentFolderService.Update(fromContentFolder);

            toContentFolder.DisplayOrder = temp;
            contentFolderService.Update(toContentFolder);

            return Json(new StatusMessageData(StatusMessageType.Success, "交换成功！"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        //ManageContentAttachments
        /// <summary>
        /// 后台附件管理
        /// </summary>
        [HttpGet]
        public ActionResult ManageContentAttachments(string userId = null, string keyword = null, DateTime? startDate = null, DateTime? endDate = null, MediaType? mediaType = null, int pageSize = 20, int pageIndex = 1)
        {
            long? id = null;
            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Trim(',');
                if (!string.IsNullOrEmpty(userId))
                {
                    id = long.Parse(userId);
                }
            }
            ViewData["userId"] = id;

            PagingDataSet<ContentAttachment> attachments = new ContentAttachmentService().Gets(id, keyword, startDate, endDate, mediaType, pageSize, pageIndex);
            pageResourceManager.InsertTitlePart("附件管理");
            return View(attachments);
        }
    }

}