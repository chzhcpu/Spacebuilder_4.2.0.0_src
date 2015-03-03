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
using Tunynet.Search;
using Spacebuilder.Search;

using Tunynet.Common.Configuration;
using System.Text.RegularExpressions;
using Tunynet.Utilities;
using DevTrends.MvcDonutCaching;
using SpecialTopic.Topic;

namespace SpecialTopic.Topic.Controllers 
{
    /// <summary>
    /// 频道专题控制器
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [AnonymousBrowseCheck]
    [TitleFilter(IsAppendSiteName = true)]
    public class ChannelTopicController : Controller
    {
        public ActivityService activityService { get; set; }
        public IPageResourceManager pageResourceManager { get; set; }
        public CategoryService categoryService { get; set; }
        public TopicService topicService { get; set; }
        public Authorizer authorizer { get; set; }
        public IdentificationService identificationService { get; set; }
        public RecommendService recommendService { get; set; }
        public UserService userService { get; set; }
        public AreaService areaService { get; set; }
        public FollowService followService { get; set; }
        private TagService tagService = new TagService(TenantTypeIds.Instance().Topic());

        /// <summary>
        /// 频道专题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Home()
        {
            pageResourceManager.InsertTitlePart("专题首页");
            return View();
        }

        /// <summary>
        /// 专题顶部的局部页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _TopicSubmenu()
        {
            return View();
        }

        /// <summary>
        /// 验证专题Key的方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ValidateTopicKey(string topicKey, long topicId)
        {
            bool result = false;
            if (topicId > 0)
            {
                result = true;
            }
            else
            {
                TopicEntity topic = topicService.Get(topicKey);
                if (topic != null)
                {
                    return Json("此专题Key已存在", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = Regex.IsMatch(topicKey, @"^[A-Za-z0-9_\-\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
                    if (!result)
                    {
                        return Json("只能输入字母数字汉字或-号", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建专题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            pageResourceManager.InsertTitlePart("创建专题");
            string errorMessage = null;
            if (!authorizer.Topic_Create(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = errorMessage,
                    Title = errorMessage,
                    StatusMessageType = StatusMessageType.Hint
                }));
            }
            TopicEditModel topic = new TopicEditModel();
            return View(topic);
        }


        //已修改
        /// <summary>
        /// 创建专题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(TopicEditModel topicEditModel)
        {
            string errorMessage = null;
            if (ModelState.HasBannedWord(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = errorMessage,
                    Title = "创建失败",
                    StatusMessageType = StatusMessageType.Hint
                }));
            }

            System.IO.Stream stream = null;
            HttpPostedFileBase topicLogo = Request.Files["TopicLogo"];


            //已修改
            IUser user = UserContext.CurrentUser;
            if (user == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));

            if (!authorizer.Topic_Create(out errorMessage))
            {
                return Redirect(SiteUrls.Instance().SystemMessage(TempData, new SystemMessageViewModel
                {
                    Body = errorMessage,
                    Title = errorMessage,
                    StatusMessageType = StatusMessageType.Hint
                }));
            }
            if (topicLogo != null && !string.IsNullOrEmpty(topicLogo.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Topic());
                if (!tenantLogoSettings.ValidateFileLength(topicLogo.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(topicEditModel);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ISettingsManager<LogoSettings>>().Get();
                if (!logoSettings.ValidateFileExtensions(topicLogo.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(topicEditModel);
                }
                stream = topicLogo.InputStream;
                topicEditModel.Logo = topicLogo.FileName;
            }
            TopicEntity topic = topicEditModel.AsTopicEntity();

            bool result = topicService.Create(user.UserId, topic);

            if (stream != null)
            {
                topicService.UploadLogo(topic.TopicId, stream);
            }
            //设置分类
            if (topicEditModel.CategoryId > 0)
            {
                categoryService.AddItemsToCategory(new List<long>() { topic.TopicId }, topicEditModel.CategoryId);
            }
            //设置标签
            string relatedTags = Request.Form.Get<string>("RelatedTags");
            if (!string.IsNullOrEmpty(relatedTags))
            {
                tagService.AddTagsToItem(relatedTags, topic.TopicId, topic.TopicId);
            }
            //发送邀请
            if (!string.IsNullOrEmpty(topicEditModel.RelatedUserIds))
            {

                //已修改
                IEnumerable<long> userIds = Request.Form.Gets<long>("RelatedUserIds", null);
                topicService.SendInvitations(topic, user, string.Empty, userIds);
            }
            return Redirect(SiteUrls.Instance().TopicHome(topic.TopicKey));
        }

        #region 专题全文检索

        /// <summary>
        /// 专题搜索
        /// </summary>
        public ActionResult Search(TopicFullTextQuery query)
        {
            query.Keyword = WebUtility.UrlDecode(query.Keyword);
            query.PageSize = 20;//每页记录数

            //调用搜索器进行搜索
            TopicSearcher topicSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
            PagingDataSet<TopicEntity> topics = topicSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    SearchHistoryService searchHistoryService = new SearchHistoryService();
                    searchHistoryService.SearchTerm(CurrentUser.UserId, TopicSearcher.CODE, query.Keyword);
                }
            }

            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(TopicSearcher.CODE, query.Keyword);
            }

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("专题搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + query.Keyword + '”' + "的相关专题");//设置页面Title
            }

            return View(topics);
        }

        /// <summary>
        /// 专题全局搜索
        /// </summary>
        public ActionResult _GlobalSearch(TopicFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;

            //调用搜索器进行搜索
            TopicSearcher topicSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
            PagingDataSet<TopicEntity> topics = topicSearcher.Search(query);

            return PartialView(topics);
        }

        /// <summary>
        /// 专题快捷搜索
        /// </summary>
        public ActionResult _QuickSearch(TopicFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.Range = TopicSearchRange.TOPICNAME;
            query.Keyword = Server.UrlDecode(query.Keyword);
            //调用搜索器进行搜索
            TopicSearcher TopicSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
            PagingDataSet<TopicEntity> topics = TopicSearcher.Search(query);

            return PartialView(topics);
        }

        /// <summary>
        /// 专题搜索自动完成
        /// </summary>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {
            //调用搜索器进行搜索
            TopicSearcher topicSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
            IEnumerable<string> terms = topicSearcher.AutoCompleteSearch(keyword, topNumber);

            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// 可能感兴趣的专题
        /// </summary>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _InterestTopic()
        {
            TagService tagService = new TagService(TenantTypeIds.Instance().User());
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                TopicFullTextQuery query = new TopicFullTextQuery();
                query.PageSize = 20;
                query.PageIndex = 1;
                query.Range = TopicSearchRange.TAG;
                query.Tags = tagService.GetTopTagsOfItem(currentUser.UserId, 100).Select(n => n.TagName);
                query.TopicIds = topicService.GetMyJoinedTopics(currentUser.UserId, 100, 1).Select(n => n.TopicId.ToString());
                //调用搜索器进行搜索
                TopicSearcher TopicSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
                IEnumerable<TopicEntity> topicsTag = null;
                if (TopicSearcher.Search(query, true).Count == 0)
                {
                    return View();
                }
                else
                {
                    topicsTag = TopicSearcher.Search(query, true).AsEnumerable<TopicEntity>();
                }
                if (topicsTag.Count() < 20)
                {
                    IEnumerable<TopicEntity> topicsFollow = topicService.FollowedUserAlsoJoinedTopics(currentUser.UserId, 20 - topicsTag.Count());
                    return View(topicsTag.Union(topicsFollow));
                }
                else
                {
                    return View(topicsTag);
                }
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region 动态内容块
        /// <summary>
        /// 创建专题动态内容块
        /// </summary>
        //[DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _CreateTopic(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);
            TopicEntity topic = topicService.Get(activity.SourceId);
            if (topic == null)
                return Content(string.Empty);
            ViewData["ActivityId"] = ActivityId;
            return View(topic);
        }

        /// <summary>
        /// 用户加入专题动态内容快
        /// </summary>
        /// <param name="ActivityId">动态id</param>
        /// <returns>用户加入专题动态内容快</returns>
        //[DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _CreateTopicMember(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);

            TopicEntity topic = topicService.Get(activity.OwnerId);
            if (topic == null)
                return Content(string.Empty);

            IEnumerable<TopicMember> topicMembers = topicService.GetTopicMembers(topic.TopicId, true, SortBy_TopicMember.DateCreated_Desc);
            ViewData["activity"] = activity;
            ViewData["TopicMembers"] = topicMembers;
            return View(topic);
        }

        /// <summary>
        /// 用户加入专题动态内容快
        /// </summary>
        /// <param name="ActivityId">动态id</param>
        /// <returns>用户加入专题动态内容快</returns>
        //[DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _JoinTopic(long ActivityId)
        {
            Activity activity = activityService.Get(ActivityId);
            if (activity == null)
                return Content(string.Empty);
            TopicMember topicMember = topicService.GetTopicMember(activity.SourceId);
            if (topicMember == null)
                return Content(string.Empty);
            TopicEntity topic = topicService.Get(topicMember.TopicId);
            if (topic == null)
                return Content(string.Empty);

            ViewData["activity"] = activity;
            return View(topic);
        }

        #endregion

        #region 屏蔽专题

        /// <summary>
        /// 屏蔽专题
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BlockTopic(long topicId)
        {
            TopicEntity blockedTopic = topicService.Get(topicId);
            if (blockedTopic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到被屏蔽专题"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您还没有登录"));
            new UserBlockService().BlockTopic(currentUser.UserId, blockedTopic.TopicId, blockedTopic.TopicName);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        /// <summary>
        /// 屏蔽专题的post方法
        /// </summary>
        /// <param name="spaceKey">屏蔽的spacekey</param>
        /// <param name="topicIds">被屏蔽的分组名称</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BlockTopics(string spaceKey, List<long> topicIds)
        {
            int addCount = 0;

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            UserBlockService service = new UserBlockService();

            if (userId > 0 && topicIds != null && topicIds.Count > 0)
                foreach (var topicId in topicIds)
                {
                    TopicEntity topic = topicService.Get(topicId);
                    if (topic == null || service.IsBlockedTopic(userId, topicId))
                        continue;
                    service.BlockTopic(userId, topic.TopicId, topic.TopicName);
                    addCount++;
                }
            if (addCount > 0)
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, string.Format("成功添加{0}个专题添加到屏蔽列表", addCount));
            else
                TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "没有任何专题被添加到屏蔽列表中");
            return Redirect(SiteUrls.Instance().BlockGroups(spaceKey));
        }

        /// <summary>
        /// 屏蔽专题
        /// </summary>
        /// <param name="spaceKey">空间名</param>
        /// <returns>屏蔽专题名</returns>
        public ActionResult _BlockTopics(string spaceKey)
        {
            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);
            if (UserContext.CurrentUser == null || (UserContext.CurrentUser.UserId != userId && authorizer.IsAdministrator(new TenantTypeService().Get(TenantTypeIds.Instance().Topic()).ApplicationId)))
                return Content(string.Empty);

            IEnumerable<UserBlockedObject> blockedTopics = new UserBlockService().GetBlockedTopics(userId);

            List<UserBlockedObjectViewModel> blockedObjectes = new List<UserBlockedObjectViewModel>();

            if (blockedTopics != null && blockedTopics.Count() > 0)
            {
                topicService.GetTopicEntitiesByIds(blockedTopics.Select(n => n.ObjectId));
                foreach (var item in blockedTopics)
                {
                    TopicEntity topic = topicService.Get(item.ObjectId);
                    if (topic == null)
                        continue;

                    UserBlockedObjectViewModel entitiy = item.AsViewModel();
                    entitiy.Logo = topic.Logo;
                    blockedObjectes.Add(entitiy);
                }
            }

            return View(blockedObjectes);
        }

        #endregion

        #region 加入专题
        /// <summary>
        /// 申请加入按钮
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>   
        //[HttpGet]
        public ActionResult _ApplyJoinButton(long topicId, bool showQuit = false, string buttonName = null)
        {

            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return new EmptyResult();
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return new EmptyResult();
            bool isApplied = topicService.IsApplied(currentUser.UserId, topicId);
            bool isMember = topicService.IsMember(topic.TopicId, currentUser.UserId);
            bool isOwner = topicService.IsOwner(topic.TopicId, currentUser.UserId);
            bool isManager = topicService.IsManager(topic.TopicId, currentUser.UserId);
            ViewData["isMember"] = isMember;
            ViewData["showQuit"] = showQuit;
            ViewData["buttonName"] = buttonName;
            ViewData["isOwner"] = isOwner;
            ViewData["isManager"] = isManager;
            ViewData["isApplied"] = isApplied;
            return View(topic);
        }

        /// <summary>
        /// 退出专题
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _QuitTopic(long topicId)
        {
            StatusMessageData message = new StatusMessageData(StatusMessageType.Success, "退出专题成功！");
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到专题！"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            try
            {
                topicService.DeleteTopicMember(topic.TopicId, currentUser.UserId);
            }
            catch
            {
                message = new StatusMessageData(StatusMessageType.Error, "退出专题失败！");
            }
            return Json(message);
        }

        /// <summary>
        /// 用户加入专题（专题无验证时）
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JoinTopic(long topicId)
        {
            //需判断是否已经加入过专题
            StatusMessageData message = null;
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到专题！"));

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            if (topic.JoinWay != TopicJoinWay.Direct)
                return Json(new StatusMessageData(StatusMessageType.Error, "当前加入方式不是直接加入"));

            //已修改

            //判断是否加入过该专题
            bool isMember = topicService.IsMember(topicId, currentUser.UserId);

            //未加入
            if (!isMember)
            {
                TopicMember member = TopicMember.New();
                member.UserId = currentUser.UserId;
                member.TopicId = topic.TopicId;
                member.IsManager = false;
                topicService.CreateTopicMember(member);
                message = new StatusMessageData(StatusMessageType.Success, "加入专题成功！");
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Hint, "您已加入过该专题！");
            }
            return Json(message);
        }

        /// <summary>
        /// 用户加入专题（专题有验证时）
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _EditApply(long topicId)
        {

            //已修改

            bool isApplied = topicService.IsApplied(UserContext.CurrentUser.UserId, topicId);
            ViewData["isApplied"] = isApplied;
            return View();
        }

        /// <summary>
        /// 用户加入专题（专题有验证时）
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _EditApply(long topicId, string applyReason)
        {
            StatusMessageData message = null;
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到专题！"));

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            if (topic.JoinWay != TopicJoinWay.ByApply)
                return Json(new StatusMessageData(StatusMessageType.Error, "当前加入方式不是需要申请"));


            //已修改
            bool isApplied = topicService.IsApplied(currentUser.UserId, topic.TopicId);
            if (!isApplied)
            {
                TopicMemberApply apply = TopicMemberApply.New();
                apply.ApplyReason = applyReason;
                apply.ApplyStatus = TopicMemberApplyStatus.Pending;
                apply.TopicId = topic.TopicId;
                apply.UserId = UserContext.CurrentUser.UserId;
                topicService.CreateTopicMemberApply(apply);
                message = new StatusMessageData(StatusMessageType.Success, "申请已发出，请等待！");
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Hint, "您已给该专题发送过申请！");
            }
            return Json(message);
        }

        /// <summary>
        ///  用户加入专题（通过问题验证）
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _ValidateQuestion(long topicId)
        {
            TopicEntity topic = topicService.Get(topicId);
            ViewData["Question"] = topic.Question;
            return View();
        }

        /// <summary>
        /// 用户加入专题（通过问题验证）
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ValidateQuestion(long topicId, string myAnswer)
        {
            StatusMessageData message = null;
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "找不到专题！"));


            //已修改
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            if (topic.JoinWay != TopicJoinWay.ByQuestion)
                return Json(new StatusMessageData(StatusMessageType.Error, "当前加入方式不是问题验证"));


            bool isMember = topicService.IsMember(topic.TopicId, currentUser.UserId);
            if (!isMember)
            {
                if (topic.Answer == myAnswer)
                {
                    TopicMember member = TopicMember.New();
                    member.UserId = UserContext.CurrentUser.UserId;
                    member.TopicId = topic.TopicId;
                    member.IsManager = false;
                    topicService.CreateTopicMember(member);
                    message = new StatusMessageData(StatusMessageType.Success, "加入专题成功！");
                }
                else
                {
                    message = new StatusMessageData(StatusMessageType.Error, "答案错误！");
                }
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Hint, "您已加入过该专题！");
            }
            return Json(message);
        }

        #endregion

        #region 推荐专题
        /// <summary>
        /// 推荐专题
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendedTopic()
        {
            IEnumerable<RecommendItem> recommendItems = recommendService.GetTops(6, "90020001");
            return View(recommendItems);
        }
        #endregion

        #region 页面

        /// <summary>
        /// 发现专题
        /// </summary>
        /// <returns></returns>
        public ActionResult FindTopic(string nameKeyword, string areaCode, long? categoryId, SortBy_Topic? sortBy, int pageIndex = 1)
        {
            nameKeyword = WebUtility.UrlDecode(nameKeyword);
            string pageTitle = string.Empty;
            IEnumerable<Category> childCategories = null;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                var category = categoryService.Get(categoryId.Value);
                if (category != null)
                {


                    if (category.ChildCount > 0)
                    {
                        childCategories = category.Children;
                    }
                    else//若是叶子节点，则取同辈分类
                    {
                        if (category.Parent != null)
                            childCategories = category.Parent.Children;
                    }
                    List<Category> allParentCategories = new List<Category>();
                    //递归获取所有父级类别，若不是叶子节点，则包含其自身
                    RecursiveGetAllParentCategories(category.ChildCount > 0 ? category : category.Parent, ref allParentCategories);
                    ViewData["allParentCategories"] = allParentCategories;
                    ViewData["currentCategory"] = category;
                    pageTitle = category.CategoryName;
                }
            }


            if (childCategories == null)
                childCategories = categoryService.GetRootCategories(TenantTypeIds.Instance().Topic());

            ViewData["childCategories"] = childCategories;

            AreaSettings areaSettings = DIContainer.Resolve<ISettingsManager<AreaSettings>>().Get();
            IEnumerable<Area> childArea = null;
            if (!string.IsNullOrEmpty(areaCode))
            {
                var area = areaService.Get(areaCode);
                if (area != null)
                {


                    if (area.ChildCount > 0)
                    {
                        childArea = area.Children;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(area.ParentCode))
                        {
                            var parentArea = areaService.Get(area.ParentCode);
                            if (parentArea != null)
                                childArea = parentArea.Children;
                        }
                    }
                }
                List<Area> allParentAreas = new List<Area>();
                RecursiveGetAllParentArea(area.ChildCount > 0 ? area : areaService.Get(area.ParentCode), areaSettings.RootAreaCode, ref allParentAreas);
                ViewData["allParentAreas"] = allParentAreas;
                ViewData["currentArea"] = area;
                if (!string.IsNullOrEmpty(pageTitle))
                    pageTitle += ",";
                pageTitle += area.Name;
            }

            if (childArea == null)
            {
                Area rootArea = areaService.Get(areaSettings.RootAreaCode);
                if (rootArea != null)
                    childArea = rootArea.Children;
                else
                    childArea = areaService.GetRoots();
            }

            ViewData["childArea"] = childArea;

            if (!string.IsNullOrEmpty(nameKeyword))
            {
                if (!string.IsNullOrEmpty(pageTitle))
                    pageTitle += ",";
                pageTitle += nameKeyword;
            }

            if (string.IsNullOrEmpty(pageTitle))
                pageTitle = "发现专题";
            pageResourceManager.InsertTitlePart(pageTitle);
            PagingDataSet<TopicEntity> topics = topicService.Gets(areaCode, categoryId, sortBy ?? SortBy_Topic.DateCreated_Desc, pageIndex: pageIndex);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_List", topics);
            }

            return View(topics);

        }
        /// <summary>
        /// 迭代获取类别
        /// </summary>
        /// <param name="category"></param>
        /// <param name="allParentCategories"></param>
        private void RecursiveGetAllParentCategories(Category category, ref List<Category> allParentCategories)
        {
            if (category == null)
                return;
            allParentCategories.Insert(0, category);
            Category parent = category.Parent;
            if (parent != null)
                RecursiveGetAllParentCategories(parent, ref allParentCategories);
        }
        /// <summary>
        /// 迭代获取地区
        /// </summary>
        /// <param name="area"></param>
        /// <param name="rootAreaCode"></param>
        /// <param name="allParentAreas"></param>
        private void RecursiveGetAllParentArea(Area area, string rootAreaCode, ref List<Area> allParentAreas)
        {
            if (area == null || area.AreaCode == rootAreaCode)
                return;


            allParentAreas.Insert(0, area);
            Area parent = areaService.Get(area.ParentCode);
            if (parent != null)
            {
                RecursiveGetAllParentArea(parent, rootAreaCode, ref allParentAreas);
            }
        }


        /// <summary>
        /// 用户的专题页
        /// </summary>
        /// <returns></returns>
        [UserSpaceAuthorize]
        public ActionResult UserJoinedTopics(string spaceKey, int pageIndex = 1)
        {
            string title = "我加入的专题";
            IUserService userService = DIContainer.Resolve<IUserService>();
            User spaceUser = userService.GetFullUser(spaceKey);
            var currentUser = UserContext.CurrentUser;
            if (spaceUser == null)
                return HttpNotFound();


            if (currentUser != null)
            {
                if (currentUser.UserId != spaceUser.UserId)
                {
                    title = spaceUser.DisplayName + "加入的专题";
                }
            }
            else
            {
                title = spaceUser.DisplayName + "加入的专题";
            }


            PagingDataSet<TopicEntity> topics = topicService.GetMyJoinedTopics(spaceUser.UserId, pageIndex: pageIndex);
            if (Request.IsAjaxRequest())
                return PartialView("_List", topics);

            ViewData["spaceUser"] = spaceUser;
            ViewData["currentUser"] = currentUser;
            pageResourceManager.InsertTitlePart(title);

            #region 身份认证
            List<Identification> identifications = identificationService.GetUserIdentifications(spaceUser.UserId);
            if (identifications.Count() > 0)
            {
                ViewData["identificationTypeVisiable"] = true;
            }
            #endregion



            //设置当前登录用户对当前页用户的关注情况





            return View(topics);
        }



        /// <summary>
        /// 用户创建的专题页
        /// </summary>
        /// <returns></returns>
        [UserSpaceAuthorize]
        public ActionResult UserCreatedTopics(string spaceKey)
        {
            string title = "我创建的专题";
            IUserService userService = DIContainer.Resolve<IUserService>();
            User spaceUser = userService.GetFullUser(spaceKey);
            if (spaceUser == null)
                return HttpNotFound();
            bool ignoreAudit = false;
            var currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                if (currentUser.UserId == spaceUser.UserId || authorizer.IsAdministrator(TopicConfig.Instance().ApplicationId))
                    ignoreAudit = true;

                if (currentUser.UserId != spaceUser.UserId)
                {
                    title = spaceUser.DisplayName + "创建的专题";
                }
            }
            else
            {
                title = spaceUser.DisplayName + "创建的专题";
            }

            pageResourceManager.InsertTitlePart(title);
            var topics = topicService.GetMyCreatedTopics(spaceUser.UserId, ignoreAudit);
            if (Request.IsAjaxRequest())
                return PartialView("_List", topics);

            ViewData["spaceUser"] = spaceUser;
            ViewData["currentUser"] = currentUser;


            return View(topics);
        }

        /// <summary>
        /// 标签显示专题列表
        /// </summary>
        public ActionResult ListByTag(string tagName, SortBy_Topic sortBy = SortBy_Topic.DateCreated_Desc, int pageIndex = 1)
        {
            tagName = WebUtility.UrlDecode(tagName);
            var tag = new TagService(TenantTypeIds.Instance().Topic()).Get(tagName);

            if (tag == null)
            {
                return HttpNotFound();
            }

            PagingDataSet<TopicEntity> topics = topicService.GetsByTag(tagName, sortBy, pageIndex: pageIndex);
            pageResourceManager.InsertTitlePart(tagName);
            ViewData["tag"] = tag;
            ViewData["sortBy"] = sortBy;
            return View(topics);
        }

        #endregion

        #region 内容块


        /// <summary>
        /// 顶部专题导航
        /// </summary>
        /// <returns></returns>
        public ActionResult _TopicGlobalNavigations()
        {
            IUser CurrentUser = UserContext.CurrentUser;
            IEnumerable<TopicEntity> topics = null;
            if (CurrentUser != null)
            {
                topics = topicService.GetMyCreatedTopics(CurrentUser.UserId, true);

                if (topics.Count() >= 9)
                    return View(topics.Take(9));

                PagingDataSet<TopicEntity> joinedTopics = topicService.GetMyJoinedTopics(CurrentUser.UserId);
                topics = topics.Union(joinedTopics).Take(9);
            }

            return View(topics);
        }

        /// <summary>
        /// 专题排行内容块
        /// </summary>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _TopTopics(int topNumber, string areaCode, long? categoryId, SortBy_Topic? sortBy, string viewName = "_TopTopics_List")
        {
            var topics = topicService.GetTops(topNumber, areaCode, categoryId, sortBy ?? SortBy_Topic.DateCreated_Desc);



            ViewData["SortBy"] = sortBy;
            return PartialView(viewName, topics);
        }


        /// <summary>
        /// 专题分类导航内容块（包含1、2级）
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _CategoryTopics()
        {
            IEnumerable<Category> categories = categoryService.GetRootCategories(TenantTypeIds.Instance().Topic());
            return PartialView(categories);
        }

        /// <summary>
        /// 专题地区导航内容块
        /// </summary>
        /// <returns></returns>
        public ActionResult _AreaTopics(int topNumber, string areaCode, long? categoryId, SortBy_Topic sortBy = SortBy_Topic.DateCreated_Desc)
        {
            IUser iUser = (User)UserContext.CurrentUser;
            User user = null;
            if (iUser == null)
            {
                user = new User();
            }
            else
            {
                user = userService.GetFullUser(iUser.UserId);
            }
            if (string.IsNullOrEmpty(areaCode) && Request.Cookies["AreaTopicCookie" + user.UserId] != null && !string.IsNullOrEmpty(Request.Cookies["AreaTopicCookie" + user.UserId].Value))
                areaCode = Request.Cookies["AreaTopicCookie" + user.UserId].Value;

            if (string.IsNullOrEmpty(areaCode))
            {
                string ip = WebUtility.GetIP();
                areaCode = IPSeeker.Instance().GetAreaCode(ip);
                if (string.IsNullOrEmpty(areaCode) && user.Profile != null)
                {
                    areaCode = user.Profile.NowAreaCode;
                }
            }
            ViewData["areaCode"] = areaCode;
            if (!string.IsNullOrEmpty(areaCode))
            {
                Area area = areaService.Get(areaCode);
                if (!string.IsNullOrEmpty(area.ParentCode))
                {
                    Area parentArea = areaService.Get(area.ParentCode);
                    ViewData["parentCode"] = parentArea.AreaCode;
                }
            }

            IEnumerable<TopicEntity> topics = topicService.GetTops(topNumber, areaCode, categoryId, sortBy);

            HttpCookie cookie = new HttpCookie("AreaTopicCookie" + user.UserId, areaCode);
            Response.Cookies.Add(cookie);

            return PartialView(topics);
        }

        /// <summary>
        /// 人气群主
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendedTopicOwners(int topNumber = 5, string recommendTypeId = null)
        {
            IEnumerable<RecommendItem> recommendUsers = recommendService.GetTops(topNumber, recommendTypeId);


            return PartialView(recommendUsers);
        }
        /// <summary>
        /// 标签地图
        /// </summary>
        /// <returns></returns>
        public ActionResult TopicTagMap()
        {
            pageResourceManager.InsertTitlePart("标签云图");
            return View();
        }
        /// <summary>
        /// 关注按钮
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="followedUser">要关注的用户</param>
        /// <returns></returns>
        public ActionResult _FollowedButton(User currentUser, User followedUser)
        {
            ViewData["currentUser"] = currentUser;
            ViewData["followedUser"] = followedUser;
            if (currentUser != null && currentUser.UserId != followedUser.UserId)
            {
                bool currentUserIsFollowedUser = false;
                ViewData["currentUserIsFollowedUser"] = currentUserIsFollowedUser;
            }
            bool isFollowed = currentUser.IsFollowed(followedUser.UserId);
            ViewData["isFollowed"] = isFollowed;

            return View();
        }

        #endregion

    }
}
