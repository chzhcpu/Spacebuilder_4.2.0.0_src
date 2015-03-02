//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-10-09</createdate>
//<author>bianchx</author>
//<email>bianchx@tunynet.com</email>
//<log date="2012-10-09" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spacebuilder.Bar
{
    public enum BarManageableCountType
    {

        //待审核的贴吧
        PendingSection,

        //全部
        All = 3,

        //待审核
        Pending = 20,

        //在审核
        Again = 30,

        //最近24小时
        Last24H

    }
}