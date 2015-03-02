//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-10-16</createdate>
//<author>jiangshl</author>
//<email>jiangshl@tunynet.com</email>
//<log date="2012-10-16" version="0.5">创建</log>
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
using DevTrends.MvcDonutCaching;

namespace Spacebuilder.Blog.Controllers
{
    /// <summary>
    /// 日志频道控制器
    /// </summary>
    [TitleFilter(TitlePart = "日志", IsAppendSiteName = true)]
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [AnonymousBrowseCheck]
    public class ChannelBlogController : Controller
    {
        public BlogService blogService { get; set; }
        public CategoryService categoryService { get; set; }
        public IPageResourceManager pageResourceManager { get; set; }
        public UserService userService { get; set; }
        public RecommendService recommendService { get; set; }
        private OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
        private BlogSettings blogSettings = DIContainer.Resolve<ISettingsManager<BlogSettings>>().Get();
        private IUser currentUser = UserContext.CurrentUser;

        #region 日志频道页面

        /// <summary>
        /// 日志频道首页
        /// </summary>
        public ActionResult Home(int pageIndex = 1)
        {
            PagingDataSet<BlogThread> blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, null, null, null, SortBy_BlogThread.DateCreated_Desc, 15, pageIndex);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_List", blogs);
            }

            //获取推荐日志
            //图片推荐（幻灯片）
            IEnumerable<RecommendItem> recommendPicItems = recommendService.GetTops(5, blogSettings.RecommendPicTypeId);
            int linkCount = recommendPicItems.Where(n => n.IsLink).Where(n => !string.IsNullOrEmpty(n.FeaturedImage)).Count();
            int blogCount = blogService.GetBlogThreads(recommendPicItems.Where(n => !n.IsLink).Select(n => n.ItemId)).Where(n => !string.IsNullOrEmpty(n.FeaturedImage)).Count();
            int count = recommendPicItems.Where(n => !n.IsLink).Where(n => !string.IsNullOrEmpty(n.FeaturedImage)).Count();
            int recommendCount = System.Math.Max(count, blogCount) + linkCount;
            ViewData["recommendPicBlogs"] = recommendPicItems;


            //文字推荐（幻灯片右侧文字区）
            IEnumerable<RecommendItem> recommendWordItems = recommendService.GetTops(6, blogSettings.RecommendWordTypeId);
            IEnumerable<BlogThread> recommendWordBlogsAll = blogService.GetBlogThreads(recommendWordItems.Where(n => !n.IsLink).Select(n => n.ItemId));

            ViewData["recommendCount"] = recommendCount;
            ViewData["recommendWordItems"] = recommendWordItems;
            pageResourceManager.InsertTitlePart("日志首页");

            return View(blogs);
        }

        /// <summary>
        /// 日志分类列表页
        /// </summary>
        /// <param name="categoryId">分类id</param>
        public ActionResult ListByCategory(long categoryId, int pageIndex = 1)
        {
            Category category = categoryService.Get(categoryId);


            if (category == null)
            {
                return HttpNotFound();
            }

            //设置路由数据中的当前导航Id
            TempData["CurrentNavigationId"] = NavigationService.GenerateDynamicNavigationId(categoryId);

            PagingDataSet<BlogThread> blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, categoryId, null, null, Blog.SortBy_BlogThread.DateCreated_Desc, 20, pageIndex);
            ViewData["categoryId"] = categoryId;
            pageResourceManager.InsertTitlePart(category.CategoryName);
            return View(blogs);
        }

        /// <summary>
        /// 日志排行页
        /// </summary>
        /// <param name="rank">区分最新日志，热门日志，热评日志，精华日志</param>
        //[DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult ListByRank(string rank, int pageIndex = 1)
        {
            PagingDataSet<BlogThread> blogs = null;

            //最新日志
            if (rank == "new")
            {
                blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, null, null, null, SortBy_BlogThread.DateCreated_Desc, 20, pageIndex);
                pageResourceManager.InsertTitlePart("最新日志");
            }
            //热门日志
            else if (rank == "hot")
            {
                blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, null, null, null, SortBy_BlogThread.StageHitTimes, 20, pageIndex);
                pageResourceManager.InsertTitlePart("热门日志");
            }
            //热评日志
            else if (rank == "comment")
            {
                blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, null, null, null, SortBy_BlogThread.CommentCount, 20, pageIndex);
                pageResourceManager.InsertTitlePart("热评日志");
            }
            //精华日志
            else
            {
                blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, null, null, true, SortBy_BlogThread.DateCreated_Desc, 20, pageIndex);
                pageResourceManager.InsertTitlePart("精华日志");
            }

            ViewData["rank"] = rank;
            return View(blogs);
        }

        /// <summary>
        /// 标签显示日志列表
        /// </summary>
        public ActionResult ListByTag(string tagName, int pageIndex = 1)
        {
            tagName = WebUtility.UrlDecode(tagName);
            var tag = new TagService(TenantTypeIds.Instance().BlogThread()).Get(tagName);

            if (tag == null)
            {
                return HttpNotFound();
            }

            PagingDataSet<BlogThread> blogs = blogService.Gets(TenantTypeIds.Instance().User(), null, false, true, null, tagName, null, SortBy_BlogThread.DateCreated_Desc, 20, pageIndex);
            pageResourceManager.InsertTitlePart(tagName);
            ViewData["tag"] = tag;
            return View(blogs);
        }

        #endregion

        #region 侧边栏及局部页


        /// <summary>
        /// 推荐用户列表
        /// </summary>        
        [ChildActionOnly]
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendUser(int topNum = 5)
        {
            IEnumerable<RecommendItem> recommendUser = recommendService.GetTops(topNum, blogSettings.RecommendUserTypeId);
            if (recommendUser != null && recommendUser.Count() > 0)
            {
                IEnumerable<long> userIds = recommendUser.Select(n => n.ItemId);
                IEnumerable<IUser> users = userService.GetFullUsers(userIds);
                Dictionary<long, IUser> usersDic = users.ToDictionary(n => n.UserId, n => n);
                ViewData["users"] = usersDic;
            }
            return View(recommendUser);
        }

        /// <summary>
        /// 用户排行列表
        /// </summary>
        [ChildActionOnly]
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _UserRank(int topNum = 5)
        {
            IEnumerable<long> userIds = ownerDataService.GetTopOwnerIds(OwnerDataKeys.Instance().ThreadCount(), topNum, OwnerData_SortBy.LongValue_DESC);
            IEnumerable<IUser> users = userService.GetFullUsers(userIds);
            Dictionary<long, long> userThreadCount = new Dictionary<long, long>();
            foreach (long userId in userIds)
            {
                //用户日志数
                long threadCount = ownerDataService.GetLong(userId, OwnerDataKeys.Instance().ThreadCount());
                userThreadCount[userId] = threadCount;
            }

            ViewData["userThreadCount"] = userThreadCount;
            return View(users);
        }

        /// <summary>
        /// 热评日志
        /// </summary>
        [ChildActionOnly]
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _HotComment(int topNum = 6)
        {
            IEnumerable<BlogThread> blogs = blogService.GetTops(TenantTypeIds.Instance().User(), topNum, null, null, SortBy_BlogThread.CommentCount);
            ViewData["blogs"] = blogs;
            return View(blogs);
        }

        /// <summary>
        /// 推荐日志
        /// </summary>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _Recommend(int topNum = 6, string recommendTypeId = null)
        {
            IEnumerable<RecommendItem> recommendBlogs = recommendService.GetTops(topNum, recommendTypeId);
            return View(recommendBlogs);
        }

        #endregion

        #region 日志全文检索

        /// <summary>
        /// 日志搜索
        /// </summary>
        public ActionResult Search(BlogFullTextQuery query)
        {
            query.Keyword = WebUtility.UrlDecode(query.Keyword);
            query.PageSize = 20;//每页记录数

            //调用搜索器进行搜索
            BlogSearcher blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
            PagingDataSet<BlogThread> blogThreads = blogSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    SearchHistoryService searchHistoryService = new SearchHistoryService();
                    searchHistoryService.SearchTerm(CurrentUser.UserId, BlogSearcher.CODE, query.Keyword);
                }
            }

            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(BlogSearcher.CODE, query.Keyword);
            }

            //获取站点分类，并设置站点分类的选中项
            IEnumerable<Category> siteCategories = categoryService.GetOwnerCategories(0, TenantTypeIds.Instance().BlogThread());
            SelectList siteCategoryList = new SelectList(siteCategories.Select(n => new { text = n.CategoryName, value = n.CategoryId }), "value", "text", query.SiteCategoryId);
            ViewData["siteCategoryList"] = siteCategoryList;

            //设置页面Meta
            if (string.IsNullOrWhiteSpace(query.Keyword))
            {
                pageResourceManager.InsertTitlePart("日志搜索");//设置页面Title
            }
            else
            {
                pageResourceManager.InsertTitlePart(query.Keyword + "的相关日志");//设置页面Title
            }

            return View(blogThreads);
        }

        /// <summary>
        /// 日志全局搜索
        /// </summary>
        public ActionResult _GlobalSearch(BlogFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;

            //调用搜索器进行搜索
            BlogSearcher blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
            PagingDataSet<BlogThread> blogThreads = blogSearcher.Search(query);

            return PartialView(blogThreads);
        }

        /// <summary>
        /// 日志快捷搜索
        /// </summary>
        public ActionResult _QuickSearch(BlogFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.Range = BlogSearchRange.SUBJECT;
            query.Keyword = Server.UrlDecode(query.Keyword);

            //调用搜索器进行搜索
            BlogSearcher blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
            PagingDataSet<BlogThread> blogThreads = blogSearcher.Search(query);

            return PartialView(blogThreads);
        }

        /// <summary>
        /// 日志搜索自动完成
        /// </summary>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {
            //调用搜索器进行搜索
            BlogSearcher blogSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
            IEnumerable<string> terms = blogSearcher.AutoCompleteSearch(keyword, topNumber);

            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion
    }
}
