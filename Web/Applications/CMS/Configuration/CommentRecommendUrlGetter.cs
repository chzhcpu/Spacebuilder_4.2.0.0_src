//<TunynetCopyright>
//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯推荐Url获取器
    /// </summary>
    public class CommentRecommendUrlGetter : IRecommendUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().Comment(); }
        }

        /// <summary>
        /// 详细页面地址
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <returns></returns>
        public string RecommendItemDetail(long itemId)
        {
            Comment comment = new CommentService().Get(itemId);
            if (comment == null)
                return string.Empty;
            return SiteUrls.Instance().Comments(comment.CommentedObjectId, comment.Id);
        }
    }
}