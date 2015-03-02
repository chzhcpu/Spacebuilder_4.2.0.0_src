//<TunynetCopyright>
//--------------------------------------------------------------
//<copyright>拓宇网络科技有限公司 2005-2012</copyright>
//<version>V0.5</verion>
//<createdate>2012-06-15</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-06-15" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Web;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Spacebuilder.Common;


namespace Spacebuilder.Bar
{
    /// <summary>
    /// 用于处理帖吧访问权限的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BarSectionAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {

        #region IAuthorizationFilter 成员
        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            AuthorizeCore(filterContext);
        }

        #endregion

        private void AuthorizeCore(AuthorizationContext filterContext)
        {
            string spaceKey = UserContext.CurrentSpaceKey(filterContext);
            long sectionId = filterContext.RequestContext.GetParameterFromRouteDataOrQueryString<long>("sectionId");
            if (sectionId == 0)
                throw new ExceptionFacade("sectionId为0");
            BarSectionService barSectionService = new BarSectionService();
            BarSection section = barSectionService.Get(sectionId);
            if (section == null)
                throw new ExceptionFacade("找不到当前帖吧");
            if (section.IsEnabled)
                return;
            if (DIContainer.Resolve<Authorizer>().BarSection_Manage(section))
                return;

            filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
            {
                Title = "帖吧未启用",
                Body = "您访问的帖吧未启用，暂时不允许访问",
                StatusMessageType = StatusMessageType.Hint
            })/* 跳向无权访问页 */);
        }
    }
}