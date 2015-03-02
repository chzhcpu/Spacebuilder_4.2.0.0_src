using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet.Common;
using System.Web.Mvc.Html;
using Spacebuilder.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 积分消息提示
    /// </summary>
    public static class HtmlHelperCreditsTipsExtensions
    {
        /// <summary>
        /// 生成动态操作链接
        /// </summary>       
        private static MvcHtmlString PointMessage(this HtmlHelper htmlHelper, long userId)
        {
            PointService pointService = new PointService();
            IEnumerable<PointCategory> pointCategories = pointService.GetPointCategories();

            htmlHelper.ViewData["ExperiencePoints"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ExperiencePoints")).CategoryName;
            htmlHelper.ViewData["ReputationPoints"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("ReputationPoints")).CategoryName;
            htmlHelper.ViewData["TradePoints"] = pointCategories.FirstOrDefault(n => n.CategoryKey.Equals("TradePoints")).CategoryName;

            PointRecord pointRecord = pointService.GetUserLastestRecord(userId);
            htmlHelper.ViewData["PointRecord"] = pointRecord;
            return htmlHelper.DisplayForModel("PointMessage");
        }

        /// <summary>
        /// 生成动态操作链接
        /// </summary>       
        public static MvcHtmlString PointMessage(this HtmlHelper htmlHelper)
        {
            if (UserContext.CurrentUser == null)
                return null;

            return htmlHelper.PointMessage(UserContext.CurrentUser.UserId);
        }
    }
}
