//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2013-07-18</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2013-07-18" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System.Collections.Generic;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.CMS.EventModules
{
    /// <summary>
    /// 处理资讯索引的EventMoudle
    /// </summary>
    public class CmsIndexEventModule : IEventMoudle
    {
        private ContentItemService contentItemService = new ContentItemService();
        private ContentFolderService contentFolderService = new ContentFolderService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().ContentItem());

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而CmsSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此CmsSearcher不能在此处构建，只能再下面的方法中构建
        private CmsSearcher cmsSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<ContentItem>.Instance().After += new CommonEventHandler<ContentItem, CommonEventArgs>(ContentItem_After);
            EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToCms_BatchAfter);
            EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);
        }

        #region 标签增量索引

        /// <summary>
        /// 为资讯添加标签时触发
        /// </summary>
        private void AddTagsToCms_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().ContentItem())
            {
                long cmsThreadId = eventArgs.ItemId;
                if (cmsSearcher == null)
                {
                    cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
                }
                cmsSearcher.Update(contentItemService.Get(cmsThreadId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().ContentItem())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)资讯
                    IEnumerable<long> cmsThreadIds = tagService.GetItemIds(sender.TagName, null);
                    if (cmsSearcher == null)
                    {
                        cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
                    }
                    cmsSearcher.Update(contentItemService.GetContentItems(cmsThreadIds));
                }
            }
        }

        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().ContentItem())
            {
                long barThreadId = sender.ItemId;
                if (cmsSearcher == null)
                {
                    cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
                }
                cmsSearcher.Update(contentItemService.Get(barThreadId));
            }
        }

        #endregion

        #region 资讯增量索引
        /// <summary>
        /// 资讯增量索引
        /// </summary>
        private void ContentItem_After(ContentItem cms, CommonEventArgs eventArgs)
        {
            if (cms == null)
            {
                return;
            }

            if (cmsSearcher == null)
            {
                cmsSearcher = (CmsSearcher)SearcherFactory.GetSearcher(CmsSearcher.CODE);
            }

            //添加索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                cmsSearcher.Insert(cms);
            }

            //删除索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                cmsSearcher.Delete(cms.ContentItemId);
            }

            //更新索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update() || eventArgs.EventOperationType == EventOperationType.Instance().Approved() || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                cmsSearcher.Update(cms);
            }
        }
        #endregion
    }
}