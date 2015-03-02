//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-08-23</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-08-23" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Bar
{

    /// <summary>
    /// 帖吧动态项
    /// </summary>
    public static class ActivityItemKeysExtension
    {
        /// <summary>
        /// 发布帖子动态项
        /// </summary>
        public static string CreateBarThread(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBarThread";
        }

        /// <summary>
        /// 发布回帖动态项
        /// </summary>
        public static string CreateBarPost(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBarPost";
        }

        /// <summary>
        /// 帖子评分动态项
        /// </summary>
        public static string CreateBarRating(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBarRating";
        }

    }

}
