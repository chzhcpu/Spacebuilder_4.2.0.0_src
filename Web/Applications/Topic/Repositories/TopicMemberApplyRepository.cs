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
    /// <summary>
    ///专题成员申请Repository
    /// </summary>
    public class TopicMemberApplyRepository : Repository<TopicMemberApply>, ITopicMemberApplyRepository
    {

        /// <summary>
        /// 获取用户申请状态为待处理的专题ID集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public IEnumerable<long> GetPendingApplyTopicIdsOfUser(long userId)
        {
            
            //以下语句可以改为：RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion,"UserId",userId)+"PendingApplyTopicIdsOfUser"
            //已修改，前边的也要改吗
            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId) + "PendingApplyTopicIdsOfUser";
            List<long> topicIds = cacheService.Get<List<long>>(cacheKey);
            if (topicIds == null)
            {
                
                //已修改
                Sql sql = Sql.Builder;
                sql.Select("TopicId")
                    .From("spt_TopicMemberApplies")
                    .Where("UserId = @0", userId)
                    .Where("ApplyStatus=@0", TopicMemberApplyStatus.Pending);
                topicIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey, topicIds, CachingExpirationType.UsualObjectCollection);
            }
            return topicIds;
        }

        
        //已加
        /// <summary>
        /// 获取专题的加入申请列表
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <param name="applyStatus">申请状态</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>加入申请分页数据</returns>
        public PagingDataSet<TopicMemberApply> GetTopicMemberApplies(long topicId, TopicMemberApplyStatus? applyStatus, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
            () =>
            {
                StringBuilder cacheKey = new StringBuilder();
                cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TopicId", topicId));
                cacheKey.Append("TopicMemberApplies");
                
                //ok
                
                
                
                return cacheKey.ToString();
            },
            () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("spt_TopicMemberApplies")
                    .Where("TopicId = @0", topicId);
                if (applyStatus.HasValue)
                {
                    sql.Where("ApplyStatus = @0", applyStatus.Value);
                }
                sql.OrderBy("ApplyStatus asc").OrderBy("ApplyDate desc");
                return sql;
            });
        }

        /// <summary>
        /// 获取成员请求书
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <returns></returns>
        public int GetMemberApplyCount(long topicId)
        {
            int version = RealTimeCacheHelper.GetAreaVersion("TopicId", topicId);
            string cacheKey = string.Format("MemberApplyCount:{0}::TopicId:{1}", version, topicId);

            int? count = cacheService.Get(cacheKey) as int?;

            if (count == null)
            {
                var sql = Sql.Builder;

                sql.Select("Count(Id)")
                   .From("spt_TopicMemberApplies")
                   .Where("TopicId = @0", topicId)
                   .Where("ApplyStatus=@0", TopicMemberApplyStatus.Pending);
                count = CreateDAO().ExecuteScalar<int>(sql);

                cacheService.Add(cacheKey, count, CachingExpirationType.UsualSingleObject);
            }

            return count == null ? 0 : count.Value;
        }
    }
}