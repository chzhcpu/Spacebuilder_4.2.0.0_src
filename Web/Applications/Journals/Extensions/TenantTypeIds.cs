//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// 定义的租户类型
    /// </summary>
    public static class TenantTypeIdsExtension
    {

        /// <summary>
        /// Journals
        /// </summary>
        /// <param name="TenantTypeIds">被扩展对象</param>
        public static string Journals(this TenantTypeIds TenantTypeIds)
        {
            return "900101";
        }


    }
}