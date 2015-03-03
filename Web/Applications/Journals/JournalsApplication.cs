//<TunynetCopyright>
//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Tunynet.Utilities;
using System.IO;
using System.Xml.Linq;
using Tunynet.UI;
using Tunynet.Globalization;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// Journals应用
    /// </summary>
    public class JournalsApplication : ApplicationBase
    {

        protected JournalsApplication(ApplicationModel model/*, ApplicationConfig config*/)
            : base(model/*, config*/)
        {  }

        /// <summary>
        /// 用户在某个呈现区域安装应用时，需要处理的数据
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        protected override bool Install(string presentAreaKey, long ownerId)
        {
            //todo：这里可以处理用户安装此应用时的一些业务逻辑
            return true;
        }

        /// <summary>
        /// 用户在某个呈现区域卸载应用时，需要处理的数据
        /// </summary>
        /// <param name="presentAreaKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        protected override bool UnInstall(string presentAreaKey, long ownerId)
        {
            //todo：这里可以处理用户卸载此应用时的一些业务逻辑
            return true;
        }

        /// <summary>
        /// 删除用户时，处理用户在应用中的数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="takeOverUserName"></param>
        /// <param name="isTakeOver"></param>
        protected override void DeleteUser(long userId, string takeOverUserName, bool isTakeOver)
        {
            //JournalsService repairService = new JournalsService();
            //repairService.DeleteUser(userId, takeOverUserName, isTakeOver);
        }
    }
}