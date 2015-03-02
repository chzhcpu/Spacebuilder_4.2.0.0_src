//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;

using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户表单呈现的Message实体
    /// </summary>
    public class MassMessagesEditModel
    {

        [Display(Name = "是否按角色群发")]
        public bool IsByRole { get; set; }

        [Display(Name = "选中的角色")]
        public string RoleName { get; set; }

        [Display(Name = "最小等级")]
        public int MinRank { get; set; }

        [Display(Name = "最大等级")]
        public int MaxRank { get; set; }

        [Display(Name = "是否采用发私信的形式")]
        public bool IsMessage { get; set; }

        /// <summary>
        ///标题
        /// </summary>
        [Display(Name = "标题")]
        [WaterMark(Content = "邮件标题")]
        [DataType(DataType.Text)]
        public string Subject { get; set; }

        /// <summary>
        ///内容
        /// </summary>
        [AllowHtml]
        [Required(ErrorMessage = "内容为必填项")]
        [StringLength(3000, ErrorMessage = "最多允许输入3千字")]
        [Display(Name = "内容")]
        [DataType(DataType.Html)]
        public string Body { get; set; }
    }
}