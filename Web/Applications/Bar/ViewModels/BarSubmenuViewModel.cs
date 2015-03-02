////<TunynetCopyright>
////--------------------------------------------------------------
////<version>V0.5</verion>
////<createdate>2012-08-28</createdate>
////<author>bianchx</author>
////<email>bianchx@tunynet.com</email>
////<log date="2012-08-28" version="0.5">创建</log>
////--------------------------------------------------------------
////</TunynetCopyright>

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Spacebuilder.Bar
//{
//    /// <summary>
//    /// 帖吧的局部页面的ViewModel
//    /// </summary>
//    public class BarSubmenuViewModel
//    {
//        /// <summary>
//        /// 帖吧的id
//        /// </summary>
//        public long SectionId { get; set; }

//        /// <summary>
//        /// 帖吧的名称
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 应用名称
//        /// </summary>
//        public string ApplicationName { get; set; }
//    }

//    /// <summary>
//    /// 将帖吧转换成ViewModel的扩展类
//    /// </summary>
//    public static class BarSubmenuExtensions
//    {
//        /// <summary>
//        /// 转换为BarSubmenuViewModel
//        /// </summary>
//        /// <returns></returns>
//        public static BarSubmenuViewModel AsBarSubmenuViewModel(this BarSection barSection)
//        {
//            return new BarSubmenuViewModel
//            {
//                ApplicationName = "帖吧",
//                Name = barSection.Name,
//                SectionId = barSection.SectionId
//            };
//        }
//    }
//}