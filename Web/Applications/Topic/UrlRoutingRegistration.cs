
using System.Configuration;
using System.Web.Mvc;
using Spacebuilder.Common;

using Tunynet.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题路由设置
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Topic"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            #region Channel

            context.MapRoute(
              "Channel_Topic_Home", // Route name
              "Topic" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelTopic", action = "Home", CurrentNavigationId = "10900202" } // Parameter defaults
            );

            context.MapRoute(
              "Channel_Topic_Create", // Route name
              "Topic/Create" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelTopic", action = "Create" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Topic_UserTopics", // Route name
                "Topic/u-{spaceKey}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelTopic", action = "UserJoinedTopics", CurrentNavigationId = "10900203" }
            );

            context.MapRoute(
                "Channel_Topic_FindTopic", // Route name
                "Topic/FindTopic" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelTopic", action = "FindTopic", CurrentNavigationId = "10900204" }
            );

            context.MapRoute(
                "Channel_Topic_Created", // Route name
                "Topic/u-{spaceKey}/Created" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelTopic", action = "UserCreatedTopics", CurrentNavigationId = "10900203" }
            );

            context.MapRoute(
                "Channel_Topic_Tag", // Route name
                "Topic/t-{tagName}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelTopic", action = "ListByTag" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Topic_Common", // Route name
                "Topic/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelTopic" } // Parameter defaults
            );

            #endregion

            #region TopicActivity
            context.MapRoute(
                string.Format("ActivityDetail_{0}_CreateTopic", TenantTypeIds.Instance().Topic()), // Route name
                "TopicActivity/CreateThread/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelTopic", action = "_CreateTopic" } // Parameter defaults
            );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateTopicMember", TenantTypeIds.Instance().User()), // Route name
               "TopicActivity/CreateTopicMember/{ActivityId}" + extensionForOldIIS, // URL with parameters
               new { controller = "ChannelTopic", action = "_CreateTopicMember" } // Parameter defaults
           );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_JoinTopic", TenantTypeIds.Instance().User()), // Route name
               "TopicActivity/JoinTopic/{ActivityId}" + extensionForOldIIS, // URL with parameters
               new { controller = "ChannelTopic", action = "_JoinTopic" } // Parameter defaults
           );

            #endregion

            #region TopicSpace

            context.MapRoute(
                "TopicSpace_Member", // Route name
                "t/{SpaceKey}/Members" + extensionForOldIIS, // URL with parameters
                new { controller = "TopicSpace", action = "Members", CurrentNavigationId = "13900280" } // Parameter defaults
            );

            #endregion

            #region TopicSpaceTheme

            //专题空间首页
            context.MapRoute(
                "TopicSpaceTheme_Home", // Route name
                "t/{SpaceKey}" + extensionForOldIIS, // URL with parameters
                new { controller = "TopicSpaceTheme", action = "Home", CurrentNavigationId = "13101101" } // Parameter defaults
            );

            context.MapRoute(
                "TopicSpaceTheme_Common", // Route name
                "Topictheme/{SpaceKey}/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "TopicSpaceTheme", action = "Home" } // Parameter defaults
            );
            #endregion

            #region TopicSettings

            context.MapRoute(
                "TopicSpace_Settings_Common", // Route name
                "t/{SpaceKey}/settings/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "TopicSpaceSettings", action = "ManageMembers" } // Parameter defaults
            );

            #endregion

            context.MapRoute(
                "TopicSpace_Common", // Route name
                "t/{SpaceKey}/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "TopicSpace", action = "Home" } // Parameter defaults
            );


            #region ControlPanel

            context.MapRoute(
                "ControlPanel_Topic_Home", // Route name
                "ControlPanelTopic/ManageTopics" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTopic", action = "ManageTopics", CurrentNavigationId = "20900201", tenantTypeId = TenantTypeIds.Instance().Topic() } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Topic_Common", // Route name
                "ControlPanelTopic/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTopic", CurrentNavigationId = "20900210" } // Parameter defaults
            );

            #endregion



        }
    }
}