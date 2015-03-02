using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 封禁用户
    /// </summary>
    public class BannedUserEditModel
    {
        /// <summary>
        /// 截止日期
        /// </summary>
        [Required(ErrorMessage = "请输入截止日期")]
        [DataType(DataType.DateTime, ErrorMessage = "截止日期必须为时间类型")]
        public DateTime BanDeadline { get; set; }

        /// <summary>
        /// 封禁原因
        /// </summary>
        [Required(ErrorMessage = "请输入封禁原因")]
        [StringLength(64, ErrorMessage = "最多输入64个字符")]
        public string BanReason { get; set; }
    }
}
