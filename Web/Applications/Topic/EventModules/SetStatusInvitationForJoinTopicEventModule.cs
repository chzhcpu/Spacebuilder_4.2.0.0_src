//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 更改状态的时候触发事件
    /// </summary>
    public class SetStatusInvitationForJoinTopicEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<Invitation>.Instance().After += new CommonEventHandler<Invitation, CommonEventArgs>(SetStatusInvitationnForJoinTopicEventModule_After);
        }

        void SetStatusInvitationnForJoinTopicEventModule_After(Invitation sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                InvitationService invitationService = DIContainer.Resolve<InvitationService>();
                Invitation invitation = invitationService.Get(sender.Id);
                if (invitation != null && invitation.InvitationTypeKey == InvitationTypeKeys.Instance().InviteJoinTopic() && invitation.Status == InvitationStatus.Accept)
                {
                    TopicService groupService = new TopicService();
                    TopicMember member=TopicMember.New();
                    member.TopicId=sender.RelativeObjectId;
                    member.UserId = sender.UserId;
                    member.IsManager = false;
                    groupService.CreateTopicMember(member);
                }
                else if (invitation != null && invitation.InvitationTypeKey == InvitationTypeKeys.Instance().ApplyJoinTopic() && invitation.Status == InvitationStatus.Accept)
                {
                    TopicService groupService = new TopicService();
                    TopicMember member = TopicMember.New();
                    member.TopicId = sender.RelativeObjectId;
                    member.UserId = sender.SenderUserId;
                    member.IsManager = false;
                    groupService.CreateTopicMember(member);
                    IEnumerable<long> a = groupService.GetTopicMemberApplies(sender.RelativeObjectId, TopicMemberApplyStatus.Pending, 20, 1).Where(n => n.UserId == sender.SenderUserId).Select(m => m.Id);
                    groupService.ApproveTopicMemberApply(a,true);
                }
                else if (invitation != null && invitation.InvitationTypeKey == InvitationTypeKeys.Instance().ApplyJoinTopic() && invitation.Status == InvitationStatus.Refuse)
                {
                    TopicService groupService = new TopicService();
                    IEnumerable<long> a = groupService.GetTopicMemberApplies(sender.RelativeObjectId, TopicMemberApplyStatus.Pending, 20, 1).Where(n => n.UserId == sender.SenderUserId).Select(m => m.Id);
                    groupService.ApproveTopicMemberApply(a,false);
                }
            }
        }
    }
}
