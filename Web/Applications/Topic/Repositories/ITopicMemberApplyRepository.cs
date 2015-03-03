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
    /// 专题成员申请仓储接口
    /// </summary>
    public interface ITopicMemberApplyRepository : IRepository<TopicMemberApply>
    {
        /// <summary>
        /// 获取用户申请状态为待处理的专题ID集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IEnumerable<long> GetPendingApplyTopicIdsOfUser(long userId);

        /// <summary>
        /// 获取专题的加入申请列表
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>加入申请分页数据</returns>
        PagingDataSet<TopicMemberApply> GetTopicMemberApplies(long groupId, TopicMemberApplyStatus? applyStatus, int pageSize, int pageIndex);

        /// <summary>
        /// 获取成员请求书
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns></returns>
        int GetMemberApplyCount(long groupId);

    }
}