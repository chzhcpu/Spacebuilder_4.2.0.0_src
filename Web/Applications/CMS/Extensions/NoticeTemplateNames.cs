//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 扩展NoticeTemplateNames
    /// </summary>
    public static class NoticeTemplateNamesExtension
    {
        /// <summary>
        /// 投稿被采纳
        /// </summary>
        public static string ContributeAccepted(this NoticeTemplateNames noticeTemplateNames)
        {
            return "ContributeAccepted";
        }

        /// <summary>
        /// 投稿被全局置顶
        /// </summary>
        public static string ContributeGlobalStickied(this NoticeTemplateNames noticeTemplateNames)
        {
            return "ContributeGlobalStickied";
        }

        /// <summary>
        /// 投稿被栏目置顶
        /// </summary>
        public static string ContributeFolderStickied(this NoticeTemplateNames noticeTemplateNames)
        {
            return "ContributeFolderStickied";
        }
    }
}