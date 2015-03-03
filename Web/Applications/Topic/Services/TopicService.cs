//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using System.IO;
using Tunynet;
using Tunynet.Events;
using Spacebuilder.Common;

namespace SpecialTopic.Topic
{


    /// <summary>
    /// 专题业务逻辑
    /// </summary>
    public class TopicService
    {
        private ITopicRepository topicRepository = null;
        private ITopicMemberRepository TopicMemberRepository = null;
        private ITopicMemberApplyRepository TopicMemberApplyRepository = null;
        private AuditService auditService = new AuditService();

        /// <summary>
        /// 构造函数
        /// </summary>
        public TopicService()
            : this(new TopicRepository(), new TopicMemberRepository(), new TopicMemberApplyRepository())
        {
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        ///<param name="TopicMemberApplyRepository">专题成员申请仓储</param>
        ///<param name="TopicMemberRepository">专题成员仓储</param>
        ///<param name="groupRepository">专题仓储</param>
        /// <param name="TopicRepository"></param>
        public TopicService(ITopicRepository groupRepository, ITopicMemberRepository TopicMemberRepository, ITopicMemberApplyRepository TopicMemberApplyRepository)
        {
            this.topicRepository = groupRepository;
            this.TopicMemberRepository = TopicMemberRepository;
            this.TopicMemberApplyRepository = TopicMemberApplyRepository;
        }

        #region 维护专题



        /// <summary>
        /// 创建专题
        /// </summary>
        /// <param name="userId">当前操作人</param>
        /// <param name="group"><see cref="TopicEntity"/></param>
        /// <param name="logoFile">专题标识图</param>
        /// <returns>创建成功返回true，失败返回false</returns>
        public bool Create(long userId, TopicEntity group)
        {
            //设计要点
            //1、使用AuditService设置审核状态；
            //2、需要触发的事件参见《设计说明书-日志》     
            //3、单独调用标签服务设置标签
            //4、使用 IdGenerator.Next() 生成TopicId
            EventBus<TopicEntity>.Instance().OnBefore(group, new CommonEventArgs(EventOperationType.Instance().Create()));
            //设置审核状态
            auditService.ChangeAuditStatusForCreate(userId, group);
            long id = 0;
            long.TryParse(topicRepository.Insert(group).ToString(), out id);

            if (id > 0)
            {
                EventBus<TopicEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Create()));
                EventBus<TopicEntity, AuditEventArgs>.Instance().OnAfter(group, new AuditEventArgs(null, group.AuditStatus));
                //用户的创建专题数+1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(group.UserId, OwnerDataKeys.Instance().CreatedTopicCount(), 1);
            }
            return id > 0;
        }

        /// <summary>
        /// 更新专题
        /// </summary>
        /// <param name="userId">当前操作人</param>
        /// <param name="topic"><see cref="TopicEntity"/></param>
        /// <param name="logoFile">专题标识图</param>
        public void Update(long userId, TopicEntity topic)
        {
            EventBus<TopicEntity>.Instance().OnBefore(topic, new CommonEventArgs(EventOperationType.Instance().Update()));
            auditService.ChangeAuditStatusForUpdate(userId, topic);
            topicRepository.Update(topic);



            EventBus<TopicEntity>.Instance().OnAfter(topic, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="isUseCustomStyle"></param>
        /// <param name="themeAppearance"></param>
        public void ChangeThemeAppearance(long groupId, bool isUseCustomStyle, string themeAppearance)
        {
            topicRepository.ChangeThemeAppearance(groupId, isUseCustomStyle, themeAppearance);
        }

        /// <summary>
        /// 删除专题
        /// </summary>
        /// <param name="groupId">专题Id</param>
        public void Delete(long groupId)
        {
            //设计要点
            //1、需要删除：专题成员、专题申请、Logo；
            TopicEntity group = topicRepository.Get(groupId);
            if (group == null)
                return;

            CategoryService categoryService = new CategoryService();
            categoryService.ClearCategoriesFromItem(groupId, null, TenantTypeIds.Instance().Topic());


            EventBus<TopicEntity>.Instance().OnBefore(group, new CommonEventArgs(EventOperationType.Instance().Delete()));
            int affectCount = topicRepository.Delete(group);
            if (affectCount > 0)
            {
                //删除访客记录
                new VisitService(TenantTypeIds.Instance().Topic()).CleanByToObjectId(groupId);
                //用户的创建专题数-1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(group.UserId, OwnerDataKeys.Instance().CreatedTopicCount(), -1);
                //删除Logo             
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Topic());
                logoService.DeleteLogo(groupId);
                //删除专题下的成员
                DeleteMembersByTopicId(groupId);
                EventBus<TopicEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Delete()));
                EventBus<TopicEntity, AuditEventArgs>.Instance().OnAfter(group, new AuditEventArgs(group.AuditStatus, null));
            }
        }

        /// <summary>
        /// 删除专题下的成员
        /// </summary>
        /// <param name="groupId"></param>
        public void DeleteMembersByTopicId(long groupId)
        {
            IEnumerable<TopicMember> TopicMembers = TopicMemberRepository.GetAllMembersOfTopic(groupId);
            foreach (var TopicMember in TopicMembers)
            {
                int affectCount = TopicMemberRepository.Delete(TopicMember);
                if (affectCount > 0)
                {
                    EventBus<TopicMember>.Instance().OnAfter(TopicMember, new CommonEventArgs(EventOperationType.Instance().Delete()));
                    //用户的参与专题数-1
                    OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                    ownerDataService.Change(TopicMember.UserId, OwnerDataKeys.Instance().JoinedTopicCount(), -1);
                }
            }
        }



        /// <summary>
        /// 发送加入专题邀请
        /// </summary>
        /// <param name="group"><see cref="TopicEntity"/></param>
        /// <param name="sender">发送人</param>
        /// <param name="userIds">邀请接收人</param>
        /// <param name="remark">附言</param>
        public void SendInvitations(TopicEntity group, IUser sender, string remark, IEnumerable<long> userIds)
        {
            //调用InvitationService的发送请求的方法
            InvitationService invitationService = new InvitationService();
            foreach (var userId in userIds)
            {
                if (!IsMember(group.TopicId, userId))
                {
                    Invitation invitation = Invitation.New();
                    invitation.ApplicationId = TopicConfig.Instance().ApplicationId;
                    invitation.InvitationTypeKey = InvitationTypeKeys.Instance().InviteJoinTopic();
                    invitation.UserId = userId;
                    invitation.SenderUserId = sender.UserId;
                    invitation.Sender = sender.DisplayName;
                    invitation.SenderUrl = SiteUrls.Instance().SpaceHome(sender.UserId);
                    invitation.RelativeObjectId = group.TopicId;
                    invitation.RelativeObjectName = group.TopicName;
                    invitation.RelativeObjectUrl = SiteUrls.Instance().TopicHome(group.TopicKey);
                    invitation.Remark = remark;
                    invitationService.Create(invitation);
                }
            }
        }

        /// <summary>
        /// 批准/不批准专题
        /// </summary>
        /// <param name="groupId">待被更新的专题Id</param>
        /// <param name="isApproved">是否通过审核</param>
        public void Approve(long groupId, bool isApproved)
        {
            //设计要点
            //1、审核状态未变化不用进行任何操作；
            //2、需要触发的事件参见《设计说明书-专题》；

            TopicEntity group = topicRepository.Get(groupId);

            AuditStatus newAuditStatus = isApproved ? AuditStatus.Success : AuditStatus.Fail;
            if (group.AuditStatus == newAuditStatus)
                return;

            AuditStatus oldAuditStatus = group.AuditStatus;
            group.AuditStatus = newAuditStatus;
            topicRepository.Update(group);

            string operationType = isApproved ? EventOperationType.Instance().Approved() : EventOperationType.Instance().Disapproved();
            EventBus<TopicEntity>.Instance().OnAfter(group, new CommonEventArgs(operationType));
            EventBus<TopicEntity, AuditEventArgs>.Instance().OnAfter(group, new AuditEventArgs(oldAuditStatus, newAuditStatus));
        }

        /// <summary>
        /// 批量批准/不批准专题
        /// </summary>
        /// <param name="groupIds">待处理的专题Id列表</param>
        /// <param name="isApproved">是否通过审核</param>
        public void Approve(IEnumerable<long> groupIds, bool isApproved)
        {
            //设计要点
            //1、审核状态未变化不用进行任何操作；
            //2、需要触发的事件参见《设计说明书-专题》；

            foreach (var threadId in groupIds)
            {
                Approve(threadId, isApproved);
            }
        }

        /// <summary>
        /// 更新专题公告
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="announcement">公告内容</param>
        public void UpdateAnnouncement(long groupId, string announcement)
        {
            TopicEntity group = topicRepository.Get(groupId);
            if (group == null)
                return;
            group.Announcement = announcement;
            topicRepository.Update(group);
        }


        /// <summary>
        /// 上传Logo
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="stream">Logo文件流</param>
        public void UploadLogo(long groupId, Stream stream)
        {
            //按现在设计应该用LogoService，但是感觉LogoService没什么必要（重构Logo/Image直连后再定）
            if (stream != null)
            {
                TopicEntity group = this.Get(groupId);
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Topic());
                group.Logo = logoService.UploadLogo(groupId, stream);
                topicRepository.Update(group);
                EventBus<TopicEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Update()));
            }
        }

        /// <summary>
        /// 删除Logo
        /// </summary>
        /// <param name="recommendId">专题Id</param>
        public void DeleteLogo(long groupId)
        {
            LogoService logoService = new LogoService(TenantTypeIds.Instance().Topic());
            logoService.DeleteLogo(groupId);
            TopicEntity group = Get(groupId);
            if (group == null)
                return;
            group.Logo = string.Empty;
            topicRepository.Update(group);
        }

        #endregion


        #region 获取专题

        /// <summary>
        /// 通过TopicKey获取专题
        /// </summary>
        /// <param name="groupKey">专题标识</param>
        /// <returns></returns>
        public TopicEntity Get(string groupKey)
        {
            long groupId = TopicIdToTopicKeyDictionary.GetTopicId(groupKey);
            return this.Get(groupId);
        }

        /// <summary>
        /// 获取专题
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns></returns>
        public TopicEntity Get(long groupId)
        {
            return topicRepository.Get(groupId);
        }

        /// <summary>
        /// 获取前N个排行专题
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetTops(int topNumber, string areaCode, long? categoryId, SortBy_Topic sortBy)
        {
            //设计要点
            //1、查询areaCode时需要包含后代地区；
            //2、查询categoryId时需要包含后代类别；
            //3、无需维护缓存即时性
            return topicRepository.GetTops(topNumber, areaCode, categoryId, sortBy);
        }

        /// <summary>
        /// 获取匹配的前N个排行专题
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetMatchTops(int topNumber, string keyword, string areaCode, long? categoryId, SortBy_Topic sortBy)
        {
            return topicRepository.GetMatchTops(topNumber, keyword, areaCode, categoryId, sortBy);
        }


        /// <summary>
        /// 分页获取排行数据
        /// </summary>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<TopicEntity> Gets(string areaCode, long? categoryId, SortBy_Topic sortBy, int pageSize = 20, int pageIndex = 1)
        {
            //无需维护缓存即时性
            return topicRepository.Gets(areaCode, categoryId, sortBy, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据标签名获取专题分页集合
        /// </summary>
        /// <param name="tagName">标签名</param></param>
        /// <param name="sortBy">排序依据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页列表</returns>
        public PagingDataSet<TopicEntity> GetsByTag(string tagName, SortBy_Topic sortBy, int pageSize = 20, int pageIndex = 1)
        {
            //无需维护缓存即时性
            return topicRepository.GetsByTag(tagName, sortBy, pageSize, pageIndex);
        }




        /// <summary>
        /// 获取用户创建的专题列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetMyCreatedTopics(long userId, bool ignoreAudit)
        {
            //需维护缓存即时性
            return topicRepository.GetMyCreatedTopics(userId, ignoreAudit);
        }

        /// <summary>
        /// 获取用户加入的专题列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<TopicEntity> GetMyJoinedTopics(long userId, int pageSize = 20, int pageIndex = 1)
        {
            //需维护缓存即时性
            return topicRepository.GetMyJoinedTopics(userId, pageSize, pageIndex);
        }

        /// <summary>
        /// 专题成员也加入的专题
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> TopicMemberAlsoJoinedTopics(long groupId, int topNumber)
        {
            //设计要点：
            //1、获取专题成员也加入的其他不重复的专题，按专题成长值倒序；
            //2、无需维护缓存即时性，缓存期限：常用集合
            return topicRepository.TopicMemberAlsoJoinedTopics(groupId, topNumber);
        }


        /// <summary>
        /// 获取我关注的用户加入的专题
        /// </summary>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> FollowedUserAlsoJoinedTopics(long userId, int topNumber)
        {
            return topicRepository.FollowedUserAlsoJoinedTopics(userId, topNumber);
        }

        /// <summary>
        /// 分页获取专题后台管理列表
        /// </summary>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="keywords">名称关键字</param>
        /// <param name="ownerUserId">群主</param>
        /// <param name="minDateTime">创建时间下限值</param>
        /// <param name="maxDateTime">创建时间上限值</param>
        /// <param name="minMemberCount">成员数量下限值</param>
        /// <param name="maxMemberCount">成员数量上限值</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<TopicEntity> GetsForAdmin(AuditStatus? auditStatus, long? categoryId, string keywords, long? ownerUserId, DateTime? minDateTime, DateTime? maxDateTime, int? minMemberCount, int? maxMemberCount, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点
            //1、查询categoryId时需要包含后代类别；
            //2、不用缓存
            return topicRepository.GetsForAdmin(auditStatus, categoryId, keywords, ownerUserId, minDateTime, maxDateTime, minMemberCount, maxMemberCount, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据专题ID集合获取专题集合
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public IEnumerable<TopicEntity> GetTopicEntitiesByIds(IEnumerable<long> groupIds)
        {
            return topicRepository.PopulateEntitiesByEntityIds(groupIds);
        }

        /// <summary>
        /// 根据审核状态获取专题数
        /// </summary>
        /// <param name="Pending">待审核</param>
        /// <param name="Again">需再审核</param>
        /// <returns></returns>
        public Dictionary<TopicManageableCountType, int> GetManageableCounts()
        {
            return topicRepository.GetManageableCounts();
        }

        #endregion


        #region 专题申请



        /// <summary>
        /// 用户是否申请过加入专题，并且申请处于待处理状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool IsApplied(long userId, long groupId)
        {
            var groupIds = TopicMemberApplyRepository.GetPendingApplyTopicIdsOfUser(userId);
            return groupIds.Contains(groupId);
        }

        /// <summary>
        /// 申请加入专题
        /// </summary>
        /// <param name="TopicMemberApply">专题加入申请</param>
        public void CreateTopicMemberApply(TopicMemberApply TopicMemberApply)
        {
            //设计要点：
            //1、用户对同一个专题不允许有多个待处理的加入申请


            if (TopicMemberApply == null)
                return;
            if (IsApplied(TopicMemberApply.UserId, TopicMemberApply.TopicId))
                return;
            long id = 0;
            long.TryParse(TopicMemberApplyRepository.Insert(TopicMemberApply).ToString(), out id);

            if (id > 0)
                EventBus<TopicMemberApply>.Instance().OnAfter(TopicMemberApply, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 接受/拒绝专题加入申请
        /// </summary>
        /// <param name="groupIds">申请Id列表</param>
        /// <param name="isApproved">是否接受</param>
        public void ApproveTopicMemberApply(IEnumerable<long> applyIds, bool isApproved)
        {
            //设计要点：
            //1、仅允许对待处理状态的申请变更状态；
            //2、通过批准的申请，直接创建TopicMember
            IEnumerable<TopicMemberApply> TopicMemberApplies = TopicMemberApplyRepository.PopulateEntitiesByEntityIds(applyIds);
            TopicMemberApplyStatus applyStatus = isApproved ? TopicMemberApplyStatus.Approved : TopicMemberApplyStatus.Disapproved;
            string operationType = isApproved ? EventOperationType.Instance().Approved() : EventOperationType.Instance().Disapproved();

            foreach (var apply in TopicMemberApplies)
            {
                if (apply.ApplyStatus != TopicMemberApplyStatus.Pending)
                    continue;



                apply.ApplyStatus = applyStatus;
                TopicMemberApplyRepository.Update(apply);



                EventBus<TopicMemberApply>.Instance().OnAfter(apply, new CommonEventArgs(operationType));

                if (isApproved)
                {
                    TopicMember member = TopicMember.New();
                    member.UserId = apply.UserId;
                    member.TopicId = apply.TopicId;
                    CreateTopicMember(member);
                }
            }
        }

        /// <summary>
        /// 删除专题加入申请
        /// </summary>
        /// <param name="id">申请Id</param>
        public void DeleteTopicMemberApply(long id)
        {
            TopicMemberApply apply = TopicMemberApplyRepository.Get(id);
            if (apply == null)
                return;

            int affectCount = TopicMemberApplyRepository.Delete(apply);
            if (affectCount > 0)
                EventBus<TopicMemberApply>.Instance().OnAfter(apply, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }


        /// <summary>
        /// 获取专题的加入申请列表
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="applyStatus">申请状态</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>加入申请分页数据</returns>
        public PagingDataSet<TopicMemberApply> GetTopicMemberApplies(long groupId, TopicMemberApplyStatus? applyStatus, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点：
            //1、排序：申请状态正序、申请时间倒序（Id代替）；            
            return TopicMemberApplyRepository.GetTopicMemberApplies(groupId, applyStatus, pageSize, pageIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns></returns>
        public int GetMemberApplyCount(long groupId)
        {
            return TopicMemberApplyRepository.GetMemberApplyCount(groupId);
        }

        #endregion


        #region 专题成员

        /// <summary>
        /// 增加专题成员
        /// </summary>
        /// <param name="TopicMember"></param>
        public void CreateTopicMember(TopicMember TopicMember)
        {
            //设计要点：
            //1、同一个专题不允许用户重复加入
            //2、群主不允许成为专题成员
            if (IsMember(TopicMember.TopicId, TopicMember.UserId))
                return;
            if (IsOwner(TopicMember.TopicId, TopicMember.UserId))
                return;
            long id = 0;
            long.TryParse(TopicMemberRepository.Insert(TopicMember).ToString(), out id);

            if (id > 0)
            {
                EventBus<TopicMember>.Instance().OnAfter(TopicMember, new CommonEventArgs(EventOperationType.Instance().Create()));
                //用户的参与专题数+1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(TopicMember.UserId, OwnerDataKeys.Instance().JoinedTopicCount(), 1);
            }
        }



        /// <summary>
        /// 移除专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        public void DeleteTopicMember(long groupId, long userId)
        {



            TopicMember TopicMember = TopicMemberRepository.GetMember(groupId, userId);
            if (TopicMember == null)
                return;

            int affectCount = TopicMemberRepository.Delete(TopicMember);
            if (affectCount > 0)
            {
                EventBus<TopicMember>.Instance().OnAfter(TopicMember, new CommonEventArgs(EventOperationType.Instance().Delete()));
                //用户的参与专题数-1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(userId, OwnerDataKeys.Instance().JoinedTopicCount(), -1);
            }
        }

        /// <summary>
        /// 批量移除专题成员
        /// </summary>
        /// <param name="userIds">待处理的成员用户Id列表</param>
        /// <param name="groupId">专题Id</param>
        public void DeleteTopicMember(long groupId, IEnumerable<long> userIds)
        {

            foreach (var userId in userIds)
            {
                DeleteTopicMember(groupId, userId);
            }
        }


        /// <summary>
        /// 设置/取消 专题管理员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="isManager">是否管理员</param>
        public bool SetManager(long groupId, long userId, bool isManager)
        {
            //设计要点：
            //1、userId必须是groupId成员
            TopicMember member = TopicMemberRepository.GetMember(groupId, userId);
            if (member == null)
                return false;
            if (member.IsManager == isManager)
                return false;
            member.IsManager = isManager;
            TopicMemberRepository.Update(member);
            if (isManager)
            {
                EventBus<TopicMember>.Instance().OnAfter(member, new CommonEventArgs(EventOperationType.Instance().SetTopicManager()));
            }
            else
            {
                EventBus<TopicMember>.Instance().OnAfter(member, new CommonEventArgs(EventOperationType.Instance().CancelTopicManager()));
            }
            return true;
        }

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="newOwnerUserId">新群主UserId</param>
        public void ChangeTopicOwner(long groupId, long newOwnerUserId)
        {
            //更换群主后，原群主转换成专题成员，如果新群主是专题成员则从成员中移除
            TopicEntity group = topicRepository.Get(groupId);
            long oldOwnerUserId = group.UserId;
            group.UserId = newOwnerUserId;
            topicRepository.ChangeTopicOwner(groupId, newOwnerUserId);

            //原群主的专题数-1，加入专题数+1
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            ownerDataService.Change(oldOwnerUserId, OwnerDataKeys.Instance().CreatedTopicCount(), -1);
            ownerDataService.Change(oldOwnerUserId, OwnerDataKeys.Instance().JoinedTopicCount(), 1);

            //原群主转换成专题成员
            TopicMember TopicMember = TopicMember.New();
            TopicMember.TopicId = groupId;
            TopicMember.UserId = oldOwnerUserId;
            TopicMemberRepository.Insert(TopicMember);

            //新群主的专题数+1,加入专题数-1
            ownerDataService.Change(newOwnerUserId, OwnerDataKeys.Instance().CreatedTopicCount(), 1);

            //如果新群主是专题成员则从成员中移除
            if (IsMember(groupId, newOwnerUserId))
                DeleteTopicMember(groupId, newOwnerUserId);
        }


        /// <summary>
        /// 是否专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是专题成员返回true，否则返回false</returns>
        public bool IsMember(long groupId, long userId)
        {
            TopicMemberRole role = GetTopicMemberRole(groupId, userId);
            if (role >= TopicMemberRole.Member)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否专题管理员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是专题管理员返回true，否则返回false</returns>
        public bool IsManager(long groupId, long userId)
        {
            TopicMemberRole role = GetTopicMemberRole(groupId, userId);
            if (role >= TopicMemberRole.Manager)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否群主
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是群主返回true，否则返回false</returns>
        public bool IsOwner(long groupId, long userId)
        {
            TopicMemberRole role = GetTopicMemberRole(groupId, userId);
            if (role == TopicMemberRole.Owner)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否群主
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是群主返回true，否则返回false</returns>
        public TopicMember GetTopicMember(long memberId)
        {
            return TopicMemberRepository.Get(memberId);
        }

        /// <summary>
        /// 检测用户在专题中属于什么角色
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns><see cref="TopicMemberRole"/></returns>
        private TopicMemberRole GetTopicMemberRole(long groupId, long userId)
        {
            //设计要点：
            //1、需要缓存，并维护缓存的即时性
            TopicMember member = TopicMemberRepository.GetMember(groupId, userId);
            if (member == null)
            {
                TopicEntity group = topicRepository.Get(groupId);
                if (group!=null && group.UserId == userId)
                    return TopicMemberRole.Owner;
                return TopicMemberRole.None;
            }
            if (member.IsManager)
                return TopicMemberRole.Manager;
            else
                return TopicMemberRole.Member;
        }

        /// <summary>
        /// 获取专题所有成员用户Id集合(用于推送动态）
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfTopic(long groupId)
        {
            TopicEntity group = topicRepository.Get(groupId);
            if (group == null)
                return new List<long>();
            //不必缓存
            IEnumerable<long> userIds = TopicMemberRepository.GetUserIdsOfTopic(groupId);
            var list = userIds.ToList();
            list.Add(group.UserId);
            return list;
        }

        /// <summary>
        /// 获取专题管理员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <returns>若没有找到，则返回空集合</returns>
        public IEnumerable<User> GetTopicManagers(long groupId)
        {
            //设计要点：
            //1、需要缓存，并维护缓存的即时性
            IEnumerable<long> manageIds = TopicMemberRepository.GetTopicManagers(groupId);
            return DIContainer.Resolve<UserService>().GetFullUsers(manageIds);
        }

        /// <summary>
        /// 获取专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="hasManager">是否包含管理员</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>专题成员分页数据</returns>
        public PagingDataSet<TopicMember> GetTopicMembers(long groupId, bool hasManager = true, SortBy_TopicMember sortBy = SortBy_TopicMember.DateCreated_Asc, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点：
            //1、排序：管理员排在前，其余按加入时间正序；
            //2、使用分区列表缓存
            return TopicMemberRepository.GetTopicMembers(groupId, hasManager, sortBy, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取我关注的用户中同时加入某个专题的专题成员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        public IEnumerable<TopicMember> GetTopicMembersAlsoIsMyFollowedUser(long groupId, long userId)
        {
            return TopicMemberRepository.GetTopicMembersAlsoIsMyFollowedUser(groupId, userId);
        }
        /// <summary>
        /// 在线专题成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<TopicMember> GetOnlineTopicMembers(long groupId)
        {
            return TopicMemberRepository.GetOnlineTopicMembers(groupId);
        }

        #endregion

        #region 删除用户数据



        /// <summary>
        /// 删除用户记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">被删除用户</param>
        /// <param name="takeOverUserName">接管用户名</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        public void DeleteUser(long userId, string takeOverUserName, bool takeOverAll)
        {
            //设计要点：
            //1.利用sql转移给接管用户、删除专题成员、专题成员申请；
            //2.删除专题成员时，维护专题的成员数；

            //如果没设置由谁接管专题，就把专题转给网站初始管理员
            long takeOverUserId = 0;
            if (string.IsNullOrEmpty(takeOverUserName))
            {
                takeOverUserId = new SystemDataService().GetLong("Founder");
            }
            else
            {
                takeOverUserId = UserIdToUserNameDictionary.GetUserId(takeOverUserName);
            }


            IUserService userService = DIContainer.Resolve<IUserService>();
            User takeOver = userService.GetFullUser(takeOverUserId);
            topicRepository.DeleteUser(userId, takeOver, takeOverAll);
        }

        #endregion

        /// <summary>
        /// 获取专题管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public Dictionary<string, long> GetManageableDatas(string tenantTypeId = null)
        {
            return topicRepository.GetManageableDatas(tenantTypeId);
        }

        /// <summary>
        /// 获取专题统计数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public Dictionary<string, long> GetStatisticDatas(string tenantTypeId = null)
        {
            return topicRepository.GetStatisticDatas();
        }

        public long GetTopicIdByTopicKey(string topicKey)
        {
            return topicRepository.GetTopicIdByTopicKey(topicKey);
        }
    }
}
