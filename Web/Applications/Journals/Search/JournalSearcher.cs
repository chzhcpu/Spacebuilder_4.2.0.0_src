//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-10-29</createdate>
//<author>wanf</author>
//<email>wanf@tunynet.com</email>
//<log date="2012-10-29" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>
using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Search;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// 日志搜索器
    /// </summary>
    public class JournalSearcher : ISearcher
    {
        private JournalsService journalService = new JournalsService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Journals());
        private CategoryService categoryService = new CategoryService();
        private AuditService auditService = new AuditService();
        private ISearchEngine searchEngine;
        private PubliclyAuditStatus publiclyAuditStatus;
        private SearchedTermService searchedTermService = new SearchedTermService();
        public static string CODE = "JournalSearcher";
        public static string WATERMARK = "搜索Journals";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public JournalSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
            publiclyAuditStatus = auditService.GetPubliclyAuditStatus(JournalsConfig.Instance().ApplicationId);
        }

        #region 搜索器属性

        /// <summary>
        /// 是否作为快捷搜索
        /// </summary>
        public bool AsQuickSearch { get; private set; }

        public string WaterMark { get { return WATERMARK; } }

        /// <summary>
        /// 关联的搜索引擎实例
        /// </summary>
        public ISearchEngine SearchEngine
        {
            get
            {
                return searchEngine;
            }
        }

        /// <summary>
        /// 搜索器的唯一标识
        /// </summary>
        public string Code { get { return CODE; } }

        /// <summary>
        /// 是否前台显示
        /// </summary>
        public bool IsDisplay
        {
            get { return true; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; private set; }

        /// <summary>
        /// 处理全局搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string GlobalSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().JournalsGlobalSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// Lucene索引路径（完整物理路径，支持unc）
        /// </summary>
        public string IndexPath { get; private set; }

        /// <summary>
        /// 是否基于Lucene实现
        /// </summary>
        public bool IsBaseOnLucene
        {
            get { return true; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 处理快捷搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string QuickSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().JournalsQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().JournalsPageSearch(keyword);
        }

        #endregion

        #region 索引内容维护


        /// <summary>
        /// 重建索引
        /// </summary>  
        public void RebuildIndex()
        {
            //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，
            //客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，
            //pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，
            //但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            int pageSize = 1000;
            int pageIndex = 1;
            long totalRecords = 0;
            bool isBeginning = true;
            bool isEndding = false;
            do
            {
                //分页获取帖子列表
                PagingDataSet<Journal> journals = journalService.Gets(pageIndex,pageSize,  "id");
                totalRecords = journals.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //重建索引
                List<Journal> journalList = journals.ToList<Journal>();

                IEnumerable<Document> docs = JournalIndexDocument.Convert(journalList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="journal">待添加的日志</param>
        public void Insert(Journal journal)
        {
            Insert(new Journal[] { journal });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="blogThreads">待添加的日志</param>
        public void Insert(IEnumerable<Journal> journals)
        {
            IEnumerable<Document> docs = JournalIndexDocument.Convert(journals);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="journalId">待删除的日志Id</param>
        public void Delete(long journalId)
        {
            searchEngine.Delete(journalId.ToString(), JournalIndexDocument.id);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="journalIds">待删除的日志Id列表</param>
        public void Delete(IEnumerable<long> journalIds)
        {
            foreach (var id in journalIds)
            {
                searchEngine.Delete(id.ToString(), JournalIndexDocument.id);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="journal">待更新的日志</param>
        public void Update(Journal journal)
        {
            Document doc = JournalIndexDocument.Convert(journal);
            searchEngine.Update(doc, journal.id.ToString(), JournalIndexDocument.id);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="journals">待更新的日志集合</param>
        public void Update(IEnumerable<Journal> journals)
        {
            IEnumerable<Document> docs = JournalIndexDocument.Convert(journals);
            IEnumerable<string> blogThreadIds = journals.Select(n => n.id.ToString());
            searchEngine.Update(docs, blogThreadIds, JournalIndexDocument.id);
        }

        #endregion

        #region 搜索
        /// <summary>
        /// 日志分页搜索
        /// </summary>
        /// <param name="journalQuery">搜索条件</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<Journal> Search(JournalFullTextQuery journalQuery)
        {
            if (journalQuery.SiteCategoryId == 0 && journalQuery.LoginId == 0 && journalQuery.UserId == 0)
            {
                if (string.IsNullOrWhiteSpace(journalQuery.Keyword) && journalQuery.IsRelationJournal == false)
                {
                    return new PagingDataSet<Journal>(new List<Journal>());
                }
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(journalQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, journalQuery.PageIndex, journalQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的日志ID
            List<long> journalIds = new List<long>();
            //获取索引中日志的标签
            Dictionary<long, IEnumerable<string>> journalTags = new Dictionary<long, IEnumerable<string>>();
            //获取索引中日志的用户分类名
            Dictionary<long, IEnumerable<string>> journalOwnerCategoryNames = new Dictionary<long, IEnumerable<string>>();

            foreach (Document doc in docs)
            {
                long journalid = long.Parse(doc.Get(JournalIndexDocument.id));
                journalIds.Add(journalid);
                //journalTags[journalid] = doc.GetValues(JournalIndexDocument.Tag).ToList<string>();
                //journalOwnerCategoryNames[journalid] = doc.GetValues(JournalIndexDocument.OwnerCategoryName).ToList<string>();
            }

            //根据日志ID列表批量查询日志实例
            IEnumerable<Journal> journalList = journalService.GetJournalsByIds(journalIds);

            /*foreach (var journal in journalList)
            {
                journal.Body = journal.GetBody();
                if (blogTags.ContainsKey(journal.ThreadId))
                {
                    journal.TagNames = blogTags[journal.ThreadId];
                }
                if (journalOwnerCategoryNames.ContainsKey(journal.ThreadId))
                {
                    journal.OwnerCategoryNames = journalOwnerCategoryNames[journal.ThreadId];
                }
            }
            */
            //组装分页对象
            PagingDataSet<Journal> journals = new PagingDataSet<Journal>(journalList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return journals;
        }

        /// <summary>
        /// 获取匹配的前几条日志热词
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="topNumber">前N条</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public IEnumerable<string> AutoCompleteSearch(string keyword, int topNumber)
        {
            IEnumerable<SearchedTerm> searchedAdminTerms = searchedTermService.GetManuals(keyword, CODE);
            IEnumerable<SearchedTerm> searchedUserTerms = searchedTermService.GetTops(keyword, topNumber, CODE);
            IEnumerable<SearchedTerm> listSearchAdminUserTerms = searchedAdminTerms.Union(searchedUserTerms);
            if (listSearchAdminUserTerms.Count() > topNumber)
            {
                listSearchAdminUserTerms.Take(topNumber);
            }
            return listSearchAdminUserTerms.Select(t => t.Term);
        }

        /// <summary>
        /// 根据帖吧搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(JournalFullTextQuery journalQuery)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            //范围
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();

            //如果查的是相关日志
            if (journalQuery.IsRelationJournal)
            {
                fieldNameAndBoosts.Add(JournalIndexDocument.journalname, BoostLevel.Hight);
                fieldNameAndBoosts.Add(JournalIndexDocument.subcat, BoostLevel.Medium);
                fieldNameAndBoosts.Add(JournalIndexDocument.publisher, BoostLevel.Low);
                searchBuilder.WithPhrases(fieldNameAndBoosts, journalQuery.Keywords, BooleanClause.Occur.SHOULD, false);
                //searchBuilder.WithField(JournalIndexDocument.PrivacyStatus, ((int)PrivacyStatus.Public).ToString(), true, BoostLevel.Hight, true);
                searchBuilder.NotWithField(JournalIndexDocument.id.ToString(), journalQuery.CurrentThreadId.ToString());
            }
            else
            {
                switch (journalQuery.JournalRange)
                {
                    /*case JournalRange.MYBlOG:
                        searchBuilder.WithField(JournalIndexDocument.UserId, journalQuery.LoginId.ToString(), true, BoostLevel.Hight, false);
                        break;
                    case JournalRange.SOMEONEBLOG:
                        searchBuilder.WithField(JournalIndexDocument.UserId, journalQuery.UserId.ToString(), true, BoostLevel.Hight, false);
                        break;
                    case JournalRange.SITECATEGORY:
                        if (journalQuery.LoginId != 0)
                            searchBuilder.WithField(JournalIndexDocument.UserId, journalQuery.LoginId.ToString(), true, BoostLevel.Hight, false);
                        if (journalQuery.UserId != 0)
                            searchBuilder.WithField(JournalIndexDocument.UserId, journalQuery.UserId.ToString(), true, BoostLevel.Hight, false);
                        if (journalQuery.SiteCategoryId != 0)
                            searchBuilder.WithField(JournalIndexDocument.SiteCategoryId, journalQuery.SiteCategoryId.ToString(), true, BoostLevel.Hight, false);
                        break;**/
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(journalQuery.Keyword))
                {
                    switch (journalQuery.Range)
                    {
                        /*case JournalSearchRange.SUBJECT:
                            searchBuilder.WithPhrase(JournalIndexDocument.Subject, journalQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case JournalSearchRange.BODY:
                            searchBuilder.WithPhrase(JournalIndexDocument.Body, journalQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case JournalSearchRange.AUTHOR:
                            searchBuilder.WithPhrase(JournalIndexDocument.Author, journalQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case JournalSearchRange.TAG:
                            searchBuilder.WithPhrase(JournalIndexDocument.Tag, journalQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case JournalSearchRange.OWNERCATEGORYNAME:
                            searchBuilder.WithPhrase(JournalIndexDocument.OwnerCategoryName, journalQuery.Keyword, BoostLevel.Hight, false);
                            break;*/
                        default:
                            fieldNameAndBoosts.Add(JournalIndexDocument.journalname, BoostLevel.Hight);
                            fieldNameAndBoosts.Add(JournalIndexDocument.publisher, BoostLevel.Hight);
                            fieldNameAndBoosts.Add(JournalIndexDocument.subcat, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(JournalIndexDocument.jdescrip, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(JournalIndexDocument.abbr, BoostLevel.Medium);
                            //fieldNameAndBoosts.Add(JournalIndexDocument.OwnerCategoryName, BoostLevel.Medium);
                            //fieldNameAndBoosts.Add(JournalIndexDocument.Author, BoostLevel.Low);
                            searchBuilder.WithPhrases(fieldNameAndBoosts, journalQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                            break;
                    }
                }

                //某个站点分类
                /*if (journalQuery.SiteCategoryId != 0)
                {
                    searchBuilder.WithField(JournalIndexDocument.SiteCategoryId, journalQuery.SiteCategoryId.ToString(), true, BoostLevel.Hight, true);
                }

                if (!(UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == journalQuery.LoginId && journalQuery.AllId != 0))
                {
                    searchBuilder.NotWithField(JournalIndexDocument.PrivacyStatus, ((int)PrivacyStatus.Private).ToString());
                }*/
            }

            //筛选
            //全部、某人的日志
            /*if (journalQuery.AllId != 0)
            {
                if (journalQuery.LoginId != 0)
                {
                    searchBuilder.WithField(JournalIndexDocument.UserId, journalQuery.LoginId.ToString(), true, BoostLevel.Hight, true);
                }
                else if (journalQuery.UserId != 0)
                {
                    searchBuilder.WithField(JournalIndexDocument.UserId, journalQuery.UserId.ToString(), true, BoostLevel.Hight, true);
                }
            }
            */
            //过滤可以显示的日志
            /*switch (publiclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    searchBuilder.WithField(JournalIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), true, BoostLevel.Hight, true);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    searchBuilder.WithinRange(JournalIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), ((int)PubliclyAuditStatus.Success).ToString(), true);
                    break;
            }*/

            return searchBuilder;
        }
        #endregion
    }
}