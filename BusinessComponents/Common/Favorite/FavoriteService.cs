//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-08-15</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-08-15" version="0.5">新建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 收藏逻辑类
    /// </summary>
    public class FavoriteService
    {
        private IFavoriteRepository favoriteRepository;
        private string tenantTypeId;
        private IEnumerable<string> tenantTypeIds;


        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public FavoriteService(string tenantTypeId)
            : this(tenantTypeId, new FavoriteRepository())
        {
            this.tenantTypeId = tenantTypeId;
        }

        

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="favoriteRepository">收藏数据访问</param>
        public FavoriteService(string tenantTypeId, IFavoriteRepository favoriteRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public FavoriteService(IEnumerable<string> tenantTypeIds)
            : this(tenantTypeIds, new FavoriteRepository())
        {
            this.tenantTypeIds = tenantTypeIds;
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="favoriteRepository">收藏数据访问</param>
        public FavoriteService(IEnumerable<string> tenantTypeIds, IFavoriteRepository favoriteRepository)
        {
            this.tenantTypeIds = tenantTypeIds;
            this.favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>true-收藏成功,false-收藏失败</returns>
        public bool Favorite(long objectId, long userId)
        {
            EventBus<long, FavoriteEventArgs>.Instance().OnBefore(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));
            bool result = favoriteRepository.Favorite(objectId, userId, tenantTypeId);
            EventBus<long, FavoriteEventArgs>.Instance().OnAfter(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="objectId">被收藏对象Id</param>
        /// <returns>true-取消成功,false-取消失败</returns>
        public bool CancelFavorite(long objectId, long userId)
        {
            EventBus<long, FavoriteEventArgs>.Instance().OnBefore(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));
            bool result = favoriteRepository.CancelFavorited(objectId, userId, tenantTypeId);
            EventBus<long, FavoriteEventArgs>.Instance().OnAfter(objectId, new FavoriteEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// 判断是否收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>true-已收藏，false-未收藏</returns>
        public bool IsFavorited(long objectId, long userId)
        {
            return favoriteRepository.IsFavorited(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// 获取收藏对象Id分页数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示内容数</param>
        ///<returns></returns>
        public PagingDataSet<long> GetPagingObjectIds(long userId, int pageIndex, int? pageSize = null)
        {
            return favoriteRepository.GetPagingObjectIds(userId, tenantTypeId, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取部分收藏对象Id分页数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示内容数</param>
        ///<returns></returns>
        public PagingDataSet<FavoriteEntity> GetPagingPartObjectIds(long userId, int pageIndex, int? pageSize = null)
        {
            return favoriteRepository.GetPagingPartObjectIds(userId, tenantTypeIds, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取前N个收藏对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">要获取Id的个数</param>
        ///<returns></returns>
        public IEnumerable<long> GetTopObjectIds(long userId, int topNumber)
        {
            return favoriteRepository.GetTopObjectIds(userId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// 获取全部收藏对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        ///<returns></returns>
        public IEnumerable<long> GetAllObjectIds(long userId)
        {
            return favoriteRepository.GetAllObjectIds(userId, tenantTypeId);
        }

        /// <summary>
        /// 根据收藏对象获取UserId
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfObject(long objectId)
        {
            return favoriteRepository.GetUserIdsOfObject(objectId, tenantTypeId);
        }

        /// <summary>
        /// 根据收藏对象获取收藏了此对象的前N个用户Id集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">要获取记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopUserIdsOfObject(long objectId, int topNumber)
        {
            return favoriteRepository.GetTopUserIdsOfObject(objectId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// 根据收藏对象获取收藏了此对象的用户Id分页集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingUserIdsOfObject(long objectId, int pageIndex)
        {
            return favoriteRepository.GetPagingUserIdsOfObject(objectId, tenantTypeId, pageIndex);
        }

        /// <summary>
        /// 根据收藏对象获取同样收藏此对象的关注用户
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        public IEnumerable<long> GetFollowedUserIdsOfObject(long objectId, long userId)
        {
            return favoriteRepository.GetFollowedUserIdsOfObject(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// 获取被收藏数
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <returns></returns>
        public int GetFavoritedUserCount(long objectId)
        {
            return favoriteRepository.GetFavoritedUserCount(objectId, tenantTypeId);
        }

        /// <summary>
        /// 根据收藏主键集合组装收藏实体集合
        /// </summary>
        /// <param name="favoriteIds">收藏主键集合</param>
        /// <returns></returns>
        public IEnumerable<FavoriteEntity> GetFavorites(IEnumerable<long> favoriteIds)
        {
            return favoriteRepository.PopulateEntitiesByEntityIds(favoriteIds);
        }
    }
}
