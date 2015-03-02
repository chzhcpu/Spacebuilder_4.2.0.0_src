//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-08-15</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-08-15" version="0.5">新建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System.Collections.Generic;
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 订阅逻辑类
    /// </summary>
    public class SubscribeService
    {
        private IFavoriteRepository favoriteRepository;
        private string tenantTypeId;

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public SubscribeService(string tenantTypeId)
            : this(tenantTypeId, new FavoriteRepository())
        {
            this.tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="favoriteRepository">订阅数据访问</param>
        public SubscribeService(string tenantTypeId, IFavoriteRepository favoriteRepository)
        {
            this.tenantTypeId = tenantTypeId;
            this.favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <param name="objectId">被订阅对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>true-订阅成功,false-订阅失败</returns>
        public bool Subscribe(long objectId, long userId)
        {
            EventBus<long, SubscribeEventArgs>.Instance().OnBefore(objectId, new SubscribeEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));
            bool result = favoriteRepository.Favorite(objectId, userId, tenantTypeId);
            EventBus<long, SubscribeEventArgs>.Instance().OnAfter(objectId, new SubscribeEventArgs(EventOperationType.Instance().Create(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="objectId">被订阅对象Id</param>
        /// <returns>true-取消成功,false-取消失败</returns>
        public bool CancelSubscribe(long objectId, long userId)
        {
            EventBus<long, SubscribeEventArgs>.Instance().OnBefore(objectId, new SubscribeEventArgs(EventOperationType.Instance().Delete(), tenantTypeId, userId));
            bool result = favoriteRepository.CancelFavorited(objectId, userId, tenantTypeId);
            EventBus<long, SubscribeEventArgs>.Instance().OnAfter(objectId, new SubscribeEventArgs(EventOperationType.Instance().Delete(), tenantTypeId, userId));

            return result;
        }

        /// <summary>
        /// 清除某个实体的所有订阅
        /// </summary>
        /// <param name="objectId">实体ID</param>
        /// <returns></returns>
        public bool CleanSubscribesFromObject(long objectId)
        {
            return favoriteRepository.CleanSubscribesFromObject(objectId);
        }

        /// <summary>
        /// 判断是否订阅
        /// </summary>
        /// <param name="objectId">被订阅对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>true-已订阅，false-未订阅</returns>
        public bool IsSubscribed(long objectId, long userId)
        {
            return favoriteRepository.IsFavorited(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// 获取订阅对象Id分页数据
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
        /// 获取前N个订阅对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">要获取Id的个数</param>
        ///<returns></returns>
        public IEnumerable<long> GetTopObjectIds(long userId, int topNumber)
        {
            return favoriteRepository.GetTopObjectIds(userId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// 获取全部订阅对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        ///<returns></returns>
        public IEnumerable<long> GetAllObjectIds(long userId)
        {
            return favoriteRepository.GetAllObjectIds(userId, tenantTypeId);
        }

        /// <summary>
        /// 根据订阅对象获取UserId
        /// </summary>
        /// <param name="objectId">订阅对象Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfObject(long objectId)
        {
            return favoriteRepository.GetUserIdsOfObject(objectId, tenantTypeId);
        }

        /// <summary>
        /// 根据订阅对象获取订阅了此对象的前N个用户Id集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="topNumber">要获取记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopUserIdsOfObject(long objectId, int topNumber)
        {
            return favoriteRepository.GetTopUserIdsOfObject(objectId, tenantTypeId, topNumber);
        }

        /// <summary>
        /// 根据订阅对象获取订阅了此对象的用户Id分页集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingUserIdsOfObject(long objectId, int pageIndex)
        {
            return favoriteRepository.GetPagingUserIdsOfObject(objectId, tenantTypeId, pageIndex);
        }

        /// <summary>
        /// 根据订阅对象获取同样订阅此对象的我的关注用户
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        public IEnumerable<long> GetFollowedUserIdsOfObject(long objectId, long userId)
        {
            return favoriteRepository.GetFollowedUserIdsOfObject(objectId, userId, tenantTypeId);
        }

        /// <summary>
        /// 获取被订阅数
        /// </summary>
        /// <param name="objectId">订阅对象Id</param>
        /// <returns></returns>
        public int GetSubscribedUserCount(long objectId)
        {
            return favoriteRepository.GetFavoritedUserCount(objectId, tenantTypeId);
        }
    }
}
