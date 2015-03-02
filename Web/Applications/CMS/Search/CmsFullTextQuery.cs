//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯全文检索条件
    /// </summary>
    public class CmsFullTextQuery
    {
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<string> Keywords { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 筛选
        /// </summary>
        public CmsSearchRange Range { get; set; }


        /// <summary>
        /// 栏目Id
        /// </summary>
        public int ContentFolderId { get; set; }

        private int pageIndex = 1;
        /// <summary>
        /// 当前显示页面页码
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex < 1)
                    return 1;
                else
                    return pageIndex;
            }
            set { pageIndex = value; }
        }

        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize = 10;

    }

    /// <summary>
    /// 搜索范围
    /// </summary>
    public enum CmsSearchRange
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL = 0,
        /// <summary>
        /// 标题
        /// </summary>
        TITLE = 1,
        /// <summary>
        /// 内容
        /// </summary>
        BODY = 2,
        /// <summary>
        /// 作者
        /// </summary>
        AUTHOR = 3,
        /// <summary>
        /// 标签
        /// </summary>
        TAG = 4
    }
}