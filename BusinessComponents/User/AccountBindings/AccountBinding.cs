//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 账号绑定实体类
    /// </summary>
    [TableName("tn_AccountBindings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class AccountBinding : SerializablePropertiesBase, IEntity
    {
        #region 构造函数

        /// <summary>
        /// 创建账号绑定
        /// </summary>
        /// <param name="user"></param>
        public static AccountBinding New()
        {
            AccountBinding accountBinding = new AccountBinding()
            {
                AccountTypeKey = string.Empty,
                Identification = string.Empty,
                AccessToken = string.Empty

            };
            return accountBinding;
        }

        #endregion


        #region 需持久化属性

        /// <summary>
        ///主键标识
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///第三方账号类型
        /// </summary>
        public string AccountTypeKey { get; set; }

        /// <summary>
        ///第三方账号标识
        /// </summary>
        public string Identification { get; set; }


        /// <summary>
        ///oauth授权凭证加密串
        /// </summary>
        public string AccessToken { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// AccessToken过期时间
        /// </summary>
        [Ignore]
        public DateTime ExpiredDate
        {
            get { return GetExtendedProperty<DateTime>("ExpiredDate", DateTime.UtcNow.AddMonths(1)); }
            set { SetExtendedProperty("ExpiredDate", value); }
        }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}