//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;

namespace Spacebuilder.Common
{

    /// <summary>
    /// 扩展对js控件的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperControlExtensions
    {
        #region DateTimePicker

        /// <summary>
        /// 输出日期时间选择器
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">选取model中属性的lamda表达式</param>
        /// <param name="dateTimePicker">日期时间选择器控件</param>
        public static MvcHtmlString DateTimePickerFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime>> expression, DateTimePicker dateTimePicker = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (dateTimePicker == null)
                dateTimePicker = new DateTimePicker();
            dateTimePicker.SelectedDate = (DateTime)metadata.Model;
            return htmlHelper.DateTimePicker(ExpressionHelper.GetExpressionText(expression), dateTimePicker);
        }

        /// <summary>
        /// 输出日期选择器
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">name属性</param>
        /// <param name="selectedDate">选中日期</param>
        /// <param name="minDate">最小允许选择日期</param>
        /// <param name="maxDate">最大允许选择日期</param>
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, DateTime? selectedDate = null, string minDate = "0", string maxDate = "+10Y")
        {
            return htmlHelper.DateTimePicker(name, new DateTimePicker() { ShowTime = false, MinDate = minDate, MaxDate = maxDate, SelectedDate = selectedDate });
        }

        /// <summary>
        /// 输出日期时间选择器
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">name属性</param>
        /// <param name="selectedDate">选中日期时间</param>
        /// <param name="minDate">最小可选时间</param>
        /// <param name="maxDate">最大可选时间</param>
        /// <remarks>
        /// <para>最小可选时间和最大可选时间的赋值说明如下：</para>
        /// </remarks>
        /// <include file='../Controls/DateTimePicker.xml' path='doc/members/member[@name="P:Tunynet.Mvc.DateTimePicker.MinDate"]/remarks'/>
        public static MvcHtmlString DateTimePicker(this HtmlHelper htmlHelper, string name, DateTime? selectedDate = null, string minDate = "0", string maxDate = "+10Y")
        {
            return htmlHelper.DateTimePicker(name, new DateTimePicker() { ShowTime = true, MinDate = minDate, MaxDate = maxDate, SelectedDate = selectedDate });
        }

        /// <summary>
        /// 输出日期时间选择器
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">name属性</param>
        /// <param name="dateTimePicker">日期时间选择器控件</param> 
        public static MvcHtmlString DateTimePicker(this HtmlHelper htmlHelper, string name, DateTimePicker dateTimePicker)
        {
            if (dateTimePicker == null)
                dateTimePicker = new DateTimePicker();
            htmlHelper.Script("~/Bundle/Scripts/jQueryUI");
            return dateTimePicker.Render(htmlHelper, name);
        }

        #endregion

        #region Tabs
        /// <summary>
        /// 输出Tabs控件
        /// </summary> 
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="tabControl">Tabs控件</param> 
        public static MvcHtmlString Tabs(this HtmlHelper htmlHelper, TabControl tabControl)
        {
            htmlHelper.Script("~/Bundle/Scripts/jQueryUI");
            return new MvcHtmlString(tabControl.Render(htmlHelper));
        }

        #endregion



    }
}
