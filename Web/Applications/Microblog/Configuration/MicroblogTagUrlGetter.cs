//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-09-28</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-09-28" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet.Common;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博Url获取
    /// </summary>
    public class MicroblogTagUrlGetter : ITagUrlGetter
    {

        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string GetUrl(string tagName, long ownerId = 0)
        {
            return string.Empty;
        }
    }
}