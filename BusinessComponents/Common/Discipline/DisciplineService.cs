//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2009</copyright>
//<version>V0.5</verion>
//<createdate>2012年5月8日15:15:03</createdate>
//<author>bianchx</author>
//<email>bianchx@tunynet.com</email>
//<log date="2012-5-8 15:14:58">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Tunynet.Caching;
using System.Linq;
using Tunynet.Common.Repositories;
using Tunynet.Events;


namespace Tunynet.Common
{
    /// <summary>
    /// 地区业务逻辑类
    /// </summary>
    public class DisciplineService
    {
        private IDisciplineRepository disciplineRepository;

        /// <summary>
        /// 构造器方法
        /// </summary>
        public DisciplineService()
            : this(new DisciplineRepository())
        {

        }

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="disciplineRepository"></param>
        public DisciplineService(IDisciplineRepository disciplineRepository)
        {
            this.disciplineRepository = disciplineRepository;
        }

        #region Create/Update/Delete

        /// <summary>
        /// 添加地区
        /// </summary>
        public void Create(Discipline discipline)
        {
            EventBus<Discipline>.Instance().OnBefore(discipline, new CommonEventArgs(EventOperationType.Instance().Create()));
            disciplineRepository.Insert(discipline);
            EventBus<Discipline>.Instance().OnAfter(discipline, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 更新地区
        /// </summary>
        /// <param name="discipline">要更新的地区</param>
        /// <returns></returns>
        public void Update(Discipline discipline)
        {
            EventBus<Discipline>.Instance().OnBefore(discipline, new CommonEventArgs(EventOperationType.Instance().Update()));
            disciplineRepository.Update(discipline);
            EventBus<Discipline>.Instance().OnAfter(discipline, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除地区
        /// </summary>
        /// <param name="disciplineCode">地区编码</param>
        public void Delete(string disciplineCode)
        {
            Discipline discipline = Get(disciplineCode);
            EventBus<Discipline>.Instance().OnBefore(discipline, new CommonEventArgs(EventOperationType.Instance().Delete()));
            disciplineRepository.Delete(disciplineCode);
            EventBus<Discipline>.Instance().OnAfter(discipline, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        #endregion

        #region Get & Gets

        /// <summary>
        /// 获取地区统计信息
        /// </summary>
        public Discipline Get(string disciplineCode)
        {
            return disciplineRepository.Get(disciplineCode);
        }


        /// <summary>
        /// 获取顶级地区集合
        /// </summary>
        public IEnumerable<Discipline> GetRoots()
        {
            return disciplineRepository.GetRoots();
        }

        /// <summary>
        /// 获取后代地区
        /// </summary>
        /// <param name="parentDisciplineCode">父级地区编码</param>
        public IEnumerable<Discipline> GetDescendants(string parentDisciplineCode)
        {
            return disciplineRepository.GetDescendants(parentDisciplineCode);
        }

        /// <summary>
        /// 判断地区是否父级地区
        /// </summary>
        /// <param name="discipline"></param>
        /// <param name="parentDisciplineCode"></param>
        /// <returns></returns>
        public bool IsChildDiscipline(string discipline, string parentDisciplineCode)
        {
            List<Discipline> disciplines = new List<Discipline>();
            RecursiveGetAllParentDiscipline(Get(discipline), ref disciplines);
            return disciplines.Any(n => n.DisciplineCode.Equals(parentDisciplineCode));
        }

        /// <summary>
        /// 获取所有父级地区
        /// </summary>
        /// <param name="disciplineCode"></param>
        /// <returns></returns>
        public List<Discipline> GetAllParentDisciplines(string disciplineCode)
        {
            List<Discipline> disciplines = new List<Discipline>();
            RecursiveGetAllParentDiscipline(Get(disciplineCode), ref disciplines);
            return disciplines;
        }

        /// <summary>
        /// 获取所有的父级地区
        /// </summary>
        /// <param name="discipline"></param>
        /// <param name="disciplines"></param>
        private void RecursiveGetAllParentDiscipline(Discipline discipline, ref List<Discipline> disciplines)
        {
            if (discipline == null || string.IsNullOrEmpty(discipline.ParentCode.Trim()))
                return;
            Discipline parentDiscipline = disciplineRepository.Get(discipline.ParentCode);
            disciplines.Add(parentDiscipline);
            RecursiveGetAllParentDiscipline(parentDiscipline, ref disciplines);
        }

        #endregion
    }
}
