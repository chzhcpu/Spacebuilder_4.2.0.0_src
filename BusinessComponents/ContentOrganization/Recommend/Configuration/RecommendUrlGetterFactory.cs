//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-11-29</createdate>
//<author>bianchx</author>
//<email>bianchx@tunynet.com</email>
//<log date="2012-11-29" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Tunynet.Common
{
    /// <summary>
    /// 推荐获取连接的工厂
    /// </summary>
    public static class RecommendUrlGetterFactory
    {
        /// <summary>
        /// 获取连接的方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns>获取连接的实例</returns>
        public static IRecommendUrlGetter Get(string tenantTypeId)
        {
            return DIContainer.Resolve<IEnumerable<IRecommendUrlGetter>>().Where(n => n.TenantTypeId.Equals(tenantTypeId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}