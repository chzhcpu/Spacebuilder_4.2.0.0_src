//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class PointItemKeysExtension
    {
        /// <summary>
        /// 创建专题
        /// </summary>
        public static string Topic_CreateTopic(this PointItemKeys pointItemKeys)
        {
            return "Topic_CreateTopic";
        }

        /// <summary>
        /// 删除专题
        /// </summary>
        public static string Topic_DeleteTopic(this PointItemKeys pointItemKeys)
        {
            return "Topic_DeleteTopic";
        }
       
    }
}
