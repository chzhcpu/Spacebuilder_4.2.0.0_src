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
    public class AreaService
    {
        private IAreaRepository areaRepository;

        /// <summary>
        /// 构造器方法
        /// </summary>
        public AreaService()
            : this(new AreaRepository())
        {

        }

        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="areaRepository"></param>
        public AreaService(IAreaRepository areaRepository)
        {
            this.areaRepository = areaRepository;
        }

        #region Create/Update/Delete

        /// <summary>
        /// 添加地区
        /// </summary>
        public void Create(Area area)
        {
            EventBus<Area>.Instance().OnBefore(area, new CommonEventArgs(EventOperationType.Instance().Create()));
            areaRepository.Insert(area);
            EventBus<Area>.Instance().OnAfter(area, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 更新地区
        /// </summary>
        /// <param name="area">要更新的地区</param>
        /// <returns></returns>
        public void Update(Area area)
        {
            EventBus<Area>.Instance().OnBefore(area, new CommonEventArgs(EventOperationType.Instance().Update()));
            areaRepository.Update(area);
            EventBus<Area>.Instance().OnAfter(area, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除地区
        /// </summary>
        /// <param name="areaCode">地区编码</param>
        public void Delete(string areaCode)
        {
            Area area = Get(areaCode);
            EventBus<Area>.Instance().OnBefore(area, new CommonEventArgs(EventOperationType.Instance().Delete()));
            areaRepository.Delete(areaCode);
            EventBus<Area>.Instance().OnAfter(area, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        #endregion

        #region Get & Gets

        /// <summary>
        /// 获取地区统计信息
        /// </summary>
        public Area Get(string areaCode)
        {
            return areaRepository.Get(areaCode);
        }


        /// <summary>
        /// 获取顶级地区集合
        /// </summary>
        public IEnumerable<Area> GetRoots()
        {
            return areaRepository.GetRoots();
        }

        /// <summary>
        /// 获取后代地区
        /// </summary>
        /// <param name="parentAreaCode">父级地区编码</param>
        public IEnumerable<Area> GetDescendants(string parentAreaCode)
        {
            return areaRepository.GetDescendants(parentAreaCode);
        }

        /// <summary>
        /// 判断地区是否父级地区
        /// </summary>
        /// <param name="area"></param>
        /// <param name="parentAreaCode"></param>
        /// <returns></returns>
        public bool IsChildArea(string area, string parentAreaCode)
        {
            List<Area> areas = new List<Area>();
            RecursiveGetAllParentArea(Get(area), ref areas);
            return areas.Any(n => n.AreaCode.Equals(parentAreaCode));
        }

        /// <summary>
        /// 获取所有父级地区
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public List<Area> GetAllParentAreas(string areaCode)
        {
            List<Area> areas = new List<Area>();
            RecursiveGetAllParentArea(Get(areaCode), ref areas);
            return areas;
        }

        /// <summary>
        /// 获取所有的父级地区
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areas"></param>
        private void RecursiveGetAllParentArea(Area area, ref List<Area> areas)
        {
            if (area == null || string.IsNullOrEmpty(area.ParentCode.Trim()))
                return;
            Area parentArea = areaRepository.Get(area.ParentCode);
            areas.Add(parentArea);
            RecursiveGetAllParentArea(parentArea, ref areas);
        }

        #endregion
    }
}
