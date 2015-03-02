//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-10-16</createdate>
//<author>jiangshl</author>
//<email>jiangshl@tunynet.com</email>
//<log date="2012-10-16" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>


using Spacebuilder.Common;
using Tunynet.Common;
using System;

using System.Web.Routing;
using Tunynet.Utilities;


namespace Spacebuilder.Blog
{
    /// <summary>
    /// 日志标签云Url获取
    /// </summary>
    public class BlogTagUrlGetter : ITagUrlGetter
    {

        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <returns>点击标签的链接</returns>
        public string GetUrl(string tagName, long ownerId = 0)
        {
            return SiteUrls.Instance().BlogListByTag(tagName);
        }
    }
}