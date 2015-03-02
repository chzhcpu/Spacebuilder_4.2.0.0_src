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

namespace Spacebuilder.Common
{
    /// <summary>
    /// tab项
    /// </summary>
    public class TabItem
    {
        /// <summary>
        /// 链接标签A的Id
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 额外的class
        /// </summary>
        public string Css { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        /// <value>false</value>
        public bool Selected { get; set; }

        /// <summary>
        /// 异步获取内容的地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Tab项链接打开方式
        /// </summary>
        public TabTargetType TabTarget { get; set; }

        /// <summary>
        /// 静态内容
        /// </summary>
        public string HTMLContent { get; set; }

        private TabItem()
        {
            this.Selected = false;
        }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="title">Tab项标题</param>
        public TabItem(string title)
            : this()
        {
            this.Title = title;
        }

        /// <summary>
        /// 设置Tabs控件的Id
        /// </summary>
        /// <param name="name">Tabs控件Id</param>
        public TabItem SetName(string name)
        {
            this.Name = name; return this;
        }


        /// <summary>
        /// 设置tab页的css class
        /// </summary>
        /// <param name="css">css class</param>
        /// <returns></returns>
        public TabItem SetCss(string css)
        {
            this.Css = css; return this;
        }

        /// <summary>
        /// 设为选中状态
        /// </summary>
        /// <returns></returns>
        public TabItem SetSelected()
        {
            this.Selected = true; return this;
        }

        /// <summary>
        /// 设置当前标签页的内容
        /// </summary>
        public TabItem SetUrl(string url)
        {
            this.Url = url;
            return this;
        }
        /// <summary>
        /// 设置Tab项链接打开方式
        /// </summary>
        /// <param name="targetType">Tab项链接打开方式</param>
        public TabItem SetTabTarget(TabTargetType targetType)
        {
            this.TabTarget = targetType;
            return this;
        }

        /// <summary>
        /// 设置当前标签页的MvcHtmlString内容
        /// </summary>
        /// <param name="content">MvcHtmlString内容</param>
        public TabItem SetContent(MvcHtmlString content)
        {
            this.HTMLContent = content.ToString();
            return this;
        }

        /// <summary>
        /// 使用内联模板参数来设置当前标签页的内容
        /// </summary>
        public TabItem SetContent(Func<dynamic, object> generateContent)
        {
            this.HTMLContent = generateContent(string.Empty).ToString();
            return this;
        }

        /// <summary>
        /// 呈现Tab项
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="index">Tab项序号</param>
        /// <returns>Tab项</returns>
        public virtual string Render(HtmlHelper htmlHelper, int index)
        {
            string css = (string.IsNullOrEmpty(this.Css)) ? string.Empty : " class=\"" + this.Css + "\"";
            string id = (string.IsNullOrEmpty(this.Name)) ? string.Empty : " id=\"" + this.Name + "\"";
            string tabtarget = string.Empty;
            if (this.TabTarget == TabTargetType.Blank)
                tabtarget = " tabtarget=\"blank\"";
            else if (this.TabTarget == TabTargetType.Self)
                tabtarget = " tabtarget=\"self\"";
            return string.Format("<li{1}><a href=\"{0}\"{3}{4}><span>{2}</span></a></li>", this.Url, css, this.Title, id, tabtarget);
        }
    }
}