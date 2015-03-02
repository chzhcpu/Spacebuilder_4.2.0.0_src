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
using Tunynet.Repositories;
using PetaPoco;

namespace Tunynet.Common
{
    /// <summary>
    /// 顶踩记录的数据访问
    /// </summary>
    public class AttitudeRecordRepository : Repository<AttitudeRecord>, IAttitudeRecordRepository
    {

        /// <summary>
        /// 获取参与用户的Id集合
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="IsSupport">用户是否支持（true为支持，false为反对）</param>
        /// <param name="topNumber">获取条数</param>
        public IEnumerable<long> GetTopOperatedUserIds(long objectId, string tenantTypeId, bool IsSupport, int topNumber)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ObjectId", objectId));
            cacheKey.AppendFormat("TenantTypeId-{0}:IsSupport-{1}", tenantTypeId, Convert.ToInt32(IsSupport));
            PagingEntityIdCollection topOperatedUserIds = cacheService.Get<PagingEntityIdCollection>(cacheKey.ToString());
            if (topOperatedUserIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("UserId")
                   .From("tn_AttitudeRecords")
                   .Where("ObjectId =@0", objectId)
                   .Where("TenantTypeId=@0", tenantTypeId)
                   .Where("IsSupport = @0", IsSupport);
                IEnumerable<object> entityIds = CreateDAO().FetchTop<long>(SecondaryMaxRecords, sql).Cast<object>();
                topOperatedUserIds = new PagingEntityIdCollection(entityIds);
                cacheService.Add(cacheKey.ToString(), topOperatedUserIds, CachingExpirationType.ObjectCollection);
            }
            IEnumerable<long> topEntityIds = topOperatedUserIds.GetTopEntityIds(topNumber).Cast<long>();
            return topEntityIds;
        }

        /// <summary>
        /// 获取用户在某一租户下顶过的内容
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="userId">用户Id</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        public PagingEntityIdCollection GetObjectIds(string tenantTypeId, long userId, int pageSize, int pageIndex)
        {
            var sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_AttitudeRecords")
               .Where("TenantTypeId = @0", tenantTypeId)
               .Where("UserId=@0", userId)
               .Where("IsSupport =1");

            sql.OrderBy("Id DESC");

            PagingEntityIdCollection objectIds = null;
            string cacheKey = GetCacheKey_ObjectIds(tenantTypeId, userId, pageIndex);
            objectIds = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (objectIds == null)
            {
                objectIds = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ObjectId", sql);
                cacheService.Add(cacheKey, objectIds, CachingExpirationType.ObjectCollection);
            }
            return objectIds;
        }

        /// <summary>
        /// 获取用户对某项的所有顶踩
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dictionary<long, bool> GetAllAttitues(string tenantTypeId, long userId)
        {
            var sql = Sql.Builder;
            sql.Select("*")
               .From("tn_AttitudeRecords")
               .Where("TenantTypeId = @0", tenantTypeId)
               .Where("UserId=@0", userId);

            sql.OrderBy("Id DESC");

            string cacheKey = GetCacheKey_AllAttitues(tenantTypeId, userId);
            Dictionary<long, bool> dict = cacheService.Get<Dictionary<long, bool>>(cacheKey);
            if (dict == null)
            {
                dict = new Dictionary<long, bool>();
                var data = CreateDAO().Fetch<AttitudeRecord>(sql);

                foreach (var item in data)
                {
                    dict[item.ObjectId] = item.IsSupport;
                }

                cacheService.Add(cacheKey, dict, CachingExpirationType.ObjectCollection);
            }
            return dict;
        }

        /// <summary>
        /// 获取用户对某项的所有顶踩
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetCacheKey_AllAttitues(string tenantTypeId, long userId)
        {
            return string.Format("AllAttitues::TenantTypeId-{0}:UserId-{1}", tenantTypeId, userId);
        }

        /// <summary>
        /// 获取操作Id集合的CacheKey
        /// </summary>
        /// <param name="tenantTypeId"> 租户类型Id</param>
        /// <param name="sortBy">排序类型</param>
        /// <returns></returns>
        private string GetCacheKey_ObjectIds(string tenantTypeId, long userId, int pageIndex)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
            cacheKey.AppendFormat("GetObjectIds::UserId-{0}:PageIndex-{1}", userId, pageIndex);
            return cacheKey.ToString();
        }
    }
}