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
    /// 处理专题申请通知的EventMoudle
    /// </summary>
    public class TopicMemberApplyEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<TopicMemberApply>.Instance().After += new CommonEventHandler<TopicMemberApply, CommonEventArgs>(TopicMemberApplyNoticeModule_After);
        }

        /// <summary>
        /// 通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void TopicMemberApplyNoticeModule_After(TopicMemberApply sender, CommonEventArgs eventArgs)
        {
            TopicService groupService = new TopicService();
            TopicEntity entity = groupService.Get(sender.TopicId);
            if (entity == null)
                return;

            User senderUser = DIContainer.Resolve<IUserService>().GetFullUser(sender.UserId);
            if (senderUser == null)
                return;
            InvitationService invitationService = new InvitationService();
            Invitation invitation;
            NoticeService noticeService = DIContainer.Resolve<NoticeService>();
            Notice notice;
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                if (sender.ApplyStatus == TopicMemberApplyStatus.Pending)
                {
                    List<long> toUserIds = new List<long>();
                    toUserIds.Add(entity.UserId);
                    toUserIds.AddRange(entity.TopicManagers.Select(n => n.UserId));
                    foreach (var toUserId in toUserIds)
                    {
                        //申请加入专题的请求
                        if (!groupService.IsMember(sender.TopicId, sender.UserId))
                        {
                            invitation = Invitation.New();
                            invitation.ApplicationId = TopicConfig.Instance().ApplicationId;
                            invitation.InvitationTypeKey = InvitationTypeKeys.Instance().ApplyJoinTopic();
                            invitation.UserId = toUserId;
                            invitation.SenderUserId = sender.UserId;
                            invitation.Sender = senderUser.DisplayName;
                            invitation.SenderUrl = SiteUrls.Instance().SpaceHome(sender.UserId);
                            invitation.RelativeObjectId = sender.TopicId;
                            invitation.RelativeObjectName = entity.TopicName;
                            invitation.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().TopicHome(entity.TopicKey));
                            invitation.Remark = sender.ApplyReason;
                            invitationService.Create(invitation);
                        }
                    }
                }
            }

            string noticeTemplateName = string.Empty;
            if (eventArgs.EventOperationType == EventOperationType.Instance().Approved())
            {
                if (sender.ApplyStatus == TopicMemberApplyStatus.Approved)
                {
                    noticeTemplateName = NoticeTemplateNames.Instance().MemberApplyApproved();
                }
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                if (sender.ApplyStatus == TopicMemberApplyStatus.Disapproved)
                {
                    noticeTemplateName = NoticeTemplateNames.Instance().MemberApplyDisapproved();
                }
            }

            if (string.IsNullOrEmpty(noticeTemplateName))
                return;

            notice = Notice.New();

            notice.UserId = sender.UserId;
            notice.ApplicationId = TopicConfig.Instance().ApplicationId;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            //notice.LeadingActorUserId = UserContext.CurrentUser.UserId;
            //notice.LeadingActor = UserContext.CurrentUser.DisplayName;
            //notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(UserContext.CurrentUser.UserId));
            notice.RelativeObjectId = sender.TopicId;
            notice.RelativeObjectName = StringUtility.Trim(entity.TopicName, 64);
            notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().TopicHome(entity.TopicKey));
            notice.TemplateName = noticeTemplateName;
            noticeService.Create(notice);
        }
    }
}