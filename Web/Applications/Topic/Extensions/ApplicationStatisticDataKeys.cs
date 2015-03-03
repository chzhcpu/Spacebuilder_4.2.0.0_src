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

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 用户计数类型扩展类
    /// </summary>
    public static class ApplicationStatisticDataKeysExtension
    {
        /// <summary>
        /// 专题待审核数
        /// </summary>
        public static string TopicPendingCount(this ApplicationStatisticDataKeys applicationStatisticDataKeys)
        {
            return "TopicPendingCount";
        }

        /// <summary>
        /// 专题需再审核数
        /// </summary>
        public static string TopicAgainCount(this ApplicationStatisticDataKeys applicationStatisticDataKeys)
        {
            return "TopicAgainCount";
        }

    }
}