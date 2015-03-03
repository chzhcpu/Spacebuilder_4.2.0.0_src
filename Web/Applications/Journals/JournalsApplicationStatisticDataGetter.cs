using System;
using System.Collections.Generic;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Spacebuilder.Journals
{
    public class JournalsApplicationStatisticDataGetter : IApplicationStatisticDataGetter
    {
        private JournalsService journalService = new JournalsService();

        /// <summary>
        /// 获取管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>管理数据列表</returns>
        public IEnumerable<ApplicationStatisticData> GetManageableDatas(string tenantTypeId = null)
        {
            List<ApplicationStatisticData> applicationStatisticDatas = new List<ApplicationStatisticData>();
            Dictionary<string, long> manageableDatas = journalService.GetManageableDatas(tenantTypeId);

            if (manageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().PendingCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().PendingCount(), "日志",
                 "杂志待审核数", manageableDatas[ApplicationStatisticDataKeys.Instance().PendingCount()])
                {
                    DescriptionPattern = "{0}个杂志待审核",
                    Url ="" //SiteUrls.Instance().BlogControlPanelManage(auditStatus: AuditStatus.Pending)
                });
            if (manageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().AgainCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().AgainCount(), "日志",
                 "杂志需再审核数", manageableDatas[ApplicationStatisticDataKeys.Instance().AgainCount()])
                {
                    DescriptionPattern = "{0}个杂志需再审核",
                    Url ="" // SiteUrls.Instance().BlogControlPanelManage(auditStatus: AuditStatus.Again)
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
            Dictionary<string, long> statisticDatas = journalService.GetStatisticDatas(tenantTypeId);
            if (statisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().TotalCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().TotalCount(), "日志",
                 "杂志总数", statisticDatas[ApplicationStatisticDataKeys.Instance().TotalCount()])
                {
                    DescriptionPattern = "共{0}个杂志",
                    Url ="" // SiteUrls.Instance().BlogControlPanelManage()
                });
            if (statisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().Last24HCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().Last24HCount(), "日志",
                 "杂志24小时新增数", statisticDatas[ApplicationStatisticDataKeys.Instance().Last24HCount()])
                {
                    DescriptionPattern = "24小时新增{0}个杂志",
                    Url = "" //SiteUrls.Instance().BlogControlPanelManage()
                });
            return applicationStatisticDatas;
        }
    }
}