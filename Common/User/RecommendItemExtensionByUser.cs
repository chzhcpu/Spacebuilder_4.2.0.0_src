//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 推荐项类型扩展类
    /// </summary>
    public static class RecommendItemExtensionByUser
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        public static User GetFullUser(this RecommendItem item)
        {
            return DIContainer.Resolve<UserService>().GetFullUser(item.ItemId);
        }
    }
}