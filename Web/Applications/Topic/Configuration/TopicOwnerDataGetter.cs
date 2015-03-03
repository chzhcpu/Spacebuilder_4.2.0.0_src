using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题用户内容数
    /// </summary>
    public class TopicOwnerDataGetter : IOwnerDataGetter
    {
        /// <summary>
        /// datakey
        /// </summary>
        public string DataKey
        {
            get { return OwnerDataKeys.Instance().CreatedTopicCount(); }
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string DataName
        {
            get { return "创建的专题数"; }
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

            return SiteUrls.Instance().UserCreatedTopics(spaceKey);
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