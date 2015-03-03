//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Events;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class EventOperationTypeExtension
    {
        /// <summary>
        /// 设置管理员
        /// </summary>
        public static string SetTopicManager(this EventOperationType EventOperationType)
        {
            return "SetTopicManager";
        }

        /// <summary>
        /// 取消管理员
        /// </summary>
        public static string CancelTopicManager(this EventOperationType EventOperationType)
        {
            return "CancelTopicManager";
        }

    }
}
