//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Concurrent;
using Tunynet;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// TopicId与TopicKey的查询器
    /// </summary>
    public abstract class TopicIdToTopicKeyDictionary
    {
        private static ConcurrentDictionary<long, string> dictionaryOfTopicIdToTopicKey = new ConcurrentDictionary<long, string>();
        private static ConcurrentDictionary<string, long> dictionaryOfTopicKeyToTopicId = new ConcurrentDictionary<string, long>();

        #region Instance

        private static volatile TopicIdToTopicKeyDictionary _defaultInstance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 获取TopicIdToTopicKeyAccessor实例
        /// </summary>
        /// <returns></returns>
        private static TopicIdToTopicKeyDictionary Instance()
        {
            if (_defaultInstance == null)
            {
                lock (lockObject)
                {
                    if (_defaultInstance == null)
                    {
                        _defaultInstance = DIContainer.Resolve<TopicIdToTopicKeyDictionary>();
                        if (_defaultInstance == null)
                            throw new ExceptionFacade("未在DIContainer注册TopicIdToTopicKeyDictionary的具体实现类");
                    }
                }
            }
            return _defaultInstance;
        }

        #endregion

        /// <summary>
        /// 根据专题Id获取专题Key
        /// </summary>
        /// <returns>
        /// 专题Key
        /// </returns>
        protected abstract string GetTopicKeyByTopicId(long groupId);

        /// <summary>
        /// 根据专题Key获取专题Id
        /// </summary>
        /// <returns>
        /// 专题Id
        /// </returns>
        protected abstract long GetTopicIdByTopicKey(string groupKey);


        /// <summary>
        /// 通过groupId获取groupKey
        /// </summary>
        /// <param name="TopicId">TopicId</param>
        public static string GetTopicKey(long groupId)
        {
            if (dictionaryOfTopicIdToTopicKey.ContainsKey(groupId))
                return dictionaryOfTopicIdToTopicKey[groupId];
            string groupKey = Instance().GetTopicKeyByTopicId(groupId);
            if (!string.IsNullOrEmpty(groupKey))
            {
                dictionaryOfTopicIdToTopicKey[groupId] = groupKey;
                if (!dictionaryOfTopicKeyToTopicId.ContainsKey(groupKey))
                    dictionaryOfTopicKeyToTopicId[groupKey] = groupId;
                return groupKey;
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过groupKey获取groupId
        /// </summary>
        /// <param name="TopicKey"></param>
        /// <returns></returns>
        public static long GetTopicId(string groupKey)
        {
            if (dictionaryOfTopicKeyToTopicId.ContainsKey(groupKey))
                return dictionaryOfTopicKeyToTopicId[groupKey];
            long groupId = Instance().GetTopicIdByTopicKey(groupKey);
            if (groupId > 0)
            {
                dictionaryOfTopicKeyToTopicId[groupKey] = groupId;
                if (!dictionaryOfTopicIdToTopicKey.ContainsKey(groupId))
                    dictionaryOfTopicIdToTopicKey[groupId] = groupKey;
            }
            return groupId;
        }

        /// <summary>
        /// 移除TopicId
        /// </summary>
        /// <param name="groupId">groupId</param>
        internal static void RemoveTopicId(long groupId)
        {
            string groupKey;
            dictionaryOfTopicIdToTopicKey.TryRemove(groupId, out groupKey);
        }

        /// <summary>
        /// 移除TopicKey
        /// </summary>
        /// <param name="groupKey">groupKey</param>
        internal static void RemoveTopicKey(string groupKey)
        {
            long groupId;
            dictionaryOfTopicKeyToTopicId.TryRemove(groupKey, out groupId);
        }
    }
}