//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Events;
using SpecialTopic.Topic;

namespace SpecialTopic.Blog.EventModules
{
    /// <summary>
    /// 处理专题索引的EventMoudle
    /// </summary>
    public class TopicIndexEventModule : IEventMoudle
    {
        private TopicService topicService = new TopicService();
        private CategoryService categoryService = new CategoryService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Topic());

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而TopicSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此TopicSearcher不能在此处构建，只能再下面的方法中构建
        private TopicSearcher groupSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<TopicEntity>.Instance().After += new CommonEventHandler<TopicEntity, CommonEventArgs>(TopicEntity_After);

            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToTopic_BatchAfter);
            EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);

            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddCategoriesToTopic_BatchAfter);
            EventBus<Category>.Instance().Before += new CommonEventHandler<Category, CommonEventArgs>(DeleteUpdateCategories_Before);
        }

        #region 分类增量索引

        /// <summary>
        /// 为专题添加分类时触发
        /// </summary>
        private void AddCategoriesToTopic_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().Topic())
            {
                long groupId = eventArgs.ItemId;
                if (groupSearcher == null)
                {
                    groupSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
                }
                groupSearcher.Update(topicService.Get(groupId));
            }
        }

        /// <summary>
        /// 删除和更新分类时触发
        /// </summary>
        private void DeleteUpdateCategories_Before(Category sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().Topic())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    IEnumerable<long> groupIds = categoryService.GetItemIds(sender.CategoryId, false);
                    if (groupSearcher == null)
                    {
                        groupSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
                    }
                    groupSearcher.Update(topicService.GetTopicEntitiesByIds(groupIds));
                }
            }
        }
        #endregion

        #region 标签增量索引
        /// <summary>
        /// 为专题添加标签时触发
        /// </summary>
        private void AddTagsToTopic_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().Topic())
            {
                long groupId = eventArgs.ItemId;
                if (groupSearcher == null)
                {
                    groupSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
                }
                groupSearcher.Update(topicService.Get(groupId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().Topic())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)专题
                    IEnumerable<long> groupIds = tagService.GetItemIds(sender.TagName, null);
                    if (groupSearcher == null)
                    {
                        groupSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
                    }
                    groupSearcher.Update(topicService.GetTopicEntitiesByIds(groupIds));
                }
            }
        }
        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().Topic())
            {
                long groupId = sender.ItemId;
                if (groupSearcher == null)
                {
                    groupSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
                }
                groupSearcher.Update(topicService.Get(groupId));
            }
        }
        #endregion

        #region 专题增量索引
        /// <summary>
        /// 专题增量索引
        /// </summary>
        private void TopicEntity_After(TopicEntity group, CommonEventArgs eventArgs)
        {
            if (group == null)
            {
                return;
            }

            if (groupSearcher == null)
            {
                groupSearcher = (TopicSearcher)SearcherFactory.GetSearcher(TopicSearcher.CODE);
            }

            //添加索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                groupSearcher.Insert(group);
            }

            //删除索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                groupSearcher.Delete(group.TopicId);
            }

            //更新索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update() || eventArgs.EventOperationType == EventOperationType.Instance().Approved() || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                groupSearcher.Update(group);
            }
        }
        #endregion
    }
}