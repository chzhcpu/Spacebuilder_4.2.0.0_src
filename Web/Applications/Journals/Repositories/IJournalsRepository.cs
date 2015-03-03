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

namespace Spacebuilder.Journals
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJournalsRepository : IRepository<Journal>
    {
        /// <summary>
        /// 返回分页数据集
        /// </summary>
        /// <param name="pageindex">第几页</param>
        /// <param name="pagesize">每页条目数</param>
        /// <param name="orderby">排序列</param>
        /// <returns>分页数据集</returns>
        PagingDataSet<Journal> Gets(int pageindex,int pagesize,string orderby);
        
    }
}