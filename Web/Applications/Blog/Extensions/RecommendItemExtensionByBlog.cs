//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-12-20</createdate>
//<author>zhaok</author>
//<email>zhaok@tunynet.com</email>
//<log date="2012-12-20" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Routing;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 推荐项类型扩展类
    /// </summary>
    public static class RecommendItemExtensionByBlog
    {
        /// <summary>
        /// 创建的群组数
        /// </summary>
        public static BlogThread GetBlogThread(this RecommendItem item)
        {
            return new BlogService().Get(item.ItemId);
        }
    }
}