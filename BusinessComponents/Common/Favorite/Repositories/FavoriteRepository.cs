//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户收藏数据访问类
    /// </summary>
    public class FavoriteRepository : Repository<FavoriteEntity>, IFavoriteRepository
    {

        private int pageSize = 20;

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>true-收藏成功,false-收藏失败</returns>
        public bool Favorite(long objectId, long userId, string tenantTypeId)
        {
            if (IsFavorited(objectId, userId, tenantTypeId))
                return false;

            FavoriteEntity favorite = FavoriteEntity.New();
            favorite.UserId = userId;
            favorite.ObjectId = objectId;
            favorite.TenantTypeId = tenantTypeId;

            long id = 0;
            long.TryParse(base.Insert(favorite).ToString(), out id);

            if (id > 0)
            {
                string cacheKey = GetCacheKey_AllFavorites(userId, tenantTypeId);
                IList<long> objectIds = cacheService.Get<IList<long>>(cacheKey);
                if (objectIds != null)
                {
                    objectIds.Insert(0, objectId);
                    cacheService.Set(cacheKey, objectIds, CachingExpirationType.UsualObjectCollection);
                }
                cacheKey = GetCacheKey_AllUserIdOfObject(objectId, tenantTypeId);
                IList<long> userIds = cacheService.Get<IList<long>>(cacheKey);
                if (userIds != null)
                {
                    userIds = new List<long>();
                    userIds.Insert(0, userId);
                    cacheService.Set(cacheKey, userIds, CachingExpirationType.UsualObjectCollection);
                }

                int count = GetFavoritedUserCount(objectId, tenantTypeId);
                count++;
                cacheKey = GetCacheKey_FavoritedUserCount(objectId, tenantTypeId);
                cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>true-取消成功,false-取消失败</returns>
        public bool CancelFavorited(long objectId, long userId, string tenantTypeId)
        {
            if (IsFavorited(objectId, userId, tenantTypeId))
            {
                Sql sql = Sql.Builder;
                sql.Append("delete from tn_Favorites where UserId = @0 and ObjectId = @1 and TenantTypeId = @2", userId, objectId, tenantTypeId);
                int affectCount = CreateDAO().Execute(sql);
                if (affectCount > 0)
                {
                    RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
                    RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                    string cacheKey = GetCacheKey_AllFavorites(userId, tenantTypeId);
                    IList<long> objectIds = cacheService.Get<IList<long>>(cacheKey);
                    if (objectIds != null && objectIds.Contains(objectId))
                    {
                        objectIds.Remove(objectId);
                        cacheService.Set(cacheKey, objectIds, CachingExpirationType.UsualObjectCollection);
                    }

                    cacheKey = GetCacheKey_AllUserIdOfObject(objectId, tenantTypeId);
                    IList<long> userIds = cacheService.Get<IList<long>>(cacheKey);
                    if (userIds != null && userIds.Contains(userId))
                    {
                        userIds.Remove(userId);
                        cacheService.Set(cacheKey, userIds, CachingExpirationType.UsualObjectCollection);
                    }

                    int count = GetFavoritedUserCount(objectId, tenantTypeId);
                    count--;
                    cacheKey = GetCacheKey_FavoritedUserCount(objectId, tenantTypeId);
                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 清除某个实体的所有订阅
        /// </summary>
        /// <param name="objectId">实体ID</param>
        /// <returns></returns>
        public bool CleanSubscribesFromObject(long objectId)
        {
            Sql sql = Sql.Builder;
            sql.Append("delete from tn_Favorites where ObjectId = @0",objectId);
            int affectCount = CreateDAO().Execute(sql);
            if (affectCount > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 判断是否收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>true-已收藏，false-未收藏</returns>
        public bool IsFavorited(long objectId, long userId, string tenantTypeId)
        {
            IEnumerable<long> objectIds = GetAllObjectIds(userId, tenantTypeId);
            return objectIds != null && objectIds.Contains(objectId);
        }

        /// <summary>
        /// 获取部分收藏对象Id分页数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的内容数</param>
        ///<returns></returns>
        public PagingDataSet<FavoriteEntity> GetPagingPartObjectIds(long userId, IEnumerable<string> tenantTypeIds, int pageIndex, int? pageSize = null)
        {
            if (!pageSize.HasValue)
            {
                pageSize = this.pageSize;
            }
            return GetPagingEntities(pageSize.Value, pageIndex, CachingExpirationType.UsualSingleObject, () =>
            {
                string cacheKey = GetCacheKey_PaingObjectIds(userId, string.Join(",", tenantTypeIds));
                return cacheKey;
            }, () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                   .From("tn_Favorites")
                   .Where("UserId = @0", userId)
                   .Where("TenantTypeId in (@tenantTypeIds)", new { tenantTypeIds = tenantTypeIds })
                   .OrderBy("Id desc");
                return sql;
            });
        }

        /// <summary>
        /// 获取收藏对象Id分页数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的内容数</param>
        ///<returns></returns>
        public PagingDataSet<long> GetPagingObjectIds(long userId, string tenantTypeId, int pageIndex, int? pageSize = null)
        {
            PagingEntityIdCollection peic = null;

            Sql sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_Favorites")
               .Where("UserId = @0", userId)
               .Where("TenantTypeId = @0", tenantTypeId)
               .OrderBy("Id desc");

            if (!pageSize.HasValue)
            {
                pageSize = this.pageSize;
            }

            Database dao = CreateDAO();
            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = GetCacheKey_PaingObjectIds(userId, tenantTypeId);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize.Value * CacheablePageCount, 1, "ObjectId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize.Value, pageIndex, "ObjectId", sql);
            }

            if (peic != null)
            {
                PagingDataSet<long> pds = new PagingDataSet<long>(peic.GetPagingEntityIds(pageSize.Value, pageIndex).Cast<long>());
                pds.PageSize = pageSize.Value;
                pds.PageIndex = pageIndex;
                pds.TotalRecords = peic.TotalRecords;
                return pds;
            }

            return null;
        }

        /// <summary>
        /// 获取前N个收藏对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">要获取Id的个数</param>
        ///<returns></returns>
        public IEnumerable<long> GetTopObjectIds(long userId, string tenantTypeId, int topNumber)
        {
            IEnumerable<long> objectIds = GetAllObjectIds(userId, tenantTypeId);
            if (objectIds != null)
            {
                return objectIds.Take(topNumber);
            }

            return null;
        }

        /// <summary>
        /// 获取全部收藏对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<returns></returns>
        public IEnumerable<long> GetAllObjectIds(long userId, string tenantTypeId)
        {
            string cacheKey = GetCacheKey_AllFavorites(userId, tenantTypeId);
            IList<long> objectIds = cacheService.Get<IList<long>>(cacheKey);
            if (objectIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("ObjectId")
                   .From("tn_Favorites")
                   .Where("UserId = @0", userId)
                   .Where("TenantTypeId = @0", tenantTypeId)
                   .OrderBy("Id desc");

                objectIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey, objectIds, CachingExpirationType.UsualObjectCollection);
            }

            return objectIds;
        }

        /// <summary>
        /// 根据收藏对象获取UserId
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfObject(long objectId, string tenantTypeId)
        {
            string cacheKey = GetCacheKey_AllUserIdOfObject(objectId, tenantTypeId);
            IEnumerable<long> userIds = cacheService.Get<IList<long>>(cacheKey);

            if (userIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("UserId")
                   .From("tn_Favorites")
                   .Where("ObjectId = @0", objectId)
                   .Where("TenantTypeId = @0", tenantTypeId)
                   .OrderBy("UserId desc");

                userIds = CreateDAO().FetchTop<long>(PrimaryMaxRecords, sql);
                cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
            }

            return userIds;
        }

        /// <summary>
        /// 根据收藏对象获取收藏了此对象的前N个用户Id集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">要获取记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopUserIdsOfObject(long objectId, string tenantTypeId, int topNumber)
        {
            IEnumerable<long> userIds = GetUserIdsOfObject(objectId, tenantTypeId);
            return userIds != null ? userIds.Take(topNumber) : userIds;
        }

        /// <summary>
        /// 根据收藏对象获取收藏了此对象的用户Id分页集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingUserIdsOfObject(long objectId, string tenantTypeId, int pageIndex)
        {
            Database dao = CreateDAO();
            PagingEntityIdCollection peic = null;

            Sql sql = Sql.Builder;
            sql.Select("UserId")
               .From("tn_Favorites")
               .Where("ObjectId = @0", objectId)
               .Where("TenantTypeId = @0", tenantTypeId)
               .OrderBy("UserId desc");

            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ObjectId", objectId) + string.Format("PagingUserIdsOfObjectId:tenantTypeId-{0}", tenantTypeId);

                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "UserId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "UserId", sql);
            }

            if (peic != null)
            {
                PagingDataSet<long> pds = new PagingDataSet<long>(peic.GetPagingEntityIds(pageSize, pageIndex).Cast<long>());
                pds.PageSize = pageSize;
                pds.PageIndex = pageIndex;
                pds.TotalRecords = peic.TotalRecords;
                return pds;
            }

            return null;
        }

        /// <summary>
        /// 根据收藏对象获取同样收藏此对象的我的关注用户
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetFollowedUserIdsOfObject(long objectId, long userId, string tenantTypeId)
        {
            string cacheKey = GetCacheKey_AllFollowedUserIdsOfObject(objectId, userId, tenantTypeId);
            IEnumerable<long> userIds = cacheService.Get<IList<long>>(cacheKey);

            if (userIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("distinct FollowedUserId")
                   .From("tn_Follows")
                   .InnerJoin("tn_Favorites Fav")
                   .On("tn_Follows.FollowedUserId = Fav.UserId")
                   .Where("Fav.ObjectId = @0", objectId)
                   .Where("Fav.TenantTypeId = @0", tenantTypeId)
                   .Where("tn_Follows.UserId = @0", userId);

                userIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
            }

            return userIds;
        }

        /// <summary>
        /// 获取被收藏数
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public int GetFavoritedUserCount(long objectId, string tenantTypeId)
        {
            string cacheKey = GetCacheKey_FavoritedUserCount(objectId, tenantTypeId);
            int? count = cacheService.Get(cacheKey) as int?;

            if (!count.HasValue)
            {
                Sql sql = Sql.Builder;
                sql.Select("Count(*)")
                   .From("tn_Favorites")
                   .Where("ObjectId = @0", objectId)
                   .Where("TenantTypeId = @0", tenantTypeId);

                count = CreateDAO().ExecuteScalar<int>(sql);

                cacheService.Add(cacheKey, count, CachingExpirationType.ObjectCollection);
            }

            return count.HasValue ? count.Value : 0;
        }

        /// <summary>
        /// 删除垃圾数据
        /// </summary>
        /// <param name="serviceKey">服务标识</param>
        public void DeleteTrashDatas(string serviceKey)
        {
            IEnumerable<TenantType> tenantTypes = new TenantTypeService().Gets(serviceKey);

            List<Sql> sqls = new List<Sql>();
            sqls.Add(Sql.Builder.Append("delete from tn_Favorites where not exists (select 1 from tn_Users where UserId = tn_Favorites.UserId)"));

            foreach (var tenantType in tenantTypes)
            {
                Type type = Type.GetType(tenantType.ClassType);
                if (type == null)
                    continue;

                var pd = TableInfo.FromPoco(type);
                sqls.Add(Sql.Builder.Append("delete from tn_Favorites")
                                    .Where("not exists (select 1 from " + pd.TableName + " where ObjectId = " + pd.PrimaryKey + ") and TenantTypeId = @0"
                                    , tenantType.TenantTypeId));
            }

            CreateDAO().Execute(sqls);
        }

        #region Helper Method

        private static string GetCacheKey_FavoritedUserCount(long objectId, string tenantTypeId)
        {
            return string.Format("FavoritedCount::UserId:{0}-TenaantTypeId:{1}", objectId, tenantTypeId);
        }

        /// <summary>
        /// 获取全部收藏CacheKey
        /// </summary>
        /// <param name="userId">收藏用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCacheKey_AllFavorites(long userId, string tenantTypeId)
        {
            return string.Format("AllFavorites:UserId-{0}:TenantTypeId-{1}", userId, tenantTypeId);
        }

        /// <summary>
        /// 获取全部收藏CacheKey
        /// </summary>
        /// <param name="userId">收藏用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCacheKey_PaingObjectIds(long userId, string tenantTypeId)
        {
            string cacheKeyPrefix = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            return cacheKeyPrefix + string.Format("PaingFavoriteObjectIds:TenantTypeId-{0}", tenantTypeId);
        }

        /// <summary>
        /// 获取收藏对象全部收藏用户
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCacheKey_AllUserIdOfObject(long objectId, string tenantTypeId)
        {
            return string.Format("AllFavorites:ObjectId-{0}:TenantTypeId-{1}", objectId, tenantTypeId);
        }

        /// <summary>
        /// 根据收藏对象获取同样收藏此对象的我的关注用户
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCacheKey_AllFollowedUserIdsOfObject(long objectId, long userId, string tenantTypeId)
        {
            return string.Format("AllFollowedUserIdsOfFavorite:ObjectId-{0}:UserId-{1}:TenantTypeId-{2}", objectId, userId, tenantTypeId);
        }

        #endregion Helper Method
    }
}
