//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet.Globalization;
using Tunynet.UI;

namespace SpecialTopic.Topic.EventModules
{
    /// <summary>
    /// 处理专题动态、积分的EventMoudle
    /// </summary>
    public class TopicEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<TopicEntity, AuditEventArgs>.Instance().After += new CommonEventHandler<TopicEntity, AuditEventArgs>(TopicEntityActivityModule_After);
            EventBus<TopicEntity, AuditEventArgs>.Instance().After += new CommonEventHandler<TopicEntity, AuditEventArgs>(TopicEntityPointModule_After);
            EventBus<TopicEntity>.Instance().After += new CommonEventHandler<TopicEntity, CommonEventArgs>(InstallApplicationsModule_After);
            EventBus<TopicEntity>.Instance().After += new CommonEventHandler<TopicEntity, CommonEventArgs>(ChangeThemeAppearanceUserCountModule_After);
        }



        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="group"></param>
        /// <param name="eventArgs"></param>
        private void TopicEntityActivityModule_After(TopicEntity group, AuditEventArgs eventArgs)
        {
            //生成动态
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //生成动态
            {
                if (group == null)
                    return;

                //生成Owner为用户的动态
                Activity actvityOfBar = Activity.New();
                actvityOfBar.ActivityItemKey = ActivityItemKeys.Instance().CreateTopic();
                actvityOfBar.ApplicationId = TopicConfig.Instance().ApplicationId;
                actvityOfBar.IsOriginalThread = true;
                actvityOfBar.IsPrivate = !group.IsPublic;
                actvityOfBar.UserId = group.UserId;
                actvityOfBar.ReferenceId = 0;//没有涉及到的实体
                actvityOfBar.ReferenceTenantTypeId = string.Empty;
                actvityOfBar.SourceId = group.TopicId;
                actvityOfBar.TenantTypeId = TenantTypeIds.Instance().Topic();
                actvityOfBar.OwnerId = group.UserId;
                actvityOfBar.OwnerName = group.User.DisplayName;
                actvityOfBar.OwnerType = ActivityOwnerTypes.Instance().User();

                activityService.Generate(actvityOfBar, true);
            }
            else if (auditDirection == false) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().Topic(), group.TopicId);
            }
        }

        /// <summary>
        /// 审核状态发生变化时处理积分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void TopicEntityPointModule_After(TopicEntity sender, AuditEventArgs eventArgs)
        {
            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;
            string description = string.Empty;
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            PointService pointService = new PointService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().Topic_CreateTopic();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().Topic_DeleteTopic();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }
            if (!string.IsNullOrEmpty(pointItemKey))
            {
                description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "专题", sender.TopicName);
            }
            pointService.GenerateByRole(sender.UserId, pointItemKey, description, eventOperationType == EventOperationType.Instance().Create() || eventOperationType == EventOperationType.Instance().Delete() && eventArgs.OperatorInfo.OperatorUserId == sender.UserId);
        }

        /// <summary>
        /// 自动安装专题的相关应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void InstallApplicationsModule_After(TopicEntity sender, CommonEventArgs eventArgs)
        {
            ApplicationService applicationService = new ApplicationService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                applicationService.InstallApplicationsOfPresentAreaOwner(PresentAreaKeysOfExtension.TopicSpace, sender.TopicId);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                applicationService.DeleteApplicationsOfPresentAreaOwner(PresentAreaKeysOfExtension.TopicSpace, sender.TopicId);
            }
        }

        /// <summary>
        /// 修改皮肤文件的使用人数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ChangeThemeAppearanceUserCountModule_After(TopicEntity sender, CommonEventArgs eventArgs)
        {
            var themeService = new ThemeService();
            PresentAreaService presentAreaService = new PresentAreaService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                var presentArea = presentAreaService.Get(PresentAreaKeysOfExtension.TopicSpace);
                themeService.ChangeThemeAppearanceUserCount(PresentAreaKeysOfExtension.TopicSpace, null, presentArea.DefaultThemeKey + "," + presentArea.DefaultAppearanceKey);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                var presentArea = presentAreaService.Get(PresentAreaKeysOfExtension.TopicSpace);
                string defaultThemeAppearance = presentArea.DefaultThemeKey + "," + presentArea.DefaultAppearanceKey;
                if (!sender.IsUseCustomStyle)
                    themeService.ChangeThemeAppearanceUserCount(PresentAreaKeysOfExtension.TopicSpace, !string.IsNullOrEmpty(sender.ThemeAppearance) ? sender.ThemeAppearance : defaultThemeAppearance, null);
            }
        }
    }
}