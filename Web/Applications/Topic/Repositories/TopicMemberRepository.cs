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
using Spacebuilder.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    ///专题成员申请Repository
    /// </summary>
    public class TopicMemberRepository : Repository<TopicMember>, ITopicMemberRepository
    {

        /// <summary>
        /// 删除专题成员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Delete(TopicMember entity)
        {
            int affectCount = base.Delete(entity);
            if (affectCount > 0)
            {
                List<Sql> sqls = new List<Sql>();
                sqls.Add(Sql.Builder.Append("update spb_Groups set MemberCount = MemberCount - 1 where GroupId = @0", entity.TopicId));
                sqls.Add(Sql.Builder.Append("delete from spb_GroupMemberApplies where UserId = @0 and GroupId = @1", entity.UserId, entity.TopicId));
                CreateDAO().Execute(sqls);

                //已修改
            }
            return affectCount;
        }

        /// <summary>
        /// 添加专题成员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object Insert(TopicMember entity)
        {
            Sql sql = Sql.Builder;


            sql.Select("count(*)")
                .From("spb_TopicMembers")
                .Where("UserId = @0 and TopicId = @1", entity.UserId, entity.TopicId);
            int result = CreateDAO().FirstOrDefault<int>(sql);
            if (result > 0)
            {
                return 0;
            }
            else
            {
                Sql updateSql = Sql.Builder;
                updateSql.Append("update spb_Groups set MemberCount = MemberCount + 1 where TopicId = @0", entity.TopicId);
                CreateDAO().Execute(updateSql);
                return base.Insert(entity);
            }
        }

        /// <summary>
        /// 获取单个专题成员
        /// </summary>
        /// <param name="groupId">专题ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public TopicMember GetMember(long groupId, long userId)
        {

            //已修改

            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TopicId", groupId) + "SingleMember" + userId;
            TopicMember groupMember = cacheService.Get<TopicMember>(cacheKey);

            if (groupMember == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("spb_GroupMembers")
                    .Where("GroupId = @0 and UserId = @1", groupId, userId);
                groupMember = CreateDAO().FirstOrDefault<TopicMember>(sql);
                cacheService.Add(cacheKey, groupMember, CachingExpirationType.SingleObject);
            }
            return groupMember;
        }

        /// <summary>
        /// 获取专题所有成员用户Id集合(用于推送动态）
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfTopic(long groupId)
        {
            Sql sql = Sql.Builder;
            sql.Select("UserId")
                .From("spb_GroupMembers")
                .Where("GroupId = @0", groupId);
            IEnumerable<long> userIds = CreateDAO().Fetch<long>(sql);
            return userIds;
        }

        /// <summary>
        /// 获取专题管理员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns>若没有找到，则返回空集合</returns>
        public IEnumerable<long> GetTopicManagers(long groupId)
        {
            string cacheKey = "GroupManagers" + groupId + "-" + RealTimeCacheHelper.GetAreaVersion("GroupId", groupId);
            List<long> managerIds = cacheService.Get<List<long>>(cacheKey);
            if (managerIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("UserId")
                    .From("spb_GroupMembers")
                    .Where("GroupId = @0 and IsManager = 1", groupId);
                managerIds = CreateDAO().Fetch<long>(sql);

                //已修改
                cacheService.Add(cacheKey, managerIds, CachingExpirationType.UsualObjectCollection);
            }
            return managerIds;
        }

        /// <summary>
        /// 获取专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="hasManager">是否包含管理员</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>专题成员分页数据</returns>
        public PagingDataSet<TopicMember> GetTopicMembers(long groupId, bool hasManager, SortBy_TopicMember sortBy, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "GroupId", groupId));
                    cacheKey.AppendFormat("PagingGroupMembers::GroupId-{0}:hasManager-{1}", groupId, hasManager);
                    return cacheKey.ToString();
                },
                () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("*")
                        .From("spb_GroupMembers")
                        .Where("GroupId = @0", groupId);
                    if (!hasManager)
                        sql.Where("IsManager = 0");
                    sql.OrderBy("IsManager desc");
                    switch (sortBy)
                    {
                        case SortBy_TopicMember.DateCreated_Asc:
                            sql.OrderBy("JoinDate asc");
                            break;
                        case SortBy_TopicMember.DateCreated_Desc:
                            sql.OrderBy("JoinDate desc");
                            break;
                        default:
                            sql.OrderBy("JoinDate asc");
                            break;
                    }
                    return sql;
                });
        }

        /// <summary>
        /// 获取我关注的用户中同时加入某个专题的专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        public IEnumerable<TopicMember> GetTopicMembersAlsoIsMyFollowedUser(long groupId, long userId)
        {
            string cacheKey = GetCacheKey_TopicMembersAlsoIsMyFollowedUser(groupId, userId);
            IEnumerable<long> groupMemberIds = cacheService.Get<IList<long>>(cacheKey);

            if (groupMemberIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("spb_GroupMembers.Id")
                   .From("spb_GroupMembers")
                   .InnerJoin("tn_Follows")
                   .On("tn_Follows.FollowedUserId = spb_GroupMembers.UserId")
                   .Where("spb_GroupMembers.GroupId = @0", groupId)
                   .Where("tn_Follows.UserId = @0", userId);
                groupMemberIds = CreateDAO().FetchTop<long>(PrimaryMaxRecords, sql);
                groupMemberIds = groupMemberIds != null && groupMemberIds.Count() > 0 ? groupMemberIds.Distinct() : groupMemberIds;
                cacheService.Add(cacheKey, groupMemberIds, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<long>(groupMemberIds);
        }
        /// <summary>
        /// 在线专题成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<TopicMember> GetOnlineTopicMembers(long groupId)
        {
            string cacheKey = GetCacheKey_OnlineTopicMembers(groupId);
            IEnumerable<long> groupMemberIds = cacheService.Get<IList<long>>(cacheKey);
            if (groupMemberIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("spb_GroupMembers.Id")
                    .From("spb_GroupMembers")
                    .InnerJoin("tn_OnlineUsers")
                    .On("tn_OnlineUsers.UserId=spb_GroupMembers.UserId")
                    .Where("spb_GroupMembers.GroupId=@0", groupId);
                groupMemberIds = CreateDAO().FetchTop<long>(PrimaryMaxRecords, sql);
                cacheService.Add(cacheKey, groupMemberIds, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<long>(groupMemberIds);
        }

        private string GetCacheKey_TopicMembersAlsoIsMyFollowedUser(long groupId, long userId)
        {
            return string.Format("GroupMembersAlsoIsMyFollowedUser:groupId-{0}:userId-{1}", groupId, userId);
        }

        private string GetCacheKey_OnlineTopicMembers(long groupId)
        {
            return string.Format("OnlineTopicMembers:groupId-{0}", groupId);
        }



        /// <summary>
        /// 获取专题下的所有成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<TopicMember> GetAllMembersOfTopic(long groupId)
        {
            var sql = Sql.Builder;
            sql.Select("*")
            .From("spb_GroupMembers")
            .Where("GroupId=@0", groupId);
            IEnumerable<TopicMember> members = CreateDAO().Fetch<TopicMember>(sql);
            return members;
        }
    }
}