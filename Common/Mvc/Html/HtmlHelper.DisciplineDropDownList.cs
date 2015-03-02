//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using Tunynet.Utilities;
using Spacebuilder;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 地区下列表
    /// </summary>
    public static class HtmlHelperDisciplineDropDownListExtensions
    {
        /// <summary>
        /// 地区下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">选择实体中类别属性的lamda表达式</param>
        /// <param name="disciplineLevel">地区层级(默认取站点地区配置）</param>
        /// <param name="rootDisciplineCode">根级地区（默认取站点地区配置）</param>
        public static MvcHtmlString DisciplineDropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, string>> expression, int? disciplineLevel = null, string rootDisciplineCode = null)
        {
            string getChildDisciplinesUrl = SiteUrls.Instance().GetChildDisciplines();
            DisciplineServiceHelper disciplineServiceHelper = new DisciplineServiceHelper();
            if (disciplineLevel == null)
            {
                DisciplineSettings disciplineSettings = DIContainer.Resolve<ISettingsManager<DisciplineSettings>>().Get();
                disciplineLevel = disciplineSettings.DisciplineLevel;
            }
            return htmlHelper.LinkageDropDownListFor<TModel, string>(expression, string.Empty, disciplineLevel.Value, disciplineServiceHelper.GetRootDisciplineDictionary(rootDisciplineCode), disciplineServiceHelper.GetParentCode, disciplineServiceHelper.GetChildrenDictionary, getChildDisciplinesUrl);
        }

        /// <summary>
        /// 地区下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">控件name属性</param>
        /// <param name="name">选中的地区编码</param>
        /// <param name="disciplineLevel">地区层级(默认取站点配置）</param>
        /// <param name="rootDisciplineCode">根级地区（默认取站点地区配置）</param>
        public static MvcHtmlString DisciplineDropDownList(this HtmlHelper htmlHelper, string name, string value, int? disciplineLevel = null, string rootDisciplineCode = null)
        {
            string getChildDisciplinesUrl = SiteUrls.Instance().GetChildDisciplines();
            DisciplineServiceHelper disciplineServiceHelper = new DisciplineServiceHelper();
            if (disciplineLevel == null)
            {
                DisciplineSettings disciplineSettings = DIContainer.Resolve<ISettingsManager<DisciplineSettings>>().Get();
                disciplineLevel = disciplineSettings.DisciplineLevel;
            }
            return htmlHelper.LinkageDropDownList<string>(name, value, string.Empty, disciplineLevel.Value, disciplineServiceHelper.GetRootDisciplineDictionary(rootDisciplineCode), disciplineServiceHelper.GetParentCode, disciplineServiceHelper.GetChildrenDictionary, getChildDisciplinesUrl);
        }
    }

    /// <summary>
    /// 地区业务逻辑扩展类
    /// </summary>
    internal class DisciplineServiceHelper
    {
        DisciplineService disciplineService = new DisciplineService();

        /// <summary>
        /// 获取父地区编码
        /// </summary>
        public string GetParentCode(string disciplineCode)
        {
            Discipline discipline = disciplineService.Get(disciplineCode);
            if (discipline != null)
                return discipline.ParentCode;
            return string.Empty;
        }

        /// <summary>
        /// 获取子地区
        /// </summary>
        public Dictionary<string, string> GetChildrenDictionary(string disciplineCode)
        {
            Discipline discipline = disciplineService.Get(disciplineCode);
            if (discipline != null)
                return discipline.Children.ToDictionary(n => n.DisciplineCode, n => n.Name);
            return null;
        }

        /// <summary>
        /// 获取一级地区
        /// </summary>
        /// <param name="rootDisciplineCode">根级地区（默认取站点地区配置）</param>
        public Dictionary<string, string> GetRootDisciplineDictionary(string rootDisciplineCode = null)
        {
            if (rootDisciplineCode == null)
            {
                ISettingsManager<DisciplineSettings> disciplineSettingsManager = DIContainer.Resolve<ISettingsManager<DisciplineSettings>>();
                if (disciplineSettingsManager != null)
                {
                    DisciplineSettings disciplineSettings = disciplineSettingsManager.Get();
                    rootDisciplineCode = disciplineSettings.RootDisciplineCode;
                }
            }
            //获取根级地区
            IEnumerable<Discipline> disciplines = null;
            if (!string.IsNullOrEmpty(rootDisciplineCode))
            {
                Discipline discipline = disciplineService.Get(rootDisciplineCode);
                if (discipline != null)
                    disciplines = discipline.Children;
            }
            else
                disciplines = disciplineService.GetRoots();
            if (disciplines == null)
                return null;

            return disciplines.ToDictionary(n => n.DisciplineCode, n => n.Name);
        }
    }
}