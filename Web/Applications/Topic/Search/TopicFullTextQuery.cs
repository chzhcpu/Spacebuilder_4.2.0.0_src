//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题全文检索条件
    /// </summary>
    public class TopicFullTextQuery
    {

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// 用户已经加入的专题ID
        /// </summary>
        public IEnumerable<string> TopicIds { get; set; }

        /// <summary>
        /// 筛选
        /// </summary>
        public TopicSearchRange Range { get; set; }

        /// <summary>
        /// 关键字是否为空
        /// </summary>
        public bool KeywordIsNull { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string NowAreaCode { get; set; }

        /// <summary>
        /// 分类ID(站点分类ID)
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public SortBy_Topic? sortBy { get; set; } 

        /// <summary>
        /// 当前显示页面页码
        /// </summary>
        private int pageIndex = 1;
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

    public enum TopicSearchRange
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL = 0,
        /// <summary>
        /// 专题名
        /// </summary>
        TOPICNAME = 1,
        /// <summary>
        /// 简介
        /// </summary>
        DESCRIPTION = 2,
        /// <summary>
        /// 标签
        /// </summary>
        TAG = 3,
        /// <summary>
        /// 分类名
        /// </summary>
        CATEGORYNAME = 4
    }
}