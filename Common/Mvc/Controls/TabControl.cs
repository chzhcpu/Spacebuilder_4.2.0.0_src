//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Web.Routing;
using System.Web.Helpers;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Tabs控件
    /// </summary>
    /// <remarks> 
    /// <para>基于jQuery UI Tabs （V1.8.7）插件构建，更多信息请参见：http://jqueryui.com/demos/tabs/ </para>     
    /// <para>如果需要更多option设置，可以通过设置AdditionalParameters属性来实现</para>
    /// <para>依赖文件：jquery.cookie.js、jquery-ui.js</para>
    /// </remarks>
    public class TabControl
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public TabControl()
        {
            this.TabItems = new List<TabItem>();
            this.Spinner = "加载中...";
            this.CacheEnabled = true;
            this.PersistenceExpires = 30;
        }

        /// <summary>
        /// tab控件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否需要缓存远程内容
        /// </summary>
        public bool CacheEnabled { get; set; }

        /// <summary>
        /// 是否使用简洁样式的Tab分页
        /// </summary>
        public bool IsSimple { get; set; }

        /// <summary>
        /// 触发Tab页切换的Javascript事件名
        /// </summary>
        /// <value>默认为click</value>
        /// <example>click,表示点击触发；mouseover表示鼠标移上去时就触发</example>
        public string Event { get; set; }

        /// <summary>
        /// 通过cookie保持tab页的选择状态
        /// </summary>
        /// <example>30,表示使用cookie保持选中状态30天</example>
        public int PersistenceExpires { get; set; }

        /// <summary>
        /// 内容加载提示信息
        /// </summary>
        public string Spinner { get; set; }
        /// <summary>
        /// 额外的插件选项
        /// </summary>
        public Dictionary<string, object> AdditionalOptions { get; set; }
        /// <summary>
        /// 控件容器选中样式
        /// </summary>
        public string ContainerCss { get; set; }
        /// <summary>
        /// tab选中事件
        /// </summary>
        public string OnSelectCallBack { get; set; }

        /// <summary>
        /// Tab项集合
        /// </summary>
        public List<TabItem> TabItems { get; set; }


        /// <summary>
        /// 设置Tabs控件的Id
        /// </summary>
        /// <param name="name">Tabs控件Id</param>
        public TabControl SetName(string name)
        {
            this.Name = name; return this;
        }

        /// <summary>
        /// 设置Tabs控件的Id
        /// </summary>
        /// <param name="name">Tabs控件Id</param>
        public TabControl SetIsSimple(bool isSimple)
        {
            this.IsSimple = isSimple; return this;
        }

        /// <summary>
        /// 设置选中事件的Javascript回调函数<br/>
        /// 原型为 function(event, ui)
        /// </summary>
        /// <param name="onSelectCallBack">Javascript回调函数名</param>
        public TabControl SetOnSelectCallBack(string onSelectCallBack)
        {
            this.OnSelectCallBack = onSelectCallBack;
            return this;
        }

        /// <summary>
        /// 设置是否需要缓存远程内容
        /// </summary>
        /// <param name="cacheEnabled">是否缓存</param>
        public TabControl SetCacheEnabled(bool cacheEnabled)
        {
            this.CacheEnabled = cacheEnabled;
            return this;
        }
        /// <summary>
        /// 触发Tab页切换的Javascript事件名
        /// </summary>
        /// <example>click,表示点击触发；mouseover表示鼠标移上去时就触发</example>
        public TabControl SetEvent(string eventName)
        {
            this.Event = eventName;
            return this;
        }

        /// <summary>
        /// 通过cookie保持tab页的选择状态
        /// </summary>
        /// <example>30,表示使用cookie保持选中状态30天</example>
        public TabControl SetPersistenceExpires(int persistenceExpires)
        {
            this.PersistenceExpires = persistenceExpires;
            return this;
        }

        /// <summary>
        /// 设置Tab按钮加载时的显示文字
        /// </summary>
        /// <param name="spinner">加载提示文字</param>
        public TabControl SetSpinner(string spinner)
        {
            this.Spinner = spinner;
            return this;
        }

        /// <summary>
        /// 设置额外的选项
        /// </summary>
        /// <param name="optionName">选项名</param>
        /// <param name="optionValue">选项值</param>
        public TabControl MergeAdditionalOption(string optionName, object optionValue)
        {
            if (this.AdditionalOptions == null)
                this.AdditionalOptions = new Dictionary<string, object>();
            this.AdditionalOptions[optionName] = optionValue;
            return this;
        }

        /// <summary>
        /// 设置Tabs控件的自定义样式名
        /// </summary>
        /// <param name="containerCss">容器样式</param>---
        public TabControl SetContainerCss(string containerCss)
        {
            this.ContainerCss = containerCss;
            return this;
        }

        /// <summary>
        /// 增加一个Tab按钮
        /// </summary>
        public TabControl AddTabItem(TabItem item)
        {
            this.TabItems.Add(item);
            return this;
        }

        /// <summary>
        /// 转为Html属性集合
        /// </summary>
        public IDictionary<string, object> ToHtmlAttributes()
        {
            var result = new Dictionary<string, object>();
            result["plugin"] = "tabs";
            var data = new Dictionary<string, object>();
            if (AdditionalOptions != null)
                data = new Dictionary<string, object>(AdditionalOptions);

            data.TryAdd("selected", GetSelected());
            data.TryAdd("select", this.OnSelectCallBack);
            data.TryAdd("cache", this.CacheEnabled);
            data.TryAdd("spinner", this.Spinner);
            data.TryAdd("event", this.Event);
            data.TryAdd("isSample", this.IsSimple);
            string idPrefix = !string.IsNullOrEmpty(this.Name) ? this.Name + "-" : string.Empty;
            if (IsSimple)
                idPrefix += "spb-tabs-";
            else
                idPrefix += "ui-tabs-";

            data.TryAdd("idPrefix", idPrefix);
            data.TryAdd("cookie", new { expires = this.PersistenceExpires });
            result.Add("data", Json.Encode(data));
            return result;
        }

        /// <summary>
        /// 呈现控件
        /// </summary>
        public string Render(HtmlHelper htmlHelper)
        {
            //组装容器
            TagBuilder containerBuilder = new TagBuilder("div");
            containerBuilder.MergeAttributes(ToHtmlAttributes());
            if (IsSimple)
                containerBuilder.AddCssClass("tn-tabs");
            else
                containerBuilder.AddCssClass("ui-tabs ui-widget ui-widget-content ui-corner-all");
            if (!string.IsNullOrEmpty(this.ContainerCss))
                containerBuilder.AddCssClass(this.ContainerCss);
            if (!string.IsNullOrEmpty(this.Name))
            {
                containerBuilder.GenerateId(this.Name);
                containerBuilder.MergeAttribute("name", this.Name, true);
            }
            //组装tabs和panels            
            int index = 0;                   
            object indexObj = htmlHelper.ViewContext.HttpContext.Items["StartIndex" + IsSimple];
            if (indexObj != null)
                int.TryParse(indexObj.ToString(), out index);

            TagBuilder ulBuilder = new TagBuilder("ul");
            if (IsSimple)
                ulBuilder.AddCssClass("tn-tabs-nav tn-border-gray tn-border-bottom tn-helper-clearfix");
            else
                ulBuilder.AddCssClass("ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all");
            StringBuilder panels = new StringBuilder();
            foreach (TabItem item in this.TabItems)
            {
                TagBuilder tabBuilder = new TagBuilder("li");
                if (IsSimple)
                    tabBuilder.AddCssClass("tn-widget-content tn-border-gray tn-border-trl");
                else
                    tabBuilder.AddCssClass("ui-state-default ui-corner-top");
                TagBuilder panelBuilder = new TagBuilder("div");
                if (IsSimple)
                    panelBuilder.AddCssClass("tn-tabs-panel");
                else
                    panelBuilder.AddCssClass("ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide");
                string panelId = !string.IsNullOrEmpty(this.Name) ? this.Name + "-" : string.Empty;
                if (IsSimple)
                    panelId = panelId + "spb-tabs-" + index;
                else
                    panelId = "ui-tabs-" + index;
                if (!string.IsNullOrEmpty(item.HTMLContent))
                {
                    item.Url = "#" + panelId;
                    panelBuilder.MergeAttribute("id", panelId);
                    panelBuilder.InnerHtml = item.HTMLContent;
                    panels.Append(panelBuilder.ToString());
                }
                ulBuilder.InnerHtml += item.Render(htmlHelper, index);
                index++;
            }
            htmlHelper.ViewContext.HttpContext.Items["StartIndex" + IsSimple] = index;
            containerBuilder.InnerHtml += ulBuilder.ToString() + panels.ToString();
            return containerBuilder.ToString();
        }

        /// <summary>
        /// 获取被选中的Tabs按钮数
        /// </summary>
        /// <returns>选中数量</returns>
        private int? GetSelected()
        {
            for (int i = 0; i < this.TabItems.Count; i++)
                if (this.TabItems[i].Selected)
                    return i;

            return null;
        }
    }
}
