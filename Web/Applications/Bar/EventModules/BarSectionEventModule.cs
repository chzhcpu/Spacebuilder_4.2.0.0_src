//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-09-14</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-09-14" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using Spacebuilder.Bar;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Spacebuilder.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet;
using Spacebuilder.Group;
using SpecialTopic.Topic;

namespace Spacebuilder.Bar.EventModules
{
    /// <summary>
    /// 处理帖吧的EventModule
    /// </summary>
    public class BarSectionEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<BarSection>.Instance().After += new CommonEventHandler<BarSection, CommonEventArgs>(BarSectionNoticeEventModule_After);
            EventBus<long, SubscribeEventArgs>.Instance().After += new CommonEventHandler<long, SubscribeEventArgs>(SubscribeBarSectionEventModule_After);
            EventBus<GroupEntity>.Instance().After += new CommonEventHandler<GroupEntity, CommonEventArgs>(AutoMaintainBarSectionModule_After);
            EventBus<TopicEntity>.Instance().After += new CommonEventHandler<TopicEntity, CommonEventArgs>(AutoMaintainBarSectionModule_After);
        }

        //todo:mazq,by zhengw:走查以下代码：用户关注帖吧时，要追溯该帖吧的动态
        /// <summary>
        /// 关注帖吧事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void SubscribeBarSectionEventModule_After(long sender, SubscribeEventArgs eventArgs)
        {
            if (eventArgs.TenantTypeId != TenantTypeIds.Instance().BarSection())
                return;
            ActivityService activityService = new ActivityService();
            if (EventOperationType.Instance().Create() == eventArgs.EventOperationType)
            {
                activityService.TraceBackInboxAboutOwner(eventArgs.UserId, sender, ActivityOwnerTypes.Instance().BarSection());
                //用户内容计数
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(eventArgs.UserId, OwnerDataKeys.Instance().FollowSectionCount(), 1);
            }
            else if (EventOperationType.Instance().Delete() == eventArgs.EventOperationType)
            {
                activityService.RemoveInboxAboutOwner(eventArgs.UserId, sender, ActivityOwnerTypes.Instance().BarSection());
                //用户内容计数
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(eventArgs.UserId, OwnerDataKeys.Instance().FollowSectionCount(), -1);
            }
        }

        /// <summary>
        /// 帖吧申请处理结果通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void BarSectionNoticeEventModule_After(BarSection sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Approved() || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {

                IUserService userService = DIContainer.Resolve<IUserService>();
                NoticeService noticeService = DIContainer.Resolve<NoticeService>();
                User toUser = userService.GetFullUser(sender.UserId);
                if (toUser == null)
                    return;
                Notice notice = Notice.New();
                notice.UserId = sender.UserId;
                notice.ApplicationId = BarConfig.Instance().ApplicationId;
                notice.TypeId = NoticeTypeIds.Instance().Reply();
                notice.LeadingActorUserId = sender.UserId;
                notice.LeadingActor = toUser.DisplayName;
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(toUser.UserName));
                notice.RelativeObjectId = sender.SectionId;
                notice.RelativeObjectName = HtmlUtility.TrimHtml(sender.Name, 64);
                if (eventArgs.EventOperationType == EventOperationType.Instance().Approved())
                {
                    //通知吧主，其申请的帖吧通过了审核 
                    notice.TemplateName = NoticeTemplateNames.Instance().ManagerApproved();
                }
                else
                {
                    //通知吧主，其申请的帖吧未通过审核
                    notice.TemplateName = NoticeTemplateNames.Instance().ManagerDisapproved();
                }

                notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().SectionDetail(sender.SectionId));
                noticeService.Create(notice);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BarSection());
                IEnumerable<long> userIds = subscribeService.GetTopUserIdsOfObject(sender.SectionId, int.MaxValue);

                foreach (long userId in userIds)
                    subscribeService.CancelSubscribe(sender.SectionId, userId);
            }
        }

        /// <summary>
        /// 贴吧所在呈现区域拥有者自动创建贴吧事件
        /// </summary>
        /// <param name="sender">群组实体</param>
        /// <param name="eventArgs">事件参数</param>
        void AutoMaintainBarSectionModule_After(GroupEntity sender, CommonEventArgs eventArgs)
        {
            BarSectionService barSectionService = new BarSectionService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {

                BarSection barSection = BarSection.New();

                barSection.TenantTypeId = TenantTypeIds.Instance().Group();
                barSection.SectionId = sender.GroupId;
                barSection.OwnerId = sender.GroupId;
                barSection.UserId = sender.UserId;
                barSection.Name = sender.GroupName;
                barSection.IsEnabled = true;
                barSection.LogoImage = sender.Logo;
                barSection.ThreadCategoryStatus = ThreadCategoryStatus.NotForceEnabled;
                barSectionService.Create(barSection, sender.UserId, null, null);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                BarSection barSection = barSectionService.Get(sender.GroupId);
                barSection.UserId = sender.UserId;
                barSection.Name = sender.GroupName;
                barSection.LogoImage = sender.Logo;

                IList<long> managerIds = null;
                if (barSection.SectionManagers != null)
                {
                    managerIds = barSection.SectionManagers.Select(n => n.UserId).ToList();
                }
                barSectionService.Update(barSection, sender.UserId, managerIds, null);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Approved() || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                BarSection barSection = barSectionService.Get(sender.GroupId);
                barSection.AuditStatus = sender.AuditStatus;
                IList<long> managerIds = null;
                if (barSection.SectionManagers != null)
                {
                    managerIds = barSection.SectionManagers.Select(n => n.UserId).ToList();
                }
                barSectionService.Update(barSection, sender.UserId, managerIds, null);
            }
            else
            {
                barSectionService.Delete(sender.GroupId);
            }
        }


        void AutoMaintainBarSectionModule_After(TopicEntity sender, CommonEventArgs eventArgs)
        {
            BarSectionService barSectionService = new BarSectionService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {

                BarSection barSection = BarSection.New();

                barSection.TenantTypeId = TenantTypeIds.Instance().Topic();
                barSection.SectionId = sender.TopicId;
                barSection.OwnerId = sender.TopicId;
                barSection.UserId = sender.UserId;
                barSection.Name = sender.TopicName;
                barSection.IsEnabled = true;
                barSection.LogoImage = sender.Logo;
                barSection.ThreadCategoryStatus = ThreadCategoryStatus.NotForceEnabled;
                barSectionService.Create(barSection, sender.UserId, null, null);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                BarSection barSection = barSectionService.Get(sender.TopicId);
                barSection.UserId = sender.UserId;
                barSection.Name = sender.TopicName;
                barSection.LogoImage = sender.Logo;

                IList<long> managerIds = null;
                if (barSection.SectionManagers != null)
                {
                    managerIds = barSection.SectionManagers.Select(n => n.UserId).ToList();
                }
                barSectionService.Update(barSection, sender.UserId, managerIds, null);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Approved() || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                BarSection barSection = barSectionService.Get(sender.TopicId);
                barSection.AuditStatus = sender.AuditStatus;
                IList<long> managerIds = null;
                if (barSection.SectionManagers != null)
                {
                    managerIds = barSection.SectionManagers.Select(n => n.UserId).ToList();
                }
                barSectionService.Update(barSection, sender.UserId, managerIds, null);
            }
            else
            {
                barSectionService.Delete(sender.TopicId);
            }
        }
    }
}