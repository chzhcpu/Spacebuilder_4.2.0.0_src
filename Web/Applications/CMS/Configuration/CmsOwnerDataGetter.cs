using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 用户资讯计数获取器
    /// </summary>
    public class CmsOwnerDataGetter : IOwnerDataGetter
    {
        /// <summary>
        /// datakey
        /// </summary>
        public string DataKey
        {
            get { return OwnerDataKeys.Instance().ContributeCount(); }
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string DataName
        {
            get { return "投稿数"; }
        }

        /// <summary>
        /// 获取链接地址
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="ownerId">用户id</param>
        /// <returns></returns>
        public string GetDataUrl(string spaceKey, long? ownerId = null)
        {
            if (!string.IsNullOrEmpty(spaceKey) && ownerId == null)
                ownerId = UserIdToUserNameDictionary.GetUserId(spaceKey);

            return SiteUrls.Instance().CmsUser(ownerId.Value);
        }

        /// <summary>
        /// 应用Id
        /// </summary>
        public long ApplicationId
        {
            get { return CmsConfig.Instance().ApplicationId; }
        }
    }
}