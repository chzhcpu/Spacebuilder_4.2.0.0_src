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

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题搜索器
    /// </summary>
    public class TopicSearcher : ISearcher
    {
        private TopicService topicService = new TopicService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Topic());
        private CategoryService categoryService = new CategoryService();
        private AuditService auditService = new AuditService();
        private ISearchEngine searchEngine;
        private PubliclyAuditStatus publiclyAuditStatus;
        public static string CODE = "TopicSearcher";
        public static string WATERMARK = "搜索专题";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public TopicSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
            publiclyAuditStatus = auditService.GetPubliclyAuditStatus(TopicConfig.Instance().ApplicationId);
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
            return SiteUrls.Instance().TopicGlobalSearch() + "?keyword=" + keyword;
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
            return SiteUrls.Instance().TopicQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().TopicPageSearch(keyword);
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
                //分页获取专题列表
                PagingDataSet<TopicEntity> groups = topicService.GetsForAdmin(null, null, null, null, null, null, null, null, pageSize, pageIndex);
                totalRecords = groups.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //重建索引
                List<TopicEntity> groupList = groups.ToList<TopicEntity>();

                IEnumerable<Document> docs = TopicIndexDocument.Convert(groupList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="TopicEntity">待添加的专题</param>
        public void Insert(TopicEntity group)
        {
            Insert(new TopicEntity[] { group });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="TopicEntitys">待添加的专题</param>
        public void Insert(IEnumerable<TopicEntity> groups)
        {
            IEnumerable<Document> docs = TopicIndexDocument.Convert(groups);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="TopicEntityId">待删除的专题Id</param>
        public void Delete(long groupId)
        {
            searchEngine.Delete(groupId.ToString(), TopicIndexDocument.TopicId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="TopicEntityIds">待删除的专题Id列表</param>
        public void Delete(IEnumerable<long> groupIds)
        {
            foreach (var groupId in groupIds)
            {
                searchEngine.Delete(groupId.ToString(), TopicIndexDocument.TopicId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="TopicEntity">待更新的专题</param>
        public void Update(TopicEntity group)
        {
            Document doc = TopicIndexDocument.Convert(group);
            searchEngine.Update(doc, group.TopicId.ToString(), TopicIndexDocument.TopicId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="TopicEntitys">待更新的专题集合</param>
        public void Update(IEnumerable<TopicEntity> groups)
        {
            IEnumerable<Document> docs = TopicIndexDocument.Convert(groups);
            IEnumerable<string> groupIds = groups.Select(n => n.TopicId.ToString());
            searchEngine.Update(docs, groupIds, TopicIndexDocument.TopicId);
        }

        #endregion

        #region 搜索

        
        
        //fixed by wanf
        /// <summary>
        /// 专题分页搜索
        /// </summary>
        /// <param name="groupQuery">搜索条件</param>
        /// <param name="interestTopic">是否是查询可能感兴趣的专题</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<TopicEntity> Search(TopicFullTextQuery groupQuery, bool interestTopic = false)
        {
            if (!interestTopic)
            {
                if (string.IsNullOrWhiteSpace(groupQuery.Keyword) && !groupQuery.KeywordIsNull)
                {
                    return new PagingDataSet<TopicEntity>(new List<TopicEntity>());
                }
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(groupQuery, interestTopic);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, groupQuery.PageIndex, groupQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的专题ID
            List<long> groupIds = new List<long>();
            //获取索引中专题的标签
            Dictionary<long, IEnumerable<string>> groupTags = new Dictionary<long, IEnumerable<string>>();
            //获取索引中专题的分类名
            Dictionary<long, string> categoryNames = new Dictionary<long, string>();

            foreach (Document doc in docs)
            {
                long groupId = long.Parse(doc.Get(TopicIndexDocument.TopicId));
                groupIds.Add(groupId);
                groupTags[groupId]=doc.GetValues(TopicIndexDocument.Tag).ToList<string>();
                categoryNames[groupId]=doc.Get(TopicIndexDocument.CategoryName);
            }

            //根据专题ID列表批量查询专题实例
            IEnumerable<TopicEntity> groupList = topicService.GetTopicEntitiesByIds(groupIds);

            foreach (var group in groupList)
            {
                if (groupTags.ContainsKey(group.TopicId))
                {
                    group.TagNames = groupTags[group.TopicId];
                }
                if (categoryNames.ContainsKey(group.TopicId))
                {
                    group.CategoryName = categoryNames[group.TopicId];
                }
            }

            //组装分页对象
            PagingDataSet<TopicEntity> TopicEntitys = new PagingDataSet<TopicEntity>(groupList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return TopicEntitys;
        }

        /// <summary>
        /// 获取匹配的前几条热门专题
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="topNumber">前N条</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public IEnumerable<string> AutoCompleteSearch(string keyword, int topNumber)
        {
            IEnumerable<TopicEntity> hotTopics = topicService.GetMatchTops(topNumber, keyword, null, null, SortBy_Topic.GrowthValue_Desc);
            if (hotTopics.Count() > topNumber)
            {
                hotTopics.Take(topNumber);
            }
            return hotTopics.Select(n => n.TopicName);
        }

        /// <summary>
        /// 根据帖吧搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="Query">搜索条件</param>
        /// <param name="interestTopic">是否是查询可能感兴趣的专题</param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(TopicFullTextQuery groupQuery, bool interestTopic = false)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();
            //关键字为空就是在搜地区时关键字为空
            if (groupQuery.KeywordIsNull)
            {
                if (!string.IsNullOrEmpty(groupQuery.NowAreaCode))
                    searchBuilder.WithField(TopicIndexDocument.AreaCode, groupQuery.NowAreaCode.TrimEnd('0'), false, BoostLevel.Hight, false);
                else
                    searchBuilder.WithFields(TopicIndexDocument.AreaCode, new string[] { "1", "2", "3" }, false, BoostLevel.Hight, false);
            }

            if (!string.IsNullOrEmpty(groupQuery.Keyword))
            {
                //范围
                switch (groupQuery.Range)
                {
                    case TopicSearchRange.TOPICNAME:
                        searchBuilder.WithPhrase(TopicIndexDocument.TopicName, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case TopicSearchRange.DESCRIPTION:
                        searchBuilder.WithPhrase(TopicIndexDocument.Description, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case TopicSearchRange.TAG:
                        searchBuilder.WithPhrase(TopicIndexDocument.Tag, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case TopicSearchRange.CATEGORYNAME:
                        searchBuilder.WithPhrase(TopicIndexDocument.CategoryName, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    default:
                            fieldNameAndBoosts.Add(TopicIndexDocument.TopicName, BoostLevel.Hight);
                            fieldNameAndBoosts.Add(TopicIndexDocument.Description, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(TopicIndexDocument.Tag, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(TopicIndexDocument.CategoryName, BoostLevel.Medium);
                            searchBuilder.WithPhrases(fieldNameAndBoosts, groupQuery.Keyword, BooleanClause.Occur.SHOULD, false);   
                        break;
                }
            }

            //根据标签搜索可能感兴趣的专题
            if (interestTopic)
            {
                searchBuilder.WithPhrases(TopicIndexDocument.Tag, groupQuery.Tags, BoostLevel.Hight, false);
                searchBuilder.NotWithFields(TopicIndexDocument.TopicId, groupQuery.TopicIds);
            }

            //筛选
            //某地区
            if (!string.IsNullOrEmpty(groupQuery.NowAreaCode))
            {
                searchBuilder.WithField(TopicIndexDocument.AreaCode, groupQuery.NowAreaCode.TrimEnd('0'), false, BoostLevel.Hight, true);
            }

            //某分类
            if (groupQuery.CategoryId != 0)
            {
                
                //fixed by wanf:发现专题已经不再用全文检索了

                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> categories = categoryService.GetDescendants(groupQuery.CategoryId);
                List<string> categoryIds = new List<string> { groupQuery.CategoryId.ToString() };
                if (categories != null && categories.Count() > 0)
                {
                    categoryIds.AddRange(categories.Select(n => n.CategoryId.ToString()));
                }

                searchBuilder.WithFields(TopicIndexDocument.CategoryId, categoryIds, true, BoostLevel.Hight, true);
            }

            //公开的专题
            searchBuilder.WithField(TopicIndexDocument.IsPublic, "1", true, BoostLevel.Hight, true);

            //过滤可以显示的专题
            switch (publiclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    searchBuilder.WithField(TopicIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), true, BoostLevel.Hight, true);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    searchBuilder.WithinRange(TopicIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), ((int)PubliclyAuditStatus.Success).ToString(), true);
                    break;
            }

            if (groupQuery.sortBy.HasValue)
            {
                switch (groupQuery.sortBy.Value)
                {
                    case SortBy_Topic.DateCreated_Desc:
                        searchBuilder.SortByString(TopicIndexDocument.DateCreated, true);
                        break;
                    case SortBy_Topic.MemberCount_Desc:
                        searchBuilder.SortByString(TopicIndexDocument.MemberCount, true);
                        break;
                    case SortBy_Topic.GrowthValue_Desc:
                        searchBuilder.SortByString(TopicIndexDocument.GrowthValue, true);
                        break;
                }
            }
            else
            {
                //时间倒序排序
                searchBuilder.SortByString(TopicIndexDocument.DateCreated, true);
            }

            return searchBuilder;
        }
        #endregion
    }
}