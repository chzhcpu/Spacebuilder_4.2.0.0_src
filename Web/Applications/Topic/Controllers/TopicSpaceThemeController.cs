//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.UI;
using Tunynet.Utilities;

namespace SpecialTopic.Topic.Controllers
{
    [Themed("TopicSpace", IsApplication = false)]
    [AnonymousBrowseCheck]
    [TitleFilter(IsAppendSiteName = true)]
    [TopicSpaceAuthorize]
    public class TopicSpaceThemeController : Controller
    {
        public TopicService topicService { get; set; }
        public IPageResourceManager pageResourceManager { get; set; }
        public IUserService userService { get; set; }
        public FollowService followService { get; set; }
        public ActivityService activityService { get; set; }
        public ApplicationService applicationService { get; set; }
        private VisitService visitService = new VisitService(TenantTypeIds.Instance().Topic());
        private SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().Topic());

        /// <summary>
        /// 页头
        /// </summary>
        /// <returns></returns>
        public ActionResult _Header(string spaceKey)
        {
            if (UserContext.CurrentUser != null)
            {
                MessageService messageService = new MessageService();
                InvitationService invitationService = new InvitationService();
                NoticeService noticeService = new NoticeService();

                long userId = UserIdToUserNameDictionary.GetUserId(UserContext.CurrentUser.UserName);
                int count = 0;
                count = invitationService.GetUnhandledCount(userId);
                count += messageService.GetUnreadCount(userId);
                count += noticeService.GetUnhandledCount(userId);
                ViewData["PromptCount"] = count;
            }

            //获取当前是在哪个应用下搜索
            RouteValueDictionary routeValueDictionary = Request.RequestContext.RouteData.DataTokens;
            string areaName = routeValueDictionary.Get<string>("area", null) + "Search";
            ViewData["search"] = areaName;

            //查询用于快捷搜索的搜索器
            IEnumerable<ISearcher> searchersQuickSearch = SearcherFactory.GetQuickSearchers(4);
            ViewData["searchersQuickSearch"] = searchersQuickSearch;

            NavigationService service = new NavigationService();
            ViewData["Navigations"] = service.GetRootNavigations(PresentAreaKeysOfBuiltIn.Channel).Where(n => n.IsVisible(UserContext.CurrentUser) == true);

            return PartialView();
        }

        /// <summary>
        /// 页脚
        /// </summary>
        /// <returns></returns>
        public ActionResult _Footer(string spaceKey)
        {

            ISettingsManager<SiteSettings> siteSettingsManager = DIContainer.Resolve<ISettingsManager<SiteSettings>>();
            ViewData["SiteSettings"] = siteSettingsManager.Get();

            return PartialView();
        }

        /// <summary>
        /// 专题头部信息
        /// </summary>
        /// <param name="spaceKey">用户标识</param>
        /// <param name="showManageButton">显示管理按钮</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _TopicHeader(string spaceKey, bool showManageButton)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewData["showManageButton"] = showManageButton;
            return View(topic);
        }

        /// <summary>
        /// 专题空间主页
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        public ActionResult Home(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            
            //已修改
            if (topic == null)
                return HttpNotFound();

            IEnumerable<ApplicationBase> applications = applicationService.GetInstalledApplicationsOfOwner(PresentAreaKeysOfExtension.TopicSpace, topic.TopicId);
            

            //这里先判断topic是否为空，再插入了专题名
            pageResourceManager.InsertTitlePart(topic.TopicName);

            //浏览计数
            new CountService(TenantTypeIds.Instance().Topic()).ChangeCount(CountTypes.Instance().HitTimes(), topic.TopicId, topic.UserId);

            
            //1.为什么匿名用户就不让访问？
            //2.这里有个大问题，私密专题应该不允许非专题成员访问，
            //可以参考Common\Mvc\Attributes\UserSpaceAuthorizeAttribute.cs，在topic\Extensions\增加一个topicSpaceAuthorizeAttribute
            //3.当设置为私密专题并且允许用户申请加入时，应允许用户浏览专题首页，但只能看部分信息，具体需求可找宝声确认；
            
            //当前用户
            IUser user = UserContext.CurrentUser;
            if (user != null)
            {
                
                //ok，传递这些结果可以吗？
                //已修改
                //这样做很不好，直接用Authorizer
                bool isMember = topicService.IsMember(topic.TopicId, user.UserId);
                visitService.CreateVisit(user.UserId, user.DisplayName, topic.TopicId, topic.TopicName);
                ViewData["isMember"] = isMember;
            }
            ViewData["isPublic"] = topic.IsPublic;
            TempData["TopicMenu"] = TopicMenu.Home;
            ViewData["applications"] = applications;

            return View(topic);
        }

        /// <summary>
        /// 专题空间主导航
        /// </summary>
        /// <param name="spaceKey">专题空间标识</param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult _Menu_App(string spaceKey)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Content(string.Empty);

            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);


            NavigationService navigationService = new NavigationService();
            Navigation navigation = navigationService.GetNavigation(PresentAreaKeysOfExtension.TopicSpace, currentNavigationId, topic.TopicId);

            IEnumerable<Navigation> navigations = new List<Navigation>();
            if (navigation != null)
            {
                if (navigation.Depth >= 1 && navigation.Parent != null)
                {
                    navigations = navigation.Parent.Children;
                }
                else if (navigation.Depth == 0)
                {
                    navigations = navigation.Children;
                }


                ManagementOperationService managementOperationService = new ManagementOperationService();
                IEnumerable<ApplicationManagementOperation> applicationManagementOperations = managementOperationService.GetShortcuts(PresentAreaKeysOfExtension.TopicSpace, false);
                if (applicationManagementOperations != null)
                {
                    ViewData["ApplicationManagementOperations"] = applicationManagementOperations.Where(n => n.ApplicationId == navigation.ApplicationId);
                }
            }

            return View(navigations);
        }

        /// <summary>
        /// 专题空间主导航
        /// </summary>
        /// <param name="spaceKey">专题空间标识</param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult _Menu_Main(string spaceKey)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Content(string.Empty);

            ManagementOperationService managementOperationService = new ManagementOperationService();
            ViewData["ApplicationManagementOperations"] = managementOperationService.GetShortcuts(PresentAreaKeysOfExtension.TopicSpace, false);

            NavigationService navigationService = new NavigationService();
            return View(navigationService.GetRootNavigations(PresentAreaKeysOfExtension.TopicSpace, topic.TopicId));
        }

        /// <summary>
        /// 专题信息
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <param name="showtopicLogo">显示专题Logo</param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult _TopicInfo(string spaceKey, bool? showTopicLogo)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Content(string.Empty);

            ViewData["showTopicLogo"] = showTopicLogo;

            return View(topic);
        }

    }
}