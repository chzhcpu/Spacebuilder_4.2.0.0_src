using System;
using Tunynet.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Tunynet.Common.Configuration;
using Tunynet.Common;
namespace Spacebuilder.CMS
{
    /// <summary>
    /// 每天更新置顶时间到期的资讯任务
    /// </summary>
    public class ExpireStickyContentItemTask:ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            ContentItemService contentItemService = new ContentItemService();
            contentItemService.ExpireStickyContentItems();    
        }
    }
}