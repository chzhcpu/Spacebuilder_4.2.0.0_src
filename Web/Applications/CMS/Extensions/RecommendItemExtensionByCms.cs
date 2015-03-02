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

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 推荐项类型扩展类
    /// </summary>
    public static class RecommendItemExtensionByBarSection
    {
        /// <summary>
        /// 资讯
        /// </summary>
        public static ContentItem GetContentItem(this RecommendItem item)
        {
            return new ContentItemService().Get(item.ItemId);
        }

        /// <summary>
        /// 评论
        /// </summary>
        public static Comment GetComment(this RecommendItem item)
        {
            return new CommentService().Get(item.ItemId);
        }
    }
}