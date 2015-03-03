//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet;
using Tunynet.Events;
using Tunynet.Globalization;
using Tunynet.Logging;
using Spacebuilder.Common;

namespace SpecialTopic.Topic.EventModules
{
    /// <summary>
    /// 处理专题操作日志
    /// </summary>
    public class TopicOperationLogEventModule : IEventMoudle
    {


        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            EventBus<TopicEntity>.Instance().After += new CommonEventHandler<TopicEntity, CommonEventArgs>(TopicOperationLogEventModule_After);
        }


        /// <summary>
        /// 专题操作日志事件处理
        /// </summary>
        private void TopicOperationLogEventModule_After(TopicEntity senders, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete()
               || eventArgs.EventOperationType == EventOperationType.Instance().Approved()
               || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetEssential()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetSticky()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelEssential()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelSticky())
            {
                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
                entry.ApplicationId = entry.ApplicationId;
                entry.Source = TopicConfig.Instance().ApplicationName;
                entry.OperationType = eventArgs.EventOperationType;
                entry.OperationObjectName = senders.TopicName;
                entry.OperationObjectId = senders.TopicId;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType, entry.ApplicationId), "专题", entry.OperationObjectName);

                OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                logService.Create(entry);
            }
        }
    }
}