//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用来在Action中中设置PageName和面包屑
    /// </summary>
    public class BreadCrumb
    {

        /// <summary>
        /// 受保护构造器
        /// </summary>
        protected BreadCrumb()
        { }

        /// <summary>
        /// 类实例化工具方法
        /// </summary>
        /// <returns></returns>
        public static BreadCrumb New()
        {
            return new BreadCrumb();
        }

        #region 属性成员


        /// <summary>
        /// BreadCrumbQueue在HttpContext.Items中的key
        /// </summary>
        static readonly string BreadCrumbKeyInHttpContext = "Item_BreadCrumbQueue";

        /// <summary>
        /// PageName在HttpContext.Items中的key
        /// </summary>
        static readonly string PageNameKeyInHttpContext = "Item_PageName";

        private bool isAutoAddSiteHomeNode = false;
        /// <summary>
        /// 是否自动添加站点首页节点
        /// </summary>
        public bool IsAutoAddSiteHomeNode
        {
            get { return isAutoAddSiteHomeNode; }
            set { isAutoAddSiteHomeNode = value; }
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 设置页面PageName
        /// </summary>
        /// <param name="innerHtml">PageName显示内容</param>
        /// <param name="href">面包屑节点对应的链接</param>
        public static void SetPageName(string innerHtml, string href = "")
        {
            if (!string.IsNullOrEmpty(innerHtml))
            {
                if (!string.IsNullOrEmpty(href))
                {
                    TagBuilder a = new TagBuilder("a");
                    a.InnerHtml = innerHtml;
                    a.MergeAttribute("title", HtmlUtility.StripForPreview(innerHtml));
                    a.MergeAttribute("href", href);
                    innerHtml = a.ToString();
                }

                HttpContext.Current.Items[PageNameKeyInHttpContext] = innerHtml;
            }
        }

        /// <summary>
        /// 添加面包屑节点
        /// </summary>
        /// <param name="innerHtml">面包屑节点内容</param>
        /// <param name="href">面包屑节点对应的链接</param>
        public BreadCrumb AddBreadCrumbItem(string innerHtml, string href = "")
        {
            if (!string.IsNullOrEmpty(innerHtml))
            {
                HttpContext.Current.Items["IsAutoAddSiteHomeNode"] = isAutoAddSiteHomeNode;
                Queue<TagBuilder> crumbNodes = HttpContext.Current.Items[BreadCrumbKeyInHttpContext] as Queue<TagBuilder>;

                if (crumbNodes == null)
                {
                    crumbNodes = new Queue<TagBuilder>();
                    HttpContext.Current.Items[BreadCrumbKeyInHttpContext] = crumbNodes;
                }

                TagBuilder anchor = new TagBuilder("a");
                anchor.InnerHtml = innerHtml;
                anchor.MergeAttribute("title", HtmlUtility.StripForPreview(innerHtml));
                anchor.MergeAttribute("href", href);

                crumbNodes.Enqueue(anchor);
            }

            return new BreadCrumb();
        }

        #endregion
    }
}
