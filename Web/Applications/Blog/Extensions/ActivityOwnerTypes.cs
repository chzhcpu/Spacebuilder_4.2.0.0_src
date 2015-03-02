//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-11-9</createdate>
//<author>wanf</author>
//<email>wanf@tunynet.com</email>
//<log date="2012-11-9" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Tunynet.Common;

namespace Spacebuilder.Blog
{
    public static class ActivityOwnerTypesExtension
    {
        /// <summary>
        /// 日志
        /// </summary>
        public static int Blog(this ActivityOwnerTypes ActivityOwnerTypes)
        {
            return 1002;
        }
    }
}