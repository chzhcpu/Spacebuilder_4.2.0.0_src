//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Xml.Linq;
using Autofac;
using Tunynet.Common;
using Tunynet.Globalization;
using Tunynet.Common.Configuration;
using Tunynet.Events;
using Spacebuilder.Common;
using SpecialTopic.Topic.EventModules;
using Spacebuilder.Search;
using Tunynet.UI;
using SpecialTopic.Topic.Configuration;
using System.Collections.Generic;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题配置类
    /// </summary>
    [Serializable]
    public class TopicConfig : ApplicationConfig
    {
        private static int applicationId = 9002;
        private XElement tenantLogoSettingsElement;

        /// <summary>
        /// 获取TopicConfig实例
        /// </summary>
        public static TopicConfig Instance()
        {
            ApplicationService applicationService = new ApplicationService();

            ApplicationBase app = applicationService.Get(applicationId);
            if (app != null)
                return app.Config as TopicConfig;
            else
                return null;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="xElement"></param>
        public TopicConfig(XElement xElement)
            : base(xElement)
        {
            this.tenantLogoSettingsElement = xElement.Element("tenantLogoSettings");
            XAttribute att = xElement.Attribute("minUserRankOfCreateTopic");
            if (att != null)
                int.TryParse(att.Value, out this.minUserRankOfCreateTopic);
        }

        private int minUserRankOfCreateTopic = 5;
        /// <summary>
        /// 允许用户创建专题的最小等级数
        /// </summary>
        public int MinUserRankOfCreateTopic
        {
            get { return minUserRankOfCreateTopic; }
        }

        private int maxDaysOfCreateMemeberActivity = 3;
        /// <summary>
        /// 专题有新成员加入动态的最大天数
        /// </summary>
        /// <remarks>超过此天数的新成员将不会在动态中显示，防止已加入时间很久的成员出现在动态中</remarks>
        public int MaxDaysOfCreateMemeberActivity
        {
            get { return maxDaysOfCreateMemeberActivity; }
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
            get { return "Topic"; }
        }

        /// <summary>
        /// 获取TopicApplication实例
        /// </summary>
        public override Type ApplicationType
        {
            get { return typeof(TopicApplication); }
        }

        /// <summary>
        /// 应用初始化
        /// </summary>
        /// <param name="containerBuilder">容器构建器</param>
        public override void Initialize(ContainerBuilder containerBuilder)
        {
            //注册标识图设置
            TenantLogoSettings.RegisterSettings(tenantLogoSettingsElement);

            //注册ResourceAccessor的应用资源
            ResourceAccessor.RegisterApplicationResourceManager(ApplicationId, "SpecialTopic.Topic.Resources.Resource", typeof(SpecialTopic.Topic.Resources.Resource).Assembly);
            InvitationType.Register(new InvitationType { Key = InvitationTypeKeys.Instance().InviteJoinTopic(), Name = "邀请参加专题", Description = "" });
            InvitationType.Register(new InvitationType { Key = InvitationTypeKeys.Instance().ApplyJoinTopic(), Name = "申请加入专题", Description = "" });
            containerBuilder.Register(c => new TopicActivityReceiverGetter()).Named<IActivityReceiverGetter>(ActivityOwnerTypes.Instance().Topic().ToString()).SingleInstance();
            //groupId与groupKey的查询器
            containerBuilder.Register(c => new DefaultTopicIdToTopicKeyDictionary()).As<TopicIdToTopicKeyDictionary>().SingleInstance();

            //注册全文检索搜索器
            containerBuilder.Register(c => new TopicSearcher("专题", "~/App_Data/IndexFiles/Topic", true, 7)).As<ISearcher>().Named<ISearcher>(TopicSearcher.CODE).SingleInstance();

            ThemeService.RegisterThemeResolver(PresentAreaKeysOfExtension.TopicSpace, new TopicSpaceThemeResolver());

            //专题推荐
            containerBuilder.Register(c => new TopicApplicationStatisticDataGetter()).Named<IApplicationStatisticDataGetter>(this.ApplicationKey).SingleInstance();
            containerBuilder.Register(c => new TopicTenantAuthorizationHandler()).As<ITenantAuthorizationHandler>().SingleInstance();
        }

        /// <summary>
        /// 应用加载
        /// </summary>
        public override void Load()
        {
            base.Load();
            TagUrlGetterManager.RegisterGetter(TenantTypeIds.Instance().Topic(), new TopicTagUrlGetter());
            //注册专题计数服务
            CountService countService = new CountService(TenantTypeIds.Instance().Topic());
            countService.RegisterCounts();//注册计数服务
            countService.RegisterCountPerDay();//需要统计阶段计数时，需注册每日计数服务
            countService.RegisterStageCount(CountTypes.Instance().HitTimes(), 7);

            OwnerDataSettings.RegisterStatisticsDataKeys(TenantTypeIds.Instance().User()
                                                         , OwnerDataKeys.Instance().CreatedTopicCount()
                                                         , OwnerDataKeys.Instance().JoinedTopicCount());

            //添加应用管理员角色
            ApplicationAdministratorRoleNames.Add(applicationId, new List<string> { "TopicAdministrator" });
        }
    }
}