//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Spacebuilder.Common;
using Tunynet.Utilities;
using Tunynet.Common;
using Tunynet;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题链接管理
    /// </summary>
    public static class SiteUrlsTopicExtension
    {
        private static readonly string TopicAreaName = TopicConfig.Instance().ApplicationKey;

        /// <summary>
        /// 频道专题首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string ChannelTopicHome(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Home", "ChannelTopic", TopicAreaName);
        }

        
        /// <summary>
        /// 创建专题
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string CreateTopic(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Create", "ChannelTopic", TopicAreaName);
        }
        #region 专题频道

        /// <summary>
        /// 用户加入专题（专题无验证时）
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string JoinTopic(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("JoinTopic", "ChannelTopic", TopicAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        /// <summary>
        /// 退出专题
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        public static string _QuitTopic(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("_QuitTopic", "ChannelTopic", TopicAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        /// <summary>
        /// 用户加入专题（专题有验证时）
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string _EditApplyOfTopic(this SiteUrls siteUrls, long topicId)
        {
            return CachedUrlHelper.Action("_EditApply", "ChannelTopic", TopicAreaName, new RouteValueDictionary { { "topicId", topicId } });
        }

        /// <summary>
        /// 用户加入专题（通过问题验证）
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string _ValidateQuestionOfTopic(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("_ValidateQuestion", "ChannelTopic", TopicAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        #endregion

        #region 专题空间

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        public static string _GetNewerTopicActivities(this SiteUrls siteUrls, string spaceKey, int? applicationId = null, long? lastActivityId = 0)
        {
            return CachedUrlHelper.Action("_GetNewerActivities", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "applicationId", applicationId }, { "lastActivityId", lastActivityId } });
        }

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        /// <returns></returns>
        public static string GetNewerTopicActivityCount(this SiteUrls siteUrls, string spaceKey, int? applicationId = null)
        {
            return CachedUrlHelper.Action("GetNewerTopicActivityCount", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "applicationId", applicationId } });
        }

        public static string _ListActivitiesOfTopic(this SiteUrls siteUrls, string spaceKey, int? pageIndex = 1, int? applicationId = null, MediaType? mediaType = null, bool? isOriginal = null, long? userId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (pageIndex.Value > 1)
            {
                dic.Add("pageIndex", pageIndex);
            }
            if (applicationId != null)
            {
                dic.Add("applicationId", applicationId);
            }
            if (mediaType != null)
            {
                dic.Add("mediaType", mediaType);
            }
            if (isOriginal != null)
            {
                dic.Add("isOriginal", isOriginal);
            }
            if (userId != null)
            {
                dic.Add("userId", userId);
            }
            return CachedUrlHelper.Action("_ListActivities", "TopicSpace", TopicAreaName, dic);
        }

        /// <summary>
        /// 删除专题动态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _DeleteTopicActivity(this SiteUrls siteUrls, string spaceKey, long activityId)
        {
            return CachedUrlHelper.Action("_DeleteTopicActivity", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "activityId", activityId } });
        }

        /// <summary>
        /// 专题空间首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string TopicHome(this SiteUrls siteUrls, long groupId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", TopicIdToTopicKeyDictionary.GetTopicKey(groupId));
            return CachedUrlHelper.Action("Home", "TopicSpaceTheme", TopicAreaName, dic);
        }


        /// <summary>
        /// 专题空间首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string TopicHome(this SiteUrls siteUrls, string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("Home", "TopicSpaceTheme", TopicAreaName, dic);
        }

        /// <summary>
        /// 设置公告
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _EditAnnouncementOfTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_EditAnnouncement", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除访客记录
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string DeleteTopicVisitor(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteTopicVisitor", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 专题资料
        /// </summary>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        public static string _TopicProfile(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_TopicProfile", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static string BatchRemoveMemberOfTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteMember", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 批量处理申请
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public static string BatchUpdateMemberAuditStatusOfTopic(this SiteUrls siteUrls, string spaceKey, bool isApproved)
        {
            return CachedUrlHelper.Action("ApproveMemberApply", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "isApproved", isApproved } });
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="id">申请Id</param>
        /// <returns>删除申请链接</returns>
        public static string DeleteMemberApplyOfTopic(this SiteUrls siteUrls, string spaceKey, long id)
        {
            return CachedUrlHelper.Action("DeleteMemberApply", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "id", id } });
        }

        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _InviteOfTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Invite", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 成员管理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static string ManageMembersOfTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("ManageMembers", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 申请处理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="groupId"></param>
        /// <param name="applyStatus"></param>
        /// <returns></returns>
        public static string ManageMemberAppliesOfTopic(this SiteUrls siteUrls, string spaceKey, TopicMemberApplyStatus? applyStatus = null)
        {
            RouteValueDictionary route = new RouteValueDictionary();
            route.Add("spaceKey", spaceKey);
            if (applyStatus != null)
                route.Add("applyStatus", applyStatus);
            return CachedUrlHelper.Action("ManageMemberApplies", "TopicSpaceSettings", TopicAreaName, route);
        }

        /// <summary>
        /// 管理员和普通成员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string MembersOfTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Members", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 关注的成员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string MyFollowedUsersOfTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("MyFollowedUsers", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string DeleteManagerOfTopic(this SiteUrls siteUrls, string spaceKey, long userId)
        {
            return CachedUrlHelper.Action("DeleteManager", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "userId", userId } });
        }

        #endregion

        #region 专题空间设置

        /// <summary>
        /// 删除专题logo
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey">专题标识</param>
        /// <returns></returns>
        public static string _DeleteTopicLogo(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_DeleteTopicLogo", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑专题
        /// </summary>
        /// <returns></returns>
        public static string EditTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("EditTopic", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }
        #endregion

        /// <summary>
        /// 用户专题页
        /// </summary>
        /// <param name="siteUrls"></param>
        public static string _CreateTopic(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_CreateTopic", "ChannelTopic", TopicAreaName);
        }

        /// <summary>
        /// 专题地区导航内容块
        /// </summary>
        /// <returns></returns>
        public static string _AreaTopics(this SiteUrls siteUrls, long topNumber = 5, string areaCode = null, long? categoryId = null, SortBy_Topic? sortBy = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(areaCode))
                routeValueDictionary.Add("areaCode", areaCode);
            if (categoryId.HasValue && categoryId.Value > 0)
                routeValueDictionary.Add("categoryId", categoryId);
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (topNumber != 0)
            {
                routeValueDictionary.Add("topNumber", topNumber);
            }
            return CachedUrlHelper.Action("_AreaTopics", "ChannelTopic", TopicAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 发现专题
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="nameKeyword"></param>
        /// <param name="areaCode"></param>
        /// <param name="categoryId"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public static string FindTopic(this SiteUrls siteUrls, string areaCode = null, long? categoryId = null, SortBy_Topic? sortBy = null, string nameKeyword = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(areaCode))
                routeValueDictionary.Add("areaCode", areaCode);
            if (categoryId.HasValue && categoryId.Value > 0)
                routeValueDictionary.Add("categoryId", categoryId);
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (!string.IsNullOrEmpty(nameKeyword))
                routeValueDictionary.Add("nameKeyword", WebUtility.UrlEncode(nameKeyword));
            return CachedUrlHelper.Action("FindTopic", "ChannelTopic", TopicAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 标签下的专题
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="tagName"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public static string ListByTagOfTopic(this SiteUrls siteUrls, string tagName, SortBy_Topic? sortBy = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (sortBy.HasValue)
            {
                routeValueDictionary.Add("sortBy", sortBy);
            }
            routeValueDictionary.Add("tagName", tagName);
            return CachedUrlHelper.Action("ListByTag", "ChannelTopic", TopicAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 用户加入的专题
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string UserJoinedTopics(this SiteUrls siteUrls, string spaceKey = null, int pageIndex = 1, bool isGetMore = false)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                dic.Add("spaceKey", spaceKey);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            if (isGetMore)
                dic.Add("isGetMore", isGetMore);
            return CachedUrlHelper.Action("UserJoinedTopics", "ChannelTopic", TopicAreaName, dic);
        }
        /// <summary>
        /// 用户创建的专题
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string UserCreatedTopics(this SiteUrls siteUrls, string spaceKey = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
            {
                dic.Add("spaceKey", spaceKey);
            }
            return CachedUrlHelper.Action("UserCreatedTopics", "ChannelTopic", TopicAreaName, dic);
        }
        /// <summary>
        /// 标签云图
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string TopicTagMap(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("TopicTagMap", "ChannelTopic", TopicAreaName);
        }
        #region 专题搜索
        /// <summary>
        /// 专题全局搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string TopicGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "ChannelTopic", TopicAreaName);
        }

        /// <summary>
        /// 专题快捷搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string TopicQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "ChannelTopic", TopicAreaName);
        }

        /// <summary>
        /// 专题搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string TopicPageSearch(this SiteUrls siteUrls, string keyword = "", string areaCode = "")
        {
            keyword = WebUtility.UrlEncode(keyword);
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                dic.Add("NowAreaCode", areaCode);
            }
            return CachedUrlHelper.Action("Search", "ChannelTopic", TopicAreaName, dic);
        }

        /// <summary>
        /// 专题搜索自动完成
        /// </summary>
        public static string TopicSearchAutoComplete(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SearchAutoComplete", "ChannelTopic", TopicAreaName);
        }
        #endregion

        #region 管理页面-后台

        public static string ManageTopics(this SiteUrls siteUrls, AuditStatus? auditStatus = null)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (auditStatus.HasValue)
                dictionary.Add("auditStatus", auditStatus);
            return CachedUrlHelper.Action("ManageTopics", "ControlPanelTopic", TopicAreaName, dictionary);
        }

        /// <summary>
        /// 批量设置专题的审核状态（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateTopicAuditStatus(this SiteUrls siteUrls, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("BatchUpdateTopicAuditStatus", "ControlPanelTopic", TopicAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 设置专题的审核状态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateTopicAuditStatu(this SiteUrls siteUrls, long groupId, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("groupId", groupId);
            routeValueDictionary.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("BatchUpdateTopicAuditStatu", "ControlPanelTopic", TopicAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除专题
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="groupId">专题Id</param>
        /// <returns>删除专题的链接</returns>
        public static string DeleteTopic(this SiteUrls siteUrls, long groupId)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            dictionary.Add("groupId", groupId);
            return CachedUrlHelper.Action("DeleteTopic", "ControlPanelTopic", TopicAreaName, dictionary);
        }

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <returns></returns>
        public static string _ChangeTopicOwner(this SiteUrls siteUrls, string spaceKey, string returnUrl)
        {
            return CachedUrlHelper.Action("_ChangeTopicOwner", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "returnUrl", WebUtility.UrlEncode(returnUrl) } });
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="Id">申请ID</param>
        /// <returns></returns>
        public static string DeleteMemberApply(this SiteUrls siteUrls, long id, string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteMemberApply", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "id", id } });
        }



        /// <summary>
        ///  设置/取消 专题管理员
        /// </summary>
        /// <param name="groupId">专题Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="isManager">是/否管理员</param>
        /// <returns></returns>
        public static string SetManagerOfTopic(this SiteUrls siteUrls, long userId, bool isManager, string spaceKey)
        {
            return CachedUrlHelper.Action("SetManager", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "userId", userId }, { "isManager", isManager } });
        }

        #endregion

        #region 屏蔽专题

        /// <summary>
        /// 屏蔽用户专题页面
        /// </summary>
        /// <param name="spaceKey">用户空间名</param>
        /// <param name="blockTopicIds">被屏蔽的专题名</param>
        /// <returns>屏蔽用户专题链接</returns>
        public static string BlockTopics1(this SiteUrls siteUrls, string spaceKey, string blockTopicIds = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (!string.IsNullOrEmpty(blockTopicIds))
                routeValueDictionary.Add("blockTopicIds", blockTopicIds);
            return CachedUrlHelper.Action("BlockTopics", "UserSpaceSettings", "Common"/*CommonAreaName*/, routeValueDictionary);
        }

        #endregion

        #region 文章
        /*public static string PapersOfTopic(this SiteUrls siteUrls,long topicId)
        {
            return CachedUrlHelper.Action("_PapersOfTopic", "TopicSpace", TopicAreaName, new RouteValueDictionary { { "topicId", topicId } });
        }
        */
        public static string BatchAddPapers(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("BatchAddPapers", "TopicSpaceSettings", TopicAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }
        #endregion

    }

}
