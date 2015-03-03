using System;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Spacebuilder.Common;
using Tunynet.Common;

namespace SpecialTopic.Topic 
{
    ///<summary>
    /// </summary>
    [TableName("spt_TopicPapers")]
    [PrimaryKey("Id", autoIncrement = true)]
    //[CacheSetting(true, PropertyNamesOfArea = "UserId,TopicId")]
    [Serializable]
    public class TopicPaper:IEntity
    {
        public static TopicPaper New()
        {
            TopicPaper topicPaper = new TopicPaper() { };

            return topicPaper;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///专题Id
        /// </summary>
        public long TopicId { get; set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long PaperId { get; set; }
        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion


        /// <summary>
        /// 专题成员
        /// </summary>
        [Ignore]
        public PaperEntity Paper
        {
            get
            {
                PaperService paperService = new PaperService();
                return paperService.GetPaperByPaperId(this.PaperId);
            }
        }
    }
}