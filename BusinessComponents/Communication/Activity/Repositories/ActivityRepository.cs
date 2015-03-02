//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{

    /// <summary>
    /// 动态仓储
    /// </summary>
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        private int pageSize = 5;
        /// <summary>
        /// 最大返回记录数
        /// </summary>
        protected override int PrimaryMaxRecords
        {
            get
            {
                return 1000;
            }
        }

        #region 动态维护

        /// <summary>
        /// 创建拥有者动态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object Insert(Activity entity)
        {
            //6、仅保证操作者的收件箱缓存即时性；
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", entity.UserId);
            return base.Insert(entity);
        }

        /// <summary>
        /// 将动态加入到用户动态收件箱
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userIds"></param>
        public void InsertUserInboxs(long activityId, IEnumerable<long> userIds)
        {
            if (userIds == null)
                return;

            var activity = Get(activityId);
            if (activity == null)
                return;

            Database dao = CreateDAO();
            dao.OpenSharedConnection();
            Sql sql;
            List<long> activityIds = new List<long> { activityId };
            if (activity.ReferenceId > 0)
            {
                sql = Sql.Builder.Select("ActivityId").From("tn_Activities").Where("TenantTypeId=@0", activity.ReferenceTenantTypeId).Where("SourceId=@0", activity.ReferenceId);
                activityIds.AddRange(dao.Fetch<long>(sql).Distinct());
            }

            //若已存在，则不再添加
            List<Sql> sqls = new List<Sql>();
            List<long> updatedUserIds = new List<long>();
            foreach (var userId in userIds.Distinct())
            {
                sql = Sql.Builder.Select("count(*)").From("tn_ActivityUserInbox").Where("ActivityId in (@activityIds)", new { activityIds = activityIds }).Where("UserId=@0", userId);
                if (dao.FirstOrDefault<long>(sql) > 0)
                    continue;  //已经存在

                sql = Sql.Builder
                .Append(@"insert tn_ActivityUserInbox (UserId,ActivityId) values(@0,@1)", userId, activityId);

                sqls.Add(sql);
                updatedUserIds.Add(userId);
            }
            dao.Execute(sqls);
            dao.CloseSharedConnection();

            //更新缓存
            foreach (var userId in updatedUserIds)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            }
        }

        /// <summary>
        /// 将动态加入到站点动态收件箱
        /// </summary>
        /// <param name="activityId"></param>
        public void InsertSiteInbox(long activityId)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();
            Sql sql = Sql.Builder.Select("ActivityId").From("tn_ActivitySiteInbox").Where("ActivityId=@0", activityId);
            IEnumerable<long> activityIds = dao.Fetch<long>(sql);
            if (activityIds == null || activityIds.Count() == 0)
            {
                sql = Sql.Builder.Append("insert tn_ActivitySiteInbox(ActivityId) values(@0)", activityId);
                dao.Execute(sql);
            }
            dao.CloseSharedConnection();
        }

        /// <summary>
        /// 把动态最后更新时间设置为当前时间
        /// </summary>
        /// <param name="activityId"></param>
        public bool UpdateLastModified(long activityId)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();
            Activity activity = Get(activityId);
            Sql sql = Sql.Builder;
            sql.Append("Update tn_Activities Set LastModified=@0", DateTime.UtcNow)
            .Where("ActivityId=@0", activityId);
            int affectCount = dao.Execute(sql);
            List<long> userIds = null;
            if (affectCount > 0)
            {
                sql = Sql.Builder.Select("UserId").From("tn_ActivityUserInbox").Where("ActivityId=@0", activityId);
                userIds = dao.Fetch<long>(sql);
            }
            dao.CloseSharedConnection();

            if (affectCount == 0)
                return false;

            #region 更新缓存
            activity.LastModified = DateTime.UtcNow;
            this.OnUpdated(activity);
            //让拥有者的粉丝及时看到这条动态的更新
            foreach (var userId in userIds)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 把动态最后更新时间设置为当前时间
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="ownerType"></param>
        /// <param name="activityItemKey"></param>
        public bool CheckExistAndUpdateLastModified(long ownerId, int ownerType, string activityItemKey)
        {
            Sql sql = Sql.Builder;
            sql.Select("ActivityId").From("tn_Activities").Where("OwnerId=@0", ownerId).Where("OwnerType=@0", ownerType).Where("ActivityItemKey=@0", activityItemKey);

            object activityId = CreateDAO().FetchTopPrimaryKeys<Activity>(1, sql).FirstOrDefault();
            bool result = false;
            if (activityId != null)
                result = UpdateLastModified((long)activityId);

            return result;
        }

        /// <summary>
        /// 主体内容动态的最后更新时间设置为当前时间
        /// </summary>        
        /// <param name="tenantTypeId"></param>
        /// <param name="sourceId"></param>
        public bool UpdateLastModified(string tenantTypeId, long sourceId)
        {
            Database dao = CreateDAO();
            Sql sql = Sql.Builder;
            sql.Append("update tn_Activities set LastModified = @0 ", DateTime.UtcNow).Where("TenantTypeId=@0", tenantTypeId).Where("SourceId=@0", sourceId).Where("ReferenceId=0");
            return dao.Execute(sql) > 0;
        }

        /// <summary>
        /// 判断用户是否对同一主体内容产生过从属内容动态，如果产生过则替换成本次操作
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>true-更新成功，false-不存在OwnerType+TenantTypeId+ReferenceId的动态记录</returns>
        public bool CheckExistAndUpdateSource(Activity activity)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();
            Sql sql = Sql.Builder;
            sql.Select("ActivityId").From("tn_Activities").Where("OwnerId=@0", activity.OwnerId).Where("OwnerType=@0", activity.OwnerType).Where("UserId=@0", activity.UserId)
                .Where("TenantTypeId=@0", activity.TenantTypeId).Where("ReferenceId=@0", activity.ReferenceId).Where("ReferenceTenantTypeId=@0", activity.ReferenceTenantTypeId);

            long oldActivityId = dao.FirstOrDefault<long>(sql);
            int affectCount = 0;
            if (oldActivityId > 0)
            {
                sql = Sql.Builder.Append("Update tn_Activities Set SourceId=@0,DateCreated=@1,LastModified=@2", activity.SourceId, DateTime.UtcNow, DateTime.UtcNow)
                 .Where("ActivityId=@0", oldActivityId);
                affectCount = dao.Execute(sql);

                Activity oldActivity = Get(oldActivityId);
                oldActivity.SourceId = activity.SourceId;
                oldActivity.DateCreated = DateTime.UtcNow;
                oldActivity.LastModified = DateTime.UtcNow;
                this.OnUpdated(oldActivity);
            }

            dao.CloseSharedConnection();

            return oldActivityId > 0;
        }

        /// <summary>
        /// 更新动态的私有状态
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="isPrivate"></param>
        public void UpdatePrivateStatus(long activityId, bool isPrivate)
        {
            Activity activity = Get(activityId);
            if (activity == null)
                return;
            Sql sql = Sql.Builder
            .Append("update tn_Activities set IsPrivate=@0 where ActivityId=@1", isPrivate, activityId);
            activity.IsPrivate = isPrivate;
            this.OnUpdated(activity);
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 根据userid删除用户动态
        /// </summary>
        /// <param name="userId">用户的id</param>
        public void CleanByUser(long userId)
        {
            var sql_Delete = PetaPoco.Sql.Builder;
            sql_Delete.Append("delete from tn_Activities where UserId = @0", userId);
            sql_Delete.Append("delete from tn_ActivityUserInbox where UserId = @0", userId);
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 删除拥有者动态
        /// </summary>
        /// <param name="activityId"></param>
        public void DeleteActivity(long activityId)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();

            this.DeleteByEntityId(activityId);

            Sql sql = Sql.Builder
            .Append("delete from tn_ActivityUserInbox where ActivityId=@0 ", activityId);
            dao.Execute(sql);

            sql = Sql.Builder
            .Append("delete from tn_ActivitySiteInbox where ActivityId=@0 ", activityId);
            dao.Execute(sql);

            dao.CloseSharedConnection();
        }


        /// <summary>
        /// 删除动态源时删除动态
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="sourceId">动态源内容id</param>
        public void DeleteSource(string tenantTypeId, long sourceId)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();

            Sql sql = Sql.Builder.Select("ActivityId").From("tn_Activities")
            .Where("(TenantTypeId=@0 and sourceId=@1) or ( ReferenceTenantTypeId=@0 and ReferenceId=@1 )", tenantTypeId, sourceId);

            IEnumerable<object> activityIds = dao.FetchFirstColumn(sql);

            if (activityIds.Count() > 0)
            {
                IEnumerable<long> longActivityIds = activityIds.Cast<long>();

                sql = Sql.Builder.Append("delete from tn_ActivityUserInbox").Where("ActivityId in (@ActivityIds)", new { ActivityIds = longActivityIds });
                dao.Execute(sql);

                sql = Sql.Builder.Append("delete from tn_ActivitySiteInbox").Where("ActivityId in (@ActivityIds)", new { ActivityIds = longActivityIds });
                dao.Execute(sql);

                foreach (var activityId in longActivityIds)
                {
                    this.DeleteByEntityId(activityId);
                }
            }

            dao.CloseSharedConnection();
        }


        /// <summary>
        /// 从用户收件箱和站点收件箱移除动态
        /// </summary>
        /// <remarks>保留动态操作者的收件箱</remarks>
        /// <param name="activityId"></param>
        public void DeleteActivityFromUserInboxAndSiteInbox(long activityId, long userId)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();

            Sql sql = Sql.Builder
            .Append("delete from tn_ActivityUserInbox").Where("ActivityId=@0", activityId).Where("UserId<>@0", userId);
            dao.Execute(sql);

            sql = Sql.Builder
            .Append("delete from tn_ActivitySiteInbox where ActivityId=@0 ", activityId);
            dao.Execute(sql);
            dao.CloseSharedConnection();
        }

        /// <summary>
        /// 从用户收件箱移除动态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="activityId">动态Id</param>
        public void DeleteFromUserInbox(long userId, long activityId)
        {
            Sql sql = Sql.Builder.Append("delete from tn_ActivityUserInbox where UserId=@0 and ActivityId=@1 ", userId, activityId);
            CreateDAO().Execute(sql);
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 从站点收件箱移除动态
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFromSiteInbox(long activityId)
        {
            Sql sql = Sql.Builder.Append("delete from tn_ActivitySiteInbox where ActivityId=@0 ", activityId);
            CreateDAO().Execute(sql);

            RealTimeCacheHelper.IncreaseGlobalVersion();
        }

        /// <summary>
        /// 从用户动态收件箱移除OwnerId的所有动态
        /// </summary>
        /// <remarks>
        /// 取消关注/退出群组、屏蔽用户/屏蔽群组时使用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="ownerId">动态拥有者Id</param>
        public void RemoveInboxAboutOwner(long userId, long ownerId, int ownerType)
        {
            //缓存需要即时
            Sql sql = Sql.Builder
             .Append(@"delete from tn_ActivityUserInbox where ActivityId in 
                            (select ActivityId from tn_Activities where OwnerId=@0 and  OwnerType=@1 ) and UserId=@2", ownerId, ownerType, userId);
            int affectCount = CreateDAO().Execute(sql);
            if (affectCount > 0)
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 在用户动态收件箱追溯OwnerId的动态
        /// </summary>
        /// <remarks>
        /// 关注用户/加入群组、取消屏蔽用户/取消屏蔽群组时使用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="ownerId">动态拥有者Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="traceBackNumber">追溯OwnerId的动态数</param>
        public void TraceBackInboxAboutOwner(long userId, long ownerId, int ownerType, int traceBackNumber)
        {
            //缓存需要即时
            //1、仅追溯traceBackNumber条非私有的动态；

            Database dao = CreateDAO();
            dao.OpenSharedConnection();

            Sql sql = Sql.Builder
                .Select("ActivityId")
                .From("tn_Activities")
                .Where("OwnerId=@0", ownerId)
                .Where("OwnerType=@0", ownerType)
                .Where("IsPrivate=0")
                .OrderBy("LastModified desc");
            IEnumerable<object> activityIds = dao.FetchTopPrimaryKeys<Activity>(traceBackNumber, sql);
            List<Sql> sqls = new List<Sql>();
            foreach (var activityId in activityIds.Cast<long>())
            {
                sql = Sql.Builder.Select("count(*)").From("tn_ActivityUserInbox").Where("ActivityId = @0", activityId).Where("UserId = @0", userId);
                if (dao.FirstOrDefault<long>(sql) > 0)
                    continue;  //已经存在

                sql = Sql.Builder.Append("insert tn_ActivityUserInbox (UserId,ActivityId) values(@0,@1)", userId, activityId);
                sqls.Add(sql);
            }
            int affectCount = dao.Execute(sqls);
            dao.CloseSharedConnection();

            if (affectCount > 0)
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        #endregion

        #region 动态获取

        /// <summary>
        /// 获取某条动态
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public Activity Get(string tenantTypeId, long sourceId)
        {
            var sql = Sql.Builder;
            sql.Select("ActivityId")
            .From("tn_Activities")
            .Where("SourceId = @0", sourceId)
            .Where("TenantTypeId = @0", tenantTypeId);
            var activityId = CreateDAO().FirstOrDefault<long>(sql);
            if (activityId > 0)
                return Get(activityId);
            return null;
        }


        /// <summary>
        /// 获取用户的时间线
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followGroupId"><para>关注用户分组Id</para><remarks>groupId为-1时获取相互关注的用户，为null时获取所有用户</remarks></param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="mediaType"><see cref="MediaType"/></param>
        /// <param name="isOriginalThread">是不是原创主题</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="applicationIds">是否是移动客户端（针对于移动客户端，用来过滤动态）</param>
        ///<returns></returns>
        public PagingDataSet<Activity> GetMyTimeline(long userId, long? followGroupId, int? applicationId, MediaType? mediaType, bool? isOriginalThread, int pageIndex, List<string> applicationIds = null)
        {
            //按tn_Activities的LastModified倒排序
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection, () =>
            {
                StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
                cacheKey.Append("MyTimeline::");
                cacheKey.AppendFormat("UserId-{0}:", userId);
                if (followGroupId.HasValue)
                    cacheKey.AppendFormat("FollowGroupId-{0}:", followGroupId.Value);
                if (applicationId.HasValue && applicationId.Value > 0)
                    cacheKey.AppendFormat("ApplicationId-{0}:", applicationId.Value);
                if (mediaType.HasValue)
                    cacheKey.AppendFormat("MediaType-{0}:", (int)mediaType.Value);
                if (isOriginalThread.HasValue)
                    cacheKey.AppendFormat("IsOriginalThread-{0}", isOriginalThread.Value);
                if (applicationIds != null && applicationIds.Count() > 0)
                    cacheKey.AppendFormat("IsMobileClient-{0}", string.Join(",", applicationIds));
                return cacheKey.ToString();
            }, () =>
            {
                Sql sql = Sql.Builder;
                sql.From("tn_Activities")
                    .InnerJoin("tn_ActivityUserInbox AUI")
                    .On("tn_Activities.ActivityId = AUI.ActivityId")
                    .Where("AUI.UserId=@0", userId);

                //done:zhengw,by mazq 相互关注怎么处理的？
                //zhengw回复：已修改
                //mazq回复：相互关注、悄悄关注 followGroupId.Value都是大于0的
                //zhengw回复：已删除判断followGroupId.Value>0
                if (followGroupId.HasValue)
                {
                    //sql.Where("tn_Activities.OwnerType=@0", ActivityOwnerTypes.Instance().User());
                    if (followGroupId.Value == FollowSpecifyGroupIds.Quietly)
                        sql.Where(@"tn_Activities.OwnerId in ( select FollowedUserId from tn_Follows where IsQuietly = 1 and UserId= @0)", userId);
                    else if (followGroupId.Value == FollowSpecifyGroupIds.Mutual)
                        sql.Where(@"tn_Activities.OwnerId in ( select FollowedUserId from tn_Follows where IsMutual = 1 and UserId= @0)", userId);
                    else if (followGroupId.Value > 0)
                        sql.Where("tn_Activities.OwnerType=@0", ActivityOwnerTypes.Instance().User())
                        .Where(@"tn_Activities.OwnerId in ( select F.FollowedUserId 
                                            from tn_Follows F inner join tn_ItemsInCategories IIC  on F.Id = IIC.ItemId 
                                            where F.UserId=@0 and IIC.CategoryId=@1 )", userId, followGroupId.Value);
                }
                if (applicationIds != null && applicationIds.Count() > 0)
                {
                    sql.Where("tn_Activities.ApplicationId in (@applicationIds)", new { applicationIds = applicationIds })
                    .Where("tn_Activities.ReferenceId=@0", 0);
                }
                if (applicationId.HasValue && applicationId.Value > 0)
                    sql.Where("tn_Activities.ApplicationId=@0", applicationId.Value);
                if (mediaType.HasValue)
                {
                    switch (mediaType)
                    {
                        case MediaType.Audio:
                            sql.Where("tn_Activities.HasMusic=1");
                            break;
                        case MediaType.Image:
                            sql.Where("tn_Activities.HasImage=1");
                            break;
                        case MediaType.Video:
                            sql.Where("tn_Activities.HasVideo=1");
                            break;
                    }
                }
                if (isOriginalThread.HasValue)
                    sql.Where("tn_Activities.IsOriginalThread=@0", isOriginalThread.Value);

                sql.OrderBy("tn_Activities.LastModified desc");
                return sql;
            });
        }

        /// <summary>
        /// 查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="lastActivityId">上次最后呈现的ActivityId</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="userId">返回首个动态操作者Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="exceptUserId">需要排除的用户Id</param>
        /// <returns>自lastActivityId以后进入用户时间线的动态个数</returns>
        public int GetNewerCount(long ownerId, long lastActivityId, int? applicationId, out long userId, int? ownerType, long? exceptUserId)
        {
            //根据lastActivityId获取到LastModified，获取晚于lastActivity.LastModified的动态并按LastModified倒排序
            string cacheKey = GetCacheKey_NewerCount(ownerId, lastActivityId, applicationId, ownerType, exceptUserId);
            List<long> userIds = cacheService.Get<List<long>>(cacheKey);
            if (userIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("A.UserId")
                    .From("tn_Activities A");
                if (ownerType == ActivityOwnerTypes.Instance().User())
                {
                    sql.InnerJoin("tn_ActivityUserInbox AUI")
                       .On("A.ActivityId = AUI.ActivityId")
                       .Where("AUI.UserId = @0", ownerId)
                       .Where("A.UserId <> @0", ownerId);
                }
                else
                {
                    sql.Where("A.OwnerId = @0", ownerId)
                       .Where("A.OwnerType = @0", ownerType.Value);
                    if (exceptUserId.HasValue && exceptUserId.Value > 0)
                        sql.Where("A.UserId <> @0", exceptUserId.Value);
                }
                sql.Where("A.LastModified > ( select LastModified from tn_Activities where ActivityId = @0 )", lastActivityId);
                if (applicationId.HasValue && applicationId.Value > 0)
                    sql.Where("A.ApplicationId = @0", applicationId.Value);
                sql.OrderBy("A.LastModified desc");
                userIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
            }
            userId = userIds.FirstOrDefault();
            return userIds.Count;
        }

        /// <summary>
        /// 查询自lastActivityId以后进入用户时间线的动态
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="lastActivityId">上次最后呈现的ActivityId</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="exceptUserId">需要排除的用户Id</param>
        /// <returns>lastActivityId</returns>
        public IEnumerable<Activity> GetNewerActivities(long ownerId, long lastActivityId, int? applicationId, int? ownerType, long? exceptUserId = null)
        {
            //根据lastActivityId获取到LastModified，获取晚于lastActivity.LastModified的动态并按LastModified倒排序
            Sql sql = Sql.Builder;
            sql.Select("A.*")
                .From("tn_Activities A");
            if (ownerType == ActivityOwnerTypes.Instance().User())
            {
                sql.InnerJoin("tn_ActivityUserInbox AUI")
                   .On("A.ActivityId = AUI.ActivityId")
                   .Where("AUI.UserId=@0", ownerId);
            }
            else
            {
                sql.Where("A.OwnerId=@0", ownerId).Where("A.OwnerType=@0", ownerType.Value);
                if (exceptUserId.HasValue && exceptUserId.Value > 0)
                    sql.Where("A.UserId <> @0", exceptUserId.Value);
            }
            sql.Where("A.LastModified > ( select LastModified from tn_Activities where ActivityId=@0 )", lastActivityId);
            if (applicationId.HasValue && applicationId.Value > 0)
                sql.Where("A.ApplicationId=@0", applicationId.Value);
            sql.OrderBy("A.LastModified desc");
            return CreateDAO().Fetch<Activity>(sql);
        }

        /// <summary>
        /// 获取拥有者的动态
        /// </summary>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="ownerId">动态拥有者Id</param>        
        /// <param name="applicationId">应用Id</param>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="isOriginalThread">是否原创</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="userId">用户Id</param>
        /// <param name="isMobileClient">是否是移动客户端（针对于移动客户端，用来过滤动态）</param> 
        ///<returns></returns>
        public PagingDataSet<Activity> GetOwnerActivities(int ownerType, long ownerId, int? applicationId, MediaType? mediaType, bool? isOriginalThread, bool? isPrivate, int pageIndex, long? userId, bool? isMobileClient = null)
        {
            //按tn_Activities的LastModified倒排序，仅获取IsPrivate=false的动态
            //使用GetPagingEntities
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection, () =>
            {
                StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
                cacheKey.Append("OwnerActivities::");
                cacheKey.AppendFormat("OwnerType-{0}", ownerType);
                if (applicationId.HasValue && applicationId.Value > 0)
                    cacheKey.AppendFormat("ApplicationId-{0}", applicationId);
                if (isPrivate.HasValue)
                    cacheKey.AppendFormat("IsPrivate-{0}", isPrivate.Value);
                if (mediaType.HasValue)
                    cacheKey.AppendFormat("MediaType-{0}", (int)mediaType.Value);
                if (isOriginalThread.HasValue)
                    cacheKey.AppendFormat("IsOriginalThread-{0}", isOriginalThread.Value);
                if (userId.HasValue)
                    cacheKey.AppendFormat("UserId-{0}", userId.Value);
                if (isMobileClient.HasValue)
                    cacheKey.AppendFormat("IsMobileClient-{0}", isMobileClient.Value);
                return cacheKey.ToString();
            }, () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("ActivityId")
                    .From("tn_Activities")
                    .Where("OwnerId=@0", ownerId)
                    .Where("OwnerType=@0", ownerType);

                if (isPrivate.HasValue)
                    sql.Where("IsPrivate=@0", isPrivate);
                if (userId.HasValue)
                    sql.Where("UserId=@0", userId);
                if (isMobileClient.HasValue && isMobileClient.Value)
                {
                    sql.Where("tn_Activities.ApplicationId in (@0,@1,@2,@3)", 1001, 1002, 1003, 1012)
                    .Where("tn_Activities.ReferenceId=@0", 0);
                }
                if (applicationId.HasValue)
                    sql.Where("ApplicationId=@0", applicationId);
                if (mediaType.HasValue)
                {
                    switch (mediaType)
                    {
                        case MediaType.Audio:
                            sql.Where("HasMusic=1");
                            break;
                        case MediaType.Image:
                            sql.Where("HasImage=1");
                            break;
                        case MediaType.Video:
                            sql.Where("HasVideo=1");
                            break;
                    }
                }

                if (isOriginalThread.HasValue)
                    sql.Where("IsOriginalThread=@0", isOriginalThread);

                sql.OrderBy("LastModified desc");
                return sql;
            });
        }

        /// <summary>
        /// 获取站点动态
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        ///<returns></returns>
        public IEnumerable<Activity> GetSiteActivities(int? applicationId, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.GlobalVersion));
                    cacheKey.Append("SiteActivities::");
                    if (applicationId.HasValue && applicationId.Value > 0)
                        cacheKey.AppendFormat("ApplicationId-{0}", applicationId);
                    return cacheKey.ToString();
                }
                , () =>
             {
                 Sql sql = Sql.Builder;
                 sql.Select("ActivityId")
                     .From("tn_ActivitySiteInbox");
                 sql.OrderBy("Id desc");
                 return sql;
             }); 
        }

        /// <summary>
        /// 动态是否存在在站点中
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public bool isExistInSiteBox(long activityId)
        {
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_ActivitySiteInbox").Where("ActivityId =@0", activityId);
            int count = CreateDAO().Execute(sql);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 加载动态的同时，加载用户实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public override IEnumerable<Activity> PopulateEntitiesByEntityIds<T>(IEnumerable<T> entityIds)
        {
            var activities = base.PopulateEntitiesByEntityIds<T>(entityIds);
            var userService = DIContainer.Resolve<IUserService>();
            IEnumerable<IUser> listUsers = userService.GetUsers(activities.Select(n => n.UserId).Distinct());
            return activities;
        }

        #endregion

        #region CacheKey
        /// <summary>
        /// 获取新动态数量缓存Key
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="lastActivityId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        private string GetCacheKey_NewerCount(long ownerId, long lastActivityId, int? applicationId, int? ownerType, long? exceptUserId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
            cacheKey.AppendFormat("ActivityNewerCount::UserId-{0}:LastActivityId-{1}:ApplicationId-{2}", ownerId, lastActivityId, applicationId);
            if (ownerType != ActivityOwnerTypes.Instance().User())
            {
                cacheKey.AppendFormat(":OwnerType-{0}", ownerType);
            }
            if (exceptUserId != null)
            {
                cacheKey.AppendFormat(":ExceptUserId-{0}", exceptUserId);
            }
            return cacheKey.ToString();
        }

        #endregion

    }
}
