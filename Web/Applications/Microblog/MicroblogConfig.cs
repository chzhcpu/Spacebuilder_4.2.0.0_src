//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-8-2</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-8-2" version="0.5">创建</log>
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
using Spacebuilder.Microblog.EventModules;
using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Globalization;
using Tunynet;


namespace Spacebuilder.Microblog
{
    [Serializable]
    public class MicroblogConfig : ApplicationConfig
    {
        private XElement tenantAttachmentSettingsElement;

        /// <summary>
        /// 获取MicroblogConfig实例
        /// </summary>
        public static MicroblogConfig Instance()
        {

            ApplicationBase app =DIContainer.Resolve<ApplicationService>().Get(ApplicationIds.Instance().Microblog());
            if (app != null)
                return app.Config as MicroblogConfig;
            else
                return null;
        }


        public MicroblogConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantAttachmentSettingsElement = xElement.Element("tenantAttachmentSettings");
        }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public override int ApplicationId
        {
            get { return ApplicationIds.Instance().Microblog(); }
        }

        /// <summary>
        /// ApplicationKey
        /// </summary>
        public override string ApplicationKey
        {
            get { return ApplicationKeys.Instance().Microblog(); }
        }

        /// <summary>
        /// 获取SocialDiscussApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(MicroblogApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {

            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.Microblog.Resources.Resource", typeof(Spacebuilder.Microblog.Resources.Resource).Assembly);

            //注册附件设置
            TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);
            containerBuilder.Register(c => new MicroblogSearcher("微博", "~/App_Data/IndexFiles/Microblog", true, 2)).As<ISearcher>().Named<ISearcher>(MicroblogSearcher.CODE).SingleInstance();
            containerBuilder.Register(c => new MicroblogBodyProcessor()).As<IMicroblogBodyProcessor>().SingleInstance();
            containerBuilder.Register(c => new MicroblogApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();
            containerBuilder.Register(c => new MicroblogTenantAuthorizationHandler()).As<ITenantAuthorizationHandler>().SingleInstance();
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();

            //注册日志Rss浏览计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().Microblog());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().CommentCount(), 1);

            //注册微博用户计数服务
            List<string> tenantTypeIds = new List<string>() { TenantTypeIds.Instance().User(), TenantTypeIds.Instance().Group() };
            OwnerDataSettings.RegisterStatisticsDataKeys(tenantTypeIds, OwnerDataKeys.Instance().ThreadCount());

            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().Microblog(), new MicroblogTagUrlGetter());

            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(ApplicationIds.Instance().Microblog(), new List<string> { "MicroblogAdministrator" });
        }
    }
}