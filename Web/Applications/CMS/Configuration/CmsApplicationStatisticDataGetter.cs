//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯应用数据Url获取器
    /// </summary>
    public class CmsApplicationStatisticDataGetter : IApplicationStatisticDataGetter
    {
        private ContentItemService contentItemService = new ContentItemService();

        /// <summary>
        /// 获取管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>管理数据列表</returns>
        public IEnumerable<ApplicationStatisticData> GetManageableDatas(string tenantTypeId = null)
        {
            List<ApplicationStatisticData> applicationStatisticDatas = new List<ApplicationStatisticData>();
            Dictionary<string, long> manageableDatas = contentItemService.GetManageableDatas(tenantTypeId);

            if (manageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().PendingCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().PendingCount(), "资讯",
                 "资讯待审核数", manageableDatas[ApplicationStatisticDataKeys.Instance().PendingCount()])
                {
                    DescriptionPattern = "{0}个资讯待审核",
                    Url = SiteUrls.Instance().CmsControlPanelManage(auditStatus: AuditStatus.Pending)
                });
            if (manageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().AgainCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().AgainCount(), "资讯",
                 "资讯需再审核数", manageableDatas[ApplicationStatisticDataKeys.Instance().AgainCount()])
                {
                    DescriptionPattern = "{0}个资讯需再审核",
                    Url = SiteUrls.Instance().CmsControlPanelManage(auditStatus: AuditStatus.Again)
                });
            return applicationStatisticDatas;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>统计数据列表</returns>
        public IEnumerable<ApplicationStatisticData> GetStatisticDatas(string tenantTypeId = null)
        {
            IList<ApplicationStatisticData> applicationStatisticDatas = new List<ApplicationStatisticData>();
            Dictionary<string, long> statisticDatas = contentItemService.GetStatisticDatas(tenantTypeId);
            if (statisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().TotalCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().TotalCount(), "资讯",
                 "资讯总数", statisticDatas[ApplicationStatisticDataKeys.Instance().TotalCount()])
                {
                    DescriptionPattern = "共{0}个资讯",
                    Url = SiteUrls.Instance().CmsControlPanelManage()
                });
            if (statisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().Last24HCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().Last24HCount(), "资讯",
                 "资讯24小时新增数", statisticDatas[ApplicationStatisticDataKeys.Instance().Last24HCount()])
                {
                    DescriptionPattern = "24小时新增{0}个资讯",
                    Url = SiteUrls.Instance().CmsControlPanelManage()
                });
            return applicationStatisticDatas;
        }
    }
}