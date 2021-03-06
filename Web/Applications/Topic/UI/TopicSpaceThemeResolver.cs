﻿//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System.Web.Routing;
using Tunynet.UI;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet;
using System.Linq;
using Spacebuilder.UI;
using System.Text;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 当前皮肤获取器接口
    /// </summary>
    public class TopicSpaceThemeResolver : IThemeResolver
    {

        #region IThemeResolver 成员

        ThemeAppearance IThemeResolver.GetRequestTheme(RequestContext controllerContext)
        {
            string spaceKey = controllerContext.GetParameterFromRouteDataOrQueryString("SpaceKey");
            TopicEntity topic = new TopicService().Get(spaceKey);
            if (topic == null)
                throw new ExceptionFacade("找不到专题");

            string themeKey = null;
            string appearanceKey = null;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfExtension.TopicSpace);
            if (pa == null)
                throw new ExceptionFacade("找不到专题呈现区域");

            if (pa.EnableThemes)
            {
                if (topic.IsUseCustomStyle)
                {
                    themeKey = "Default";
                    appearanceKey = "Default";
                }
                else
                {
                    string[] themeAppearanceArray = topic.ThemeAppearance.Split(',');
                    var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfExtension.TopicSpace, topic.ThemeAppearance);

                    if (appearance != null && themeAppearanceArray.Count() == 2)
                    {
                        themeKey = themeAppearanceArray[0];
                        appearanceKey = themeAppearanceArray[1];
                    }
                }
            }
            if (themeKey == null || appearanceKey == null)
            {
                themeKey = pa.DefaultThemeKey;
                appearanceKey = pa.DefaultAppearanceKey;
            }

            return new ThemeService().GetThemeAppearance(PresentAreaKeysOfExtension.TopicSpace, themeKey, appearanceKey);
        }

        void IThemeResolver.IncludeStyle(RequestContext controllerContext)
        {
            string spaceKey = controllerContext.GetParameterFromRouteDataOrQueryString("SpaceKey");
            TopicEntity topic = new TopicService().Get(spaceKey);
            if (topic == null)
                return;
            PresentArea presentArea = new PresentAreaService().Get(PresentAreaKeysOfExtension.TopicSpace);
            if (presentArea == null)
                return;


            string themeKey = null;
            string appearanceKey = null;

            IPageResourceManager resourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            if (!presentArea.EnableThemes)
            {
                string themeCssPath = string.Format("{0}/{1}/theme.css", presentArea.ThemeLocation, presentArea.DefaultThemeKey);
                string appearanceCssPath = string.Format("{0}/{1}/Appearances/{2}/appearance.css", presentArea.ThemeLocation, presentArea.DefaultThemeKey, presentArea.DefaultAppearanceKey);

                resourceManager.IncludeStyle(themeCssPath);
                resourceManager.IncludeStyle(appearanceCssPath);
                return;
            }

            if (topic.IsUseCustomStyle)
            {
                var customStyleEntity = new CustomStyleService().Get(presentArea.PresentAreaKey, topic.TopicId);
                if (customStyleEntity == null)
                    return;
                CustomStyle customStyle = customStyleEntity.CustomStyle;
                if (customStyle == null)
                    return;
                string themeCssPath = string.Format("{0}/Custom/theme{1}.css", presentArea.ThemeLocation, customStyle.IsDark ? "-deep" : "");
                string appearanceCssPath = SiteUrls.Instance().CustomStyle(presentArea.PresentAreaKey, topic.TopicId);
                resourceManager.IncludeStyle(themeCssPath);
                resourceManager.IncludeStyle(appearanceCssPath);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(".tn-page-bg{");
                if (customStyle.IsUseBackgroundImage && !string.IsNullOrEmpty(customStyle.BackgroundImageStyle.Url))
                {
                    builder.AppendLine("background-image:url('" + customStyle.BackgroundImageStyle.Url + @"');");
                    builder.AppendFormat("background-repeat:{0};\n", customStyle.BackgroundImageStyle.IsRepeat ? "repeat" : "no-repeat");
                    builder.AppendFormat("background-attachment:{0};\n", customStyle.BackgroundImageStyle.IsFix ? "fixed" : "scroll");
                    string position = "center";
                    switch (customStyle.BackgroundImageStyle.BackgroundPosition)
                    {
                        case BackgroundPosition.Left:
                            position = "left";
                            break;
                        case BackgroundPosition.Center:
                            position = "center";
                            break;
                        case BackgroundPosition.Right:
                            position = "right";
                            break;
                        default:
                            position = "center";
                            break;
                    }
                    builder.AppendFormat("background-position:{0} top;\n", position);
                }

                builder.AppendLine("}");

                resourceManager.RegisterStyleBlock(builder.ToString());
            }
            else
            {
                string[] themeAppearanceArray = topic.ThemeAppearance.Split(',');
                var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfExtension.TopicSpace, topic.ThemeAppearance);

                if (appearance != null && themeAppearanceArray.Count() == 2)
                {
                    themeKey = themeAppearanceArray[0];
                    appearanceKey = themeAppearanceArray[1];
                }
                else
                {
                    themeKey = presentArea.DefaultThemeKey;
                    appearanceKey = presentArea.DefaultAppearanceKey;
                }

                string themeCssPath = string.Format("{0}/{1}/theme.css", presentArea.ThemeLocation, themeKey);
                string appearanceCssPath = string.Format("{0}/{1}/Appearances/{2}/appearance.css", presentArea.ThemeLocation, themeKey, appearanceKey);

                resourceManager.IncludeStyle(themeCssPath);
                resourceManager.IncludeStyle(appearanceCssPath);
            }


        }

        #endregion

        /// <summary>
        /// 获取用户当前选中的皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、专题Id）</param>
        /// <returns></returns>
        public string GetThemeAppearance(long ownerId)
        {
            var topicService = new TopicService();
            TopicEntity topic = topicService.Get(ownerId);
            if (topic == null)
                return string.Empty;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfExtension.TopicSpace);
            if (pa != null && !pa.EnableThemes)
            {
                return pa.DefaultThemeKey + "," + pa.DefaultAppearanceKey;
            }

            if (topic.IsUseCustomStyle)
            {
                return "Default,Default";
            }
            else if (!string.IsNullOrEmpty(topic.ThemeAppearance))
            {
                var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfExtension.TopicSpace, topic.ThemeAppearance);
                if (appearance != null)
                    return topic.ThemeAppearance;
            }

            if (pa != null)
            {
                return pa.DefaultThemeKey + "," + pa.DefaultAppearanceKey;
            }
            return string.Empty;
        }


        /// <summary>
        /// 验证当前用户是否修改皮肤的权限
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public bool Validate(long ownerId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfExtension.TopicSpace);
            if (!pa.EnableThemes)
                return false;

            if (DIContainer.Resolve<Authorizer>().Topic_Manage(new TopicService().Get(ownerId)))
                return true;
            return false;
        }

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、专题Id）</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">themeKey与appearanceKey用逗号关联</param>
        public void ChangeThemeAppearance(long ownerId, bool isUseCustomStyle, string themeAppearance)
        {
            var topicService = new TopicService();
            TopicEntity topic = topicService.Get(ownerId);
            if (topic == null)
                throw new ExceptionFacade("找不到专题");

            new ThemeService().ChangeThemeAppearanceUserCount(PresentAreaKeysOfExtension.TopicSpace, topic.IsUseCustomStyle ? string.Empty : topic.ThemeAppearance, isUseCustomStyle ? string.Empty : themeAppearance);
            new TopicService().ChangeThemeAppearance(ownerId, isUseCustomStyle, themeAppearance);
        }
    }
}
