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
    /// <summary>
    /// 
    /// </summary>
    public interface IPaperRepository : IRepository<PaperEntity>
    {
        IEnumerable<PaperEntity> GetPapersByTopicId(long topicId);

        PaperEntity GetPaperByDOI(string doi);
    }
}