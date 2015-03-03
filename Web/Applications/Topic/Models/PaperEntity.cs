using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Utilities;
using Tunynet.Common.Configuration;


namespace SpecialTopic.Topic 
{
    /// <summary>
    /// 专题实体
    /// </summary>
    [TableName("spt_Papers")]
    [PrimaryKey("PaperId", autoIncrement = true)]
    //[CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class PaperEntity : /*SerializablePropertiesBase, */IEntity
    {
        //继承自SerializablePropertiesBase可序列化属性基类后，自动生成了两个列：PropertyNames和PropertyValues，详见该类定义

        /// <summary>
        /// 专题实体
        /// </summary>
        public static PaperEntity New()
        {
            PaperEntity topic = new PaperEntity()
            {
                //PaperId = -1,
                Title = string.Empty,
                Doi = string.Empty,

            };
            return topic;
        }

        #region 需持久化属性

        /// <summary>
        ///TopicId
        /// </summary>
        public long PaperId { get; protected set; }
        /// <summary>
        ///专题名称
        /// </summary>
        /// 
        public string Doi { get; set; }        
        
        public string Title { get; set; }
       public string PmId{ get; set; }    
      public string Authors{ get; set; }
      public string Journal{ get; set; }
      public string JIssn{ get; set; }
      public string Volume{ get; set; }
      public string Issue{ get; set; }
      public string Pagination{ get; set; }
      public string Pubdate{ get; set; }
      public string PublicationType{ get; set; }
      public string Abstract { get; set; }

      
        #endregion


        #region IEntity 成员

        object IEntity.EntityId { get { return this.PaperId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}