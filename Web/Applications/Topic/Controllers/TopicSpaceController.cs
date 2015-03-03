//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;

using Tunynet.UI;
using Tunynet.Utilities;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;

namespace SpecialTopic.Topic.Controllers
{
    [Themed(PresentAreaKeysOfExtension.TopicSpace, IsApplication = true)]
    [AnonymousBrowseCheck]
    [TitleFilter(IsAppendSiteName = true)]
    [TopicSpaceAuthorize]
    public class TopicSpaceController : Controller
    {
        public TopicService topicService { get; set; }
        public PaperService paperService { get; set; }
        public IPageResourceManager pageResourceManager { get; set; }
        public IUserService userService { get; set; }
        public FollowService followService { get; set; }
        public ActivityService activityService { get; set; }
        public ApplicationService applicationService { get; set; }
        public PrivacyService privacyService { get; set; }
        public Authorizer authorizer { get; set; }
        private VisitService visitService = new VisitService(TenantTypeIds.Instance().Topic());
        private SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().Topic());

        /// <summary>
        /// 专题标签云
        /// </summary>_CommentList
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">显示的标签数量</param>
        /// <param name="ownerId">拥有者ID</param>
        /// <param name="showInNewPage">新页显示</param>
        /// <returns></returns>
        public ActionResult _TagCloud(string tenantTypeId = "", int topNumber = 20, long ownerId = 0, bool showInNewPage = false)
        {
            TagService tagService = new TagService(tenantTypeId);
            Dictionary<TagInOwner, int> topicTags = tagService.GetOwnerTopTags(topNumber, ownerId);

            ViewData["ownerId"] = ownerId;
            ViewData["showInNewPage"] = showInNewPage;
            return View(topicTags);
        }

        /// <summary>
        /// 最近访客控件
        /// </summary>
        public ActionResult _LastTopicVisitors(string spaceKey, int topNumber = 12)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            IEnumerable<Visit> visits = visitService.GetTopVisits(topicId, topNumber);
            ViewData["topicId"] = topicId;
            return View(visits);
        }

        /// <summary>
        /// 专题首页动态列表
        /// </summary>
        [HttpGet]
        public ActionResult _ListActivities(string spaceKey, int? pageIndex, int? applicationId, MediaType? mediaType, bool? isOriginal, long? userId)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            PagingDataSet<Activity> activities = activityService.GetOwnerActivities(ActivityOwnerTypes.Instance().Topic(), topicId, applicationId, mediaType, isOriginal, null, pageIndex ?? 1, userId);
            if (activities.FirstOrDefault() != null)
            {
                ViewData["lastActivityId"] = activities.FirstOrDefault().ActivityId;
            }
            ViewData["pageIndex"] = pageIndex;
            ViewData["applicationId"] = applicationId;
            ViewData["mediaType"] = mediaType;
            ViewData["isOriginal"] = isOriginal;
            ViewData["userId"] = userId;
            return View(activities);
        }

        /// <summary>
        /// 获取以后进入用户时间线的动态
        /// </summary>
        public ActionResult _GetNewerActivities(string spaceKey, int? applicationId, long? lastActivityId = 0)
        {
            if (UserContext.CurrentUser == null)
                return new EmptyResult();
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            IEnumerable<Activity> newActivities = activityService.GetNewerActivities(topicId, lastActivityId.Value, applicationId, ActivityOwnerTypes.Instance().Topic(), UserContext.CurrentUser.UserId);

            if (newActivities != null && newActivities.Count() > 0)
            {
                ViewData["lastActivityId"] = newActivities.FirstOrDefault().ActivityId;
            }
            return View(newActivities);
        }

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        [HttpPost]
        public JsonResult GetNewerTopicActivityCount(string spaceKey, long lastActivityId, int? applicationId)
        {
            if (UserContext.CurrentUser == null)
                return Json(new { });
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            string name;
            int count = activityService.GetNewerCount(topicId, lastActivityId, applicationId, out name, ActivityOwnerTypes.Instance().Topic(), UserContext.CurrentUser.UserId);
            if (count == 0)
            {
                return Json(new { lastActivityId = lastActivityId, hasNew = false });
            }
            else
            {
                string showName;
                if (count == 1)
                    showName = name + "更新了动态，点击查看";
                else
                    showName = name + "等多位群友更新了动态，点击查看";
                return Json(new { hasNew = true, showName = showName });
            }
        }

        /// <summary>
        /// 删除专题动态
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteTopicActivity(string spaceKey, long activityId)
        {
            var activity = activityService.Get(activityId);
            if (!authorizer.Topic_DeleteTopicActivity(activity))
                return Json(new StatusMessageData(StatusMessageType.Error, "没有删除专题动态的权限"));
            activityService.DeleteActivity(activityId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除专题动态成功！"));
        }


        /// <summary>
        /// 删除该条访客记录
        /// </summary>
        [HttpPost]
        public ActionResult DeleteTopicVisitor(string spaceKey)
        {
            TopicEntity tipic = topicService.Get(spaceKey);
            if (tipic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到专题！"));

            //如何做

            //已修复
            long userId = Request.Form.Get<long>("userId", 0);
            long id = Request.Form.Get<long>("id", 0);
            if (authorizer.Topic_DeleteVisitor(tipic.TopicId, userId))
            {
                visitService.Delete(id);
                return RedirectToAction("_LastTopicVisitors");
            }
            else
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "没有删除该条访客记录的权限！"));
            }
        }


        /// <summary>
        /// 群友还喜欢去
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _TopicMemberAlsoJoinedTopics(string spaceKey, int topNumber = 10)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            IEnumerable<TopicEntity> topics = topicService.TopicMemberAlsoJoinedTopics(topicId, topNumber);
            return View(topics);
        }

        /// <summary>
        /// 编辑专题公告页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditAnnouncement(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            ViewData["announcement"] = topic.Announcement;
            return View();
        }
        /// <summary>
        /// 在线的专题成员
        /// </summary>
        /// <param name="spaceKey">专题</param>
        /// <param name="topNumber">前多少条</param>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _OnlineTopicMembers(string spaceKey, int topNumber = 12)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            ViewData["User"] = topic.User;
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            IEnumerable<TopicMember> topicMembers = topicService.GetOnlineTopicMembers(topicId);
            if (topic.User.IsOnline)
            {
                return View(topicMembers.Take(topNumber - 1));
            }
            else
            {
                return View(topicMembers.Take(topNumber));
            }
        }
        /// <summary>
        /// 专题资料
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        public ActionResult _TopicProfile(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            return View(topic);
        }

        /// <summary>
        /// 编辑专题公告
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <param name="announcement">公告</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _EditAnnouncement(string spaceKey, string announcement)
        {
            string errorMessage = null;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, errorMessage));
            }

            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return Json(new { });
            if (!authorizer.Topic_Manage(topic))
                return Json(new StatusMessageData(StatusMessageType.Error, "没有更新公告的权限"));

            topicService.UpdateAnnouncement(topic.TopicId, announcement);
            return Json(new { shortAnnouncement = StringUtility.Trim(announcement, 100), longAnnouncement = announcement });
        }


        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Invite(string spaceKey)
        {
            return View();
        }



        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _Invite(string spaceKey, string userIds, string remark)
        {
            StatusMessageData message = null;
            string unInviteFriendNames = string.Empty;
            TopicEntity topic = topicService.Get(spaceKey);


            if (topic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到专题！"));

            //在显示时做了判断
            //已修改
            IUser currentUser = UserContext.CurrentUser;

            List<long> couldBeInvetedUserIds = new List<long>();
            //被邀请人的隐私设置
            IEnumerable<long> inviteUserIds = Request.Form.Gets<long>("userIds", null);
            int count = 0;
            foreach (long inviteUserId in inviteUserIds)
            {

                if (!privacyService.Validate(inviteUserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Invitation()))
                {
                    User user = userService.GetFullUser(inviteUserId);
                    unInviteFriendNames += user.DisplayName + ",";

                }
                else
                {
                    count++;
                    couldBeInvetedUserIds.Add(inviteUserId);
                }
            }



            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));

            if (!authorizer.Topic_Invite(topic))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = "没有邀请好友的权限！",
                    Title = "没有权限",
                    StatusMessageType = StatusMessageType.Hint
                }));
            }

            if (!string.IsNullOrEmpty(userIds))
            {

                //已修改

                IEnumerable<long> ids = Request.Form.Gets<long>("userIds", null);
                if (ids != null && ids.Count() > 0)
                {
                    topicService.SendInvitations(topic, currentUser, remark, couldBeInvetedUserIds);
                    if (count < ids.Count())
                    {
                        message = new StatusMessageData(StatusMessageType.Hint, "共有" + count + "个好友邀请成功，" + unInviteFriendNames.Substring(0, unInviteFriendNames.Count() - 1) + "不能被邀请！");
                    }
                    else
                    {
                        message = new StatusMessageData(StatusMessageType.Success, "邀请好友成功！");
                    }
                }
                else
                {
                    message = new StatusMessageData(StatusMessageType.Hint, "您尚未选择好友！");
                }
            }
            return Json(message);
        }

        /// <summary>
        /// 成员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Members(string spaceKey, int pageIndex = 1)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(topic.TopicName);
            pageResourceManager.InsertTitlePart("管理成员列表页");
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            PagingDataSet<TopicMember> topicMembers = topicService.GetTopicMembers(topicId, false, pageSize: 60, pageIndex: pageIndex);
            
            ViewData["Topic"] = topic;

            return View(topicMembers);
        }

        /// <summary>
        /// 我关注的专题成员
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult MyFollowedUsers(string spaceKey, int pageIndex = 1)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            var currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Redirect(SiteUrls.Instance().Login(true));

            pageResourceManager.InsertTitlePart(topic.TopicName);
            pageResourceManager.InsertTitlePart("管理成员列表页");

            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            IEnumerable<TopicMember> topicMember = topicService.GetTopicMembersAlsoIsMyFollowedUser(topicId, currentUser.UserId);
            PagingDataSet<TopicMember> topicMembers = new PagingDataSet<TopicMember>(topicMember);

            if (currentUser.IsFollowed(topic.User.UserId))
            {
                ViewData["topicOwner"] = topic.User;
            }




            return View(topicMembers);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult DeleteManager(string spaceKey, long userId)
        {
            TopicEntity topic = topicService.Get(spaceKey);


            if (!authorizer.Topic_DeleteMember(topic, userId))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除管理员的权限"));

            topicService.DeleteTopicMember(topic.TopicId, userId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除成功"));
        }


        /// <summary>
        /// 最新加入
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <param name="topNumber">前几条数据</param>
        /// <returns></returns>
        public ActionResult _ListMembers(string spaceKey, int topNumber)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return new EmptyResult();
            PagingDataSet<TopicMember> topicMembers = topicService.GetTopicMembers(topicId, false, SortBy_TopicMember.DateCreated_Desc);
            IEnumerable<TopicMember> members = topicMembers.Take(topNumber);

            return View(members);
        }

        /// <summary>
        /// 专题空间导航
        /// </summary>
        /// <param name="spaceKey">专题空间标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _TopicMenu(string spaceKey)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Content(string.Empty);

            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);
            IEnumerable<Navigation> navigations = new List<Navigation>();

            NavigationService navigationService = new NavigationService();
            Navigation navigation = navigationService.GetNavigation(PresentAreaKeysOfExtension.TopicSpace, currentNavigationId, topic.TopicId);

            if (navigation != null && navigation.Children.Count() > 0)
            {
                navigations = navigation.Children;
            }
            else
            {
                navigations = navigationService.GetRootNavigations(PresentAreaKeysOfExtension.TopicSpace, topic.TopicId);
            }

            return View(navigations);
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _Announcement(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            return View(topic);
        }

        /// <summary>
        /// 专题首页动态
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _TopicActivities(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();

            IEnumerable<ApplicationBase> applications = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfExtension.TopicSpace, topic.TopicId);
            ViewData["applications"] = applications;
            return View(topic);
        }

        public ActionResult _PapersOfTopic(string spaceKey)
        {
             long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
             IEnumerable<PaperEntity> papers = paperService.GetPapersByTopicId(topicId);
             return View(papers);
        }
    }

    public enum TopicMenu
    {
        /// <summary>
        /// 主页
        /// </summary>
        Home,

        /// <summary>
        /// 管理成员
        /// </summary>
        ManageMember,

        /// <summary>
        /// 专题设置
        /// </summary>
        TopicSettings

    }
}
