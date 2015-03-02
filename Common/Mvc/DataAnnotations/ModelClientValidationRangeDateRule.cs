//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 日期范围客户端规则
    /// </summary>
    public class ModelClientValidationRangeDateRule : ModelClientValidationRule
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="errorMessage">出错信息</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        public ModelClientValidationRangeDateRule(string errorMessage, DateTime? minValue, DateTime? maxValue)
        {
            ErrorMessage = errorMessage;
            ValidationType = "rangedate";
            if (minValue.HasValue)
                ValidationParameters["min"] = minValue.Value.ToString("yyyy-MM-dd");
            if (maxValue.HasValue)
                ValidationParameters["max"] = maxValue.Value.ToString("yyyy-MM-dd");
        }
    }
}