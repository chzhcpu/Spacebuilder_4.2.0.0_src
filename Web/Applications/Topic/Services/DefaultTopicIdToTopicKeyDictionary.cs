//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 通过专题数据仓储实现查询
    /// </summary>
    public class DefaultTopicIdToTopicKeyDictionary : TopicIdToTopicKeyDictionary
    {
        private ITopicRepository groupRepository;
        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultTopicIdToTopicKeyDictionary()
            : this(new TopicRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public DefaultTopicIdToTopicKeyDictionary(ITopicRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        /// <summary>
        /// 根据专题Id获取专题Key
        /// </summary>
        /// <returns>
        /// 专题Id
        /// </returns>
        protected override string GetTopicKeyByTopicId(long groupId)
        {
            TopicEntity group = groupRepository.Get(groupId);
            if (group != null)
                return group.TopicKey;
            return null;
        }

        /// <summary>
        /// 根据专题Key获取专题Id
        /// </summary>
        /// <param name="groupKey">专题Key</param>
        /// <returns>
        /// 专题Id
        /// </returns>
        protected override long GetTopicIdByTopicKey(string groupKey)
        {
            return groupRepository.GetTopicIdByTopicKey(groupKey);
        }
    }
}
