using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;

namespace SpecialTopic.Topic
{
    public class JoinTopicOwnerDataGetter : IOwnerDataGetter
    {
        /// <summary>
        /// datakey
        /// </summary>
        public string DataKey
        {
            get { return OwnerDataKeys.Instance().JoinedTopicCount(); }
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string DataName
        {
            get { return "加入的专题数"; }
        }

        /// <summary>
        /// 获取链接地址
        /// </summary>
        /// <param name="spaceKey">用户名</param>
        /// <param name="ownerId">用户id</param>
        /// <returns></returns>
        public string GetDataUrl(string spaceKey, long? ownerId = null)
        {
            if (string.IsNullOrEmpty(spaceKey) && ownerId.HasValue)
                spaceKey = UserIdToUserNameDictionary.GetUserName(ownerId.Value);

            return SiteUrls.Instance().UserJoinedTopics(spaceKey);
        }

        /// <summary>
        /// 应用Id
        /// </summary>
        public long ApplicationId
        {
            get { return TopicConfig.Instance().ApplicationId; }
        }
    }
}