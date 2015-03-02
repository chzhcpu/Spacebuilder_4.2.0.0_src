//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-08-08</createdate>
//<author>yangmj</author>
//<email>yangmj@tunynet.com</email>
//<log date="2012-08-08" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;

using Tunynet.Search;
using Tunynet.UI;
using Tunynet.Utilities;
using System;
using DevTrends.MvcDonutCaching;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 频道微博
    /// </summary>
    [AnonymousBrowseCheck]
    [TitleFilter(TitlePart = "微博", IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    public partial class ChannelMicroblogController : Controller
    {
        #region Service

        public IPageResourceManager pageResourceManager { get; set; }

        public MicroblogService microblogService { get; set; }
        public CommentService commentService { get; set; }
        public UserService userService { get; set; }
        public FollowService followService { get; set; }
        public RecommendService recommendService { get; set; }
        public PrivacyService privacyService { get; set; }
        private TagService tagService = new TagService(TenantTypeIds.Instance().Microblog());

        #endregion

        #region 公共控件、页面

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult _Header(string tab = "", string mode = "")
        {
            ViewData["tab"] = tab;
            ViewData["mode"] = mode;

            return View();
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult Ranking()
        {
            int topNum = 15;

            ViewData["tagsByPreDayItemCount"] = tagService.GetTopTags(topNum, null, SortBy_Tag.PreDayItemCountDesc);
            ViewData["tagsByPreWeekItemCount"] = tagService.GetTopTags(topNum, null, SortBy_Tag.PreWeekItemCountDesc);
            ViewData["tagsByItemCount"] = tagService.GetTopTags(topNum, null, SortBy_Tag.ItemCountDesc);
            ViewData["usersByFollower"] = userService.GetTopUsers(topNum, SortBy_User.FollowerCount);
            ViewData["usersByReputationPoints"] = userService.GetTopUsers(topNum, SortBy_User.PreWeekReputationPoints);

            pageResourceManager.InsertTitlePart("排行榜");

            return View();
        }

        /// <summary>
        /// 绑定第三方帐号提示信息
        /// </summary>
        /// <param name="accountTypeKey"></param>
        /// <returns></returns>
        public ActionResult _BindThirdAccount(string accountTypeKey)
        {
            var accountBindingService = new AccountBindingService();
            AccountType accountType = accountBindingService.GetAccountType(accountTypeKey);
            var currentUserId = UserContext.CurrentUser != null ? UserContext.CurrentUser.UserId : 0;
            AccountBinding accountBinding = accountBindingService.GetAccountBinding(currentUserId, accountType.AccountTypeKey);
            ViewData["isExpired"] = accountBinding != null && accountBinding.ExpiredDate.CompareTo(DateTime.UtcNow) < 0;
            return View(accountType);
        }
        #endregion

        #region 微博

        /// <summary>
        /// 微博列表模式页
        /// </summary>
        /// <returns></returns>
        public ActionResult Microblog(SortBy_Microblog sortBy = SortBy_Microblog.DateCreated, long tagGroupId = 0)
        {
            pageResourceManager.InsertTitlePart("广场");
            //获取标签分组
            ViewData["tagGroup"] = tagService.GetGroups(TenantTypeIds.Instance().Microblog());

            ViewData["sortBy"] = sortBy;
            ViewData["tagGroupId"] = tagGroupId;
            return View();
        }

        /// <summary>
        /// 获取标签分组
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _TagGroups()
        {
            //获取标签分组
            IEnumerable<TagGroup> listTagGroups = tagService.GetGroups(TenantTypeIds.Instance().Microblog());
            return View(listTagGroups);
        }

        /// <summary>
        /// 最新微博数
        /// </summary>
        /// <param name="lastMicroblogId">之前最后一条微博ID</param>
        /// <returns></returns>
        public JsonResult _GetNewerCount(long lastMicroblogId)
        {
            if (lastMicroblogId == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            int newerCount = microblogService.GetNewerCount(lastMicroblogId);
            return Json(newerCount, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 最新的微博
        /// </summary>
        /// <param name="lastMicroblogId">之前最后一条微博ID</param>
        /// <returns></returns>
        public ActionResult _GetNewerMicroblogs(long lastMicroblogId)
        {
            IEnumerable<MicroblogEntity> microblogs = microblogService.GetNewerMicroblogs(lastMicroblogId, TenantTypeIds.Instance().User());
            ////获取最新微博中的最新一条微博的ID
            //if (microblogs.Count() > 0)
            //{
            //    ViewData["lastMicroblogId"] = microblogs.OrderByDescending(m => m.MicroblogId).Select(m => m.MicroblogId).First();
            //}
            return View(microblogs);
        }

        /// <summary>
        /// 微博排行列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="sortBy"></param>
        /// <param name="tagGroupId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _List(SortBy_Microblog? sortBy = null, long? tagGroupId = null, int pageIndex = 1)
        {
            //获取微博分页数据
            PagingDataSet<MicroblogEntity> microblogs = null;
            if (tagGroupId.HasValue && tagGroupId.Value > 0)
            {
                IEnumerable<string> tagNames = tagService.GetTagsOfGroup(tagGroupId.Value);
                MicroblogFullTextQuery query = new MicroblogFullTextQuery();
                query.PageIndex = pageIndex;
                query.PageSize = 30;//每页记录数
                query.Keywords = tagNames;
                //调用搜索器进行搜索
                MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
                microblogs = microblogSearcher.Search(query);
            }
            else
            {
                microblogs = microblogService.GetPagings(pageIndex, tenantTypeId: TenantTypeIds.Instance().User(), sortBy: sortBy ?? SortBy_Microblog.DateCreated);

                ////当第一次加载页面时获取当前页的最新一条微博的ID
                //if (pageIndex == 1)
                //{
                //    ViewData["lastMicroblogId"] = microblogs.OrderByDescending(m => m.MicroblogId).Select(m => m.MicroblogId).FirstOrDefault();
                //}
            }
            return View(microblogs);
        }


        #region 图片模式

        /// <summary>
        /// 微博图片模式页
        /// </summary>
        /// <returns></returns>
        public ActionResult Waterfall()
        {
            pageResourceManager.InsertTitlePart("广场");
            return View();
        }

        /// <summary>
        /// 微博图片模式数据页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="mediaType"></param>
        /// <param name="isOriginal"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult _Waterfall(int pageIndex = 1, string tenantTypeId = "", MediaType? mediaType = null, bool? isOriginal = null, SortBy_Microblog sortBy = SortBy_Microblog.DateCreated)
        {
            //获取微博分页数据
            PagingDataSet<MicroblogEntity> MicroblogEntities = microblogService.GetPagings(pageIndex, tenantTypeId: TenantTypeIds.Instance().User(), mediaType: mediaType, sortBy: sortBy);

            //获取微博图片
            AttachmentService<Attachment> attachementService = new AttachmentService<Attachment>(TenantTypeIds.Instance().Microblog());
            foreach (var MicroblogEntity in MicroblogEntities.Where(n => n.HasPhoto))
            {
                IEnumerable<Attachment> attachments = attachementService.GetsByAssociateId(MicroblogEntity.MicroblogId);

                if (attachments != null && attachments.Count<Attachment>() > 0)
                {
                    MicroblogEntity.ImageWidth = attachments.First().Width;
                    MicroblogEntity.ImageUrl = SiteUrls.Instance().ImageUrl(attachments.First(), TenantTypeIds.Instance().Microblog(), ImageSizeTypeKeys.Instance().Big());
                }
            }

                //设置当前登录用户对当前页用户的关注情况

                    //如果当前登录用户关注了该用户

            return View(MicroblogEntities.AsEnumerable<MicroblogEntity>());
        }

        /// <summary>
        /// 翻页获取微博(图片模式 按标签分组搜时)
        /// </summary>
        /// <param name="tagGroupId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult _WaterfallGetMicroblogByTagGroup(long tagGroupId, int pageIndex = 1, int pageSize = 30)
        {
            IEnumerable<string> tagNames = tagService.GetTagsOfGroup(tagGroupId);
            MicroblogFullTextQuery query = new MicroblogFullTextQuery();
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;//每页记录数
            query.Keywords = tagNames;

            //调用搜索器进行搜索
            MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            PagingDataSet<MicroblogEntity> microblogEntities = microblogSearcher.Search(query);
            return View("_Waterfall", microblogEntities.AsEnumerable<MicroblogEntity>());
        }

        #endregion

        #region 搜索

        /// <summary>
        /// 微博搜索
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Search(MicroblogFullTextQuery query)
        {
            query.Keyword = WebUtility.UrlDecode(query.Keyword);
            query.PageSize = 20;//每页记录数

            //调用搜索器进行搜索
            MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            PagingDataSet<MicroblogEntity> microblogEntities = microblogSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                SearchHistoryService searchHistoryService = new SearchHistoryService();
                searchHistoryService.SearchTerm(CurrentUser.UserId, MicroblogSearcher.CODE, query.Keyword);
            }
            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(MicroblogSearcher.CODE, query.Keyword);
            }

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart('“' + query.Keyword + '”' + "的相关微博");//设置页面Title
            }

            return View(microblogEntities);
        }

        /// <summary>
        /// 微博搜索
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult _GlobalSearch(MicroblogFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.IsGroup = false;

            //调用搜索器进行搜索
            MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            PagingDataSet<MicroblogEntity> microblogEntities = microblogSearcher.Search(query);

            return PartialView(microblogEntities);
        }

        /// <summary>
        /// 微博快捷搜索
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult _QuickSearch(MicroblogFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.IsGroup = false;

            //调用搜索器进行搜索
            MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            PagingDataSet<MicroblogEntity> microblogEntities = microblogSearcher.Search(query);

            return PartialView(microblogEntities);
        }

        /// <summary>
        /// 微博搜索自动完成
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {

            //调用搜索器进行搜索
            MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            IEnumerable<string> topics = microblogSearcher.AutoCompleteSearch(keyword, topNumber);
            if (topics != null)
            {
                var jsonResult = Json(topics.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
                return jsonResult;
            }

            return null;
        }

        /// <summary>
        /// 重建用户索引，为测试用户搜索时的临时功能，待后台相关功能完成需删除
        /// </summary>
        /// <returns></returns>
        public JsonResult RebuildMicroblogIndex()
        {
            ISearcher microblogSearcher = SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            microblogSearcher.RebuildIndex();

            return Json(microblogSearcher, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 评论

        //todo:bianchx,by zhengw:ownerId是什么？觉得没必要传递originalAuthor，可以通过commentedObjectId获取到
        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public ActionResult _Comment(long commentedObjectId, long ownerId, string tenantTypeId, string originalAuthor = null, string subject = null)
        {
            MicroblogCommentEditModel commentModel = new MicroblogCommentEditModel
            {
                CommentedObjectId = commentedObjectId,
                OwnerId = ownerId,
                TenantTypeId = tenantTypeId,
                OriginalAuthor = originalAuthor,
                Subject = subject
            };
            return View(commentModel);
        }
        //todo:bianchx,by zhengw:建议parentId改为可空参数，因为在SiteUrls中_MicroblogChildComment方法并没有传递parentId参数，这样会导致生成Url会抛异常的情况。之前版本是抛异常的，你没有报错吗？
        //在Channel下存在相同问题
        /// <summary>
        /// 子级评论（首先加载评论控件。异步添加评论）
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>子级评论</returns>
        [HttpGet]
        public ActionResult _ChildComment(long? parentId)
        {
            if (!parentId.HasValue) return HttpNotFound();

            CommentService commentService = new CommentService();
            Comment comment = commentService.Get(parentId.Value);
            if (comment == null) return HttpNotFound();

            MicroblogEntity entity = microblogService.Get(comment.CommentedObjectId);
            if (entity == null) return HttpNotFound();

            MicroblogCommentEditModel editModel = comment.AsMicroblogCommentEditModel();
            editModel.OriginalAuthor = entity.Author;

            return View(editModel);
        }

        /// <summary>
        /// 留言列表
        /// </summary>
        /// <returns>留言列表</returns>
        [HttpGet]
        public ActionResult _CommentList(string tenantType, long commentedObjectId, int? pageIndex = null, SortBy_Comment sortBy = SortBy_Comment.DateCreatedDesc, long? commentId = null, bool showBefore = true, bool showAfter = false)
        {
            ViewData["tenantType"] = tenantType;
            ViewData["commentedObjectId"] = commentedObjectId;
            if (commentId.HasValue)
            {
                pageIndex = new CommentService().GetPageIndexForCommentInCommens(commentId.Value, tenantType, commentedObjectId, sortBy);
                showAfter = true;
                showBefore = true;
            }
            if (!pageIndex.HasValue)
                pageIndex = 1;
            ViewData["ShowBefore"] = showBefore;
            ViewData["ShowAfter"] = showAfter;
            return View(new CommentService().GetRootComments(tenantType, commentedObjectId, pageIndex.Value, sortBy));
        }

        /// <summary>
        /// 获取一条评论
        /// </summary>
        /// <param name="id">评论id</param>
        /// <returns>获取一条评论</returns>
        public ActionResult _OneComment(long id)
        {
            Comment comment = new CommentService().Get(id);
            if (comment == null)
                return Content(string.Empty);

            return View(comment);
        }

        /// <summary>
        /// 创建评论
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PostInterval(PostIntervalType = PostIntervalType.MicroContent)]
        public ActionResult Comment(MicroblogCommentEditModel model)
        {

            string message = string.Empty;
            if (ModelState.HasBannedWord(out message))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, message));
            }

            IUser currentUser = UserContext.CurrentUser;
            long userId = microblogService.Get(model.CommentedObjectId).UserId;
            //被评论用户的隐私判断

            if (!privacyService.Validate(userId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().Comment()))
            {
                return Json(new StatusMessageData(StatusMessageType.Hint, "该用户不允许你评论他的内容！"));
            }

            CommentService commentService = new CommentService();
            if (model.IsValidate)
            {
                Comment comment = model.AsComment();
                //todo:bianchx,by zhengw:可以考虑在model.AsComment方法中处理IsPrivate问题
                if (comment.ParentId != 0)
                {
                    Comment parentComment = commentService.Get(comment.ParentId);
                    if (parentComment != null)
                        comment.IsPrivate = parentComment.IsPrivate ? true : comment.IsPrivate;
                }

                if (commentService.Create(comment))
                {
                    if (model.CommentOriginalAuthor)
                    {
                        MicroblogEntity entity = microblogService.Get(comment.CommentedObjectId);
                        //todo:bianchx,by zhengw:应判断entity.User是否为null

                        if (entity != null)
                        {
                            Comment originalAuthorComment = model.AsComment();
                            entity = entity.OriginalMicroblog;
                            if (entity != null)
                            {
                                originalAuthorComment.ToUserId = entity.UserId;
                                originalAuthorComment.ToUserDisplayName = entity.User.DisplayName;
                                originalAuthorComment.CommentedObjectId = entity.MicroblogId;
                                commentService.Create(originalAuthorComment);
                            }
                        }
                    }
                    if (model.ForwardMicrobo)
                    {
                        MicroblogEntity microblogEntity = microblogService.Get(model.CommentedObjectId);
                        if (microblogEntity != null)
                        {
                            MicroblogEntity microblog = MicroblogEntity.New();
                            microblog.Body = "转发微博";
                            microblog.Author = currentUser.DisplayName;
                            microblog.UserId = currentUser.UserId;
                            microblog.OwnerId = currentUser.UserId;
                            microblog.TenantTypeId = TenantTypeIds.Instance().User();

                            microblog.ForwardedMicroblogId = microblogEntity.MicroblogId;
                            microblog.OriginalMicroblogId = microblogEntity.OriginalMicroblogId > 0 ? microblogEntity.OriginalMicroblogId : microblog.ForwardedMicroblogId;

                            long toUserId = microblog.UserId;

                            MicroblogEntity entity = microblogService.Get(microblog.OriginalMicroblogId);
                            long toOriginalUserId = entity == null ? 0 : entity.UserId;

                            long microblogId = 0;
                            microblogService.Forward(microblog, false, false, toUserId, toOriginalUserId, out microblogId);
                        }
                    }
                    return Json(new { commentid = comment.Id });
                }
            }
            WebUtility.SetStatusCodeForError(Response);
            return Json(new StatusMessageData(StatusMessageType.Error, "创建留言失败了！"));
        }

        #endregion

        #endregion

        #region 话题

        /// <summary>
        /// 话题详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult Topic(string topic)
        {
            topic = WebUtility.UrlDecode(topic);

            Tag tag = tagService.Get(topic);
            if (tag == null)
            {
                return HttpNotFound();
            }

            if (UserContext.CurrentUser != null)
            {
                FavoriteService FavoriteService = new FavoriteService(TenantTypeIds.Instance().Tag());
                ViewData["isFavorited"] = FavoriteService.IsFavorited(tag.TagId, UserContext.CurrentUser.UserId);
            }
            pageResourceManager.InsertTitlePart(tag.TagName);
            if (!string.IsNullOrEmpty(tag.Description))
            {
                pageResourceManager.SetMetaOfDescription(HtmlUtility.StripHtml(tag.Description, true, false));
            }
            //话题下有没有微博
            string tagName = tag.TagName;
            return View(tag);
        }

        /// <summary>
        /// 话题相关的微博列表
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult _TopicMicroblogs(long tagId, int pageSize = 20, int pageIndex = 1)
        {
            Tag tag = tagService.Get(tagId);
            string tagName = tag.TagName;

            //调用搜索器进行搜索
            MicroblogFullTextQuery query = new MicroblogFullTextQuery();
            query.PageIndex = pageIndex;
            query.PageSize = pageSize;//每页记录数
            query.Keywords = new List<string> { tagName };

            MicroblogSearcher microblogSearcher = (MicroblogSearcher)SearcherFactory.GetSearcher(MicroblogSearcher.CODE);
            PagingDataSet<MicroblogEntity> microblogs = microblogSearcher.Search(query);
            ViewData["tagId"] = tagId;
            return View(microblogs);
        }

        /// <summary>
        /// 推荐话题
        /// </summary>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendTopic(int topNum = 10, string recommendTypeId = "10010101")
        {
            IEnumerable<RecommendItem> recommendTopics = recommendService.GetTops(topNum, recommendTypeId);
            return View(recommendTopics);
        }

        /// <summary>
        /// 指定数量的话题列表
        /// </summary>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _TopTopics(int topNum, bool? isFeatured, SortBy_Tag? sortby)
        {
            IEnumerable<Tag> topics = tagService.GetTopTags(topNum, isFeatured, sortby ?? SortBy_Tag.ItemCountDesc);
            return View(topics);
        }

        #endregion

        #region 用户

        /// <summary>
        /// 人气关注
        /// </summary>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _HotFollower(int topNum = 10)
        {
            var hotFollower = userService.GetTopUsers(topNum, SortBy_User.FollowerCount);
            return View(hotFollower);
        }

        /// <summary>
        /// 人气微博
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _RelevantMicroblog(int topNum = 9, string tagName = null)
        {
            List<IUser> relevantMicroblog = new List<IUser>();
            if (!string.IsNullOrEmpty(tagName))
            {
                Tag tag = tagService.Get(tagName);
                if (!string.IsNullOrEmpty(tag.RelatedObjectIds))
                {
                    IEnumerable<long> userIds = tag.RelatedObjectIds.TrimStart(',').TrimEnd(',').Split(',').Select(n => long.Parse(n));
                    if (userIds != null && userIds.Count() > 0)
                    {
                        relevantMicroblog = userService.GetUsers(userIds).ToList();
                    }
                }
            }
            return View(relevantMicroblog);
        }

        #endregion
    }
}