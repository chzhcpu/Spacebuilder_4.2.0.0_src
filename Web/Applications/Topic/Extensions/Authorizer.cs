//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 扩展权限方法
    /// </summary>
    public static class AuthorizerTopicExtension
    {
        /// <summary>
        /// 是否具有创建Topic的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <returns></returns>
        public static bool Topic_Create(this Authorizer authorizer)
        {
            string errorMessage = string.Empty;
            return authorizer.Topic_Create(out errorMessage);
        }

        /// <summary>
        /// 是否具有创建Topic的权限
        /// </summary>        
        /// <param name="authorizer"></param>
        /// <param name="errorMessage">无权信息提示</param>
        /// <returns></returns>
        public static bool Topic_Create(this Authorizer authorizer, out string errorMessage)
        {
            errorMessage = string.Empty;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                errorMessage = "您需要先登录，才能创建专题";
                return false;
            }

            if (authorizer.IsAdministrator(TopicConfig.Instance().ApplicationId))
                return true;

            if (currentUser.Rank < TopicConfig.Instance().MinUserRankOfCreateTopic)
            {
                errorMessage = string.Format("只有等级达到{0}级，才能创建专题，您现在的等级是{1}", TopicConfig.Instance().MinUserRankOfCreateTopic, currentUser.Rank);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否拥有设置管理员权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="groupId">帖群id</param>
        /// <returns>是否拥有设置管理员的权限</returns>
        public static bool Topic_SetManager(this Authorizer authorizer, long groupId)
        {
            return authorizer.Topic_SetManager(new TopicService().Get(groupId));
        }

        /// <summary>
        /// 是否具有设置群管理员的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool Topic_SetManager(this Authorizer authorizer, TopicEntity group)
        {
            if (group == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            //群主
            if (group.UserId == currentUser.UserId)
                return true;

            if (authorizer.IsAdministrator(TopicConfig.Instance().ApplicationId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有管理Topic的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool Topic_Manage(this Authorizer authorizer, long groupId)
        {
            TopicEntity group = new TopicService().Get(groupId);
            return Topic_Manage(authorizer, group);
        }

        /// <summary>
        /// 是否具有删除访客记录的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool Topic_DeleteVisitor(this Authorizer authorizer, long groupId, long userId)
        {
            bool result = false;
            TopicEntity group = new TopicService().Get(groupId);
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserId == userId)
            {
                result = true;
            }
            else
            {
                result = Topic_Manage(authorizer, group);
            }
            return result;
        }
        /// <summary>
        /// 是否拥有删除专题成员的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="group"></param>
        /// <param name="userId">被删除的用户Id</param>
        /// <returns>是否拥有删除专题成员的权限</returns>
        public static bool Topic_DeleteMember(this Authorizer authorizer, TopicEntity group, long userId)
        {
            if (group == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            //群主
            if (group.UserId == currentUser.UserId)
                return true;

            if (authorizer.IsAdministrator(TopicConfig.Instance().ApplicationId))
                return true;
            TopicService groupService = new TopicService();
            //群管理员
            if (groupService.IsManager(group.TopicId, currentUser.UserId) && !groupService.IsManager(group.TopicId, userId))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 是否具有删除专题动态的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool Topic_DeleteTopicActivity(this Authorizer authorizer, Activity activity)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            if (authorizer.Topic_Manage(activity.OwnerId))
                return true;
            if (currentUser.UserId == activity.UserId)
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有管理Topic的权限
        /// </summary>
        /// <param name="Topic"></param>
        /// <returns></returns>
        public static bool Topic_Manage(this Authorizer authorizer, TopicEntity group)
        {


            if (group == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (authorizer.IsAdministrator(TopicConfig.Instance().ApplicationId))
                return true;

            if (currentUser.IsContentAdministrator())
                return true;

            //群主
            if (group.UserId == currentUser.UserId)
                return true;

            TopicService groupService = new TopicService();
            //群管理员
            if (groupService.IsManager(group.TopicId, currentUser.UserId))
                return true;

            return false;
        }


        /// <summary>
        /// 是否具有邀请好友加入专题的权限
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool Topic_Invite(this Authorizer authorizer, TopicEntity group)
        {
            if (group == null)
                return false;
            if (UserContext.CurrentUser == null)
                return false;

            TopicService groupService = new TopicService();
            if (authorizer.Topic_Manage(group))
                return true;
            if (group.EnableMemberInvite && groupService.IsMember(group.TopicId, UserContext.CurrentUser.UserId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有浏览专题的权限
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static bool Topic_View(this Authorizer authorizer, TopicEntity topic)
        {
            if (topic == null)
                return false;

            if (topic.AuditStatus == AuditStatus.Success)
            {
                if (topic.IsPublic)
                    return true;

                if (UserContext.CurrentUser == null)
                    return false;

                if (authorizer.Topic_Manage(topic))
                    return true;

                TopicService groupService = new TopicService();
                if (groupService.IsMember(topic.TopicId, UserContext.CurrentUser.UserId))
                    return true;
            }

            if (authorizer.IsAdministrator(TopicConfig.Instance().ApplicationId))
                return true;

            return false;
        }

    }
}