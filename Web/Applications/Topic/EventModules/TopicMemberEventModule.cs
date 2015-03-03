//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using SpecialTopic.Topic;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Spacebuilder.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet;

namespace SpecialTopic.Topic.EventModules
{
    /// <summary>
    /// 处理专题成员退出专题通知的EventMoudle
    /// </summary>
    public class TopicMemberEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<TopicMember>.Instance().After += new CommonEventHandler<TopicMember, CommonEventArgs>(TopicMemberActivityModule_After);
            EventBus<TopicMember>.Instance().After += new CommonEventHandler<TopicMember, CommonEventArgs>(TopicMemberNoticeModule_After);
            EventBus<TopicMember>.Instance().After += new CommonEventHandler<TopicMember, CommonEventArgs>(SetManagerNoticeEventModule_After);
        }

        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="groupMember"></param>
        /// <param name="eventArgs"></param>
        private void TopicMemberActivityModule_After(TopicMember groupMember, CommonEventArgs eventArgs)
        {
            ActivityService activityService = new ActivityService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                //生成动态
                if (groupMember == null)
                    return;
                var group = new TopicService().Get(groupMember.TopicId);
                if (group == null)
                    return;
                //生成Owner为专题的动态
                Activity actvityOfTopic = Activity.New();
                actvityOfTopic.ActivityItemKey = ActivityItemKeys.Instance().CreateTopicMember();
                actvityOfTopic.ApplicationId = TopicConfig.Instance().ApplicationId;
                actvityOfTopic.IsOriginalThread = true;
                actvityOfTopic.IsPrivate = !group.IsPublic;
                actvityOfTopic.UserId = groupMember.UserId;
                actvityOfTopic.ReferenceId = 0;
                actvityOfTopic.ReferenceTenantTypeId = string.Empty;
                actvityOfTopic.SourceId = groupMember.Id;
                actvityOfTopic.TenantTypeId = TenantTypeIds.Instance().User();
                actvityOfTopic.OwnerId = group.TopicId;
                actvityOfTopic.OwnerName = group.TopicName;
                actvityOfTopic.OwnerType = ActivityOwnerTypes.Instance().Topic();

                activityService.Generate(actvityOfTopic, false);

                //生成Owner为用户的动态
                Activity actvityOfUser = Activity.New();
                actvityOfUser.ActivityItemKey = ActivityItemKeys.Instance().JoinTopic();
                actvityOfUser.ApplicationId = actvityOfTopic.ApplicationId;
                actvityOfUser.HasImage = actvityOfTopic.HasImage;
                actvityOfUser.HasMusic = actvityOfTopic.HasMusic;
                actvityOfUser.HasVideo = actvityOfTopic.HasVideo;
                actvityOfUser.IsOriginalThread = actvityOfTopic.IsOriginalThread;
                actvityOfUser.IsPrivate = actvityOfTopic.IsPrivate;
                actvityOfUser.UserId = actvityOfTopic.UserId;
                actvityOfUser.ReferenceId = actvityOfTopic.ReferenceId;
                actvityOfTopic.ReferenceTenantTypeId = actvityOfTopic.ReferenceTenantTypeId;
                actvityOfUser.SourceId = actvityOfTopic.SourceId;

                actvityOfUser.TenantTypeId = actvityOfTopic.TenantTypeId;
                actvityOfUser.OwnerId = groupMember.UserId;
                actvityOfUser.OwnerName = groupMember.User.DisplayName;
                actvityOfUser.OwnerType = ActivityOwnerTypes.Instance().User();
                activityService.Generate(actvityOfUser, false);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete()) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().User(), groupMember.UserId);
            }
        }


        /// <summary>
        /// 通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void TopicMemberNoticeModule_After(TopicMember sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType != EventOperationType.Instance().Delete() && eventArgs.EventOperationType != EventOperationType.Instance().Create() && sender != null)
                return;
            TopicService groupService = new TopicService();
            TopicEntity entity = groupService.Get(sender.TopicId);
            if (entity == null)
                return;

            User senderUser = DIContainer.Resolve<IUserService>().GetFullUser(sender.UserId);
            if (senderUser == null)
                return;

            NoticeService noticeService = DIContainer.Resolve<NoticeService>();
            Notice notice;

            List<long> toUserIds = new List<long>();
            toUserIds.Add(entity.UserId);
            toUserIds.AddRange(entity.TopicManagers.Select(n => n.UserId));
            //删除专题成员通知群管理员
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                foreach (var toUserId in toUserIds)
                {
                    if (toUserId == sender.UserId)
                        continue;
                    notice = Notice.New();
                    notice.UserId = toUserId;
                    notice.ApplicationId = TopicConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActorUserId = sender.UserId;
                    notice.LeadingActor = senderUser.DisplayName;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                    notice.RelativeObjectId = sender.TopicId;
                    notice.RelativeObjectName = StringUtility.Trim(entity.TopicName, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().TopicHome(entity.TopicKey));
                    notice.TemplateName = NoticeTemplateNames.Instance().MemberQuit();
                    noticeService.Create(notice);
                }
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Create()) //添加群成员时向群管理员发送通知
            {
                foreach (var toUserId in toUserIds)
                {
                    if (toUserId == sender.UserId)
                        continue;
                    notice = Notice.New();
                    notice.UserId = toUserId;
                    notice.ApplicationId = TopicConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActorUserId = sender.UserId;
                    notice.LeadingActor = senderUser.DisplayName;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                    notice.RelativeObjectId = sender.TopicId;
                    notice.RelativeObjectName = StringUtility.Trim(entity.TopicName, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().TopicHome(entity.TopicKey));
                    notice.TemplateName = NoticeTemplateNames.Instance().MemberJoin();
                    noticeService.Create(notice);
                }
                //向加入者发送通知

                //notice = Notice.New();
                //notice.UserId = sender.UserId;
                //notice.ApplicationId = TopicConfig.Instance().ApplicationId;
                //notice.TypeId = NoticeTypeIds.Instance().Hint();
                //notice.LeadingActorUserId = sender.UserId;
                //notice.LeadingActor = senderUser.DisplayName;
                //notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                //notice.RelativeObjectId = sender.TopicId;
                //notice.RelativeObjectName = StringUtility.Trim(entity.TopicName, 64);
                //notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().TopicHome(entity.TopicKey));
                //notice.TemplateName = NoticeTemplateNames.Instance().MemberApplyApproved();
                //noticeService.Create(notice);
            }
        }

        /// <summary>
        /// 设置/取消管理员通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SetManagerNoticeEventModule_After(TopicMember sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType != EventOperationType.Instance().SetTopicManager() && eventArgs.EventOperationType != EventOperationType.Instance().CancelTopicManager())
                return;

            TopicService groupService = new TopicService();
            TopicEntity entity = groupService.Get(sender.TopicId);
            if (entity == null)
                return;

            User senderUser = DIContainer.Resolve<IUserService>().GetFullUser(sender.UserId);
            if (senderUser == null)
                return;

            NoticeService noticeService = DIContainer.Resolve<NoticeService>();

            Notice notice = Notice.New();
            notice.UserId = sender.UserId;
            notice.ApplicationId = TopicConfig.Instance().ApplicationId;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            notice.LeadingActorUserId = 0;
            notice.LeadingActor = string.Empty;
            notice.LeadingActorUrl = string.Empty;
            notice.RelativeObjectId = sender.TopicId;
            notice.RelativeObjectName = StringUtility.Trim(entity.TopicName, 64);
            notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().TopicHome(entity.TopicKey));

            if (eventArgs.EventOperationType == EventOperationType.Instance().SetTopicManager())
            {
                notice.TemplateName = NoticeTemplateNames.Instance().SetTopicManager();
            }
            else
            {
                notice.TemplateName = NoticeTemplateNames.Instance().CannelTopicManager();
            }
            noticeService.Create(notice);
        }
    }
}