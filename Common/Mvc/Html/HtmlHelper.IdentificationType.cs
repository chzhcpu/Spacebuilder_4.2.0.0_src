//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    public static class HtmlHelperIdentificationTypeExtensions
    {
        /// <summary>
        /// 获取某人通过验证的身份认证标识
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static MvcHtmlString IdentificationType(this HtmlHelper htmlHelper, long userId)
        {
            if (userId <= 0)
                return MvcHtmlString.Empty;
            //获取某人通过验证的身份认证
            IdentificationService identificationService = new IdentificationService();
            var identificationTypes = identificationService.GetIdentificationTypes(userId);

            if (identificationTypes == null)
                return MvcHtmlString.Empty;

            StringBuilder builder = new StringBuilder();
            TagBuilder imgBuilder;

            foreach (var identificationType in identificationTypes)
            {
                imgBuilder = new TagBuilder("img");
                imgBuilder.MergeAttribute("src", SiteUrls.Instance().LogoUrl(identificationType.IdentificationTypeLogo, TenantTypeIds.Instance().IdentificationType(), ImageSizeTypeKeys.Instance().Small()));
                imgBuilder.MergeAttribute("alt", identificationType.Name);
                imgBuilder.MergeAttribute("title", identificationType.Name);
                builder.Append(imgBuilder.ToString());
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
