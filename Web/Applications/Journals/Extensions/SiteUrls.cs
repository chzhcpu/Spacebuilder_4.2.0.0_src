//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Routing;
using Spacebuilder.Journals;
using Tunynet.Common;
using Tunynet;
using Tunynet.Utilities;
using System.Collections.Generic;
using Spacebuilder.Common;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// Journals链接管理
    /// </summary>
    public static class SiteUrlsExtension
    {
        private static readonly string JournalsAreaName = JournalsConfig.Instance().ApplicationKey;
         
        #region Journals频道

        /// <summary>
        /// 主页
        /// </summary>
        public static string JournalsHome(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Home", "ChannelJournals", JournalsAreaName);
        }

        #endregion

        /// <summary>
        /// 杂志详情页
        /// </summary>
        public static string JournalDetail(this SiteUrls siteUrls, string spaceKey, long journalId, long? commentId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary { { "spaceKey", spaceKey }, { "journalId", journalId } };
            if (commentId.HasValue)
                dic.Add("commentId", commentId.Value);
            //  return CachedUrlHelper.Action("Detail", "Blog", BlogAreaName, dic) + (commentId.HasValue ? "#" + commentId.Value : "");
            return CachedUrlHelper.Action("Detail", "Journals", JournalsAreaName, dic);
        }

        /// <summary>
        /// 我的/他的杂志
        /// </summary>
        public static string Journals(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Journals", "Journals", JournalsAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 收藏journal
        /// </summary>
        public static string JournalFavorite(this SiteUrls siteUrls, string spaceKey, long itemId, long userId)
        {
            string url= CachedUrlHelper.Action("Favorite", "Journals", JournalsAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "itemId", itemId }, { "userId", userId } });
            return url;
        }
  

        /// <summary>
        /// 群组全局搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string JournalsGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "ChannelJournals", JournalsAreaName);
        }
        /// <summary>
        /// 群组快捷搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string JournalsQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "ChannelJournals", JournalsAreaName);
        }

        /// <summary>
        /// 群组搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string JournalsPageSearch(this SiteUrls siteUrls, string keyword = "", string areaCode = "")
        {
            keyword = WebUtility.UrlEncode(keyword);
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                dic.Add("NowAreaCode", areaCode);
            }
            return CachedUrlHelper.Action("Search", "ChannelJournals", JournalsAreaName, dic);
        }

    }
}