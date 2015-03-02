//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于IHttpHandler的RouteHandler
    /// </summary>
    public class HttpHandlerRouteHandler<THttpHandler> : IRouteHandler where THttpHandler : IHttpHandler
    {
        Func<RequestContext, THttpHandler> _handlerFactory = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handlerFactory"></param>
        public HttpHandlerRouteHandler(Func<RequestContext, THttpHandler> handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        /// <summary>
        /// 获取IHttpHandler
        /// </summary>
        /// <param name="requestContext"><see cref="RequestContext"/></param>
        /// <returns></returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return _handlerFactory(requestContext);
        }
    }
}
