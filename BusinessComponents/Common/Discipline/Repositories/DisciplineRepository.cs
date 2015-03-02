//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Common;
using Tunynet.Caching;
using System.Collections.Concurrent;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 地区的数据访问类
    /// </summary>
    public class DisciplineRepository : IDisciplineRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 缓存设置
        /// </summary>
        protected static RealTimeCacheHelper RealTimeCacheHelper { get { return EntityData.ForType(typeof(Discipline)).RealTimeCacheHelper; } }


        /// <summary>
        /// 默认Database实例
        /// </summary>
        protected Database CreateDAO()
        {
            return Database.CreateInstance();
        }



        #region Insert,Delete,Update

        /// <summary>
        /// 插入地区数据
        /// </summary>
        /// <param name="discipline"></param>
        public void Insert(Discipline discipline)
        {

            Database database = CreateDAO();
            database.OpenSharedConnection();
            if (string.IsNullOrEmpty(discipline.ParentCode))
            {
                discipline.Depth = 0;
                discipline.ChildCount = 0;
            }
            else
            {
                Discipline disciplineParent = Get(discipline.ParentCode);
                if (disciplineParent == null)
                    return;
                discipline.Depth = disciplineParent.Depth + 1;
                discipline.ChildCount = 0;
            }
            object disciplineCode = database.Insert(discipline);
            if (disciplineCode != null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("update tn_Disciplines set ChildCount=ChildCount+1 where DisciplineCode=@0", discipline.ParentCode);
                database.Execute(sql);
            }
            var sql_selectMaxDisplayOrder = Sql.Builder.Append("select MAX(DisplayOrder) from tn_Disciplines", discipline.DisciplineCode);
            int maxvalue = database.Execute(sql_selectMaxDisplayOrder);
            string sqltext = string.Format("update tn_Disciplines set DisplayOrder= {0}+1 where DisciplineCode = @0", maxvalue);

            var sql_SetDisplayOrder = Sql.Builder
                .Append(sqltext, discipline.DisciplineCode);
            database.Execute(sql_SetDisplayOrder);

            database.CloseSharedConnection();
            //清空缓存
            ClearChache();
        }

        /// <summary>
        /// 更新子节点信息
        /// </summary>
        /// <param name="discipline">要更新的地区实体</param>
        /// <returns>更新之后的实体</returns>
        public void Update(Discipline discipline)
        {
            Database database = CreateDAO();
            int newParentDepth = 0;
            //过滤错误：判定，如果不为空但是取不到就是错的
            if (!string.IsNullOrEmpty(discipline.ParentCode))
            {
                Discipline newParentDiscipline = Get(discipline.ParentCode);
                if (newParentDiscipline != null)
                    newParentDepth = newParentDiscipline.Depth;
                else
                    return;
            }

            var sql_selete = PetaPoco.Sql.Builder;
            sql_selete.Select("*").From("tn_Disciplines")
                .Where("DisciplineCode = @0", discipline.DisciplineCode);
            Discipline oldDiscipline = database.FirstOrDefault<Discipline>(sql_selete);

            discipline.Depth = newParentDepth + 1;
            IList<PetaPoco.Sql> sql_updates = new List<PetaPoco.Sql>();
            //在没有更新父节点的情况下，仅更新自身的属性。
            sql_updates.Add(new PetaPoco.Sql("update tn_Disciplines set Name = @1,PostCode = @2,DisplayOrder = @3 where DisciplineCode= @0", discipline.DisciplineCode, discipline.Name, discipline.PostCode, discipline.DisplayOrder));
            //如果用户调整了父节点
            if (discipline.ParentCode.ToLower() != oldDiscipline.ParentCode.ToLower())
            {
                //如果用户更新了其父节点，更新自己的深度，更新原来的父节点和新的父节点的childcount
                sql_updates.Add(new PetaPoco.Sql("update tn_Disciplines set Depth = @1,ParentCode = @2 where DisciplineCode = @0", discipline.DisciplineCode, discipline.Depth, discipline.ParentCode));
                sql_updates.Add(new PetaPoco.Sql("update tn_Disciplines set ChildCount = ChildCount - 1 where DisciplineCode = @0", oldDiscipline.ParentCode));
                sql_updates.Add(new PetaPoco.Sql("update tn_Disciplines set ChildCount = ChildCount + 1 where DisciplineCode = @0", discipline.ParentCode));

                int differenceDepth = discipline.Depth - oldDiscipline.Depth;

                //如果原来的父节点与新的父节点不是在同一等级上，更新所有的子节点的深度。
                if (differenceDepth != 0)
                {
                    IEnumerable<Discipline> childDisciplines = GetDescendants(discipline.DisciplineCode);
                    if (childDisciplines != null && childDisciplines.Count() > 0)
                    {
                        foreach (Discipline childDiscipline in childDisciplines)
                            sql_updates.Add(new PetaPoco.Sql("update tn_Disciplines set Depth = Depth + @1 where DisciplineCode = @0", childDiscipline.DisciplineCode, differenceDepth));
                    }
                }
            }
            database.Execute(sql_updates);
            ClearChache();
        }

        /// <summary>
        /// 删除地区点
        /// </summary>
        /// <param name="disciplineCode">地区编码</param>
        public void Delete(string disciplineCode)
        {
            //删除数据库数据
            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();
            sqls.Add(new PetaPoco.Sql("delete from tn_Disciplines where DisciplineCode = @0", disciplineCode));

            IEnumerable<Discipline> descendantDisciplines = GetDescendants((string)disciplineCode);
            foreach (Discipline item in descendantDisciplines)
            {
                sqls.Add(new PetaPoco.Sql("delete from tn_Disciplines where DisciplineCode = @0", item.DisciplineCode));
            }

            Discipline discipline = Get(disciplineCode);
            sqls.Add(new PetaPoco.Sql("update tn_Disciplines set ChildCount=ChildCount-1 where DisciplineCode=@0", discipline.ParentCode));
            CreateDAO().Execute(sqls);
            //更新缓存
            ClearChache();
        }

        #endregion

        #region Get && Gets
        /// <summary>
        /// 获取某一地区
        /// </summary>
        /// <param name="disciplineCode">地区编码</param>
        /// <returns>地区实体</returns>
        public Discipline Get(string disciplineCode)
        {
            Dictionary<string, Discipline> disciplineDictionary = GetDisciplineDictionary();
            if (disciplineDictionary.ContainsKey(disciplineCode))
                return disciplineDictionary[disciplineCode];
            return null;
        }

        /// <summary>
        /// 获取所有子地区
        /// </summary>
        public IEnumerable<Discipline> GetDescendants(string parentDisciplineCode)
        {
            Discipline parentDiscipline = Get(parentDisciplineCode);
            Dictionary<string, Discipline> allChildDisciplines = new Dictionary<string, Discipline>();
            if (parentDiscipline != null)
                RecursiveGetAllDisciplines(parentDiscipline, ref allChildDisciplines);

            return allChildDisciplines.Values;
        }

        /// <summary>
        /// 获取根地区
        /// </summary>
        /// <returns>根地区列表</returns>
        public IEnumerable<Discipline> GetRoots()
        {
            List<Discipline> rootDisciplines = cacheService.Get<List<Discipline>>(GetCacheKey_DisciplineRootIEnumerable());
            if (rootDisciplines == null)
            {
                Dictionary<string, Discipline> disciplineDictionary = GetDisciplineDictionary();
                if (disciplineDictionary == null)
                    return null;
                rootDisciplines = disciplineDictionary.Values.Where(n => string.IsNullOrEmpty(n.ParentCode)).ToList();
                cacheService.Add(GetCacheKey_DisciplineRootIEnumerable(), rootDisciplines, CachingExpirationType.Stable);
            }
            return rootDisciplines;
        }

        #endregion

        #region Help Methods
        /// <summary>
        /// 获取地区的字典类型
        /// </summary>
        /// <returns>地区的字典类型</returns>
        private Dictionary<string, Discipline> GetDisciplineDictionary()
        {
            string cachekey = GetCacheKey_DisciplineDictionary();
            Dictionary<string, Discipline> disciplineDictionary = cacheService.Get<Dictionary<string, Discipline>>(cachekey);
            if (disciplineDictionary == null)
            {
                disciplineDictionary = GetAllDisciplines();
                cacheService.Add(cachekey, disciplineDictionary, CachingExpirationType.Stable);
            }
            return disciplineDictionary;
        }
        /// <summary>
        /// 递归获取parentDiscipline所有子Discipline
        /// </summary>
        /// <param name="parentDiscipline">父地区</param>
        /// <param name="allChildDisciplines">递归获取的所有子地区</param>
        private void RecursiveGetAllDisciplines(Discipline parentDiscipline, ref Dictionary<string, Discipline> allChildDisciplines)
        {
            if (parentDiscipline.Children.Count() > 0)
            {
                foreach (Discipline discipline in parentDiscipline.Children)
                {
                    allChildDisciplines[discipline.DisciplineCode] = discipline;
                    RecursiveGetAllDisciplines(discipline, ref allChildDisciplines);
                }
            }
        }

        /// <summary>
        /// 获取全部的地区的方法
        /// </summary>
        /// <returns>所有的地区</returns>
        private Dictionary<string, Discipline> GetAllDisciplines()
        {
            Database database = CreateDAO();

            database.OpenSharedConnection();
            var sql = PetaPoco.Sql.Builder;
            sql.Append("select Max(Depth) from tn_Disciplines");
            int maxDepth = database.FirstOrDefault<int?>(sql) ?? 0;
            var sql_GetAll = PetaPoco.Sql.Builder;
            sql_GetAll.Select("*")
                .From("tn_Disciplines")
                .OrderBy("DisplayOrder,DisciplineCode");
            List<Discipline> DisciplinesList = database.Fetch<Discipline>(sql_GetAll);
            database.CloseSharedConnection();
            return Organize(DisciplinesList, maxDepth);
        }

        /// <summary>
        /// 生成类别深度信息并对类别进行计数统计
        /// </summary>
        private Dictionary<string, Discipline> Organize(List<Discipline> allDisciplines, int maxDepth)
        {
            Dictionary<string, Discipline>[] disciplinesDictionaryArray = new Dictionary<string, Discipline>[maxDepth + 1];
            for (int i = 0; i <= maxDepth; i++)
            {
                disciplinesDictionaryArray[i] = new Dictionary<string, Discipline>();
            }

            foreach (Discipline _discipline in allDisciplines)
            {
                disciplinesDictionaryArray[_discipline.Depth][_discipline.DisciplineCode] = _discipline;
            }

            //组织Discipline.Childs
            for (int i = maxDepth; i > 0; i--)
            {
                foreach (KeyValuePair<string, Discipline> pair in disciplinesDictionaryArray[i])
                {
                    disciplinesDictionaryArray[i - 1][pair.Value.ParentCode].AppendChild(pair.Value);
                }
            }

            Dictionary<string, Discipline> organizedDisciplines = new Dictionary<string, Discipline>();
            foreach (Discipline discipline in allDisciplines)
            {
                organizedDisciplines[discipline.DisciplineCode] = discipline;
            }
            return organizedDisciplines;
        }


        /// <summary>
        /// 获取地区的地点集合的cachekey
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey_DisciplineDictionary()
        {
            return "DisciplineDictionary" + RealTimeCacheHelper.GetGlobalVersion();
        }

        private string GetCacheKey_DisciplineRootIEnumerable()
        {
            return "DisciplineRootList" + RealTimeCacheHelper.GetGlobalVersion();
        }

        private void ClearChache()
        {
            RealTimeCacheHelper.IncreaseGlobalVersion();
        }
        #endregion

    }
}
