//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-11-07</createdate>
//<author>jiangshl</author>
//<email>jiangshl@tunynet.com</email>
//<log date="2012-11-07" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet.Common;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 扩展NoticeTemplateNames
    /// </summary>
    public static class NoticeTemplateNamesExtension
    {
        
        /// <summary>
        /// 日志有新评论
        /// </summary>
        public static string NewBlogComment(this NoticeTemplateNames noticeTemplateNames)
        {
            return "NewBlogComment";
        }

    }
}