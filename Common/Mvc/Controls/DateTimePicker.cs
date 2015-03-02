//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Web.Routing;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 日期时间选择器
    /// </summary>
    /// <remarks> 
    /// <para>基于jQuery UI Datepicker （V1.8.7）和 jquery-ui-timepicker-addon 插件构建，更多信息请参见：</para> 
    /// <list type="number">
    /// <item>http://jqueryui.com/demos/datepicker/</item>
    /// <item>http://trentrichardson.com/examples/timepicker/</item>
    /// </list>    
    /// <para>如果需要更多option设置，可以通过设置AdditionalParameters属性来实现</para>
    /// <para>依赖文件：</para>
    /// <list type="number">
    /// <item>jquery-ui.js</item>
    /// <item>jquery-ui-timepicker-addon.js（V0.9.7）</item>
    /// <item>jquery.ui.datepicker-zh-CN.js</item>
    /// </list>
    /// </remarks>
    public class DateTimePicker
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <remarks>在构造器中为属性赋默认值</remarks>
        public DateTimePicker()
        {
            this.ChangeMonth = true;
            this.ChangeYear = true;
            this.DateFormat = "yyyy-MM-dd";
            this.TimeFormat = "HH:mm";
            this.MinDate = "0";
            this.MaxDate = "+10Y";
            this.ShowTime = false;
        }
        /// <summary>
        /// 是否显示时间
        /// </summary>
        /// <value>false</value>
        public bool ShowTime { get; set; }

        /// <summary>
        /// 默认日期
        /// </summary>        
        public DateTime? SelectedDate { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        /// <example>'yyyy-MM-dd' </example>
        /// <value>"yyyy-MM-dd"</value>
        public string DateFormat { get; set; }


        /// <summary>
        /// 时间格式
        /// </summary>
        /// <example>"HH:mm"</example>
        /// <value>"HH:mm"</value>
        public string TimeFormat { get; set; }

        /// <summary>
        /// 最大可选时间
        /// </summary>
        /// <value>"+10Y"</value>
        /// <include file='DateTimePicker.xml' path='doc/members/member[@name="P:Tunynet.Mvc.DateTimePicker.MinDate"]/remarks'/>
        public string MaxDate { get; set; }

        /// <summary>
        /// 最小可选时间
        /// </summary>
        /// <value>"0"</value>
        /// <include file='DateTimePicker.xml' path='doc/members/member[@name="P:Tunynet.Mvc.DateTimePicker.MinDate"]/remarks'/>
        public string MinDate { get; set; }

        /// <summary>
        /// 是否可改变月份
        /// </summary>
        /// <value>true</value>
        public bool ChangeMonth { get; set; }
        /// <summary>
        /// 是否可改变月份
        /// </summary>
        /// <value>true</value>
        public bool ChangeYear { get; set; }

        /// <summary>
        /// 呈现日期选择器前的Javascript回调函数<br/>
        /// 原型为 function(input, inst)
        /// </summary>
        public string BeforeShowCallBack { get; set; }

        /// <summary>
        /// 选中事件的Javascript回调函数<br/>
        /// 原型为 fn(dateText,inst)
        /// </summary>
        public string OnSelectCallBack { get; set; }

        /// <summary>
        /// 设置关闭事件的Javascript回调函数<br/>
        /// 原型为 fn(dateText,inst)
        /// </summary>
        public string OnCloseCallBack { get; set; }

        /// <summary>
        /// datepicker或timepicker插件中提供的其他option选项
        /// </summary>
        public Dictionary<string, object> AdditionalOptions { get; set; }

        /// <summary>
        /// 文本框的html属性集合
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }


        /// <summary>
        /// 默认时间值
        /// </summary>
        private string DefaultDateString
        {
            get
            {
                if (SelectedDate == null || SelectedDate == DateTime.MinValue)
                    return string.Empty;

                return ShowTime ? SelectedDate.Value.ToString(this.DateFormat + " " + this.TimeFormat) : SelectedDate.Value.ToString(this.DateFormat);
            }
        }

        #region 连缀方法

        /// <summary>
        /// 设置是否显示时间
        /// </summary>
        /// <param name="showTime">是否显示时间</param>
        public DateTimePicker SetShowTime(bool showTime)
        {
            this.ShowTime = showTime;
            return this;
        }

        /// <summary>
        /// 设置默认日期
        /// </summary>
        /// <param name="selectedDate">默认日期</param>
        public DateTimePicker SetSelectedDate(DateTime? selectedDate)
        {
            this.SelectedDate = selectedDate;
            return this;
        }

        /// <summary>
        /// 设置日期格式
        /// </summary>
        /// <param name="dateFormat">日期格式</param>
        /// <example>'yyyy-MM-dd'</example>
        public DateTimePicker SetDateFormat(string dateFormat)
        {
            this.DateFormat = dateFormat;
            return this;
        }

        /// <summary>
        /// 设置时间格式
        /// </summary>
        /// <param name="timeFormat">时间格式</param>
        /// <example>"HH:mm"</example>
        public DateTimePicker SetTimeFormat(string timeFormat)
        {
            this.TimeFormat = timeFormat;
            return this;
        }

        /// <summary>
        /// 设置最大时间
        /// </summary>
        /// <param name="maxDate">最大时间</param>
        public DateTimePicker SetMaxDate(string maxDate)
        {
            this.MaxDate = maxDate;
            return this;
        }

        /// <summary>
        /// 设置最小时间
        /// </summary>
        /// <param name="minDate">最小时间</param>
        public DateTimePicker SetMinDate(string minDate)
        {
            this.MinDate = minDate;
            return this;
        }

        /// <summary>
        /// 设置是否可改变月份
        /// </summary>
        /// <param name="changeMonth">是否可改变月份</param>
        public DateTimePicker SetChangeMonth(bool changeMonth)
        {
            this.ChangeMonth = changeMonth;
            return this;
        }

        /// <summary>
        /// 设置是否可改变月份
        /// </summary>
        /// <param name="changeYear">是否可改变月份</param>
        public DateTimePicker SetChangeYear(bool changeYear)
        {
            this.ChangeYear = changeYear;
            return this;
        }

        /// <summary>
        /// 设置呈现日期选择器前的Javascript回调函数<br/>
        /// 原型为 function(input, inst)
        /// </summary>
        /// <param name="beforeShowCallBack">呈现日期选择器前的Javascript回调函数</param>
        public DateTimePicker SetBeforeShowCallBack(string beforeShowCallBack)
        {
            this.BeforeShowCallBack = beforeShowCallBack;
            return this;
        }

        /// <summary>
        /// 设置选中事件的Javascript回调函数<br/>
        /// 原型为 function(dateText,inst)
        /// </summary>
        /// <param name="onSelectCallBack">选中事件的Javascript回调函数</param>
        public DateTimePicker SetOnSelectCallBack(string onSelectCallBack)
        {
            this.OnSelectCallBack = onSelectCallBack;
            return this;
        }

        /// <summary>
        /// 设置关闭事件的Javascript回调函数<br/>
        /// 原型为 function(dateText,inst)
        /// </summary>
        /// <param name="onCloseCallBack">关闭事件的Javascript回调函数</param>
        public DateTimePicker SetOnCloseCallBack(string onCloseCallBack)
        {
            this.OnCloseCallBack = onCloseCallBack;
            return this;
        }

        /// <summary>
        /// datepicker或timepicker插件中提供的其他option选项
        /// </summary>
        /// <param name="optionName">选项名</param>
        /// <param name="optionValue">选项值</param>
        public DateTimePicker MergeAdditionalOption(string optionName, object optionValue)
        {
            if (this.AdditionalOptions == null)
                this.AdditionalOptions = new Dictionary<string, object>();
            this.AdditionalOptions[optionName] = optionValue;
            return this;
        }

        /// <summary>
        /// 添加html属性
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">属性值</param>
        /// <remarks>如果存在，则覆盖</remarks>
        public DateTimePicker MergeHtmlAttribute(string attributeName, object attributeValue)
        {
            if (this.HtmlAttributes == null)
                this.HtmlAttributes = new Dictionary<string, object>();
            this.HtmlAttributes[attributeName] = attributeValue;
            return this;
        }

        #endregion

        /// <summary>
        /// 转为Html属性集合
        /// </summary>
        public IDictionary<string, object> ToHtmlAttributes()
        {
            var result = new Dictionary<string, object>();
            if (HtmlAttributes != null)
                result = new Dictionary<string, object>(HtmlAttributes);

            result["plugin"] = "datetimepicker";
            var data = new Dictionary<string, object>();
            if (AdditionalOptions != null)
                data = new Dictionary<string, object>(AdditionalOptions);

            data.TryAdd("changeMonth", ChangeMonth);
            data.TryAdd("changeYear", ChangeYear);

            data.TryAdd("onSelect", OnSelectCallBack);
            data.TryAdd("onClose", OnCloseCallBack);
            data.TryAdd("beforeShow", BeforeShowCallBack);

            data.TryAdd("defaultDate", DefaultDateString);
            data.TryAdd("dateFormat", ConvertToClientDateFormat(DateFormat));
            if (!String.IsNullOrWhiteSpace(MinDate) && !data.ContainsKey("minDate"))
            {
                //判断是否为字符串类型的整数
                int minDateInt = 0;
                if (int.TryParse(MinDate, out minDateInt))
                    data["minDate"] = minDateInt;
                else
                    data["minDate"] = MinDate;
            }
            if (!String.IsNullOrWhiteSpace(MaxDate) && !data.ContainsKey("maxDate"))
            {
                //判断是否为字符串类型的整数
                int maxDateInt = 0;
                if (int.TryParse(MaxDate, out maxDateInt))
                    data["maxDate"] = maxDateInt;
                else
                    data["maxDate"] = MaxDate;
            }

            if (ShowTime)
            {
                data["showTime"] = true;
                bool ampm = false;
                string clientTimeFormat = ConvertToClientTimeFormat(this.TimeFormat, out ampm);
                data.TryAdd("timeFormat", clientTimeFormat);
                if (ampm)
                    data["ampm"] = true;
                if (SelectedDate.HasValue)
                {
                    data["hour"] = SelectedDate.Value.Hour;
                    data["minute"] = SelectedDate.Value.Minute;
                }
            }

            result.Add("data", Json.Encode(data));
            return result;
        }

        /// <summary>
        /// 呈现控件
        /// </summary>
        public virtual MvcHtmlString Render(HtmlHelper htmlHelper, string name)
        {
            //组装容器             
            return htmlHelper.TextBox(name, DefaultDateString, this.ShowTime ? InputWidthTypes.Medium : InputWidthTypes.Short, new RouteValueDictionary(this.ToHtmlAttributes()));
        }

        /// <summary>
        /// 转换为客户端可用的日期格式
        /// </summary>
        /// <param name="dateFormat">服务器端的日期格式</param>
        /// <returns>客户端的日期格式</returns>
        private static string ConvertToClientDateFormat(string dateFormat)
        {
            //译码表
            //客户端编码|服务器端编码|含义
            //d --------|d-----------|天，不足两位不会在前面补零，比如：1
            //dd--------|dd----------|天，不足两位需要在前面补零，比如：01
            //D---------|ddd---------|一周的第几天，简写形式，比如：周一
            //DD--------|dddd--------|一周的第几天，全写形式，比如：星期一
            //m---------|M-----------|月，不足两位不会在前面补零，比如：1
            //mm--------|MM----------|月，不足两位需要在前面补零，比如：01
            //M---------|MMM---------|一年的第几个月，简写形式，比如：十二
            //MM--------|MMMM--------|一年的第几个月，全写形式，比如：十二月
            //y---------|yy----------|天，不足两位不会在前面补零，比如：1
            //yy--------|yyyy--------|天，不足两位需要在前面补零，比如：01
            string result = dateFormat;
            //替换天
            if (result.Contains("dddd"))
                result = result.Replace("dddd", "DD");
            else if (result.Contains("ddd"))
                result = result.Replace("ddd", "D");
            //替换月
            if (result.Contains("MMMM"))
                result = result.Replace("MMMM", "MM");
            else if (result.Contains("MMM"))
                result = result.Replace("MMM", "M");
            else if (result.Contains("MM"))
                result = result.Replace("MM", "mm");
            else if (result.Contains("M"))
                result = result.Replace("M", "m");
            //替换年
            if (result.Contains("yyyy"))
                result = result.Replace("yyyy", "yy");
            else if (result.Contains("yy"))
                result = result.Replace("yy", "y");
            return result;
        }

        /// <summary>
        /// 转换为客户端可用的时间格式
        /// </summary>
        /// <param name="timeFormat">服务器端的时间格式</param>
        /// <param name="ampm">输出参数，是否为12小时制</param>
        /// <returns>客户端的时间格式</returns>
        private static string ConvertToClientTimeFormat(string timeFormat, out bool ampm)
        {
            //译码表
            //客户端编码|服务器端编码|含义
            //h --------|h-----------|时，不足两位不会在前面补零，比如：1
            //hh--------|hh----------|时，不足两位需要在前面补零，比如：01

            //m --------|m-----------|分，不足两位不会在前面补零，比如：1
            //mm--------|mm----------|分，不足两位需要在前面补零，比如：01

            //s---------|s-----------|秒，不足两位不会在前面补零，比如：1
            //ss--------|ss----------|秒，不足两位需要在前面补零，比如：01

            //t---------|t-----------|12小时制的简写形式，比如：上
            //tt--------|tt----------|12小时制的全写形式，比如：上午

            ampm = true;
            if (timeFormat.Contains("H"))
                ampm = false;

            string result = timeFormat;
            //替换时
            if (result.Contains("HH"))
                result = result.Replace("HH", "hh");
            else if (result.Contains("H"))
                result = result.Replace("H", "h");
            return result;
        }

    }
}
