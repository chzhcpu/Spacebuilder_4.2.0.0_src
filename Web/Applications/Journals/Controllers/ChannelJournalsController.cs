//<TunynetCopyright>
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
using Tunynet.Utilities;
using Spacebuilder.Search;


namespace Spacebuilder.Journals.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = true)]
    [TitleFilter(IsAppendSiteName = true)]
    public class ChannelJournalsController : Controller
    {
        public JournalsService journalsService { get; set; }
        public CategoryService categoryService { get; set; }
        public IPageResourceManager pageResourceManager { get; set; }
        public UserService userService { get; set; }
        public RecommendService recommendService { get; set; }

        /// <summary>
        /// 主页
        /// </summary>
        public ActionResult Home(int pageindex=1)
        {
            var jnls = journalsService.Gets(pageindex, 8, "id");
            return View(jnls);
        }



        #region 日志全文检索

        /// <summary>
        /// 日志搜索
        /// </summary>
        public ActionResult Search(JournalFullTextQuery query)
        {
            query.Keyword = WebUtility.UrlDecode(query.Keyword);
            query.PageSize = 10;//每页记录数

            //调用搜索器进行搜索
            JournalSearcher journalSearcher = (JournalSearcher)SearcherFactory.GetSearcher(JournalSearcher.CODE);
            PagingDataSet<Journal> journals = journalSearcher.Search(query);

            //添加到用户搜索历史 
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    SearchHistoryService searchHistoryService = new SearchHistoryService();
                    searchHistoryService.SearchTerm(CurrentUser.UserId, JournalSearcher.CODE, query.Keyword);
                }
            }

            //添加到热词
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                SearchedTermService searchedTermService = new SearchedTermService();
                searchedTermService.SearchTerm(JournalSearcher.CODE, query.Keyword);
            }

            //获取站点分类，并设置站点分类的选中项
            IEnumerable<Category> siteCategories = categoryService.GetOwnerCategories(0, TenantTypeIds.Instance().Journals());
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

            return View(journals);
        }

        /// <summary>
        /// 日志全局搜索
        /// </summary>
        public ActionResult _GlobalSearch(JournalFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;

            //调用搜索器进行搜索
            JournalSearcher journalSearcher = (JournalSearcher)SearcherFactory.GetSearcher(JournalSearcher.CODE);
            PagingDataSet<Journal> journals = journalSearcher.Search(query);

            return PartialView(journals);
        }

        /// <summary>
        /// 日志快捷搜索
        /// </summary>
        public ActionResult _QuickSearch(JournalFullTextQuery query, int topNumber)
        {
            query.PageSize = topNumber;//每页记录数
            query.PageIndex = 1;
            query.Range = JournalSearchRange.SUBJECT;
            query.Keyword = Server.UrlDecode(query.Keyword);

            //调用搜索器进行搜索
            JournalSearcher journalSearcher = (JournalSearcher)SearcherFactory.GetSearcher(JournalSearcher.CODE);
            PagingDataSet<Journal> journals = journalSearcher.Search(query);

            return PartialView(journals);
        }

        /// <summary>
        /// 日志搜索自动完成
        /// </summary>
        public JsonResult SearchAutoComplete(string keyword, int topNumber)
        {
            //调用搜索器进行搜索
            JournalSearcher journalSearcher = (JournalSearcher)SearcherFactory.GetSearcher(JournalSearcher.CODE);
            IEnumerable<string> terms = journalSearcher.AutoCompleteSearch(keyword, topNumber);

            var jsonResult = Json(terms.Select(t => new { tagName = t, tagNameWithHighlight = SearchEngine.Highlight(keyword, string.Join("", t.Take(34)), 100) }), JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion


    }
}
