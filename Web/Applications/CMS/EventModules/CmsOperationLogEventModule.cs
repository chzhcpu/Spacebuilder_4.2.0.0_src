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

namespace Spacebuilder.CMS.EventModules
{
    /// <summary>
    /// 处理资讯操作日志
    /// </summary>
    public class CMSOperationLogEventModule : IEventMoudle
    {


        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            EventBus<ContentItem>.Instance().After += new CommonEventHandler<ContentItem, CommonEventArgs>(CmsOperationLogEventModule_After);
            EventBus<ContentFolder>.Instance().After += new CommonEventHandler<ContentFolder, CommonEventArgs>(ContentFolderOperationLogEventModule_After);
        }

        /// <summary>
        /// 资讯操作日志事件处理
        /// </summary>
        private void CmsOperationLogEventModule_After(ContentItem sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetEssential()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelEssential()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetFolderSticky()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetGlobalSticky()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelFolderSticky()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelGlobalSticky())
            {
                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
                entry.ApplicationId = CmsConfig.Instance().ApplicationId;
                entry.Source = CmsConfig.Instance().ApplicationName;
                entry.OperationType = eventArgs.EventOperationType;
                entry.OperationObjectName = sender.Title;
                entry.OperationObjectId = sender.ContentItemId;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType, entry.ApplicationId), "资讯", entry.OperationObjectName);

                OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                logService.Create(entry);
            }
        }
        /// <summary>
        /// 资讯操作日志事件处理
        /// </summary>
        private void ContentFolderOperationLogEventModule_After(ContentFolder sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create()
                || eventArgs.EventOperationType == EventOperationType.Instance().Update()
                || eventArgs.EventOperationType == EventOperationType.Instance().Delete()
                )
            {
                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
                entry.ApplicationId = CmsConfig.Instance().ApplicationId;
                entry.Source = CmsConfig.Instance().ApplicationName;
                entry.OperationType = eventArgs.EventOperationType;
                entry.OperationObjectName = sender.FolderName;
                entry.OperationObjectId = sender.ContentFolderId;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType, entry.ApplicationId), "资讯栏目", entry.OperationObjectName);

                OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                logService.Create(entry);
            }
        }
    }
}