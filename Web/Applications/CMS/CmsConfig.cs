//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Xml.Linq;
using Autofac;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Tunynet.Globalization;
using System.Collections.Generic;
using System.Reflection;


namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯配置类
    /// </summary>
    [Serializable]
    public class CmsConfig : ApplicationConfig
    {
        private static int applicationId = 1015;
        private XElement tenantAttachmentSettingsElement;
        private XElement tenantCommentSettingsElement;

        /// <summary>
        /// 获取CMSConfig实例
        /// </summary>
        public static CmsConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(applicationId);
            if (app != null)
                return app.Config as CmsConfig;
            else
                return null;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="xElement"></param>
        public CmsConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantAttachmentSettingsElement = xElement.Element("tenantAttachmentSettings");
            this.tenantCommentSettingsElement = xElement.Element("tenantCommentSettings");
            XAttribute att = xElement.Attribute("enableSocialComment");
            if (att != null)
                bool.TryParse(att.Value, out this.enableSocialComment);
        }

        private bool enableSocialComment = false;
        /// <summary>
        /// 是否启用社会化评论组件
        /// </summary>
        public bool EnableSocialComment
        {
            get { return enableSocialComment; }
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
            get { return "CMS"; }
        }


        /// <summary>
        /// 获取CMSApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(CmsApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {
            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "Spacebuilder.CMS.Resources.Resource", typeof(Spacebuilder.CMS.Resources.Resource).Assembly);

            //注册EventModule
            containerBuilder.RegisterAssemblyTypes(Assembly.Load("Spacebuilder.CMS")).Where(t => typeof(IEventMoudle).IsAssignableFrom(t)).As<IEventMoudle>().SingleInstance();

            //注册全文检索搜索器
            containerBuilder.Register(c => new CmsSearcher("资讯", "~/App_Data/IndexFiles/Cms", true, 4)).As<ISearcher>().Named<ISearcher>(CmsSearcher.CODE).SingleInstance();

            //资讯应用数据统计
            containerBuilder.Register(c => new CmsApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();

            //注册资讯解析器
            containerBuilder.Register(c => new CmsBodyProcessor()).Named<IBodyProcessor>(TenantTypeIds.Instance().ContentItem()).SingleInstance();

            //注册附件设置
            TenantAttachmentSettings.RegisterSettings(tenantAttachmentSettingsElement);

            //评论设置的注册
            TenantCommentSettings.RegisterSettings(tenantCommentSettingsElement);
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
            //注册计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().ContentItem());
            countService.RegisterCounts();
            countService.RegisterCountPerDay();
            countService.RegisterStageCount(CountTypes.Instance().HitTimes(), 7);
            countService.RegisterStageCount(CountTypes.Instance().CommentCount(), 7);

            countService = new CountService(TenantTypeIds.Instance().ContentAttachment());
            countService.RegisterCounts();

            //注册用户计数服务（用于内容计数）
            OwnerDataSettings.RegisterStatisticsDataKeys(TenantTypeIds.Instance().User(), OwnerDataKeys.Instance().ContributeCount());

            //注册标签云标签链接接口实现
            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().ContentItem(), new CmsTagUrlGetter());

            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(applicationId, new List<string> { "CMSAdministrator" });

        }
    }
}