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
using Tunynet.Common;
using System.Xml.Linq;
using Autofac;
using Tunynet.Globalization;
using Spacebuilder.Common;
using Spacebuilder.Search;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// ApplicationConfig
    /// </summary>
    public class JournalsConfig : ApplicationConfig
    {
        private static int applicationId = 9001;

        /// <summary>
        /// 获取JournalsConfig实例
        /// </summary>
        public static JournalsConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(applicationId);
            if (app != null)
                return app.Config as JournalsConfig;
            else
                return null;
        }


        public JournalsConfig(XElement xElement)
            : base(xElement)
        {
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return applicationId; }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return "Journals"; }
        }

        /// <summary>
        /// 获取JournalsApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(JournalsApplication); }
        }

        /// <summary>
        /// 注册ResourceAccessor的应用资源
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {
            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Journals.Resources.Resource", typeof(Spacebuilder.Journals.Resources.Resource).Assembly);

            //注册附件设置
            //TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
            //TenantCommentSettings.RegisterSettings(tenantCommentSettingsElement);

            //注册日志正文解析器
            //containerBuilder.Register(c => new BlogBodyProcessor()).Named<IBodyProcessor>(TenantTypeIds.Instance().Blog()).SingleInstance();

            //注册EventModule
            //containerBuilder.Register(c => new BlogActivityReceiverGetter()).Named<IActivityReceiverGetter>(ActivityOwnerTypes.Instance().Blog().ToString()).SingleInstance();

            //注册全文检索搜索器
            containerBuilder.Register(c => new JournalSearcher("sci杂志", "~/App_Data/IndexFiles/Journals", true, 3)).As<ISearcher>().Named<ISearcher>(JournalSearcher.CODE).SingleInstance();

            //日志应用数据统计
            containerBuilder.Register(c => new JournalsApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();

        }

    }
}