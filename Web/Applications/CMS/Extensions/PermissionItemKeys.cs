//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 权限项标识扩展类
    /// </summary>
    public static class PermissionItemKeysExtension
    {

        /// <summary>
        /// 资讯投稿的权限项标识
        /// </summary>
        /// <param name="pik"><see cref="PermissionItemKeys"/></param>
        /// <returns></returns>
        public static string CMS_ContentItem(this PermissionItemKeys pik)
        {
            return "CMS_ContentItem";
        }
    }
}