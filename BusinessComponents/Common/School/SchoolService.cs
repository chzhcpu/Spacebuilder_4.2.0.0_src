//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-12-25</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-12-25" version="0.5">新建</log>
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
using NPinyin;

namespace Tunynet.Common
{
    /// <summary>
    /// 学校业务逻辑类
    /// </summary>
    public class SchoolService
    {
        private ISchoolRepository schoolRepository;

        /// <summary>
        /// 构造器方法
        /// </summary>
        public SchoolService()
            : this(new SchoolRepository())
        {

        }

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="schoolRepository"></param>
        public SchoolService(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        #region Create/Update/Delete

        /// <summary>
        /// 添加学校
        /// </summary>
        public void Create(School school)
        {
            EventBus<School>.Instance().OnBefore(school, new CommonEventArgs(EventOperationType.Instance().Create()));
            school.PinyinName = Pinyin.GetPinyin(school.Name);
            school.ShortPinyinName = Pinyin.GetInitials(school.Name);
            schoolRepository.Insert(school);
            school.DisplayOrder = school.Id;
            schoolRepository.Update(school);
            EventBus<School>.Instance().OnAfter(school, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 更新学校
        /// </summary>
        /// <param name="school">要更新的学校</param>
        /// <returns></returns>
        public void Update(School school)
        {
            EventBus<School>.Instance().OnBefore(school, new CommonEventArgs(EventOperationType.Instance().Update()));
            school.PinyinName = Pinyin.GetPinyin(school.Name);
            school.ShortPinyinName = Pinyin.GetInitials(school.Name);
            schoolRepository.Update(school);
            EventBus<School>.Instance().OnAfter(school, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除学校
        /// </summary>
        /// <param name="schoolId">学校编码</param>
        public void Delete(long schoolId)
        {
            School school = Get(schoolId);
            EventBus<School>.Instance().OnBefore(school, new CommonEventArgs(EventOperationType.Instance().Delete()));
            schoolRepository.Delete(school);
            EventBus<School>.Instance().OnAfter(school, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 变更学校的排列顺序
        /// </summary>
        /// <param name="id">待调整的Id</param>
        /// <param name="referenceId">参照Id</param>        
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            schoolRepository.ChangeDisplayOrder(id, referenceId);
        }

        #endregion


        #region Get & Gets

        /// <summary>
        /// 获取学校统计信息
        /// </summary>
        public School Get(long schoolId)
        {
            return schoolRepository.Get(schoolId);
        }

        /// <summary>
        /// 查询学校
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        /// <param name="keyword">关键词（支持拼音搜索）</param>
        /// <param name="schoolType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagingDataSet<School> Gets(string areaCode, string keyword, SchoolType? schoolType, int pageSize, int pageIndex)
        {

            return schoolRepository.Gets(areaCode, keyword, schoolType, pageSize, pageIndex);



            //设计要点：
            //1.缓存策略：当Keyword为null时，使用AreaCode分区缓存，否则不使用缓存；
            //2.缓存周期：稳定数据；
            //3.keyword:Name、PinyinName、ShortPinyinName,支持模糊搜索；
            //4.排序：按照DisplayOrder升序排序；



        }

        #endregion
    }
}
