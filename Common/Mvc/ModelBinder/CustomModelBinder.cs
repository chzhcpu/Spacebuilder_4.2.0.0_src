

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;

namespace Spacebuilder.Common
{
    public class CustomModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.FullName.Contains("System.String"))
            {
                bool cleanHtml = false;
                if (!bindingContext.ModelMetadata.RequestValidationEnabled)
                {
                    cleanHtml = true;
                }
                bindingContext.ModelMetadata.RequestValidationEnabled = false;
                var value = base.BindModel(controllerContext, bindingContext);

                if (value == null)
                {
                    return value;
                }

                if (value is Array)
                {
                    string[] tempArray = (string[])value;

                    //处理敏感词
                    if (tempArray != null && tempArray.Length > 0)
                    {
                        for (int i = 0; i < tempArray.Length; i++)
                        {

                            if (FilterSensitiveWord(bindingContext, tempArray[i], out tempArray[i]))
                                break;
                            tempArray[i] = this.formatInputValue(bindingContext, tempArray[i], cleanHtml);
                        }
                    }

                    return tempArray;
                }
                else if (value is string)
                {
                    string tempValue = value as string;
                    if (!string.IsNullOrEmpty(tempValue))
                        tempValue = tempValue.Trim();

                    Type type = bindingContext.ModelMetadata.ContainerType;
                    PropertyInfo propertyInfo = null;
                    if (type != null)
                    {
                        propertyInfo = type.GetProperty(bindingContext.ModelName);
                    }

                    var noFilterWordAttribute = propertyInfo != null ? Attribute.GetCustomAttribute(propertyInfo, typeof(NoFilterWordAttribute)) as NoFilterWordAttribute : null;
                    if (noFilterWordAttribute == null)
                    {
                        if (FilterSensitiveWord(bindingContext, tempValue, out tempValue))
                            return tempValue;
                    }
                    var noCleanHtmlAttribute = propertyInfo != null ? Attribute.GetCustomAttribute(propertyInfo, typeof(NoCleanHtmlAttribute)) as NoCleanHtmlAttribute : null;
                    if (noCleanHtmlAttribute != null)
                    {
                        return tempValue;
                    }
                    return this.formatInputValue(bindingContext, tempValue, cleanHtml);
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
        private string formatInputValue(ModelBindingContext bindingContext, string inputValue, bool cleanHtml)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                return inputValue;
            }
            if (cleanHtml)
            {
                //处理多行纯文本
                inputValue = HtmlUtility.CleanHtml(inputValue, TrustedHtmlLevel.HtmlEditor);
            }
            else
            {
                //处理html标签
                inputValue = HtmlUtility.StripHtml(inputValue, true, false);

                inputValue = Formatter.FormatMultiLinePlainTextForStorage(inputValue, false);
            }
            inputValue = StringUtility.StripSQLInjection(inputValue);

            if (string.IsNullOrEmpty(inputValue))
            {
                bindingContext.ModelState.AddModelError("UnTrustedHtml", "内容未通过验证或包含非法字符如<、>！");
            }

            return inputValue;
        }

        private bool FilterSensitiveWord(ModelBindingContext bindingContext, string inputText, out string outputText)
        {
            WordFilterStatus status = WordFilterStatus.Banned;
            outputText = WordFilter.SensitiveWordFilter.Filter(inputText, out status);
            if (status == WordFilterStatus.Banned)
            {
                bindingContext.ModelState.AddModelError("SensitiveWord", "内容未通过验证或包含非法词语！");
                return true;
            }

            return false;
        }
    }
}
