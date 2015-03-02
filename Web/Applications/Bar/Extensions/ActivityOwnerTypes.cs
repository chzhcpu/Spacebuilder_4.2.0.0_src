//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-09-25</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-09-25" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Tunynet.Common;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧拥有者类型
    /// </summary>
    public static class ActivityOwnerTypesExtension
    {
        /// <summary>
        /// 帖吧
        /// </summary>
        public static int BarSection(this ActivityOwnerTypes ActivityOwnerTypes)
        {
            return 12;
        }
    }

}
