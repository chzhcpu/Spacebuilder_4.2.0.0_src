//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;

namespace Spacebuilder.Common
{
    /// <summary>
    /// To add a watermark
    /// </summary>
    /// <remarks>
    /// 基于jquery.watermark实现
    /// 具体参见：http://jquery-watermark.googlecode.com/
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class WaterMarkAttribute : Attribute
    {

        private string _content = string.Empty;
        /// <summary>
        /// 水印文字内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public WaterMarkAttribute()
        {
        }
    }
}
