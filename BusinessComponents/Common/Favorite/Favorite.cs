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
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户收藏实体类
    /// </summary>
    [TableName("tn_Favorites")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class FavoriteEntity : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static FavoriteEntity New()
        {
            FavoriteEntity favorite = new FavoriteEntity()
            {
                TenantTypeId = string.Empty

            };

            return favorite;
        }

        #region 需持久化属性

        /// <summary>
        /// 标识列
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///收藏用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///收藏对象Id
        /// </summary>
        public long ObjectId { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}