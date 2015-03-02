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
    /// Tab项链接打开方式
    /// </summary>
    public enum TabTargetType
    {
        /// <summary>
        /// 默认异步加载Tab内容
        /// </summary>
        Ajax = 0,
        /// <summary>
        /// 在本窗口打开链接
        /// </summary>
        Self = 1,
        /// <summary>
        /// 在新窗口打开链接
        /// </summary>
        Blank = 2
    }
}
