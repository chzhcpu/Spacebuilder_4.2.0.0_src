//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.UI;
using Spacebuilder.Common;
using Tunynet;


namespace SpecialTopic.Topic.Controllers
{
    /// <summary>
    /// 专题管理Controller
    /// </summary>
    [Themed(PresentAreaKeysOfExtension.TopicSpace, IsApplication = true)]
    [AnonymousBrowseCheck]
    [TitleFilter(TitlePart = "专题", IsAppendSiteName = true)]
    [ManageAuthorize(CheckCookie = false)]
    public class ControlPanelTopicController : Controller
    {
        #region Private Items
        public IPageResourceManager pageResourceManager { get; set; }
        public TopicService groupService { get; set; }
        #endregion

        #region 页面
        /// <summary>
        /// 管理专题
        /// </summary>
        /// <param name="model">专题editmodel</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>专题列表</returns>
        public ActionResult ManageTopics(ManageTopicEditModel model, int pageIndex = 1)
        {
            pageResourceManager.InsertTitlePart("专题管理");

            TopicEntityQuery group = model.GetTopicQuery();

            ViewData["Topics"] = groupService.GetsForAdmin(group.AuditStatus, group.CategoryId, group.TopicNameKeyword, group.UserId,
                group.StartDate, group.EndDate, group.minMemberCount, group.maxMemberCount, model.PageSize ?? 20, pageIndex);
            return View(model);
        }

        /// <summary>
        /// 删除专题
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns>删除专题操作</returns>
        [HttpPost]
        public ActionResult DeleteTopic(long groupId)
        {
            groupService.Delete(groupId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }

        #region 批量操作-专题
        /// <summary>
        /// 设置专题的审核状态
        /// </summary>
        /// <param name="groupIds">专题id</param>
        /// <param name="isApproved">审核是否通过</param>
        /// <returns>返回审核操作</returns>
        [HttpPost]
        public ActionResult BatchUpdateTopicAuditStatus(List<long> groupIds, bool isApproved = true)
        {
            groupService.Approve(groupIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "批量设置审核状态成功"));
        }

        /// <summary>
        /// 设置专题的审核状态
        /// </summary>
        /// <param name="groupId">专题id</param>
        /// <param name="isApproved">审核是否通过</param>
        /// <returns>返回审核操作</returns>
        [HttpPost]
        public ActionResult BatchUpdateTopicAuditStatu(long groupId, bool isApproved = true)
        {
            List<long> groupIds = new List<long>() { groupId };
            groupService.Approve(groupIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "设置审核状态成功"));
        }
        #endregion

        #endregion
    }
}
