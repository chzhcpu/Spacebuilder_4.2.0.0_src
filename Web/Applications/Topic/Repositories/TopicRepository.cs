//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common;
using System.Linq;
using Tunynet.Utilities;

namespace SpecialTopic.Topic
{

    //已过滤掉加入方式为仅邀请加入的专题
    /// <summary>
    ///专题Repository
    /// </summary>
    public class TopicRepository : Repository<TopicEntity>, ITopicRepository
    {

        public TopicRepository() { }

        public TopicRepository(PubliclyAuditStatus publiclyAuditStatus)
        {
            this.publiclyAuditStatus = publiclyAuditStatus;
        }

        #region 维护专题
        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">皮肤标识</param>
        public void ChangeThemeAppearance(long groupId, bool isUseCustomStyle, string themeAppearance)
        {
            TopicEntity group = Get(groupId);
            if (group == null)
                return;

            var sql_Update = PetaPoco.Sql.Builder;
            sql_Update.Append("update spt_Topics set ThemeAppearance = @0,IsUseCustomStyle = @1 where TopicId = @2", themeAppearance ?? string.Empty, isUseCustomStyle, groupId);
            int affectedCount = CreateDAO().Execute(sql_Update);
            if (affectedCount > 0)
            {
                group.ThemeAppearance = themeAppearance ?? string.Empty;
                group.IsUseCustomStyle = isUseCustomStyle;
                base.OnUpdated(group);
            }
        }

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="newOwnerUserId">新群主UserId</param>
        public void ChangeTopicOwner(long groupId, long newOwnerUserId)
        {
            Sql sql = Sql.Builder;
            sql.Append("update spt_Topics set UserId = @0 where TopicId = @1", newOwnerUserId, groupId);
            CreateDAO().Execute(sql);
            TopicEntity group = Get(groupId);
            if (group != null)
            {
                group.UserId = newOwnerUserId;
                base.OnUpdated(group);
            }

            //已修改



        }

        /// <summary>
        /// 每天定时计算各个专题的成长值
        /// </summary>
        public void CalculateGrowthValues()
        {
            Sql sql = Sql.Builder;
            sql.Append(@"update spt_Topics set GrowthValue=
            (select COUNT(*) from spb_BarThreads where SectionId=TopicId and TenantTypeId=@0) * 2 + 
            (select COUNT(*) from spb_BarPosts where SectionId = TopicId and TenantTypeId=@0) + MemberCount *5 +
            (select COUNT(*) from spb_Microblogs where OwnerId = TopicId and TenantTypeId=@0)", TenantTypeIds.Instance().Topic());
            CreateDAO().Execute(sql);
        }


        /// <summary>
        /// 删除专题实体
        /// </summary>
        /// <param name="entity">专题实体</param>
        /// <returns></returns>
        public override int Delete(TopicEntity entity)
        {
            int affectCount = base.Delete(entity);
            if (affectCount > 0)
            {
                var sql = Sql.Builder.Append("delete from spt_TopicMemberApplies").Where("TopicId=@0", entity.TopicId);
                CreateDAO().Execute(sql);
            }
            return affectCount;
        }

        /// <summary>
        /// 插入专题实体
        /// </summary>
        /// <param name="entity">专题实体</param>
        /// <returns></returns>
        public override object Insert(TopicEntity entity)
        {
            entity.TopicId = IdGenerator.Next();
            return base.Insert(entity);
        }

        #endregion

        #region 获取专题

        /// <summary>
        /// 根据专题Key获取专题Id
        /// </summary>
        /// <param name="groupKey">专题Key</param>
        /// <returns>专题Id</returns>
        public long GetTopicIdByTopicKey(string groupKey)
        {
            var sql_Select = Sql.Builder.Select("TopicId").From("spt_Topics").Where("TopicKey = @0", groupKey);
            return CreateDAO().FirstOrDefault<long>(sql_Select);
        }




        /// <summary>
        /// 获取前N个排行专题
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetTops(int topNumber, string areaCode, long? categoryId, SortBy_Topic sortBy)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("TopTopics::areaCode-{0}:categoryId-{1}:sortBy-{2}", areaCode, categoryId, sortBy);
                    return cacheKey.ToString();
                },
                () =>
                {
                    return Getsqls(areaCode, categoryId, null, sortBy);
                });
        }

        /// <summary>
        /// 获取匹配的前N个排行专题
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="keyword">关键字</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetMatchTops(int topNumber, string keyword, string areaCode, long? categoryId, SortBy_Topic sortBy)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("TopTopics::areaCode-{0}:categoryId-{1}:sortBy-{2}:keyword-{3}", areaCode, categoryId, sortBy, keyword);
                    return cacheKey.ToString();
                },
                () =>
                {
                    return Getsqls(areaCode, categoryId, keyword, sortBy);
                });
        }

        /// <summary>
        /// 根据标签名获取专题分页集合
        /// </summary>
        /// <param name="tagName">标签名</param></param>
        /// <param name="sortBy">排序依据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页列表</returns>
        public PagingDataSet<TopicEntity> GetsByTag(string tagName, SortBy_Topic sortBy, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("TopicsByTag::TagName-{0}:SortBy-{1}", tagName, sortBy);
                    return cacheKey.ToString();
                },
                () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("spt_Topics.*").From("spt_Topics");

                    sql.InnerJoin("tn_ItemsInTags").On("TopicId = tn_ItemsInTags.ItemId")
                    .Where("tn_ItemsInTags.TagName = @0 and tn_ItemsInTags.TenantTypeId = @1", tagName, TenantTypeIds.Instance().Topic())
                    .Where("spt_Topics.IsPublic = 1");

                    switch (sortBy)
                    {
                        case SortBy_Topic.DateCreated_Desc:
                            sql.OrderBy("DateCreated desc");
                            break;
                        case SortBy_Topic.GrowthValue_Desc:
                            sql.OrderBy("GrowthValue desc");
                            break;
                        case SortBy_Topic.MemberCount_Desc:
                            sql.OrderBy("MemberCount desc");
                            break;
                        default:
                            sql.OrderBy("GrowthValue desc");
                            break;
                    }
                    return sql;
                });
        }

        //已修改
        /// <summary>
        /// 获取用户创建的专题列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="ignoreAudit">是否忽略审核状态（作者或管理员查看时忽略审核状态）</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetMyCreatedTopics(long userId, bool ignoreAudit)
        {



            string cacheKey = "TopicsOfUser" + "-" + RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId) + "-ignoreAudit" + ignoreAudit;

            List<long> groupIds = cacheService.Get<List<long>>(cacheKey);
            if (groupIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("TopicId")
                   .From("spt_Topics")
                   .Where("UserId=@0", userId);
                if (!ignoreAudit)
                {


                    switch (this.PubliclyAuditStatus)
                    {
                        case PubliclyAuditStatus.Again:
                        case PubliclyAuditStatus.Fail:
                        case PubliclyAuditStatus.Pending:
                        case PubliclyAuditStatus.Success:
                            sql.Where("AuditStatus=@0", this.PubliclyAuditStatus);
                            break;
                        case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                        case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                            sql.Where("AuditStatus>@0", this.PubliclyAuditStatus);
                            break;
                        default:
                            break;
                    }
                }
                sql.OrderBy("TopicId desc");


                groupIds = CreateDAO().Fetch<long>(sql);
                if (groupIds != null)
                {
                    cacheService.Add(cacheKey, groupIds, CachingExpirationType.UsualObjectCollection);
                }
            }
            return PopulateEntitiesByEntityIds<long>(groupIds);
        }

        /// <summary>
        /// 分页获取排行数据
        /// </summary>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<TopicEntity> Gets(string areaCode, long? categoryId, SortBy_Topic sortBy, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.AppendFormat("PagingTopicRanks::areaCode-{0}:categoryId-{1}:sortBy-{2}", areaCode, categoryId, sortBy);
                    return cacheKey.ToString();
                },
                () =>
                {
                    return Getsqls(areaCode, categoryId, null, sortBy);
                });
        }

        /// <summary>
        /// 专题成员也加入的专题
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> TopicMemberAlsoJoinedTopics(long groupId, int topNumber)
        {
            string cacheKey = string.Format("TopicMemberAlsoJoinedTopics::groupId-{0}", groupId);
            var ids = cacheService.Get<List<object>>(cacheKey);
            if (ids == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("distinct spt_Topics.*")
                   .From("spt_Topics")
                   .InnerJoin("spt_TopicMembers M")
                   .On("M.TopicId = spt_Topics.TopicId")
                   .Where("M.UserId in (select UserId from spt_TopicMembers where TopicId=@0) and spt_Topics.TopicId!=@0", groupId)
                   .Where("spt_Topics.IsPublic = 1");
                sql.OrderBy("spt_Topics.GrowthValue desc");
                ids = CreateDAO().Fetch<dynamic>(sql).Select(n => n.TopicId).ToList();
                cacheService.Add(cacheKey, ids, CachingExpirationType.UsualObjectCollection);
            }
            return PopulateEntitiesByEntityIds(ids.Take(topNumber));
        }

        /// <summary>
        /// 获取我关注的用户加入的专题
        /// </summary>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> FollowedUserAlsoJoinedTopics(long userId, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection,
        () =>
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.AppendFormat("FollowedUserAlsoJoinedTopics::userId-{0}", userId);
            return cacheKey.ToString();
        },
        () =>
        {
            Sql sql = Sql.Builder;
            sql.Select("distinct spt_Topics.*")
               .From("spt_Topics")
               .InnerJoin("spt_TopicMembers M")
               .On("M.TopicId = spt_Topics.TopicId")
               .InnerJoin("tn_Follows F")
               .On("F.FollowedUserId = M.UserId")
               .Where("F.UserId = @0 or spt_Topics.UserId = F.FollowedUserId", userId)
               .Where("spt_Topics.IsPublic = 1");
            sql.OrderBy("spt_Topics.GrowthValue desc");
            return sql;
        });
        }

        /// <summary>
        /// 获取用户加入的专题列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<TopicEntity> GetMyJoinedTopics(long userId, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
            () =>
            {
                StringBuilder cacheKey = new StringBuilder();
                cacheKey.Append(EntityData.ForType(typeof(TopicMember)).RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                cacheKey.AppendFormat("MyJoinedTopics::UserId-{0}", userId);
                return cacheKey.ToString();
            },
            () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("distinct TopicId")
                   .From("spt_TopicMembers")
                   .Where("UserId = @0", userId);


                //sql.OrderBy("JoinDate desc");
                return sql;
            });
        }

        /// <summary>
        /// 分页获取专题后台管理列表
        /// </summary>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="keywords">名称关键字</param>
        /// <param name="ownerUserId">群主</param>
        /// <param name="minDateTime">创建时间下限值</param>
        /// <param name="maxDateTime">创建时间上限值</param>
        /// <param name="minMemberCount">成员数量下限值</param>
        /// <param name="maxMemberCount">成员数量上限值</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<TopicEntity> GetsForAdmin(AuditStatus? auditStatus = null, long? categoryId = null, string keywords = null, long? ownerUserId = null, DateTime? minDateTime = null, DateTime? maxDateTime = null, int? minMemberCount = null, int? maxMemberCount = null, int pageSize = 20, int pageIndex = 1)
        {
            Sql sql = Sql.Builder;
            sql.Select("*").From("spt_Topics");

            if (categoryId != null && categoryId.Value > 0)
            {
                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> categories = categoryService.GetDescendants(categoryId.Value);
                List<long> categoryIds = new List<long> { categoryId.Value };
                if (categories != null && categories.Count() > 0)
                    categoryIds.AddRange(categories.Select(n => n.CategoryId));
                sql.InnerJoin("tn_ItemsInCategories")
               .On("spt_Topics.TopicId = tn_ItemsInCategories.ItemId");
                sql.Where("tn_ItemsInCategories.CategoryId in(@categoryIds)", new { categoryIds = categoryIds });
            }

            if (auditStatus.HasValue)
            {

                //ok
                sql.Where("AuditStatus = @0", auditStatus);
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                sql.Where("spt_Topics.TopicName like @0", "%" + StringUtility.StripSQLInjection(keywords) + "%");
            }
            if (ownerUserId.HasValue)
            {
                sql.Where("spt_Topics.UserId = @0", ownerUserId);
            }
            if (minDateTime.HasValue)
            {
                sql.Where("DateCreated >= @0", minDateTime.Value.Date);
            }
            if (maxDateTime.HasValue)
            {

                sql.Where("DateCreated < @0", maxDateTime.Value.Date.AddDays(1));
            }

            if (minMemberCount.HasValue)
            {
                sql.Where("MemberCount >= @0", minMemberCount);
            }
            if (maxMemberCount.HasValue)
            {

                sql.Where("MemberCount <= @0", maxMemberCount);
            }

            //已修改
            sql.OrderBy("DateCreated desc");
            return GetPagingEntities(pageSize, pageIndex, sql);
        }




        /// <summary>
        /// Gets和GetTops的sql语句
        /// </summary>
        private Sql Getsqls(string areaCode, long? categoryId, string keyword, SortBy_Topic sortBy)
        {
            Sql sql = Sql.Builder;
            var whereSql = Sql.Builder;
            var orderSql = Sql.Builder;

            sql.Select("spt_Topics.*").From("spt_Topics");

            if (categoryId != null && categoryId.Value > 0)
            {
                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> categories = categoryService.GetDescendants(categoryId.Value);
                List<long> categoryIds = new List<long> { categoryId.Value };
                if (categories != null && categories.Count() > 0)
                    categoryIds.AddRange(categories.Select(n => n.CategoryId));
                sql.InnerJoin("tn_ItemsInCategories")
               .On("spt_Topics.TopicId = tn_ItemsInCategories.ItemId");
                whereSql.Where("tn_ItemsInCategories.CategoryId in(@categoryIds)", new { categoryIds = categoryIds });
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                if (areaCode.Equals("A1560000", StringComparison.CurrentCultureIgnoreCase))
                {

                    //已修改
                    whereSql.Where("AreaCode like '1%' or AreaCode like '2%' or AreaCode like '3%' or AreaCode like '4%' or AreaCode like '5%' or AreaCode like '6%' or AreaCode like '7%' or AreaCode like '8%' or AreaCode like '9%' ");
                }
                else
                {
                    areaCode = areaCode.TrimEnd('0');
                    if (areaCode.Length % 2 == 1)
                        areaCode = areaCode + "0";
                    whereSql.Where("AreaCode like @0 ", StringUtility.StripSQLInjection(areaCode) + "%");
                }
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereSql.Where("TopicName like @0", StringUtility.StripSQLInjection(keyword) + "%");
            }
            whereSql.Where("spt_Topics.IsPublic = 1");

            //已修改
            switch (this.PubliclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    whereSql.Where("AuditStatus=@0", this.PubliclyAuditStatus);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    whereSql.Where("AuditStatus>@0", this.PubliclyAuditStatus);
                    break;
                default:
                    break;
            }
            CountService countService = new CountService(TenantTypeIds.Instance().Topic());
            string countTableName = countService.GetTableName_Counts();
            switch (sortBy)
            {
                case SortBy_Topic.DateCreated_Desc:
                    orderSql.OrderBy("DateCreated desc");
                    break;
                case SortBy_Topic.GrowthValue_Desc:
                    orderSql.OrderBy("GrowthValue desc");
                    break;
                case SortBy_Topic.MemberCount_Desc:
                    orderSql.OrderBy("MemberCount desc");
                    break;
                case SortBy_Topic.HitTimes:
                    sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, CountTypes.Instance().HitTimes()))
                    .On("TopicId = c.ObjectId");
                    orderSql.OrderBy("c.StatisticsCount desc");
                    break;
                case SortBy_Topic.StageHitTimes:
                    StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Topic());
                    int stageCountDays = stageCountTypeManager.GetMaxDayCount(CountTypes.Instance().HitTimes());
                    string stageCountType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().HitTimes(), stageCountDays);
                    sql.LeftJoin(string.Format("(select * from {0} WHERE ({0}.CountType = '{1}')) c", countTableName, stageCountType))
                    .On("TopicId = c.ObjectId");
                    orderSql.OrderBy("c.StatisticsCount desc");
                    break;
                default:

                    orderSql.OrderBy("DateCreated desc");
                    break;
            }

            sql.Append(whereSql).Append(orderSql);
            return sql;
        }

        #endregion


        /// <summary>
        /// 删除用户记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">被删除用户</param>
        /// <param name="takeOver">接管用户</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        public void DeleteUser(long userId, Spacebuilder.Common.User takeOver, bool takeOverAll)
        {
            List<Sql> sqls = new List<Sql>();
            if (takeOver != null)
            {
                //更改群主
                sqls.Add(Sql.Builder.Append("update spt_Topics set UserId = @0 where UserId = @1", takeOver.UserId, userId));

                //获取用户Id为userId创建的专题
                Sql havedTopics = Sql.Builder;
                havedTopics.Select("TopicId")
                    .From("spt_Topics")
                    .Where("UserId = @0", userId);
                IEnumerable<long> groupIds = CreateDAO().Fetch<long>(havedTopics);


                //获取我加入用户Id为userId创建的专题的专题ID
                if (groupIds.Count() > 0)
                {
                    Sql joinedTopics = Sql.Builder;
                    joinedTopics.Select("TopicId")
                        .From("spt_TopicMembers")
                        .Where("UserId = @userId and TopicId in (@groupIds)", new { userId = takeOver.UserId }, new { groupIds = groupIds });
                    IEnumerable<long> joinedIds = CreateDAO().Fetch<long>(joinedTopics);


                    sqls.Add(Sql.Builder.Append("delete from spt_TopicMembers where UserId = (@userId) and TopicId in (@groupIds)", new { userId = takeOver.UserId }, new { groupIds = groupIds }));
                    sqls.Add(Sql.Builder.Append("delete from spt_TopicMemberApplies where UserId = (@userId) and TopicId in (@groupIds)", new { userId = takeOver.UserId }, new { groupIds = groupIds }));
                    if (joinedIds.Count() > 0)
                    {
                        sqls.Add(Sql.Builder.Append("update spt_Topics set MemberCount = MemberCount - 1 where TopicId in(@joinedIds)", new { joinedIds = joinedIds }));
                    }
                }
                //此选项尚未用到
                if (takeOverAll)
                { }
            }

            //获取用户ID为userId加入的专题
            Sql userJoinedTopics = Sql.Builder;
            userJoinedTopics.Select("TopicId")
                .From("spt_TopicMembers")
                .Where("UserId = @userId", new { userId = userId });
            IEnumerable<long> userJoinedIds = CreateDAO().Fetch<long>(userJoinedTopics);

            sqls.Add(Sql.Builder.Append("delete from spt_TopicMembers where UserId = @0", userId));
            sqls.Add(Sql.Builder.Append("delete from spt_TopicMemberApplies where UserId = @0", userId));
            if (userJoinedIds.Count() > 0)
            {
                sqls.Add(Sql.Builder.Append("update spt_Topics set MemberCount = MemberCount - 1 where TopicId in(@userJoinedIds)", new { userJoinedIds = userJoinedIds }));
            }

            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 专题应用可对外显示的审核状态
        /// </summary>
        private PubliclyAuditStatus? publiclyAuditStatus;
        /// <summary>
        /// 专题应用可对外显示的审核状态
        /// </summary>
        protected PubliclyAuditStatus PubliclyAuditStatus
        {
            get
            {
                if (publiclyAuditStatus == null)
                {
                    AuditService auditService = new AuditService();
                    publiclyAuditStatus = auditService.GetPubliclyAuditStatus(TopicConfig.Instance().ApplicationId);
                }
                return publiclyAuditStatus.Value;
            }
            set
            {
                this.publiclyAuditStatus = value;
            }
        }

        /// <summary>
        /// 根据审核状态获取专题数
        /// </summary>
        /// <returns></returns>
        public Dictionary<TopicManageableCountType, int> GetManageableCounts()
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();

            Dictionary<TopicManageableCountType, int> countType = new Dictionary<TopicManageableCountType, int>();

            var sql_selectIsActivated = PetaPoco.Sql.Builder;
            sql_selectIsActivated.Select("count(*)").From("spt_Topics");

            sql_selectIsActivated.Where("AuditStatus = @0", AuditStatus.Pending);

            countType[TopicManageableCountType.Pending] = dao.FirstOrDefault<int>(sql_selectIsActivated);

            var sql_selectIsAll = PetaPoco.Sql.Builder;
            sql_selectIsAll.Select("count(*)").From("spt_Topics");

            countType[TopicManageableCountType.IsAll] = dao.FirstOrDefault<int>(sql_selectIsAll);

            var sql_selectIsLast24 = PetaPoco.Sql.Builder;
            sql_selectIsLast24.Select("count(*)").From("spt_Topics");
            sql_selectIsLast24.Where("DateCreated >= @0 and  DateCreated < @1", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);

            countType[TopicManageableCountType.IsLast24] = dao.FirstOrDefault<int>(sql_selectIsLast24);

            dao.CloseSharedConnection();

            return countType;
        }

        /// <summary>
        /// 获取专题管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public Dictionary<string, long> GetManageableDatas(string tenantTypeId = null)
        {
            var dao = CreateDAO();
            Dictionary<string, long> manageableDatas = new Dictionary<string, long>();

            Sql sql = Sql.Builder;
            sql.Select("count(*)")
                .From("spt_Topics")
                .Where("AuditStatus=@0", AuditStatus.Pending);

            manageableDatas.Add(ApplicationStatisticDataKeys.Instance().TopicPendingCount(), dao.FirstOrDefault<long>(sql));
            sql = Sql.Builder;
            sql.Select("count(*)")
                .From("spt_Topics")
                .Where("AuditStatus=@0", AuditStatus.Again);

            manageableDatas.Add(ApplicationStatisticDataKeys.Instance().TopicAgainCount(), dao.FirstOrDefault<long>(sql));

            return manageableDatas;
        }

        /// <summary>
        /// 获取专题统计数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, long> GetStatisticDatas(string tenantTypeId = null)
        {
            var dao = CreateDAO();
            string cacheKey = "TopicStatisticData";
            Dictionary<string, long> statisticDatas = cacheService.Get<Dictionary<string, long>>(cacheKey);
            if (statisticDatas == null)
            {
                statisticDatas = new Dictionary<string, long>();
                Sql sql = Sql.Builder;
                sql.Select("count(*)")
                    .From("spt_Topics");
                statisticDatas.Add(ApplicationStatisticDataKeys.Instance().TotalCount(), dao.FirstOrDefault<long>(sql));

                sql = Sql.Builder;
                sql.Select("count(*)")
                    .From("spt_Topics")
                    .Where("DateCreated > @0", DateTime.UtcNow.AddDays(-1));

                statisticDatas.Add(ApplicationStatisticDataKeys.Instance().Last24HCount(), dao.FirstOrDefault<long>(sql));
                cacheService.Add(cacheKey, statisticDatas, CachingExpirationType.UsualSingleObject);
            }

            return statisticDatas;
        }
    }
}