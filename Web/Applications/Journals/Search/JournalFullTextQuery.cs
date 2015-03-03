//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-10-29</createdate>
//<author>wanf</author>
//<email>wanf@tunynet.com</email>
//<log date="2012-10-29" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// 日志全文检索条件
    /// </summary>
    public class JournalFullTextQuery
    {
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<string> Keywords { get; set; }

        private bool isRelationJournal = false;
        /// <summary>
        /// 是否查相关标签
        /// </summary>
        public bool IsRelationJournal
        {
            get { return isRelationJournal; }
            set { isRelationJournal = value; }
        }

        private long currentThreadId = 0;
        /// <summary>
        /// 当前日志ID
        /// </summary>
        public long CurrentThreadId
        {
            get { return currentThreadId; }
            set { currentThreadId = value; }
        }


        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 筛选
        /// </summary>
        public JournalSearchRange Range { get; set; }

        /// <summary>
        /// 我的日志、某人的日志、站点分类
        /// </summary>
        public JournalRange JournalRange { get; set; }

        /// <summary>
        /// 范围
        /// </summary>
        private long allId = 0;
        public long AllId
        {
            get { return allId; }
            set { allId = value; }
        }

        private long loginId = 0;
        public long LoginId
        {
            get { return loginId; }
            set { loginId = value; }
        }

        private long userId = 0;
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// 站点分类ID
        /// </summary>
        public long SiteCategoryId { get; set; }

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

    public enum JournalRange
    {
        DEFAULT=0,
         /// <summary>
        /// 我的日志
        /// </summary>
        MYBlOG=1,
        /// <summary>
        /// 某人的日志
        /// </summary>
        SOMEONEBLOG=2,
        /// <summary>
        /// 站点分类
        /// </summary>
        SITECATEGORY=3
    }

    public enum JournalSearchRange
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL = 0,
        /// <summary>
        /// 标题
        /// </summary>
        SUBJECT = 1,
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
        TAG = 4,
        /// <summary>
        /// 用户分类名
        /// </summary>
        OWNERCATEGORYNAME = 5
    }
}