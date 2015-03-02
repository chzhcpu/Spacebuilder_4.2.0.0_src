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
    /// 异步提交表单选项
    /// </summary>
    /// <remarks> 
    /// <para>已实现以下功能：</para>
    /// <list type="number">
    /// <item>内置block功能防止用户多次提交表单</item>
    /// <item>利用服务器端返回的数据类型不同，区别开表单是否成功提交，并提供了提交表单成功/失败回调函数</item>
    /// </list>
    /// <para>基于jquery.form 插件构建，更多信息请参见：</para> 
    /// <list type="number">
    /// <item>http://jquery.malsup.com/form/</item>
    /// </list>    
    /// <para>依赖文件：</para>
    /// <list type="number">
    /// <item>jquery.form.js</item>
    /// </list>
    /// </remarks>
    public class AjaxFormOptions
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public AjaxFormOptions()
        {
            this.CloseDialog = true;
        }

        /// <summary>
        /// 提交表单的请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 提交表单的请求方式
        /// </summary>        
        public FormMethod? Method { get; set; }

        /// <summary>
        /// 提交表单时，额外传递的数据集合
        /// </summary>
        /// <example>new{a=1,b=3}</example>
        public object ExtraData { get; set; }

        /// <summary>
        /// 服务器端返回异步内容的数据类型<br/>
        /// <b>强烈建议不要轻易设置该值，因为该值为null时，会自动检测服务器返回数据的类型</b>
        /// </summary>
        /// <example>AjaxDataType.Html</example>
        public AjaxDataType? DataType { get; set; }

        /// <summary>
        /// 成功提交表单后，需要更新的目标元素
        /// </summary>
        /// <example>"#listComments"</example>
        public string Target { get; set; }

        /// <summary>
        /// 是否替换目标元素
        /// </summary>
        public bool ReplaceTarget { get; set; }


        /// <summary>
        /// 成功提交表单后，是否自动清除表单内容
        /// </summary>
        public bool ClearForm { get; set; }

        /// <summary>
        /// 成功提交表单后，是否自动重置表单
        /// </summary>
        public bool ResetForm { get; set; }

        /// <summary>
        /// 成功提交表单后，是否自动关闭对话框
        /// </summary>
        public bool CloseDialog { get; set; }

        /// <summary>
        /// 提交表单前的Javascript回调函数<br/>
        /// 原型为 function(arr, $form, options)
        /// </summary>
        public string BeforeSubmitCallBack { get; set; }

        /// <summary>
        /// 成功提交表单时的Javascript回调函数，若不想自动更新<br/>
        /// 原型为 function(response, statusText, xhr, $form)
        /// </summary>
        public string OnSuccessCallBack { get; set; }

        /// <summary>
        /// 提交表单失败时的Javascript回调函数，若不想自动更新<br/>
        /// 原型为 function(data, statusText, xhr, $form),其中data为字符串类型，若需要其他类型，请自行转换
        /// </summary>
        public string OnErrorCallBack { get; set; }

        #region 连缀方法

        /// <summary>
        /// 设置提交表单的请求地址
        /// </summary>
        /// <param name="url">提交表单的请求地址</param>
        public AjaxFormOptions SetUrl(string url)
        {
            this.Url = url;
            return this;
        }

        /// <summary>
        /// 设置提交表单的请求方式
        /// </summary>
        /// <param name="method">提交表单的请求方式</param>
        public AjaxFormOptions SetMethod(FormMethod? method)
        {
            this.Method = method;
            return this;
        }


        /// <summary>
        /// 设置服务器端返回异步内容的数据类型
        /// </summary>
        /// <param name="dataType">服务器端返回异步内容的数据类型</param>
        public AjaxFormOptions SetDataType(AjaxDataType? dataType)
        {
            this.DataType = dataType;
            return this;
        }

        /// <summary>
        /// 设置提交表单时，额外传递的数据集合
        /// </summary>
        /// <param name="extraData">提交表单时，额外传递的数据集合</param>
        /// <example>new{a=1,b=3}</example>
        public AjaxFormOptions SetExtraData(object extraData)
        {
            this.ExtraData = extraData;
            return this;
        }

        /// <summary>
        /// 设置成功提交表单后，需要更新的目标元素Id
        /// </summary>
        /// <param name="target">成功提交表单后，需要更新的目标元素Id</param>
        /// <example>"listComments"</example>
        public AjaxFormOptions SetTarget(string target)
        {
            this.Target = target;
            return this;
        }

        /// <summary>
        /// 设置是否替换目标元素
        /// </summary>
        /// <param name="replaceTarget">是否替换目标元素</param>
        public AjaxFormOptions SetReplaceTarget(bool replaceTarget)
        {
            this.ReplaceTarget = replaceTarget;
            return this;
        }

        /// <summary>
        /// 设置成功时是否自动清除表单
        /// </summary>
        /// <param name="clearForm">是否清除表单</param>
        public AjaxFormOptions SetClearForm(bool clearForm)
        {
            this.ClearForm = clearForm;
            return this;
        }

        /// <summary>
        /// 设置成功时是否自动重置表单
        /// </summary>
        /// <param name="resetForm">是否重置表单</param>
        public AjaxFormOptions SetResetForm(bool resetForm)
        {
            this.ResetForm = resetForm;
            return this;
        }

        /// <summary>
        /// 设置成功时是否自动关闭模式框
        /// </summary>
        /// <param name="closeDialog">是否自动关闭模式框</param>
        public AjaxFormOptions SetCloseDialog(bool closeDialog)
        {
            this.CloseDialog = closeDialog;
            return this;
        }


        /// <summary>
        /// 设置提交表单前的Javascript回调函数<br/>
        /// 原型为 function(arr, $form, options)
        /// </summary>
        /// <param name="beforeSubmitCallBack">提交表单前的Javascript回调函数</param>
        public AjaxFormOptions SetBeforeSubmitCallBack(string beforeSubmitCallBack)
        {
            this.BeforeSubmitCallBack = beforeSubmitCallBack;
            return this;
        }

        /// <summary>
        /// 设置提交表单成功时的Javascript回调函数<br/>
        /// 原型为 function(response, statusText, xhr, $form)
        /// </summary>
        /// <param name="onSuccessCallBack">提交表单成功时的Javascript回调函数</param>
        public AjaxFormOptions SetOnSuccessCallBack(string onSuccessCallBack)
        {
            this.OnSuccessCallBack = onSuccessCallBack;
            return this;
        }

        /// <summary>
        /// 设置提交表单失败时的Javascript回调函数<br/>
        /// 若不赋值，则默认自动加载服务器返回的html内容<br/>
        /// 原型为 function(response, statusText, xhr, $form)
        /// </summary>
        public AjaxFormOptions SetOnErrorCallBack(string onErrorCallBack)
        {
            this.OnErrorCallBack = onErrorCallBack;
            return this;
        }

        #endregion

        /// <summary>
        /// 转为Html属性集合
        /// </summary>
        public IDictionary<string, object> ToHtmlAttributes()
        {
            var result = new Dictionary<string, object>();
            result["plugin"] = "ajaxForm";
            var data = new Dictionary<string, object>();
            data.TryAdd("target", this.Target);
            data.TryAdd("replaceTarget", this.ReplaceTarget);
            if (this.Method.HasValue)
                data.TryAdd("type", this.Method.Value == FormMethod.Post ? "POST" : "GET");
            if (this.DataType.HasValue)
            {
                string dataType = "html";

                switch (this.DataType)
                {
                    case AjaxDataType.Html:
                        break;
                    case AjaxDataType.Xml:
                        dataType = "xml";
                        break;
                    case AjaxDataType.Json:
                        dataType = "json";
                        break;
                    case AjaxDataType.Script:
                        dataType = "script";
                        break;
                    default:
                        break;
                }
                data.TryAdd("dataType", dataType);
            }
            data.TryAdd("beforeSubmitFn", this.BeforeSubmitCallBack);
            data.TryAdd("successFn", this.OnSuccessCallBack);
            data.TryAdd("errorFn", this.OnErrorCallBack);
            data.TryAdd("resetForm", this.ResetForm);
            data.TryAdd("clearForm", this.ClearForm);
            data.TryAdd("closeDialog", this.CloseDialog);
            data.TryAdd("data", this.ExtraData);
            data.TryAdd("url", this.Url);
            result.Add("data", Json.Encode(data));
            return result;
        }
    }
}
