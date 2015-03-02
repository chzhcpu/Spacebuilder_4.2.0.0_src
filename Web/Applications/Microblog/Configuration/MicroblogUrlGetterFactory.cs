//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-12-03</createdate>
//<author>zhaok</author>
//<email>zhaok@tunynet.com</email>
//<log date="2012-12-03" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微薄中获取连接的接口
    /// </summary>
    public static class MicroblogUrlGetterFactory
    {
        /// <summary>
        /// 获取连接的方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns>获取连接的实例</returns>
        public static IMicroblogUrlGetter Get(string tenantTypeId)
        {
            return DIContainer.Resolve<IEnumerable<IMicroblogUrlGetter>>().Where(n => n.TenantTypeId.Equals(tenantTypeId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}