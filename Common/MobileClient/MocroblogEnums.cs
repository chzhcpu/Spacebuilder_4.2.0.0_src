using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.MobileClient.Common
{
    /// <summary>
    /// 微博发布方式
    /// </summary>
    public enum PostWay
    {
        /// <summary>
        /// 网页
        /// </summary>
        Web,

        /// <summary>
        /// 桌面客户端
        /// </summary>
        PCClient,

        /// <summary>
        /// 移动客户端
        /// </summary>
        Android,

        /// <summary>
        /// 安卓平板
        /// </summary>
        AndroidPad,

        /// <summary>
        /// IPhone
        /// </summary>
        IPhone,

        /// <summary>
        /// IPad
        /// </summary>
        IPad,

        /// <summary>
        /// WindowsPhone
        /// </summary>
        WindowsPhone,

        /// <summary>
        /// Surface
        /// </summary>
        Surface
    }

    /// <summary>
    /// 获取枚举类型的名称
    /// </summary>
    public static class PostWayExpand
    {
        public static string GetName(this PostWay postWay)
        {
            string name = "";
            switch (postWay)
            {
                case PostWay.Web:
                    name = "网页";
                    break;
                case PostWay.PCClient:
                    name = "近乎PC客户端";
                    break;
                case PostWay.Android:
                    name = "近乎Android客户端";
                    break;
                case PostWay.AndroidPad:
                    name = "近乎Android平板";
                    break;
                case PostWay.IPhone:
                    name = "近乎IPhone客户端";
                    break;
                case PostWay.IPad:
                    name = "近乎IPad客户端";
                    break;
                case PostWay.WindowsPhone:
                    name = "近乎WindowsPhone客户端";
                    break;
                case PostWay.Surface:
                    name = "近乎Surface客户端";
                    break;
            }
            return name;
        }
    }
}
