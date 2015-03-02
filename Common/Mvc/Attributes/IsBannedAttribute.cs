using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 判断用户是否被封禁
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class IsBannedAttribute : FilterAttribute, IAuthorizationFilter
    {
      
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (filterContext.IsChildAction)
                return;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                if (currentUser.IsBanned)
                {
                    IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
                    authenticationService.SignOut();
                    filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
                    {
                        Title = "帐号被封禁！",
                        Body = "由于您的非法操作，您的帐号已被封禁，如有疑问，请联系管理员",
                        StatusMessageType = StatusMessageType.Error
                    }));
                }
                return;
            }
            return;
        }
    }
}
