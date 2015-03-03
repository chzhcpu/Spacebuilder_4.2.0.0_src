using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;

using Tunynet.UI;
using Tunynet.Utilities;
//using DevTrends.MvcDonutCaching;

namespace Spacebuilder.Journals.Controllers
{
    /// <summary>
    /// 日志控制器
    /// </summary>
    //[TitleFilter(TitlePart = "杂志", IsAppendSiteName = true)]
    //[Themed(PresentAreaKeysOfBuiltIn.UserSpace, IsApplication = true)]
    //[AnonymousBrowseCheck]
    //[UserSpaceAuthorize]
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [TitleFilter(IsAppendSiteName = true)]
    public class JournalsController : Controller
    {
        public JournalsService journalsService { get; set; }         
        public IPageResourceManager pageResourceManager { get; set; }
        public IUserService userService { get; set; }


        private FavoriteService favoriteService = new FavoriteService(TenantTypeIds.Instance().Journals());
        //
        // GET: /Journals/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 我的日志/Ta的日志
        /// </summary>
        public ActionResult Journals(string spaceKey, int pageIndex = 1)
        {
            PagingDataSet<long> journals = null;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserName == spaceKey)
            {
                //journals = journalsService.GetOwnerThreads(TenantTypeIds.Instance().User(), currentUser.UserId, true, false, null, null, true, 20, pageIndex);
                journals =favoriteService.GetPagingObjectIds(currentUser.UserId, pageIndex);
                //journals = journalsService.GetJournals(pds) as PagingDataSet<Journal>;
                pageResourceManager.InsertTitlePart("我的杂志");
                return View("My", journals);
            }
            else
            {
                User user = userService.GetFullUser(spaceKey);
                if (user == null)
                {
                    return HttpNotFound();
                }

                journals = favoriteService.GetPagingObjectIds(user.UserId, pageIndex);
                //journals = journalsService.GetJournalsByIds(pds) as PagingDataSet<Journal>;

                pageResourceManager.InsertTitlePart(user.DisplayName + "的日志");
                return View("Ta", journals);
            }
        }


        /// <summary>
        /// 日志详细页
        /// </summary>
        public ActionResult Detail(string spaceKey, long journalId)
        {
            Journal journal = journalsService.Get(journalId);

            if (journal == null)
            {
                return HttpNotFound();
            }
                        

            //更新浏览计数
            //CountService countService = new CountService(TenantTypeIds.Instance().Journals);
            //countService.ChangeCount(CountTypes.Instance().HitTimes(), blogThread.ThreadId, blogThread.UserId, 1, false);

            //设置SEO信息
            pageResourceManager.InsertTitlePart(journal.journal);
            pageResourceManager.InsertTitlePart(journal.publisher);

            //List<string> keywords = new List<string>();
            //keywords.AddRange(blogThread.TagNames);
            //keywords.AddRange(blogThread.OwnerCategoryNames);
            //string keyword = string.Join(" ", keywords.Distinct());
            //if (!string.IsNullOrEmpty(blogThread.Keywords))
            //{
            //   keyword += " " + blogThread.Keywords;
            //}

            //pageResourceManager.SetMetaOfKeywords(keyword);

            if (!string.IsNullOrEmpty(journal.jdescrip))
            {
                pageResourceManager.SetMetaOfDescription(HtmlUtility.StripHtml(journal.jdescrip, true, false));
            }

            ISearcher searcher= SearcherFactory.GetSearcher(JournalSearcher.CODE);
            searcher.RebuildIndex();

            /*JournalFullTextQuery query = new JournalFullTextQuery();
            query.PageSize = 5;//每页记录数
            query.PageIndex = 1;
            query.Range = JournalSearchRange.SUBJECT;
            query.Keyword = "history";

            //调用搜索器进行搜索
            JournalSearcher journalSearcher = (JournalSearcher)SearcherFactory.GetSearcher(JournalSearcher.CODE);
            PagingDataSet<Journal> journals = journalSearcher.Search(query);
            */
            return View(journal);
        }


        #region 收藏

        /// <summary>
        ///  添加收藏、取消收藏
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="itemId">微博Id</param>
        /// <param name="userId">用户Id</param>
        [HttpPost]
        public JsonResult Favorite(string spaceKey, long itemId, long userId)
        {
            bool isFavorited = favoriteService.IsFavorited(itemId, userId);

            bool result = false;
            if (isFavorited)
                result = favoriteService.CancelFavorite(itemId, userId);
            else
                result = favoriteService.Favorite(itemId, userId);

            //reply:已修改

            if (result)
            {
                return Json(new { ok = true, msg = "操作成功" });
            }
            else
            {
                return Json(new { msg = "操作失败" });
            }
        }

        /// <summary>
        ///  我的收藏页面
        /// </summary>
        /// <param name="spaceKey">spaceKey</param>
        /// <param name="pageIndex">页数</param>
        [HttpGet]
        [UserSpaceAuthorize(RequireOwnerOrAdministrator = true)]
        public ActionResult ListFavorites(string spaceKey, int? pageIndex)
        {
            pageResourceManager.InsertTitlePart("我的收藏");

            long userId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            PagingDataSet<long> pdsObjectIds = favoriteService.GetPagingObjectIds(userId, pageIndex ?? 1);
            IEnumerable<Journal> journals = journalsService.GetJournals(pdsObjectIds);
            PagingDataSet<Journal> pds = new PagingDataSet<Journal>(journals)
            {
                TotalRecords = pdsObjectIds.TotalRecords,
                PageSize = pdsObjectIds.PageSize,
                PageIndex = pdsObjectIds.PageIndex,
                QueryDuration = pdsObjectIds.QueryDuration
            };

            return View(pds);
        }
#endregion

    }
}
