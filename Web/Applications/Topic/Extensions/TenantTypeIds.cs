//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题定义的租户类型
    /// </summary>
    public static class TenantTypeIdsExtension
    {
        /// <summary>
        /// 专题应用
        /// </summary>
        /// <param name="TenantTypeIds">被扩展对象</param>
        public static string Topic(this TenantTypeIds TenantTypeIds)
        {
            return "900200";
        }
    }
}