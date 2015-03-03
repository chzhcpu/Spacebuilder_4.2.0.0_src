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
               .InnerJoin("spt_TopicPapers tp")
               .On("tp.PaperId = spt_papers.PaperId")
               .Where("tp.TopicId = @0", topicId);
           
            return CreateDAO().Fetch<PaperEntity>(sql);

        }

        public PaperEntity GetPaperByDOI(string doi)
        {
            Sql sql = Sql.Builder;
            sql = sql.Append("select * from spt_papers where doi='@0'",doi);
            var query=CreateDAO().Query<PaperEntity>(sql);
            PaperEntity paper = null;
            if (query != null)
            {
                if(query.Count()>0)
                    paper = query.First<PaperEntity>();
            }
            return paper;
        }

        public void Create(long topicId,PaperEntity paper)
        {
            long id = 0;
            long.TryParse(this.Insert(paper).ToString(), out id);
            
            Sql sql = Sql.Builder;
            sql = sql.Append("insert into spt_TopicPapers(topicid,paperid) values(@0,@1)", topicId, paper.PaperId);
            CreateDAO().Execute(sql);
        }

        public object Insert(PaperEntity paper)
        {
            return base.Insert(paper);
        }
    }
}