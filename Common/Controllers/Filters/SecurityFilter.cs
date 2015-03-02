
using System;
using System.Web.Mvc;
using Tunynet.Logging;


namespace Spacebuilder.Common
{
    /// <summary>
    /// 安全校验过滤器
    /// </summary>
    public class SecurityFilter : IAuthorizationFilter, IActionFilter
    {
        /// <summary>
        /// 校验CSRF Token
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //根据Area分区Name将移动端的请求直接返回
            var routeDataDictionary = filterContext.RequestContext.RouteData.DataTokens;
            if (routeDataDictionary.ContainsKey("area"))
            {
                string areaName = routeDataDictionary["area"].ToString();
                if (areaName.ToLower().Contains("mobileclient"))
                    return;
            }

            if (!filterContext.HttpContext.Request.IsAjaxRequest() && string.Equals("post", filterContext.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                string token = string.Empty;

                token = filterContext.HttpContext.Request.Form.Get<string>("CurrentUserIdToken", string.Empty);

                if (string.IsNullOrEmpty(token))
                    token = filterContext.HttpContext.Request.QueryString.Get<string>("CurrentUserIdToken", string.Empty);

                if (!string.IsNullOrEmpty(token))
                {
                    token = Tunynet.Utilities.WebUtility.UrlDecode(token);
                    bool isTimeOut = false;
                    long userId = Utility.DecryptTokenForUploadfile(token.ToString(), out isTimeOut);
                    if (userId > 0)
                        return;
                }

                ValidateAntiForgeryTokenAttribute _validator = new ValidateAntiForgeryTokenAttribute();
                try
                {
                    _validator.OnAuthorization(filterContext);
                }
                catch (Exception ex)
                {
                    LoggerFactory.GetLogger().Error(ex, "安全校验过滤器,校验CSRF Token时失败。");
                    filterContext.HttpContext.Response.Redirect(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
                        {
                            StatusMessageType = StatusMessageType.Error,
                            Title = "安全校验失败",
                            Body = "安全校验失败！"
                        }));
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //根据Area分区Name将移动端的请求直接返回
            var routeDataDictionary = filterContext.RequestContext.RouteData.DataTokens;
            if (routeDataDictionary.ContainsKey("area"))
            {
                string areaName = routeDataDictionary["area"].ToString();
                if (areaName.ToLower().Contains("mobileclient"))
                    return;
            }

            if (string.Equals("post", filterContext.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                if (!filterContext.Controller.ViewData.ModelState.IsValid && filterContext.Controller.ViewData.ModelState.Keys.Contains("UnTrustedHtml"))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                        filterContext.Result = new JsonResult() { Data = new StatusMessageData(StatusMessageType.Error, "表单验证失败，请检查输入数据是否有误，例如文本不能包含特殊字符<、>！") };
                    else
                        filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
                        {
                            StatusMessageType = StatusMessageType.Error,
                            Title = "表单验证失败",
                            Body = "请检查输入数据是否有误，例如文本不能包含特殊字符<、>！"
                        }));
                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

    }
}