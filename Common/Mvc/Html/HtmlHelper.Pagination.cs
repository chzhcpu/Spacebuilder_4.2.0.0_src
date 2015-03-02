//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Resources;
using System.Web.Helpers;
using System.Web.Routing;
using Tunynet;

namespace Spacebuilder.Common
{

    /// <summary>
    /// 分页按钮控件
    /// </summary>
    public static class HtmlHelperPaginationExtensions
    {
        /// <summary>
        /// 呈现普通分页按钮
        /// </summary>
        /// <param name="paginationMode">分页按钮显示模式</param>
        /// <param name="html">被扩展的HtmlHelper</param>
        /// <param name="pagingDataSet">数据集</param>
        /// <param name="numericPagingButtonCount">数字分页按钮显示个数</param>
        /// <returns>分页按钮html代码</returns>
        public static MvcHtmlString PagingButton(this HtmlHelper html, IPagingDataSet pagingDataSet, PaginationMode paginationMode = PaginationMode.NumericNextPrevious, int numericPagingButtonCount = 7)
        {
            return PagingButton(html, pagingDataSet, false, null, paginationMode, numericPagingButtonCount);
        }

        /// <summary>
        /// 呈现异步分页按钮
        /// </summary>
        /// <param name="paginationMode">分页按钮显示模式</param>
        /// <param name="html">被扩展的HtmlHelper</param>
        /// <param name="pagingDataSet">数据集</param>
        /// <param name="updateTargetId">异步分页时，被更新的目标元素Id</param>
        /// <param name="numericPagingButtonCount">数字分页按钮显示个数</param>
        /// <returns>分页按钮html代码</returns>
        public static MvcHtmlString AjaxPagingButton(this HtmlHelper html, IPagingDataSet pagingDataSet, string updateTargetId, PaginationMode paginationMode = PaginationMode.NumericNextPrevious, int numericPagingButtonCount = 7, string ajaxUrl = null)
        {
            return PagingButton(html, pagingDataSet, true, updateTargetId, paginationMode, numericPagingButtonCount, ajaxUrl);
        }

        /// <summary>
        /// 呈现分页按钮
        /// </summary>
        /// <param name="html">被扩展的HtmlHelper</param>
        /// <param name="pagingDataSet">数据集</param>
        /// <param name="updateTargetId">异步分页时，被更新的目标元素Id</param>
        /// <param name="paginationMode">分页按钮显示模式</param>
        /// <param name="numericPagingButtonCount">数字分页按钮显示个数</param>
        /// <param name="enableAjax">是否使用ajax分页</param>
        /// <returns>分页按钮html代码</returns>
        private static MvcHtmlString PagingButton(this HtmlHelper html, IPagingDataSet pagingDataSet, bool enableAjax, string updateTargetId, PaginationMode paginationMode = PaginationMode.NumericNextPrevious, int numericPagingButtonCount = 7, string ajaxUrl = null)
        {
            if (pagingDataSet.TotalRecords == 0 || pagingDataSet.PageSize == 0)
                return MvcHtmlString.Empty;

            //计算总页数
            int totalPages = (int)(pagingDataSet.TotalRecords / (long)pagingDataSet.PageSize);
            if ((pagingDataSet.TotalRecords % pagingDataSet.PageSize) > 0)
                totalPages++;

            //未超过一页时不显示分页按钮
            if (totalPages <= 1)
                return MvcHtmlString.Empty;

            bool showFirst = false;
            if (paginationMode == PaginationMode.NextPreviousFirstLast)
                showFirst = true;

            bool showLast = false;
            if (paginationMode == PaginationMode.NextPreviousFirstLast)
                showLast = true;

            bool showPrevious = true;
            //if (paginationMode == PaginationMode.NextPrevious || paginationMode == PaginationMode.NextPreviousFirstLast || paginationMode == PaginationMode.NumericNextPrevious)
            //    showPrevious = true;

            bool showNext = true;
            //if (paginationMode == PaginationMode.NextPrevious || paginationMode == PaginationMode.NextPreviousFirstLast || paginationMode == PaginationMode.NumericNextPrevious)
            //    showNext = true;

            bool showNumeric = false;
            if (paginationMode == PaginationMode.NumericNextPrevious)
                showNumeric = true;

            //显示多少个数字分页按钮
            //int numericPageButtonCount = 10;

            //对pageIndex进行修正
            if ((pagingDataSet.PageIndex < 1) || (pagingDataSet.PageIndex > totalPages))
                pagingDataSet.PageIndex = 1;

            string pagingContainer = "<div class=\"tn-pagination-btn\"";
            if (enableAjax)
                pagingContainer += " plugin=\"ajaxPagingButton\" data=\"" + HttpUtility.HtmlEncode(Json.Encode(new { updateTargetId = updateTargetId })) + "\"";
            pagingContainer += ">";

            StringBuilder pagingButtonsHtml = new StringBuilder(pagingContainer);

            //构建 "首页"
            if (showFirst)
            {
                if ((pagingDataSet.PageIndex > 1) && (totalPages > numericPagingButtonCount))
                {
                    pagingButtonsHtml.AppendLine();
                    pagingButtonsHtml.AppendFormat(BuildLink("&lt;&lt;", GetPagingNavigateUrl(html, 1, ajaxUrl), "tn-page-first tn-page-thumb"));
                }
                else if (paginationMode == PaginationMode.NextPreviousFirstLast)
                {
                    pagingButtonsHtml.AppendLine();
                    pagingButtonsHtml.AppendFormat("<span class=\"tn-page-first tn-page-thumb\">{0}</span>", "&lt;&lt;");
                }
            }


            //回复：和苏工沟通了下，可能会调整标签结构及class名。但隔离开挺麻烦，暂时保留这个问题
            //mazq回复：是否可以把html代码放入view，另外如果允许用户选择每页记录数如何处理？

            //构建 "上一页"
            if (showPrevious)
            {
                pagingButtonsHtml.AppendLine();
                if (pagingDataSet.PageIndex == 1)
                    pagingButtonsHtml.AppendFormat("<span class=\"tn-page-prev tn-page-thumb\">{0}</span>", "上一页");
                else
                    pagingButtonsHtml.AppendFormat(BuildLink("上一页", GetPagingNavigateUrl(html, pagingDataSet.PageIndex - 1, ajaxUrl), "tn-page-prev tn-page-thumb"));
            }

            //构建 数字分页部分
            if (showNumeric)
            {
                int startNumericPageIndex;
                if (numericPagingButtonCount > totalPages || pagingDataSet.PageIndex - (numericPagingButtonCount / 2) <= 0)
                    startNumericPageIndex = 1;
                else if (pagingDataSet.PageIndex + (numericPagingButtonCount / 2) > totalPages)
                    startNumericPageIndex = totalPages - numericPagingButtonCount + 1;
                else
                    startNumericPageIndex = pagingDataSet.PageIndex - (numericPagingButtonCount / 2);

                if (startNumericPageIndex < 1)
                    startNumericPageIndex = 1;

                int lastNumericPageIndex = startNumericPageIndex + numericPagingButtonCount - 1;
                if (lastNumericPageIndex > totalPages)
                    lastNumericPageIndex = totalPages;

                if (startNumericPageIndex > 1)
                {
                    for (int i = 1; i < startNumericPageIndex; i++)
                    {
                        pagingButtonsHtml.AppendLine();

                        if (i > 3)
                            break;
                        if (i == 3)
                            pagingButtonsHtml.Append("<span class=\"tn-page-break\">...</span>");
                        else
                        {
                            if (pagingDataSet.PageIndex == i)
                                pagingButtonsHtml.AppendFormat("<span class=\"tn-page-number tn-selected\">{0}</span>", i);
                            else
                                pagingButtonsHtml.AppendFormat(BuildLink(i.ToString(), GetPagingNavigateUrl(html, i, ajaxUrl)));
                        }
                    }
                }

                for (int i = startNumericPageIndex; i <= lastNumericPageIndex; i++)
                {
                    pagingButtonsHtml.AppendLine();
                    if (pagingDataSet.PageIndex == i)
                        pagingButtonsHtml.AppendFormat("<span class=\"tn-page-number tn-selected\">{0}</span>", i);
                    else
                        pagingButtonsHtml.AppendFormat(BuildLink(i.ToString(), GetPagingNavigateUrl(html, i, ajaxUrl)));
                }

                if (lastNumericPageIndex < totalPages)
                {
                    int lastStart = lastNumericPageIndex + 1;
                    if (totalPages - lastStart > 2)
                        lastStart = totalPages - 2;

                    for (int i = lastStart; i <= totalPages; i++)
                    {
                        pagingButtonsHtml.AppendLine();
                        if ((i == lastStart) && (totalPages - lastNumericPageIndex > 3))
                        {
                            pagingButtonsHtml.AppendLine();
                            pagingButtonsHtml.Append("<span class=\"tn-page-break\">...</span>");
                            continue;
                        }

                        if (pagingDataSet.PageIndex == i)
                            pagingButtonsHtml.AppendFormat("<span class=\"tn-page-number tn-selected\">{0}</span>", i);
                        else
                            pagingButtonsHtml.AppendFormat(BuildLink(i.ToString(), GetPagingNavigateUrl(html, i, ajaxUrl)));
                    }
                }

            }

            if (showNext)
            {
                pagingButtonsHtml.AppendLine();
                if (pagingDataSet.PageIndex == totalPages)
                    pagingButtonsHtml.AppendFormat("<span class=\"tn-page-thumb tn-page-next\">{0}</span>", "下一页");
                else
                    pagingButtonsHtml.AppendFormat(BuildLink("下一页", GetPagingNavigateUrl(html, pagingDataSet.PageIndex + 1, ajaxUrl), "tn-page-thumb tn-page-next"));
            }

            if (showLast)
            {
                if ((pagingDataSet.PageIndex < totalPages) && (totalPages > numericPagingButtonCount))
                {
                    pagingButtonsHtml.AppendLine();
                    pagingButtonsHtml.AppendFormat(BuildLink("&gt;&gt;", GetPagingNavigateUrl(html, totalPages, ajaxUrl), "tn-page-thumb tn-page-last"));
                }
                else if (paginationMode == PaginationMode.NextPreviousFirstLast)
                {
                    pagingButtonsHtml.AppendLine();
                    pagingButtonsHtml.AppendFormat("<span class=\"tn-page-thumb tn-page-last\">{0}</span>", "&gt;&gt;");
                }
            }
            pagingButtonsHtml.Append("</div>");
            return MvcHtmlString.Create(pagingButtonsHtml.ToString());
        }

        /// <summary>
        /// 构建分页按钮的链接
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>分页按钮的url字符串</returns>
        public static string GetPagingNavigateUrl(this HtmlHelper htmlHelper, int pageIndex, string currentUrl = null)
        {
            object pageIndexObj = null;
            if (htmlHelper.ViewContext.RouteData.Values.TryGetValue("pageIndex", out pageIndexObj))
            {
                htmlHelper.ViewContext.RouteData.Values["pageIndex"] = pageIndex;

                return UrlHelper.GenerateUrl(null, null, null, htmlHelper.ViewContext.RouteData.Values, RouteTable.Routes, htmlHelper.ViewContext.RequestContext, true);
            }

            if (string.IsNullOrEmpty(currentUrl))
                currentUrl = HttpUtility.HtmlEncode(htmlHelper.ViewContext.HttpContext.Request.RawUrl);

            if (currentUrl.IndexOf("?") == -1)
            {
                return currentUrl + string.Format("?pageIndex={0}", pageIndex);
            }
            else
            {
                if (currentUrl.IndexOf("pageIndex=", StringComparison.InvariantCultureIgnoreCase) == -1)
                    return currentUrl + string.Format("&pageIndex={0}", pageIndex);
                else
                    return Regex.Replace(currentUrl, @"pageIndex=(\d+\.?\d*|\.\d+)", "pageIndex=" + pageIndex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }

        /// <summary>
        /// 生成带Href的链接
        /// </summary>
        private static string BuildLink(string linkText, string url, string cssClassName = "tn-page-number")
        {
            return string.Format("<a href=\"{0}\" {1}>{2}</a>", url, string.IsNullOrEmpty(cssClassName) ? string.Empty : string.Format("class=\"{0}\"", cssClassName), linkText);
        }
    }



    /// <summary>
    /// 分页按钮显示模式
    /// </summary>
    public enum PaginationMode
    {
        /// <summary>
        /// 上一页/下一页 模式
        /// </summary>
        NextPrevious,

        /// <summary>
        /// 首页/末页/上一页/下一页 模式
        /// </summary>
        NextPreviousFirstLast,

        /// <summary>
        /// 上一页/下一页 + 数字 模式，例如： 上一页 1 2 3 4 5 下一页
        /// </summary>
        NumericNextPrevious,
    }
}
