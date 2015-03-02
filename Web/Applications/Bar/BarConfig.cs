//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-08-23</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-08-23" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Tunynet.Common;
using Autofac;
using Spacebuilder.Common;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Spacebuilder.Search;
using Spacebuilder.Bar.Search;
using Spacebuilder.Bar.EventModules;
using Tunynet.Globalization;

namespace Spacebuilder.Bar
{
    [Serializable]
    public class BarConfig : ApplicationConfig
    {
        private XElement tenantAttachmentSettingsElement;
        private XElement tenantLogoSettingsElement;

        /// <summary>
        /// 获取BarConfig实例
        /// </summary>
        public static BarConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(ApplicationIds.Instance().Bar());
            if (app != null)
                return app.Config as BarConfig;
            else
                return null;
        }


        public BarConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantAttachmentSettingsElement = xElement.Element("tenantAttachmentSettings");
            this.tenantLogoSettingsElement = xElement.Element("tenantLogoSettings");
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return ApplicationIds.Instance().Bar(); }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return ApplicationKeys.Instance().Bar(); }
        }

        /// <summary>
        /// 获取SocialDiscussApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(BarApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {

            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Bar.Resources.Resource", typeof(Spacebuilder.Bar.Resources.Resource).Assembly);

            //注册附件设置
            TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
            //注册标识图设置
            TenantLogoSettings.RegisterSettings(tenantLogoSettingsElement);

            //注册帖吧正文解析器
            containerBuilder.Register(c => new BarBodyProcessor()).Named<IBodyProcessor>(TenantTypeIds.Instance().Bar()).SingleInstance();
            containerBuilder.Register(c => new BarSectionActivityReceiverGetter()).Named<IActivityReceiverGetter>(ActivityOwnerTypes.Instance().BarSection().ToString()).SingleInstance();
            containerBuilder.Register(c => new BarSearcher("帖吧", "~/App_Data/IndexFiles/Bar", true, 6)).As<ISearcher>().Named<ISearcher>(BarSearcher.CODE).SingleInstance();

            containerBuilder.Register(c => new BarApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();

            containerBuilder.Register(c => new BarTenantAuthorizationHandler()).As<ITenantAuthorizationHandler>().SingleInstance();
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
            //注册帖吧计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().BarSection());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().ThreadAndPostCount(), 1);
            //注册帖子计数服务
            countService = new CountService(TenantTypeIds.Instance().BarThread());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().HitTimes(), 1, 7);


            //注册贴吧用户计数服务
            List<string> tenantTypeIds = new List<string>() { TenantTypeIds.Instance().User(), TenantTypeIds.Instance().Group() };
            OwnerDataSettings.RegisterStatisticsDataKeys(tenantTypeIds
                                                         , OwnerDataKeys.Instance().ThreadCount()
                                                         , OwnerDataKeys.Instance().PostCount()
                                                         , OwnerDataKeys.Instance().FollowSectionCount());

            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().BarThread(), new BarTagUrlGetter());
            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().Group(), new BarTagUrlGetter());
            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(ApplicationIds.Instance().Bar(), new List<string> { "BarAdministrator" });

        }
    }
}