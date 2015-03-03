//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Tunynet.Common;
using Spacebuilder.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 拥有者计数类型扩展类
    /// </summary>
    public static class UserExtensionByTopic
    {
        /// <summary>
        /// 创建的专题数
        /// </summary>
        public static long CreatedTopicCount(this User user)
        {
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            return ownerDataService.GetLong(user.UserId, OwnerDataKeys.Instance().CreatedTopicCount());
        }
        /// <summary>
        /// 加入的专题数
        /// </summary>
        public static long JoinedTopicCount(this User user)
        {
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            return ownerDataService.GetLong(user.UserId, OwnerDataKeys.Instance().JoinedTopicCount());
        }
    }
}