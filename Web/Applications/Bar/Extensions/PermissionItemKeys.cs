//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2013-3-27</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2013-3-27" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet.Common;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 权限项标识扩展类
    /// </summary>
    public static class PermissionItemKeysExtension
    {
        /// <summary>
        /// 创建问题的权限项标识
        /// </summary>
        /// <param name="pik"><see cref="PermissionItemKeys"/></param>
        /// <returns></returns>
        public static string Bar_CreateThread(this PermissionItemKeys pik)
        {
            return "Bar_CreateThread";
        }

        /// <summary>
        /// 回答问题的权限项标识
        /// </summary>
        /// <param name="pik"><see cref="PermissionItemKeys"/></param>
        /// <returns></returns>
        public static string Bar_CreatePost(this PermissionItemKeys pik)
        {
            return "Bar_CreatePost";
        }
    }
}