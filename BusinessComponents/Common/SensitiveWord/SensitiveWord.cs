//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-05-24</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-05-24" version="0.5">新建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 敏感词实体类
    /// </summary>
    [TableName("tn_SensitiveWords")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TypeId")]
    [Serializable]
    public class SensitiveWord : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static SensitiveWord New()
        {
            SensitiveWord sensitiveWord = new SensitiveWord()
            {
                Word = string.Empty,
                Replacement = string.Empty

            };

            return sensitiveWord;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 敏感词
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 敏感词类型Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 替换后的字符
        /// </summary>
        public string Replacement { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId
        {
            get
            {
                return this.Id;
            }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
