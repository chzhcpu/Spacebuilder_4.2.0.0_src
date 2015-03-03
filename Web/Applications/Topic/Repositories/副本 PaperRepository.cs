using System;
using System.Collections.Generic;
using System.Linq;
using Tunynet.Repositories;
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;
using Spacebuilder.Common;
using PetaPoco;

namespace SpecialTopic.Topic
{
    public class PaperRepository : Repository<PaperEntity>, IPaperRepository
    {
         public PaperRepository() { }

        public IEnumerable<PaperEntity> GetPapersByTopicId(long  topicId)
        {
            //select spt_papers.* from spt_papers p, spt_TopicPaper tp where 
            //p.paperid=tp.paperid and tp.topicid=?? 
            Sql sql = Sql.Builder;
            sql.Select("spt_papers.*")
               .From("spt_papers")
               .InnerJoin("spt_TopicPaper tp")
               .On("tp.PaperId = spt_papers.PaperId")
               .Where("tp.TopicId = @0", topicId);
           
            return CreateDAO().Fetch<PaperEntity>(sql);

        }

        public void Create(long topicId,PaperEntity paper)
        {
            long id = 0;
            long.TryParse(this.Insert(paper).ToString(), out id);
            
            Sql sql = Sql.Builder;
            sql =sql.Append("insert into spt(topicid,paperid) values(@0,@1)",topicId,paper.PaperId);
            CreateDAO().Execute(sql);
        }


    }
}