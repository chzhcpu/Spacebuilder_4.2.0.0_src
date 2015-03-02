//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-09-28</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-09-28" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Spacebuilder.Common;
using Tunynet.Utilities;
using Tunynet.Common;
using Spacebuilder.Group;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧Url获取
    /// </summary>
    public class BarTagUrlGetter : ITagUrlGetter
    {

        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string GetUrl(string tagName, long ownerId = 0)
        {
            if (ownerId > 0)
            {
                return SiteUrls.Instance().GroupThreadListByTag(GroupIdToGroupKeyDictionary.GetGroupKey(ownerId), tagName);
            }
            else
            {
                return SiteUrls.Instance().ListsByTag(tagName);
            }
        }
    }
}