using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common;
using System.Linq;
using Tunynet.Utilities;
using Spacebuilder.Common;

namespace SpecialTopic.Topic.Repositories
{
    public class TopicPaperRepository : Repository<TopicPaper>, ITopicPaperRepository
    {
        /// <summary>
        /// 获取专题下所有文章
        /// </summary>
        public IEnumerable<TopicPaper> GetAllPapersOfTopic(long topicId)
        {
            var sql = Sql.Builder;
            sql.Select("*")
            .From("spt_TopicPapers")
            .Where("topicId=@0", topicId);
            IEnumerable<TopicPaper> topicpapers = CreateDAO().Fetch<TopicPaper>(sql);
            return topicpapers;
        }

        public TopicPaper GetTopicPaper(long topicid, long paperid)
        {
            var sql = Sql.Builder;
            sql.Select("*")
            .From("spt_TopicPapers")
            .Where("topicId=@0 and paperid=@1", topicid, paperid);
            return CreateDAO().First<TopicPaper>(sql);
        }
        public void AddPaperToTopic(PaperEntity paper, long topicId)
        {
            CreateDAO().Insert(paper);
            base.Insert(new TopicPaper {PaperId=paper.PaperId,TopicId=topicId });
        }
        public void AddPaperToTopic(long topicId, long paperid)
        {
            base.Insert(new TopicPaper { PaperId = paperid, TopicId = topicId });
        }


        public void DeletePaperOfTopic(TopicPaper entity)
        {
            base.Delete(entity);
        }
    }
}