//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-12-21</createdate>
//<author>zhaok</author>
//<email>zhaok@tunynet.com</email>
//<log date="2012-12-21" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博推荐Url获取器
    /// </summary>
    public class MicroblogTopicRecommendUrlGetter : IRecommendUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().Microblog(); }
        }

        /// <summary>
        /// 详细页面地址
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <returns></returns>
        public string RecommendItemDetail(long itemId)
        {
            Tag topic = new TagService(TenantTypeIds.Instance().Microblog()).Get(itemId);
            if (topic == null)
                return string.Empty;
            return SiteUrls.Instance().MicroblogTopic(topic.TagName);
        }
    }
}