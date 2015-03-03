using System;
using System.Collections.Generic;
using System.Linq;
using Tunynet.Repositories;
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;
using Spacebuilder.Common;

namespace SpecialTopic.Topic 
{
    public interface ITopicPaperRepository : IRepository<TopicPaper>
    {
        /// <summary>
        /// 获取专题下所有文章
        /// </summary>
        IEnumerable<TopicPaper> GetAllPapersOfTopic(long topicId);

        TopicPaper GetTopicPaper(long topicid, long paperid);

        void AddPaperToTopic(PaperEntity paper, long topicId);
        void AddPaperToTopic(long topicId, long paperid);
        void DeletePaperOfTopic(TopicPaper entity);


    }
}