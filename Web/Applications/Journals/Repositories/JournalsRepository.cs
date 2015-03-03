//<TunynetCopyright>
//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Repositories;
using Tunynet;
using PetaPoco;

namespace Spacebuilder.Journals
{
    public class JournalsRepository : Repository<Journal>, IJournalsRepository
    {
        public PagingDataSet<Journal> Gets(int pageindex, int pagesize,string orderby)
        {
            Sql sql=PetaPoco.Sql.Builder;
            sql.OrderBy(new string[]{orderby});
            var r=GetPagingEntities(pagesize,pageindex,sql);
            return r;
        }
        
    }
}