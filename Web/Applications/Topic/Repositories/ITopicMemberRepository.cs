//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题成员仓储接口
    /// </summary>
    public interface ITopicMemberRepository : IRepository<TopicMember>
    {
        /// <summary>
        /// 获取单个专题成员
        /// </summary>
        /// <param name="groupId">专题ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        TopicMember GetMember(long groupId, long userId);

        /// <summary>
        /// 获取专题所有成员用户Id集合(用于推送动态）
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns></returns>
        IEnumerable<long> GetUserIdsOfTopic(long groupId);

        /// <summary>
        /// 获取专题管理员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns>若没有找到，则返回空集合</returns>
        IEnumerable<long> GetTopicManagers(long groupId);

        /// <summary>
        /// 获取专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="hasManager">是否包含管理员</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>专题成员分页数据</returns>
        PagingDataSet<TopicMember> GetTopicMembers(long groupId, bool hasManager, SortBy_TopicMember sortBy, int pageSize, int pageIndex);


        /// <summary>
        /// 获取我关注的用户中同时加入某个专题的专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        IEnumerable<TopicMember> GetTopicMembersAlsoIsMyFollowedUser(long groupId, long userId);
        /// <summary>
        /// 在线专题成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IEnumerable<TopicMember> GetOnlineTopicMembers(long groupId);

        /// <summary>
        /// 获取专题下的所有成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IEnumerable<TopicMember> GetAllMembersOfTopic(long groupId);
    }
}