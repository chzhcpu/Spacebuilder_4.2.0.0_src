//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2013</copyright>
//<version>V0.5</verion>
//<createdate>2013-07-01</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2013-07-01" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using Spacebuilder.CMS;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Spacebuilder.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet;

namespace Spacebuilder.CMS.EventModules
{

    /// <summary>
    /// 处理资讯动态、积分的EventMoudle
    /// </summary>
    public class ContentItemEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<ContentItem, AuditEventArgs>.Instance().After += new CommonEventHandler<ContentItem, AuditEventArgs>(ContentItemPointModule_After);
            EventBus<ContentItem, AuditEventArgs>.Instance().After += new CommonEventHandler<ContentItem, AuditEventArgs>(ContentItemNoticeModule_After);
            EventBus<ContentItem>.Instance().After += new CommonEventHandler<ContentItem, CommonEventArgs>(ContentItemPointModuleForManagerOperation_After);
            EventBus<Comment, AuditEventArgs>.Instance().After += new CommonEventHandler<Comment, AuditEventArgs>(CmsCommentActivityEventModule_After);
        }

        /// <summary>
        /// 积分处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ContentItemPointModule_After(ContentItem sender, AuditEventArgs eventArgs)
        {
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);

            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;

            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().CMS_ContributeAccepted();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().CMS_ContributeDeleted();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }

            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();

                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "资讯", sender.Title);
                pointService.GenerateByRole(sender.UserId, pointItemKey, description, eventOperationType == EventOperationType.Instance().Create() || eventOperationType == EventOperationType.Instance().Delete() && eventArgs.OperatorInfo.OperatorUserId == sender.UserId);
            }
        }

        /// <summary>
        /// 处理置顶操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ContentItemPointModuleForManagerOperation_After(ContentItem sender, CommonEventArgs eventArgs)
        {
            NoticeService noticeService = new NoticeService();
            string pointItemKey = string.Empty;
            if (eventArgs.EventOperationType == EventOperationType.Instance().SetGlobalSticky() || eventArgs.EventOperationType == EventOperationType.Instance().SetFolderSticky())
            {
                pointItemKey = PointItemKeys.Instance().CMS_StickyNews();
                if (sender.UserId > 0)
                {
                    Notice notice = Notice.New();
                    notice.UserId = sender.UserId;
                    notice.ApplicationId = CmsConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActor = sender.Author;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                    notice.RelativeObjectName = sender.Title;
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().ContentItemDetail(sender.ContentItemId));
                    if (eventArgs.EventOperationType == EventOperationType.Instance().SetGlobalSticky())
                        notice.TemplateName = NoticeTemplateNames.Instance().ContributeGlobalStickied();
                    else
                        notice.TemplateName = NoticeTemplateNames.Instance().ContributeFolderStickied();
                    noticeService.Create(notice);
                }
            }

            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventArgs.EventOperationType), "资讯", sender.Title);
                pointService.GenerateByRole(sender.UserId, pointItemKey, description);
            }
        }

        /// <summary>
        /// 通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ContentItemNoticeModule_After(ContentItem sender, AuditEventArgs eventArgs)
        {
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true)
            {

                long toUserId = sender.UserId;

                NoticeService noticeService = Tunynet.DIContainer.Resolve<NoticeService>();
                Notice notice = Notice.New();

                notice.UserId = toUserId;
                notice.ApplicationId = CmsConfig.Instance().ApplicationId;
                notice.TypeId = NoticeTypeIds.Instance().Manage();
                notice.LeadingActorUserId = sender.UserId;
                notice.LeadingActor = sender.Author;
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                notice.RelativeObjectId = sender.ContentItemId;
                notice.RelativeObjectName = sender.Title;
                notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().ContentItemDetail(sender.ContentItemId));
                notice.TemplateName = NoticeTemplateNames.Instance().ContributeAccepted();
                noticeService.Create(notice);
            }
        }

        /// <summary>
        /// 评论资讯动态处理程序
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="eventArgs"></param>
        private void CmsCommentActivityEventModule_After(Comment comment, AuditEventArgs eventArgs)
        {
            NoticeService noticeService = new NoticeService();
            ContentItem contentItem = null;

            if (comment.TenantTypeId == TenantTypeIds.Instance().ContentItem())
            {
                //生成动态
                ActivityService activityService = new ActivityService();
                AuditService auditService = new AuditService();
                bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
                if (auditDirection == true)
                {
                    //创建评论的动态[关注评论者的粉丝可以看到该评论]
                    Activity activity = Activity.New();
                    activity.ActivityItemKey = ActivityItemKeys.Instance().CreateCmsComment();
                    activity.ApplicationId = CmsConfig.Instance().ApplicationId;

                    ContentItemService contentItemService = new ContentItemService();
                    contentItem = contentItemService.Get(comment.CommentedObjectId);
                    if (contentItem == null || contentItem.UserId == comment.UserId)
                    {
                        return;
                    }
                    activity.IsOriginalThread = true;
                    activity.IsPrivate = false;
                    activity.OwnerId = comment.UserId;
                    activity.OwnerName = comment.Author;
                    activity.OwnerType = ActivityOwnerTypes.Instance().User();
                    activity.ReferenceId = contentItem.ContentItemId;
                    activity.ReferenceTenantTypeId = TenantTypeIds.Instance().ContentItem();
                    activity.SourceId = comment.Id;
                    activity.TenantTypeId = TenantTypeIds.Instance().Comment();
                    activity.UserId = comment.UserId;

                    activityService.Generate(activity, false);
                }
                else if (auditDirection == false)
                {
                    activityService.DeleteSource(TenantTypeIds.Instance().Comment(), comment.Id);
                }
            }
        }

    }
}