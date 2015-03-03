//<TunynetCopyright>
//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using PetaPoco;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// 业务逻辑类
    /// </summary>
    public class JournalsService
    {
        private IJournalsRepository repository;

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public JournalsService()
            : this(new JournalsRepository())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">仓储实现</param>
        public JournalsService(IJournalsRepository repository)
        {
            this.repository = repository;
        }

        #endregion


        /// <summary>
        /// 删除用户时处理其Journals应用下的数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="takeOverUserName"></param>
        /// <param name="isTakeOver"></param>
        public void DeleteUser(string userId, string takeOverUserName, bool isTakeOver)
        {
            //todo:处理删除用户时，同时处理用户的应用数据。
        }

        public Journal Get(long id)
        {
           return repository.Get(id);
        }

        public IEnumerable<Journal> GetAll()
        {
            return repository.GetAll();
        }
        public PagingDataSet<Journal> Gets(int pageindex, int pagesize,string orderby)
        {
           return  repository.Gets(pageindex, pagesize,orderby);
        }

        /// <summary>
        /// 通过ID批量获得实体
        /// </summary>
        /// <param name="journalIds">ID号</param>
        /// <returns></returns>
        public IEnumerable<Journal> GetJournalsByIds(IEnumerable<long> journalIds)
        {
            return repository.PopulateEntitiesByEntityIds(journalIds);
        }

        public Dictionary<string, long> GetManageableDatas(string tenantTypeId=null)
        {
            Dictionary<string, long> manageableDatas = new Dictionary<string, long>();
            //todo:实现统计数据的代码
            
            
            return manageableDatas;
        }

        public Dictionary<string, long> GetStatisticDatas(string tenantTypeId=null)
        {
            Dictionary<string, long> manageableDatas = new Dictionary<string, long>();
            //todo:实现统计数据的代码


            return manageableDatas;
        }

        public IEnumerable<Journal> GetJournals(PagingDataSet<long> pdsObjectIds)
        {
            List<Journal> jlist = new List<Journal>();
            foreach (long id in pdsObjectIds)
            {
                var j = Get(id);
                if(j!=null)
                {
                    jlist.Add(j);
                }
            }

            return jlist;
        }
    }
}