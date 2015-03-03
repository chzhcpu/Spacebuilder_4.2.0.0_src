using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using System.IO;
using Tunynet;
using Tunynet.Events;
using Spacebuilder.Common;
using PetaPoco;

namespace SpecialTopic.Topic 
{
    public class PaperService
    {
        public ITopicPaperRepository topicPaperRepository { get; set; }
        public ITopicRepository topicRepository { get; set; }


        private IPaperRepository paperRepository = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PaperService()
            : this(new PaperRepository())
        {
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public PaperService(IPaperRepository paperRepository)
        {
            this.paperRepository = paperRepository;
        }

        public IEnumerable<PaperEntity> GetPapersByTopicId(long topicId)
        {
            var result= paperRepository.GetPapersByTopicId(topicId);
            return result;
        }

        public PaperEntity GetPaperByPaperId(long paperId)
        {
            return paperRepository.Get(paperId);
        }

        public void AddPaperToTopic(long topicId,PaperEntity paperEntity)
        {
            if (paperEntity.PaperId > 0)
            {
                //paper已经存在了，只需添加到topicpaper中即可
                topicPaperRepository.AddPaperToTopic(paperEntity, topicId);
            }
            else {
                //新构建的paper，需要添加到paper表中；
                long retPaperId= (long)Create(paperEntity);
                AddPaperToTopic(topicId, retPaperId);
            }
        }

        public void AddPaperToTopic(long topicId, long paperid)
        {
            topicPaperRepository.AddPaperToTopic(topicId,paperid);
        }

        public void DeletePaperOfTopic(PaperEntity entity,long topicId)
        {
            TopicPaper tp = new TopicPaper() {TopicId=topicId,PaperId=entity.PaperId };

            topicPaperRepository.DeletePaperOfTopic(tp);
        }

        public PaperEntity GetPaperByPaperDOI(string doi)
        {
            var paper = paperRepository.GetPaperByDOI(doi); //.Where<PaperEntity>(p=>p.doi==doi).First<PaperEntity>();
            return paper;
        }
        public bool TopicPaperExist(long topicid, long paperid)
        {
            var topicpaper= topicPaperRepository.GetTopicPaper(topicid, paperid);
            return topicpaper==null ? false:true;
        }

        public bool PaperExist(string doi)
        {
            var paper= paperRepository.GetPaperByDOI(doi);
            return paper == null ? false : true;
        }

        #region 维护专题



        /// <summary>
        /// 创建专题
        /// </summary>
        /// <param name="userId">当前操作人</param>
        /// <param name="paper"><see cref="TopicEntity"/></param>
        /// <param name="logoFile">专题标识图</param>
        /// <returns>创建成功返回true，失败返回false</returns>
        public object Create(PaperEntity paper)
        {
            return paperRepository.Insert(paper);
        }
        /// <summary>
        /// 删除专题
        /// </summary>
        /// <param name="groupId">专题Id</param>
        public int Delete(PaperEntity paper)
        {
            return paperRepository.Delete(paper);            
        }



        #endregion

    }
}