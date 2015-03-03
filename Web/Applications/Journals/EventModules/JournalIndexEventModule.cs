//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-10-31</createdate>
//<author>wanf</author>
//<email>wanf@tunynet.com</email>
//<log date="2012-10-31" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System.Collections.Generic;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Events;
using Spacebuilder.Journals;

namespace Spacebuilder.Blog.EventModules
{
    /// <summary>
    /// 处理日志索引的EventMoudle
    /// </summary>
    public class JournalIndexEventModule : IEventMoudle
    {
        private JournalsService journalService = new JournalsService();
        private CategoryService categoryService = new CategoryService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Journals());

        //因为EventModule.RegisterEventHandler()在web启动时初始化，而BlogSearcher的构造函数依赖于WCF服务（分布式搜索部署情况下），此时WCF服务尚无法连接，因此BlogSearcher不能在此处构建，只能再下面的方法中构建
        private JournalSearcher journalSearcher = null;

        public void RegisterEventHandler()
        {
            EventBus<Journal>.Instance().After += new CommonEventHandler<Journal, CommonEventArgs>(Journal_After);

            //EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddTagsToBlog_BatchAfter);
            //EventBus<Tag>.Instance().Before += new CommonEventHandler<Tag, CommonEventArgs>(DeleteUpdateTags_Before);
            //EventBus<ItemInTag>.Instance().After += new CommonEventHandler<ItemInTag, CommonEventArgs>(DeleteItemInTags);

            //EventBus<string, TagEventArgs>.Instance().BatchAfter += new BatchEventHandler<string, TagEventArgs>(AddCategoriesToBlog_BatchAfter);
            //EventBus<Category>.Instance().Before += new CommonEventHandler<Category, CommonEventArgs>(DeleteUpdateCategories_Before);
        }

        /*
        //todo:wanf 分类 及 标签 索引
        #region 分类增量索引

        /// <summary>
        /// 为日志添加分类时触发
        /// </summary>
        private void AddCategoriesToBlog_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                long blogThreadId = eventArgs.ItemId;
                if (journalSearcher == null)
                {
                    journalSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                }
                journalSearcher.Update(journalService.Get(blogThreadId));
            }
        }

        /// <summary>
        /// 删除和更新分类时触发
        /// </summary>
        private void DeleteUpdateCategories_Before(Category sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() || eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    IEnumerable<long> blogThreadIds = categoryService.GetItemIds(sender.CategoryId, false);
                    if (journalSearcher == null)
                    {
                        journalSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                    }
                    journalSearcher.Update(journalService.GetBlogThreads(blogThreadIds));
                }
            }
        }
        #endregion

        #region 标签增量索引

        /// <summary>
        /// 为日志添加标签时触发
        /// </summary>
        private void AddTagsToBlog_BatchAfter(IEnumerable<string> senders, TagEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                long blogThreadId = eventArgs.ItemId;
                if (journalSearcher == null)
                {
                    journalSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                }
                journalSearcher.Update(journalService.Get(blogThreadId));
            }
        }
        /// <summary>
        /// 删除和更新标签时触发
        /// </summary>
        private void DeleteUpdateTags_Before(Tag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId==TenantTypeIds.Instance().BlogThread())
            {
                if (eventArgs.EventOperationType==EventOperationType.Instance().Delete()||eventArgs.EventOperationType==EventOperationType.Instance().Update())
                {
                    //根据标签获取所有使用该标签的(内容项)日志
                    IEnumerable<long> blogThreadIds = tagService.GetItemIds(sender.TagName, null);
                    if (journalSearcher == null)
                    {
                       journalSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                    }
                    journalSearcher.Update(journalService.GetBlogThreads(blogThreadIds));
                }
            }
        }
        private void DeleteItemInTags(ItemInTag sender, CommonEventArgs eventArgs)
        {
            if (sender.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                long barThreadId = sender.ItemId;
                if (journalSearcher == null)
                {
                    journalSearcher = (BlogSearcher)SearcherFactory.GetSearcher(BlogSearcher.CODE);
                }
                journalSearcher.Update(journalService.Get(barThreadId));
            }
        }
        #endregion    
*/
        
        #region 日志增量索引
        /// <summary>
        /// 日志增量索引
        /// </summary>
        private void Journal_After(Journal journal, CommonEventArgs eventArgs)
        {
            if (journal == null)
            {
                return;
            }

            if (journalSearcher == null)
            {
                journalSearcher = (JournalSearcher)SearcherFactory.GetSearcher(JournalSearcher.CODE);
            }

            //添加索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                journalSearcher.Insert(journal);
            }

            //删除索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                journalSearcher.Delete(journal.id);
            }

            //更新索引
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update() || eventArgs.EventOperationType == EventOperationType.Instance().Approved() || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                journalSearcher.Update(journal);

            }
        }
        #endregion
         
    }
}