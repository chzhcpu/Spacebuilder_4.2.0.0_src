//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 服务器返回异步内容的数据类型
    /// </summary>
    public enum AjaxDataType
    {
        /// <summary>
        /// 默认返回纯文本 HTML 信息；包含 script 元素
        /// </summary>
        Html = 0,
        /// <summary>
        /// 返回 XML 文档
        /// </summary>
        Xml = 1,
        /// <summary>
        ///  返回 JSON 数据
        /// </summary>
        Json = 2,
        /// <summary>
        /// 返回纯文本 JavaScript 代码
        /// </summary>
        Script = 3
    }
}