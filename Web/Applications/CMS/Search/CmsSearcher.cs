//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

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

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯搜索器
    /// </summary>
    public class CmsSearcher : ISearcher
    {
        private ContentItemService contentItemService = new ContentItemService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().ContentItem());
        private ContentFolderService contentFolderService = new ContentFolderService();
        private AuditService auditService = new AuditService();
        private ISearchEngine searchEngine;
        private PubliclyAuditStatus publiclyAuditStatus;
        private SearchedTermService searchedTermService = new SearchedTermService();
        public static string CODE = "CmsSearcher";
        public static string WATERMARK = "搜索资讯";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public CmsSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
            publiclyAuditStatus = auditService.GetPubliclyAuditStatus(CmsConfig.Instance().ApplicationId);
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
            return SiteUrls.Instance().CmsGlobalSearch() + "?keyword=" + keyword;
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
            return SiteUrls.Instance().CmsQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().CmsPageSearch(keyword);
        }

        #endregion

        #region 索引内容维护


        /// <summary>
        /// 重建索引
        /// </summary>  
        public void RebuildIndex()
        {
            //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            int pageSize = 1000;
            int pageIndex = 1;
            long totalRecords = 0;
            bool isBeginning = true;
            bool isEndding = false;
            do
            {
                //分页获取帖子列表
                PagingDataSet<ContentItem> contentItems = contentItemService.GetContentItemsForAdmin(null, null, null, pageSize: pageSize, pageIndex: pageIndex);
                totalRecords = contentItems.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //重建索引
                List<ContentItem> cmsThreadList = contentItems.ToList<ContentItem>();

                IEnumerable<Document> docs = CmsIndexDocument.Convert(cmsThreadList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="cmsThread">待添加的资讯</param>
        public void Insert(ContentItem cmsThread)
        {
            Insert(new ContentItem[] { cmsThread });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="contentItems">待添加的资讯</param>
        public void Insert(IEnumerable<ContentItem> contentItems)
        {
            IEnumerable<Document> docs = CmsIndexDocument.Convert(contentItems);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="cmsContentItemId">待删除的资讯Id</param>
        public void Delete(long cmsContentItemId)
        {
            searchEngine.Delete(cmsContentItemId.ToString(), CmsIndexDocument.ContentItemId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="cmsContentItemIds">待删除的资讯Id列表</param>
        public void Delete(IEnumerable<long> cmsContentItemIds)
        {
            foreach (var cmsContentItemId in cmsContentItemIds)
            {
                searchEngine.Delete(cmsContentItemId.ToString(), CmsIndexDocument.ContentItemId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="cmsThread">待更新的资讯</param>
        public void Update(ContentItem cmsThread)
        {
            Document doc = CmsIndexDocument.Convert(cmsThread);
            searchEngine.Update(doc, cmsThread.ContentItemId.ToString(), CmsIndexDocument.ContentItemId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="contentItems">待更新的资讯集合</param>
        public void Update(IEnumerable<ContentItem> contentItems)
        {
            IEnumerable<Document> docs = CmsIndexDocument.Convert(contentItems);
            IEnumerable<string> cmsContentItemIds = contentItems.Select(n => n.ContentItemId.ToString());
            searchEngine.Update(docs, cmsContentItemIds, CmsIndexDocument.ContentItemId);
        }

        #endregion

        #region 搜索
        /// <summary>
        /// 资讯分页搜索
        /// </summary>
        /// <param name="cmsQuery">搜索条件</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<ContentItem> Search(CmsFullTextQuery cmsQuery)
        {
            if (cmsQuery.ContentFolderId == 0)
            {
                if (string.IsNullOrWhiteSpace(cmsQuery.Keyword))
                {
                    return new PagingDataSet<ContentItem>(new List<ContentItem>());
                }
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(cmsQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, cmsQuery.PageIndex, cmsQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的资讯ID
            List<long> cmsContentItemIds = new List<long>();
            //获取索引中资讯的标签
            Dictionary<long, IEnumerable<string>> cmsTags = new Dictionary<long, IEnumerable<string>>();

            foreach (Document doc in docs)
            {
                long cmsContentItemId = long.Parse(doc.Get(CmsIndexDocument.ContentItemId));
                cmsContentItemIds.Add(cmsContentItemId);
                cmsTags[cmsContentItemId] = doc.GetValues(CmsIndexDocument.Tag).ToList<string>();
            }

            //根据资讯ID列表批量查询资讯实例
            IEnumerable<ContentItem> contentItemList = contentItemService.GetContentItems(cmsContentItemIds);

            foreach (var contentItem in contentItemList)
            {
                if (cmsTags.ContainsKey(contentItem.ContentItemId))
                {
                    contentItem.TagNames = cmsTags[contentItem.ContentItemId];
                }
            }

            //组装分页对象
            PagingDataSet<ContentItem> contentItems = new PagingDataSet<ContentItem>(contentItemList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return contentItems;
        }

        /// <summary>
        /// 获取匹配的前几条资讯热词
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
        private LuceneSearchBuilder BuildLuceneSearchBuilder(CmsFullTextQuery cmsQuery)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            //范围
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();

            if (!string.IsNullOrEmpty(cmsQuery.Keyword))
            {
                switch (cmsQuery.Range)
                {
                    case CmsSearchRange.TITLE:
                        searchBuilder.WithPhrase(CmsIndexDocument.Title, cmsQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case CmsSearchRange.BODY:
                        searchBuilder.WithPhrase(CmsIndexDocument.Body, cmsQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case CmsSearchRange.AUTHOR:
                        searchBuilder.WithPhrase(CmsIndexDocument.Author, cmsQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case CmsSearchRange.TAG:
                        searchBuilder.WithPhrase(CmsIndexDocument.Tag, cmsQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    default:
                        fieldNameAndBoosts.Add(CmsIndexDocument.Title, BoostLevel.Hight);
                        fieldNameAndBoosts.Add(CmsIndexDocument.Summary, BoostLevel.Hight);
                        fieldNameAndBoosts.Add(CmsIndexDocument.Tag, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(CmsIndexDocument.Body, BoostLevel.Medium);
                        fieldNameAndBoosts.Add(CmsIndexDocument.Author, BoostLevel.Low);
                        searchBuilder.WithPhrases(fieldNameAndBoosts, cmsQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                        break;
                }
            }

            //某个栏目
            if (cmsQuery.ContentFolderId > 0)
            {
                searchBuilder.WithField(CmsIndexDocument.ContentFolderId, cmsQuery.ContentFolderId.ToString(), true, BoostLevel.Hight, true);
            }

            //过滤可以显示的资讯
            switch (publiclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    searchBuilder.WithField(CmsIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), true, BoostLevel.Hight, true);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    searchBuilder.WithinRange(CmsIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), ((int)PubliclyAuditStatus.Success).ToString(), true);
                    break;
            }

            return searchBuilder;
        }
        #endregion
    }
}