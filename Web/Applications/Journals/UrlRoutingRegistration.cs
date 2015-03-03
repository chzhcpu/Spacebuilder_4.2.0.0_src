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
using System.Web.Mvc;
using System.Configuration;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// Url Routing注册
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        /// <summary>
        /// AreaName
        /// </summary>
        public override string AreaName
        {
            get { return "Journals"; }
        }

        /// <summary>
        /// 注册Area的Url路由数据
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            string routeNamePrefix = "Journal" + "_";

            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;

            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            //context.MapRoute(routeNamePrefix + "ChannelJournals_Common", "Journals/{action}" + extensionForOldIIS,
             //   new { action = "Home", Controller = "ChannelJournals", CurrentNavigationId = "109001001" });

            //j频道首页
            context.MapRoute(
              "Channel_Journals_Home", // Route name
              "Journals" + extensionForOldIIS , // URL with parameters
              new { controller = "ChannelJournals", action = "Home", 
                  CurrentNavigationId = "109001001" } // Parameter defaults}
            );
            //j详细页
            context.MapRoute(
              "UserSpace_Journals_Detail", // Route name
              "u/{SpaceKey}/J-{journalId}" + extensionForOldIIS, // URL with parameters
              new { controller = "Journals", action = "Detail",
                    CurrentNavigationId = "109001001" }, // Parameter defaults
              new { journalId = @"(\d+)|(\{\d+\})" }
            );
            //favorite
            /*context.MapRoute(
                "UserSpace_Journals_Favorites", // Route name
                "u/{SpaceKey}/journalsFavorites" + extensionForOldIIS, // URL with parameters
                new { controller = "Journals", action = "ListFavorites", CurrentNavigationId = "11900104" } // Parameter defaults
            );*/
            context.MapRoute(
                "UserSpace_Journals_MyFavorites", // Route name
                "journalsFavorites" + extensionForOldIIS, // URL with parameters
                new { controller = "Journals", action = "Favorite" } //, Parameter defaults
                //new {itemId=@"(\d+)|(\{\d+\})", userId=@"(\d+)|(\{\d+\})"}
            );

            //我的杂志/他的杂志
            context.MapRoute(
                "UserSpace_Journals_Journals", // Route name
                "u/{SpaceKey}/Journals" + extensionForOldIIS, // URL with parameters
                new { controller = "Journals", action = "Journals",
                     CurrentNavigationId = "109001001" 
                } // Parameter defaults
            );

            context.MapRoute(
                "journalsQuickSearch",
                "journals/QuickSearch" + extensionForOldIIS,
                new { controller = "ChannelJournals", action = "_QuickSearch" }
                );
            context.MapRoute(
                "journalsSearch",
                "journals/Search" + extensionForOldIIS,
                new { controller = "ChannelJournals", action = "Search" }
                );

            context.MapRoute(
                "journalsGlobalSearch",
                "journals/GlobalSearch" + extensionForOldIIS,
                new { controller = "ChannelJournals", action = "_GlobalSearch" }
                );

            context.MapRoute(
                "journalsSearchAutoComplete",
                "journals/SearchAutoComplete" + extensionForOldIIS,
                new { controller = "ChannelJournals", action = "SearchAutoComplete" }
                );

            //我的杂志/他的杂志
            /*context.MapRoute(
                "ApplicationCount_Journals", // Route name
                "u/{SpaceKey}/Journals" + extensionForOldIIS, // URL with parameters
                new { controller = "Journals", action = "Journals", CurrentNavigationId = "1090010019" } // Parameter defaults
            );*/

        }
    }
}