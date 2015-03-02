//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Spacebuilder.Setup
{
    /// <summary>
    /// 数据库服务器类型
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// SqlServer数据库服务器
        /// </summary>
        [Display(Name = "SqlServer")]
        SqlServer,
        /// <summary>
        /// MySql数据库服务器
        /// </summary>
        [Display(Name = "MySql")]
        MySql
        ///// <summary>
        ///// SqlCE数据库服务器
        ///// </summary>
        //[Display(Name = "SqlCE")]
        //SqlCE
        //PostgreSQL,
        //Oracle,
        //SQLite
    }
}