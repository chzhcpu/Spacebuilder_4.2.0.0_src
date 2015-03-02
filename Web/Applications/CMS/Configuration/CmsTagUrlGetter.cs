//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Spacebuilder.Common;
using Tunynet.Common;


namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯标签云Url获取
    /// </summary>
    public class CmsTagUrlGetter : ITagUrlGetter
    {
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string GetUrl(string tagName, long ownerId = 0)
        {
            return SiteUrls.Instance().CmsTagDetail(tagName);
        }
    }
}