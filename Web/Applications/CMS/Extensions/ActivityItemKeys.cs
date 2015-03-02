//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.CMS
{

    /// <summary>
    /// 资讯动态项
    /// </summary>
    public static class ActivityItemKeysExtension
    {
        /// <summary>
        /// 评论资讯
        /// </summary>
        public static string CreateCmsComment(this ActivityItemKeys activityItemKeys)
        {
            return "CreateCmsComment";
        }
    }
}