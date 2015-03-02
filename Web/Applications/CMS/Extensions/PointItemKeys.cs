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

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class PointItemKeysExtension
    {
        /// <summary>
        /// 投稿被采纳
        /// </summary>
        public static string CMS_ContributeAccepted(this PointItemKeys pointItemKeys)
        {
            return "CMS_ContributeAccepted";
        }

        /// <summary>
        /// 投稿被删除
        /// </summary>
        public static string CMS_ContributeDeleted(this PointItemKeys pointItemKeys)
        {
            return "CMS_ContributeDeleted";
        }

        /// <summary>
        /// 投稿被置顶
        /// </summary>
        public static string CMS_StickyNews(this PointItemKeys pointItemKeys)
        {
            return "CMS_StickyNews";
        }
    }
}