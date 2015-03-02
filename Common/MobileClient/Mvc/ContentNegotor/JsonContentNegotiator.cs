using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace Spacebuilder.MobileClient.Common
{
    
    /// <summary>
    /// Json执行内容协商
    /// </summary>
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        /// <summary>
        /// 通过在已为给定 request 传入的 formatters 中选择可以序列化给定 type 的对象的最适当 System.Net.Http.Formatting.MediaTypeFormatter，来执行内容协商。
        /// </summary>
        /// <param name="type">要序列化的类型</param>
        /// <param name="request">请求消息，其中包含用于执行协商的标头值。</param>
        /// <param name="formatters">可供选择的 System.Net.Http.Formatting.MediaTypeFormatter 对象集。</param>
        /// <returns></returns>
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }
    }
}
