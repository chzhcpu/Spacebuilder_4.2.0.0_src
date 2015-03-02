//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 菜单项
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconTypes? IconType { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        private string url = string.Empty;
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url
        {
            get { return WebUtility.ResolveUrl(this.url); }
            set { this.url = value; }
        }
    }
}