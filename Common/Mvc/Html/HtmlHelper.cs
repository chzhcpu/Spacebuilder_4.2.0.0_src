//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Tunynet.Globalization;
using Tunynet.Utilities;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 封装对HtmlHelper的扩展方法
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 生成BodyId
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        public static string GenerateBodyId(this HtmlHelper htmlHelper)
        {
            RouteValueDictionary routeValueDictionary = htmlHelper.ViewContext.RouteData.Values;
            return routeValueDictionary.Get<string>("Controller", string.Empty) + "_" + routeValueDictionary.Get<string>("Action", string.Empty);
        }

        #region 图标

        /// <summary>
        /// 输出图标
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="type">图标类型</param>
        /// <param name="title">鼠标提示文字</param>
        /// <param name="htmlAttributes">html属性集合</param>
        /// <returns>图标的html代码</returns>
        public static MvcHtmlString Icon(this HtmlHelper htmlHelper, IconTypes type, string title = null, object htmlAttributes = null)
        {
            TagBuilder span = new TagBuilder("span");
            switch (type)
            {

                case IconTypes.TriangleUp:
                    span.AddCssClass("tn-icon-triangle-up");
                    break;
                case IconTypes.TriangleRight:
                    span.AddCssClass("tn-icon-triangle-right");
                    break;
                case IconTypes.TriangleDown:
                    span.AddCssClass("tn-icon-triangle-down");
                    break;
                case IconTypes.TriangleLeft:
                    span.AddCssClass("tn-icon-triangle-left");
                    break;

                case IconTypes.CollapseOpen:
                    span.AddCssClass("tn-icon-collapse-open");
                    break;
                case IconTypes.CollapseClose:
                    span.AddCssClass("tn-icon-collapse-close");
                    break;


                case IconTypes.Download:
                    span.AddCssClass("tn-icon-download");
                    break;
                case IconTypes.Upload:
                    span.AddCssClass("tn-icon-upload");
                    break;

                case IconTypes.Expand:
                    span.AddCssClass("tn-icon-expand");
                    break;
                case IconTypes.Fold:
                    span.AddCssClass("tn-icon-fold");
                    break;
                case IconTypes.SlideNext:
                    span.AddCssClass("tn-icon-slide-next");
                    break;
                case IconTypes.SlidePrev:
                    span.AddCssClass("tn-icon-slide-prev");
                    break;
                case IconTypes.First:
                    span.AddCssClass("tn-icon-first");
                    break;
                case IconTypes.Last:
                    span.AddCssClass("tn-icon-last");
                    break;
                case IconTypes.Add:
                    span.AddCssClass("tn-icon-add");
                    break;
                case IconTypes.Write:
                    span.AddCssClass("tn-icon-write");
                    break;
                case IconTypes.Update:
                    span.AddCssClass("tn-icon-update");
                    break;
                case IconTypes.Set:
                    span.AddCssClass("tn-icon-set");
                    break;
                case IconTypes.Config:
                    span.AddCssClass("tn-icon-config");
                    break;
                case IconTypes.Cross:
                    span.AddCssClass("tn-icon-cross");
                    break;
                case IconTypes.Accept:
                    span.AddCssClass("tn-icon-accept");
                    break;
                case IconTypes.RotateLeft:
                    span.AddCssClass("tn-icon-rotate-left");
                    break;
                case IconTypes.RotateRight:
                    span.AddCssClass("tn-icon-rotate-right");
                    break;
                case IconTypes.Enlarge:
                    span.AddCssClass("tn-icon-enlarge");
                    break;
                case IconTypes.School:
                    span.AddCssClass("tn-icon-school");
                    break;
                case IconTypes.Question:
                    span.AddCssClass("tn-icon-question");
                    break;
                case IconTypes.Friendly:
                    span.AddCssClass("tn-icon-friendly");
                    break;
                case IconTypes.Sneak:
                    span.AddCssClass("tn-icon-sneak");
                    break;
                //case IconTypes.Forward:
                //    span.AddCssClass("tn-icon-forward");
                //    break;
                case IconTypes.Stop:
                    span.AddCssClass("tn-icon-stop");
                    break;
                //case IconTypes.Set:
                //    span.AddCssClass("tn-icon-set");
                //    break;
                case IconTypes.Elite:
                    span.AddCssClass("tn-icon-elite");
                    break;
                case IconTypes.EmailOpen:
                    span.AddCssClass("tn-icon-email-open");
                    break;
                case IconTypes.Top:
                    span.AddCssClass("tn-icon-top");
                    break;
                case IconTypes.Flag:
                    span.AddCssClass("tn-icon-flag");
                    break;
                case IconTypes.Lock:
                    span.AddCssClass("tn-icon-lock");
                    break;
                case IconTypes.Key:
                    span.AddCssClass("tn-icon-key");
                    break;
                case IconTypes.Limit:
                    span.AddCssClass("tn-icon-limit");
                    break;
                case IconTypes.Coins:
                    span.AddCssClass("tn-icon-coins");
                    break;
                case IconTypes.Fire:
                    span.AddCssClass("tn-icon-fire");
                    break;
                case IconTypes.Move:
                    span.AddCssClass("tn-icon-move");
                    break;
                case IconTypes.Approve:
                    span.AddCssClass("tn-icon-approve");
                    break;
                case IconTypes.View:
                    span.AddCssClass("tn-icon-view");
                    break;
                case IconTypes.ThumbUp:
                    span.AddCssClass("tn-icon-thumb-up");
                    break;
                case IconTypes.ThumbDown:
                    span.AddCssClass("tn-icon-thumb-down");
                    break;
                case IconTypes.Share:
                    span.AddCssClass("tn-icon-share");
                    break;
                case IconTypes.Bubble:
                    span.AddCssClass("tn-icon-bubble");
                    break;
                case IconTypes.Favorite:
                    span.AddCssClass("tn-icon-favorite");
                    break;
                case IconTypes.Star:
                    span.AddCssClass("tn-icon-star");
                    break;
                case IconTypes.Feed:
                    span.AddCssClass("tn-icon-feed");
                    break;
                //case IconTypes.QuotesBefore:
                //    span.AddCssClass("tn-icon-quotes-before");
                //    break;
                //case IconTypes.Topic:
                //    span.AddCssClass("tn-icon-topic");
                //    break;
                case IconTypes.Label:
                    span.AddCssClass("tn-icon-label");
                    break;
                case IconTypes.User:
                    span.AddCssClass("tn-icon-user");
                    break;
                case IconTypes.UserAdd:
                    span.AddCssClass("tn-icon-user-add");
                    break;
                case IconTypes.UserAllow:
                    span.AddCssClass("tn-icon-user-allow");
                    break;
                case IconTypes.UserStop:
                    span.AddCssClass("tn-icon-user-stop");
                    break;
                case IconTypes.UserRelation:
                    span.AddCssClass("tn-icon-user-relation");
                    break;
                case IconTypes.UserAvatar:
                    span.AddCssClass("tn-icon-user-avatar");
                    break;
                case IconTypes.UserInvite:
                    span.AddCssClass("tn-icon-user-invite");
                    break;
                case IconTypes.UserCard:
                    span.AddCssClass("tn-icon-user-card");
                    break;
                case IconTypes.Chain:
                    span.AddCssClass("tn-icon-chain");
                    break;
                case IconTypes.Pen:
                    span.AddCssClass("tn-icon-pen");
                    break;
                case IconTypes.Creator:
                    span.AddCssClass("tn-icon-creator");
                    break;
                case IconTypes.Manager:
                    span.AddCssClass("tn-icon-manager");
                    break;
                case IconTypes.BrowseList:
                    span.AddCssClass("tn-icon-browse-list");
                    break;
                case IconTypes.BrowseDetail:
                    span.AddCssClass("tn-icon-browse-detail");
                    break;
                case IconTypes.BrowseMedium:
                    span.AddCssClass("tn-icon-browse-medium");
                    break;
                case IconTypes.BrowseSmall:
                    span.AddCssClass("tn-icon-browse-small");
                    break;
                case IconTypes.BrowseSlide:
                    span.AddCssClass("tn-icon-browse-slide");
                    break;
                case IconTypes.Dress:
                    span.AddCssClass("tn-icon-dress");
                    break;
                case IconTypes.Jump:
                    span.AddCssClass("tn-icon-jump");
                    break;
                case IconTypes.Zomm:
                    span.AddCssClass("tn-icon-zomm");
                    break;
                case IconTypes.Camera:
                    span.AddCssClass("tn-icon-camera");
                    break;
                case IconTypes.Calendar:
                    span.AddCssClass("tn-icon-calendar");
                    break;
                //case IconTypes.Color:
                //    span.AddCssClass("tn-icon-color");
                //    break;
                case IconTypes.Admin:
                    span.AddCssClass("tn-icon-admin");
                    break;
                case IconTypes.Auadmin:
                    span.AddCssClass("tn-icon-auadmin");
                    break;
                case IconTypes.Female:
                    span.AddCssClass("tn-icon-female");
                    break;
                case IconTypes.Male:
                    span.AddCssClass("tn-icon-male");
                    break;
                case IconTypes.Movie:
                    span.AddCssClass("tn-icon-moive");
                    break;
                case IconTypes.Sound:
                    span.AddCssClass("tn-icon-sound");
                    break;
                case IconTypes.Music:
                    span.AddCssClass("tn-icon-music");
                    break;
                case IconTypes.Skin:
                    span.AddCssClass("tn-icon-skin");
                    break;
                case IconTypes.Blog:
                    span.AddCssClass("tn-icon-blog");
                    break;
                case IconTypes.Archive:
                    span.AddCssClass("tn-icon-archive");
                    break;
                case IconTypes.Forum:
                    span.AddCssClass("tn-icon-forum");
                    break;
                case IconTypes.Vote:
                    span.AddCssClass("tn-icon-vote");
                    break;
                case IconTypes.News:
                    span.AddCssClass("tn-icon-news");
                    break;
                case IconTypes.Cms:
                    span.AddCssClass("tn-icon-cms");
                    break;
                case IconTypes.Job:
                    span.AddCssClass("tn-icon-job");
                    break;
                case IconTypes.World:
                    span.AddCssClass("tn-icon-world");
                    break;
                case IconTypes.Home:
                    span.AddCssClass("tn-icon-home");
                    break;
                //case IconTypes.Question:
                //    span.AddCssClass("tn-icon-question");
                //    break;
                case IconTypes.Notice:
                    span.AddCssClass("tn-icon-notice");
                    break;
                case IconTypes.Escalator:
                    span.AddCssClass("tn-icon-escalator");
                    break;
                case IconTypes.Alert:
                    span.AddCssClass("tn-icon-alert");
                    break;
                case IconTypes.Exclamation:
                    span.AddCssClass("tn-icon-exclamation");
                    break;
                case IconTypes.CrossCircle:
                    span.AddCssClass("tn-icon-cross-circle");
                    break;
                case IconTypes.AcceptCircle:
                    span.AddCssClass("tn-icon-accept-circle");
                    break;
                case IconTypes.Apply:
                    span.AddCssClass("tn-icon-apply");
                    break;
                case IconTypes.Logout:
                    span.AddCssClass("tn-icon-logout");
                    break;
                case IconTypes.Join:
                    span.AddCssClass("tn-icon-join");
                    break;
                case IconTypes.Quit:
                    span.AddCssClass("tn-icon-quit");
                    break;
                case IconTypes.Emotion:
                    span.AddCssClass("tn-icon-emotion");
                    break;
                case IconTypes.PaperClip:
                    span.AddCssClass("tn-icon-paper-clip");
                    break;
                case IconTypes.Folder:
                    span.AddCssClass("tn-icon-folder");
                    break;
                case IconTypes.Album:
                    span.AddCssClass("tn-icon-album");
                    break;
                case IconTypes.Banned:
                    span.AddCssClass("tn-icon-colorful tn-icon-colorful-banned");
                    break;
                case IconTypes.Moderated:
                    span.AddCssClass("tn-icon-colorful tn-icon-colorful-moderated");
                    break;
                case IconTypes.Group:
                    span.AddCssClass("tn-icon-group");
                    break;
                case IconTypes.Event:
                    span.AddCssClass("tn-icon-event");
                    break;
                case IconTypes.Find:
                    span.AddCssClass("tn-icon-find");
                    break;
                case IconTypes.Picture:
                    span.AddCssClass("tn-icon-picture");
                    break;
                //case IconTypes.Coins:
                //    span.AddCssClass("tn-icon-coins");
                //    break;
                case IconTypes.Topic:
                    span.AddCssClass("tn-icon-topic");
                    break;
                //case IconTypes.Fire:
                //    span.AddCssClass("tn-icon-fire");
                //    break;
                case IconTypes.QuotesBefore:
                    span.AddCssClass("tn-icon-quotes-before");
                    break;
                case IconTypes.Color:
                    span.AddCssClass("tn-icon-color");
                    break;
                case IconTypes.App:
                    span.AddCssClass("tn-icon-app");
                    break;
                case IconTypes.Market:
                    span.AddCssClass("tn-icon-market");
                    break;
                case IconTypes.Answer:
                    span.AddCssClass("tn-icon-answer");
                    break;
                case IconTypes.Microblog:
                    span.AddCssClass("tn-icon-microblog");
                    break;
                case IconTypes.At:
                    span.AddCssClass("tn-icon-at");
                    break;
                case IconTypes.Play:
                    span.AddCssClass("tn-icon-play");
                    break;
                case IconTypes.Pause:
                    span.AddCssClass("tn-icon-pause");
                    break;
                case IconTypes.Discovery:
                    span.AddCssClass("tn-icon-discovery");
                    break;
                case IconTypes.Gift:
                    span.AddCssClass("tn-icon-gift");
                    break;
                case IconTypes.Function:
                    span.AddCssClass("tn-icon-function");
                    break;
                case IconTypes.Chart:
                    span.AddCssClass("tn-icon-chart");
                    break;
                case IconTypes.Clock:
                    span.AddCssClass("tn-icon-clock");
                    break;
                case IconTypes.Pending:
                    span.AddCssClass("tn-icon-pending");
                    break;
                case IconTypes.Datasheet:
                    span.AddCssClass("tn-icon-datasheet");
                    break;
                case IconTypes.System:
                    span.AddCssClass("tn-icon-system");
                    break;
                case IconTypes.Product:
                    span.AddCssClass("tn-icon-product");
                    break;
                case IconTypes.AddPicture:
                    span.AddCssClass("tn-icon-add-picture");
                    break;
                case IconTypes.Ask:
                    span.AddCssClass("tn-icon-ask");
                    break;
                case IconTypes.PointMall:
                    span.AddCssClass("tn-icon-pointmall");
                    break;
                case IconTypes.Bar:
                    span.AddCssClass("tn-icon-bar");
                    break;
                case IconTypes.Wiki:
                    span.AddCssClass("tn-icon-world");
                    break;
                case IconTypes.SmallTriangleUp:
                    span.AddCssClass("tn-smallicon-triangle-up");
                    break;
                case IconTypes.SmallTriangleRight:
                    span.AddCssClass("tn-smallicon-triangle-right");
                    break;
                case IconTypes.SmallTriangleDown:
                    span.AddCssClass("tn-smallicon-triangle-down");
                    break;
                case IconTypes.SmallTriangleLeft:
                    span.AddCssClass("tn-smallicon-triangle-left");
                    break;
                case IconTypes.SmallCollapseOpen:
                    span.AddCssClass("tn-smallicon-collapse-open");
                    break;
                case IconTypes.SmallCollapseClose:
                    span.AddCssClass("tn-smallicon-collapse-close");
                    break;
                case IconTypes.SmallDownload:
                    span.AddCssClass("tn-smallicon-download");
                    break;
                case IconTypes.SmallUpload:
                    span.AddCssClass("tn-smallicon-upload");
                    break;
                case IconTypes.SmallWrite:
                    span.AddCssClass("tn-smallicon-write");
                    break;
                case IconTypes.SmallUpdate:
                    span.AddCssClass("tn-smallicon-update");
                    break;
                case IconTypes.SmallSet:
                    span.AddCssClass("tn-smallicon-set");
                    break;
                case IconTypes.SmallConfig:
                    span.AddCssClass("tn-smallicon-config");
                    break;
                case IconTypes.SmallAdd:
                    span.AddCssClass("tn-smallicon-add");
                    break;
                case IconTypes.SmallCross:
                    span.AddCssClass("tn-smallicon-cross");
                    break;
                case IconTypes.SmallAccept:
                    span.AddCssClass("tn-smallicon-accept");
                    break;
                case IconTypes.SmallStop:
                    span.AddCssClass("tn-smallicon-stop");
                    break;
                case IconTypes.SmallExpand:
                    span.AddCssClass("tn-smallicon-expand");
                    break;
                case IconTypes.SmallFold:
                    span.AddCssClass("tn-smallicon-fold");
                    break;
                case IconTypes.SmallSlideNext:
                    span.AddCssClass("tn-smallicon-slide-next");
                    break;
                case IconTypes.SmallSlidePrev:
                    span.AddCssClass("tn-smallicon-slide-prev");
                    break;
                case IconTypes.SmallSlideFirst:
                    span.AddCssClass("tn-smallicon-slide-first");
                    break;
                case IconTypes.SmallSlideLast:
                    span.AddCssClass("tn-smallicon-slide-last");
                    break;
                case IconTypes.SmallTop:
                    span.AddCssClass("tn-smallicon-top");
                    break;
                case IconTypes.SmallMicroblog:
                    span.AddCssClass("tn-smallicon-microblog");
                    break;
                case IconTypes.SmallRotateLeft:
                    span.AddCssClass("tn-smallicon-rotate-left");
                    break;
                case IconTypes.SmallRotateRight:
                    span.AddCssClass("tn-smallicon-rotate-right");
                    break;
                case IconTypes.SmallEnlarge:
                    span.AddCssClass("tn-smallicon-enlarge");
                    break;
                case IconTypes.SmallLabel:
                    span.AddCssClass("tn-smallicon-label");
                    break;
                case IconTypes.SmallFind:
                    span.AddCssClass("tn-smallicon-find");
                    break;
                case IconTypes.SmallAlert:
                    span.AddCssClass("tn-smallicon-alert");
                    break;
                case IconTypes.SmallElite:
                    span.AddCssClass("tn-smallicon-elite");
                    break;
                case IconTypes.SmallFriendly:
                    span.AddCssClass("tn-smallicon-friendly");
                    break;
                case IconTypes.SmallSneak:
                    span.AddCssClass("tn-smallicon-sneak");
                    break;
                case IconTypes.SmallFemale:
                    span.AddCssClass("tn-smallicon-female");
                    break;
                case IconTypes.SmallMale:
                    span.AddCssClass("tn-smallicon-male");
                    break;
                case IconTypes.SmallAsk:
                    span.AddCssClass("tn-smallicon-ask");
                    break;
                case IconTypes.SmallGroup:
                    span.AddCssClass("tn-smallicon-group");
                    break;
                case IconTypes.SmallPointMall:
                    span.AddCssClass("tn-smallicon-pointmall");
                    break;
                case IconTypes.SmallBar:
                    span.AddCssClass("tn-smallicon-bar");
                    break;
                //32*32_big
                case IconTypes.BigTriangleUp:
                    span.AddCssClass("tn-bigicon-triangle-up");
                    break;
                case IconTypes.BigTriangleRight:
                    span.AddCssClass("tn-bigicon-triangle-right");
                    break;
                case IconTypes.BigTriangleDown:
                    span.AddCssClass("tn-bigicon-triangle-down");
                    break;
                case IconTypes.BigTriangleLeft:
                    span.AddCssClass("tn-bigicon-triangle-left");
                    break;
                case IconTypes.BigCollapseOpen:
                    span.AddCssClass("tn-bigicon-collapse-open");
                    break;
                case IconTypes.BigCollapseClose:
                    span.AddCssClass("tn-bigicon-collapse-close");
                    break;
                case IconTypes.BigDownLoad:
                    span.AddCssClass("tn-bigicon-download");
                    break;
                case IconTypes.BigUpLoad:
                    span.AddCssClass("tn-bigicon-upLoad");
                    break;
                case IconTypes.BigExpand:
                    span.AddCssClass("tn-bigicon-expand");
                    break;
                case IconTypes.BigFold:
                    span.AddCssClass("tn-bigicon-fold");
                    break;
                case IconTypes.BigSlideNext:
                    span.AddCssClass("tn-bigicon-slide-next");
                    break;
                case IconTypes.BigSlidePrev:
                    span.AddCssClass("tn-bigicon-slide-prev");
                    break;
                case IconTypes.BigFirst:
                    span.AddCssClass("tn-bigicon-first");
                    break;
                case IconTypes.BigLast:
                    span.AddCssClass("tn-bigicon-last");
                    break;
                case IconTypes.BigWrite:
                    span.AddCssClass("tn-bigicon-write");
                    break;
                case IconTypes.BigUpdate:
                    span.AddCssClass("tn-bigicon-update");
                    break;
                case IconTypes.BigSet:
                    span.AddCssClass("tn-bigicon-set");
                    break;
                case IconTypes.BigConfig:
                    span.AddCssClass("tn-bigicon-config");
                    break;
                case IconTypes.BigAdd:
                    span.AddCssClass("tn-bigicon-add");
                    break;
                case IconTypes.BigCross:
                    span.AddCssClass("tn-bigicon-cross");
                    break;
                case IconTypes.BigRotateLeft:
                    span.AddCssClass("tn-bigicon-rotate-left");
                    break;
                case IconTypes.BigRotateRight:
                    span.AddCssClass("tn-bigicon-rotate-right");
                    break;
                case IconTypes.BigEnlarge:
                    span.AddCssClass("tn-bigicon-enlarge");
                    break;
                case IconTypes.BigEmail:
                    span.AddCssClass("tn-bigicon-email");
                    break;
                case IconTypes.BigTop:
                    span.AddCssClass("tn-bigicon-top");
                    break;
                case IconTypes.BigLock:
                    span.AddCssClass("tn-bigicon-lock");
                    break;
                case IconTypes.BigShare:
                    span.AddCssClass("tn-bigicon-share");
                    break;
                case IconTypes.BigBubble:
                    span.AddCssClass("tn-bigicon-bubble");
                    break;
                case IconTypes.BigFavorite:
                    span.AddCssClass("tn-bigicon-favorite");
                    break;
                case IconTypes.BigQuotesBefore:
                    span.AddCssClass("tn-bigicon-quotes-before");
                    break;
                case IconTypes.BigQuotesAfter:
                    span.AddCssClass("tn-bigicon-quotes-after");
                    break;
                case IconTypes.BigTopic:
                    span.AddCssClass("tn-bigicon-topic");
                    break;
                case IconTypes.BigUser:
                    span.AddCssClass("tn-bigicon-user");
                    break;
                case IconTypes.BigGroup:
                    span.AddCssClass("tn-bigicon-group");
                    break;
                case IconTypes.BigChain:
                    span.AddCssClass("tn-bigicon-chain");
                    break;
                case IconTypes.BigZoom:
                    span.AddCssClass("tn-bigicon-zoom");
                    break;
                case IconTypes.BigAlert:
                    span.AddCssClass("tn-bigicon-alert");
                    break;
                case IconTypes.BigExclamation:
                    span.AddCssClass("tn-bigicon-exclamation");
                    break;
                case IconTypes.BigAcceptCircle:
                    span.AddCssClass("tn-bigicon-accept-circle");
                    break;
                case IconTypes.BigCrossCircle:
                    span.AddCssClass("tn-bigicon-cross-circle");
                    break;
                case IconTypes.BigLogout:
                    span.AddCssClass("tn-bigicon-logout");
                    break;
                case IconTypes.BigHome:
                    span.AddCssClass("tn-bigicon-home");
                    break;
                case IconTypes.BigEmotion:
                    span.AddCssClass("tn-bigicon-emotion");
                    break;
                case IconTypes.BigFolder:
                    span.AddCssClass("tn-bigicon-folder");
                    break;
                case IconTypes.BigMoive:
                    span.AddCssClass("tn-bigicon-moive");
                    break;
                case IconTypes.BigPicture:
                    span.AddCssClass("tn-bigicon-picture");
                    break;
                case IconTypes.BigAlbum:
                    span.AddCssClass("tn-bigicon-album");
                    break;
                case IconTypes.BigSound:
                    span.AddCssClass("tn-bigicon-sound");
                    break;
                case IconTypes.BigBlog:
                    span.AddCssClass("tn-bigicon-blog");
                    break;
                case IconTypes.BigEvent:
                    span.AddCssClass("tn-bigicon-event");
                    break;
                case IconTypes.BigVote:
                    span.AddCssClass("tn-bigicon-vote");
                    break;
                case IconTypes.BigNews:
                    span.AddCssClass("tn-bigicon-news");
                    break;
                case IconTypes.BigJob:
                    span.AddCssClass("tn-bigicon-job");
                    break;
                case IconTypes.BigApp:
                    span.AddCssClass("tn-bigicon-app");
                    break;
                case IconTypes.BigAt:
                    span.AddCssClass("tn-bigicon-at");
                    break;
                case IconTypes.BigPlay:
                    span.AddCssClass("tn-bigicon-play");
                    break;
                case IconTypes.BigPause:
                    span.AddCssClass("tn-bigicon-pause");
                    break;
                case IconTypes.BigDiscovery:
                    span.AddCssClass("tn-bigicon-discovery");
                    break;
                case IconTypes.BigSkin:
                    span.AddCssClass("tn-bigicon-skin");
                    break;
                case IconTypes.BigAddPicture:
                    span.AddCssClass("tn-bigicon-add-picture");
                    break;
                case IconTypes.BigCamera:
                    span.AddCssClass("tn-bigicon-camera");
                    break;
                case IconTypes.BigMusic:
                    span.AddCssClass("tn-bigicon-music");
                    break;
                case IconTypes.BigAsk:
                    span.AddCssClass("tn-bigicon-ask");
                    break;
                case IconTypes.BigBar:
                    span.AddCssClass("tn-bigicon-bar");
                    break;
                case IconTypes.BigPointMall:
                    span.AddCssClass("tn-bigicon-pointmall");
                    break;

                //64*64_large
                case IconTypes.LargeLock:
                    span.AddCssClass("tn-largeicon-lock");
                    break;
                case IconTypes.LargeUser:
                    span.AddCssClass("tn-largeicon-user");
                    break;
                case IconTypes.LargeAlert:
                    span.AddCssClass("tn-largeicon-alert");
                    break;
                case IconTypes.LargeExclamation:
                    span.AddCssClass("tn-largeicon-exclamation");
                    break;
                case IconTypes.LargeCrossCircle:
                    span.AddCssClass("tn-largeicon-cross-circle");
                    break;
                case IconTypes.LargeAcceptCircle:
                    span.AddCssClass("tn-largeicon-accept-circle");
                    break;
                default:
                    break;
            }
            span.AddCssClass("tn-icon");

            if (!string.IsNullOrEmpty(title))
                span.MergeAttribute("title", title);
            if (htmlAttributes != null)
            {
                RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (attributes.Any(n => n.Key.ToLower() == "class"))
                    span.AddCssClass(attributes.Single(n => n.Key.ToLower() == "class").Value.ToString());
                span.MergeAttributes(attributes, false);
            }

            return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region 下拉菜单按钮

        /// <summary>
        /// 带下拉菜单的按钮
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="name">按钮的name属性</param>
        /// <param name="buttonText">按钮文字</param>
        /// <param name="menuItemType">菜单项类型</param>
        /// <param name="menuItems">菜单项列表</param>
        /// <param name="buttonUrl">按钮链接地址</param>        
        /// <param name="disabled">是否禁用按钮</param> 
        /// <param name="highlightStyle">按钮高亮显示类型</param>
        /// <param name="buttonHtmlAttributes">按钮html属性集合</param>
        /// <remarks>表单中只有一个表示名称的数据,返回的数据类型：{id：XX,name:XX}</remarks>
        /// <returns>按钮的html代码</returns>
        public static MvcHtmlString MenuButton(this HtmlHelper htmlHelper, string name, string buttonText,
                                                                MenuItemType menuItemType = MenuItemType.Link, /*菜单项类型*/
                                                                IEnumerable<MenuItem> menuItems = null, /*菜单项列表*/
                                                                string buttonUrl = null, /*按钮链接地址*/
                                                                bool disabled = false, /*是否禁用按钮*/
                                                                HighlightStyles highlightStyle = HighlightStyles.Default,
                                                                object buttonHtmlAttributes = null)
        {
            //构造菜单
            TagBuilder menuContainer = new TagBuilder("div");
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string containerName = fullName + "_Menu";
            menuContainer.GenerateId(containerName);
            menuContainer.MergeAttribute("name", containerName, true);
            menuContainer.MergeAttribute("style", "display:none");
            menuContainer.AddCssClass("tn-menu-button-position tn-menu tn-helper-reset tn-widget-content");

            if (menuItems == null)
            {
                menuItems = htmlHelper.GetMenuData(fullName);
            }
            TagBuilder ulBuilder = new TagBuilder("ul");
            ulBuilder.AddCssClass("tn-menu-list");
            foreach (var item in menuItems)
            {
                ulBuilder.InnerHtml += ConvertMenuItemToLi(item, htmlHelper, menuItemType, fullName);
            }
            menuContainer.InnerHtml += ulBuilder.ToString();
            //if (!string.IsNullOrEmpty(newItemPostUrl))
            //    menuContainer.InnerHtml += @"<div class=""tn-menu-add tn-border-light tn-border-top"">"
            //    + @"<a href=""javascript:;"" class=""editUserTag"">新建</a></div>";


            //生成按钮，并附加menuButton插件所需参数
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(buttonHtmlAttributes);
            if (attributes == null)
                attributes = new RouteValueDictionary();
            string ulID = menuContainer.Attributes["id"];
            attributes["menu"] = "#" + ulID;
            attributes["name"] = fullName + "_Button";

            string datamenuPreix = "data_menu_";

            if (menuItemType != MenuItemType.Link)
                attributes[datamenuPreix + "clickTrigger"] = true.ToString().ToLower();
            if (disabled)
                attributes[datamenuPreix + "disabled"] = true.ToString().ToLower();

            return MvcHtmlString.Create(Button(htmlHelper, buttonText, ButtonTypes.Button, highlightStyle, ButtonSizes.Default, IconTypes.SmallTriangleDown, TextIconLayout.TextIcon, buttonUrl, attributes).ToString()
            + menuContainer.ToString());
        }

        /// <summary>
        /// 将菜单项实体转为Li元素
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="menuItemType">菜单项类型</param>
        /// <param name="fullName">菜单项Id</param>
        /// <returns>Li元素的html代码</returns>
        private static string ConvertMenuItemToLi(MenuItem item, HtmlHelper htmlHelper, MenuItemType menuItemType, string fullName)
        {
            TagBuilder liBuilder = new TagBuilder("li");
            liBuilder.AddCssClass("tn-menu-item");
            if (item.IconType != null)
                liBuilder.InnerHtml += Icon(htmlHelper, item.IconType.Value);
            TagBuilder itemBuilder;
            if (menuItemType == MenuItemType.Link)
            {
                itemBuilder = new TagBuilder("a");
                if (string.IsNullOrEmpty(item.Url))
                    item.Url = "javascript:;";
                itemBuilder.MergeAttribute("href", item.Url);
                itemBuilder.AddCssClass("tn-menu-text");
            }
            else if (menuItemType == MenuItemType.CheckBox)
            {
                itemBuilder = new TagBuilder("input");
                itemBuilder.MergeAttribute("type", "checkbox");
                itemBuilder.AddCssClass("tn-checkbox");
            }
            else
            {
                itemBuilder = new TagBuilder("input");
                itemBuilder.MergeAttribute("type", "radio");
                itemBuilder.AddCssClass("tn-radiobutton");
            }

            if (item.Checked)
                itemBuilder.MergeAttribute("checked", "checked");
            string itemId = string.Empty;
            if (!string.IsNullOrEmpty(item.Value))
            {
                itemBuilder.MergeAttribute("value", item.Value);
                itemId = fullName + item.Value;
                itemBuilder.MergeAttribute("id", itemId);
                itemBuilder.MergeAttribute("name", fullName);
            }

            if (!string.IsNullOrEmpty(item.Text))
            {
                if (menuItemType == MenuItemType.Link)
                {
                    itemBuilder.InnerHtml = item.Text;
                    liBuilder.InnerHtml += itemBuilder.ToString();
                }
                else
                {
                    TagBuilder label = new TagBuilder("label");
                    label.MergeAttribute("for", itemId);
                    label.InnerHtml = item.Text;
                    liBuilder.InnerHtml += itemBuilder.ToString() + label.ToString();
                }
            }

            return liBuilder.ToString();
        }

        /// <summary>
        /// 从ViewData中获取菜单列表数据
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="name">菜单项Id</param>
        private static IEnumerable<MenuItem> GetMenuData(this HtmlHelper htmlHelper, string name)
        {
            object o = null;
            if (htmlHelper.ViewData != null && htmlHelper.ViewData.ContainsKey(name))
            {
                o = htmlHelper.ViewData[name];
            }
            if (o == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "ViewData中没有对{0}赋值",
                        name,
                        "IEnumerable<MenuItem>"));
            }
            IEnumerable<MenuItem> menuItems = o as IEnumerable<MenuItem>;
            if (menuItems == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "ViewData中没有对{0}赋值",
                        name,
                        o.GetType().FullName,
                        "IEnumerable<MenuItem>"));
            }
            return menuItems;
        }

        #endregion


        #region 按钮

        /// <summary>
        /// 链接按钮
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="text">链接文字</param>
        /// <param name="url">链接地址</param>
        /// <param name="highlightStyle">高亮显示类型</param>
        /// <param name="size">按钮尺寸</param>
        /// <param name="iconType">包含图标的类型</param>
        /// <param name="textIconLayout">文字图标排列顺序</param>
        /// <param name="htmlAttributes">按钮的html属性</param>  
        /// <returns>链接的html代码</returns>
        public static MvcHtmlString LinkButton(this HtmlHelper htmlHelper, string text, string url,
                                                                          HighlightStyles highlightStyle = HighlightStyles.Default,
                                                                          ButtonSizes size = ButtonSizes.Default,
                                                                          IconTypes? iconType = null,
                                                                          TextIconLayout textIconLayout = TextIconLayout.IconText,
                                                                          object htmlAttributes = null
                                                                          )
        {
            return Button(htmlHelper, text, ButtonTypes.Link, highlightStyle, size, iconType, textIconLayout, url, htmlAttributes);
        }

        /// <summary>
        /// 输出按钮
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="text">文本内容</param>
        /// <param name="url">点击跳转向的地址</param>
        /// <param name="iconType">图标类型</param>
        /// <param name="textIconLayout">图标和文本排列顺序</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="size">按钮大小</param>
        /// <param name="highlightStyle">高亮显示类型</param>
        /// <param name="htmlAttributes">按钮Html属性集合</param>
        /// <returns>按钮的html代码</returns>
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string text, ButtonTypes buttonType,
                                                                    HighlightStyles highlightStyle = HighlightStyles.Default,
                                                                    ButtonSizes size = ButtonSizes.Default,
                                                                    IconTypes? iconType = null,
                                                                    TextIconLayout textIconLayout = TextIconLayout.IconText,
                                                                    string url = null,
                                                                   object htmlAttributes = null
                                                                  )
        {
            return Button(htmlHelper, text, buttonType, highlightStyle, size, iconType, textIconLayout, url, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// 输出按钮
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="text">文本内容</param>
        /// <param name="url">点击跳转向的地址</param>
        /// <param name="iconType">图标类型</param>
        /// <param name="textIconLayout">图标和文本排列顺序</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="size">按钮大小</param>
        /// <param name="highlightStyle">高亮显示类型</param>
        /// <param name="htmlAttributes">按钮Html属性集合</param>
        /// <returns>按钮的html代码</returns>
        private static MvcHtmlString Button(this HtmlHelper htmlHelper, string text, ButtonTypes buttonType,
                                                                    HighlightStyles highlightStyle = HighlightStyles.Default,
                                                                    ButtonSizes size = ButtonSizes.Default,
                                                                    IconTypes? iconType = null,
                                                                    TextIconLayout textIconLayout = TextIconLayout.IconText,
                                                                    string url = null,
                                                                   RouteValueDictionary htmlAttributes = null
                                                                  )
        {
            TagBuilder builder = null;
            if (buttonType == ButtonTypes.Link)
            {
                builder = new TagBuilder("a");

                if (!string.IsNullOrEmpty(url))
                    builder.MergeAttribute("href", WebUtility.ResolveUrl(url));
                if (htmlAttributes != null)
                {
                    if (htmlAttributes.Any(n => n.Key.ToLower() == "href"))
                        builder.MergeAttribute("href", htmlAttributes.Single(n => n.Key.ToLower() == "href").Value.ToString());
                }
                builder.MergeAttribute("href", "javascript:;", false);
            }
            else
            {
                builder = new TagBuilder("Button");
                if (!string.IsNullOrEmpty(url))
                    builder.MergeAttribute("url", url);

                switch (buttonType)
                {
                    case ButtonTypes.Submit:
                        builder.MergeAttribute("type", "submit");
                        break;
                    case ButtonTypes.Cancel:
                        builder.MergeAttribute("type", "button");
                        break;
                    case ButtonTypes.Reset:
                        builder.MergeAttribute("type", "reset");
                        break;
                    case ButtonTypes.Button:
                    default:
                        builder.MergeAttribute("type", "button");
                        break;
                }
            }
            builder.AddCssClass("tn-button tn-corner-all");

            //设置按钮内容
            if (iconType == null && string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("text 或 iconType至少有一个不为 null");
            }
            //设置图标
            if (iconType != null)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    if (textIconLayout == TextIconLayout.IconText)
                        builder.AddCssClass("tn-button-text-icon-primary");
                    else
                        builder.AddCssClass("tn-button-text-icon-secondary");
                }
                else
                {
                    builder.AddCssClass("tn-button-icon-only");
                    text = "&nbsp;";
                }
                builder.InnerHtml = Icon(htmlHelper, iconType.Value).ToString();
            }
            else
                builder.AddCssClass("tn-button-text-only");

            string buttonText = "<span class=\"tn-button-text\">" + text + "</span>";
            if (textIconLayout == TextIconLayout.IconText)
                builder.InnerHtml += buttonText;
            else
                builder.InnerHtml = buttonText + builder.InnerHtml;

            //设置按钮大小
            if (size == ButtonSizes.Large)
                builder.AddCssClass("tn-button-large");

            //设置按钮高亮类型
            switch (highlightStyle)
            {
                case HighlightStyles.Primary:
                    builder.AddCssClass("tn-button-primary");
                    break;
                case HighlightStyles.Secondary:
                    builder.AddCssClass("tn-button-secondary");
                    break;
                case HighlightStyles.Lite:
                    builder.AddCssClass("tn-button-lite");
                    break;
                case HighlightStyles.Hollow:
                    builder.AddCssClass("tn-button-hollow");
                    break;
                case HighlightStyles.Default:
                default:
                    builder.AddCssClass("tn-button-default");
                    break;
            }

            if (htmlAttributes != null)
            {
                if (htmlAttributes.Any(n => n.Key.ToLower() == "class"))
                    builder.AddCssClass(htmlAttributes.Single(n => n.Key.ToLower() == "class").Value.ToString());
            }
            builder.MergeAttributes(htmlAttributes);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region 录入框
        /// <summary>
        /// 录入框
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="name">录入框的name属性</param>
        /// <param name="value">录入框的value属性</param>
        /// <param name="widthType">宽度类型</param>
        /// <param name="htmlAttributes">html属性集合</param>
        /// <returns>录入框的html代码</returns>
        public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, string name, string value = null, InputWidthTypes? widthType = null, RouteValueDictionary htmlAttributes = null)
        {
            if (htmlAttributes == null)
                htmlAttributes = new RouteValueDictionary();
            htmlAttributes.AddCssClass("tn-textbox tn-border-gray");
            htmlAttributes.AddCssClass(ResolveWidth(widthType));
            return InputExtensions.TextBox(htmlHelper, name, value, htmlAttributes);
        }

        /// <summary>
        /// 模型中的录入框
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <typeparam name="TModel">模型的类型</typeparam>
        /// <typeparam name="TProperty">模型中对应输出的属性</typeparam>
        /// <param name="expression">获取模型中的对应的属性</param>
        /// <param name="widthType">录入框的宽度类型</param>
        /// <param name="htmlAttributes">html属性集合</param>
        /// <returns>录入框的html代码</returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, InputWidthTypes? widthType = null, RouteValueDictionary htmlAttributes = null)
        {
            if (htmlAttributes == null)
                htmlAttributes = new RouteValueDictionary();

            GetWaterMark<TModel, TProperty>(expression, htmlAttributes);

            htmlAttributes.AddCssClass("tn-textbox tn-border-gray");
            htmlAttributes.AddCssClass(ResolveWidth(widthType));
            return InputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 输出文本域
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="name">文本域的name属性</param>
        /// <param name="value">文本域的value属性</param>
        /// <param name="widthType">宽度类型</param>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="htmlAttributes">html属性集合</param>
        /// <returns>文本域的html代码</returns>
        public static MvcHtmlString TextArea(this HtmlHelper htmlHelper, string name, string value = null, InputWidthTypes? widthType = null, int rows = 0, int columns = 0, RouteValueDictionary htmlAttributes = null)
        {
            if (htmlAttributes == null)
                htmlAttributes = new RouteValueDictionary();

            htmlAttributes.AddCssClass("tn-textarea tn-border-gray");
            htmlAttributes.AddCssClass(ResolveWidth(widthType));

            //多行纯文本反向处理
            value = value.Replace(WebUtility.HtmlNewLine, "\n");
            value = value.Replace("&nbsp;", " ");

            return TextAreaExtensions.TextArea(htmlHelper, name, value, rows, columns, htmlAttributes);
        }

        /// <summary>
        /// 在模型中输出文本域
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="expression">获取模型中的对应的属性</param>
        /// <param name="widthType">宽度类型</param>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="htmlAttributes">html属性集合</param>
        /// <returns>文本域的html代码</returns>
        public static MvcHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, InputWidthTypes? widthType = null, int rows = 0, int columns = 0, RouteValueDictionary htmlAttributes = null)
        {
            if (htmlAttributes == null)
                htmlAttributes = new RouteValueDictionary();

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                if (memberExpression.Member is PropertyInfo)
                {
                    Type model = typeof(TModel);
                    string propertyName = memberExpression.Member.Name;
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        PropertyInfo property = model.GetProperty(propertyName);

                        if (property != null)
                        {
                            Attribute attr = (Attribute)property.GetCustomAttributes(false).FirstOrDefault(a => a is WaterMarkAttribute);
                            object val = attr != null ? attr.GetType().GetProperty("Content").GetValue(attr, null) : null;

                            if (val != null)
                            {
                                htmlAttributes.Add("watermark", val.ToString());
                            }

                            //字符串处理
                            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
                            if (modelMetadata.Model != null)
                            {
                                string value = modelMetadata.Model.ToString();

                                //多行纯文本反向处理
                                value = value.Replace(WebUtility.HtmlNewLine, "\n");
                                value = value.Replace("&nbsp;", " ");

                                property.SetValue(htmlHelper.ViewData.Model, value, null);
                            }
                        }
                    }
                }
            }

            htmlAttributes.AddCssClass("tn-textarea tn-border-gray");
            htmlAttributes.AddCssClass(ResolveWidth(widthType));

            return TextAreaExtensions.TextAreaFor(htmlHelper, expression, rows, columns, htmlAttributes);
        }

        /// <summary>
        /// 根据宽度类型解析出相应的cssClass值
        /// </summary>
        /// <param name="widthType">宽度类型</param>
        /// <returns>cssClass值</returns>
        private static string ResolveWidth(InputWidthTypes? widthType)
        {
            string cssClass = string.Empty;
            if (widthType != null)
                switch (widthType.Value)
                {
                    case InputWidthTypes.Short:
                        cssClass += " tn-input-short";
                        break;
                    case InputWidthTypes.Medium:
                        cssClass += " tn-input-medium";
                        break;
                    case InputWidthTypes.Long:
                        cssClass += " tn-input-long";
                        break;
                    case InputWidthTypes.Longest:
                        cssClass += " tn-input-longest";
                        break;
                    default:
                        cssClass += " tn-input-medium";
                        break;
                }
            return cssClass;
        }

        #endregion

        #region Label


        /// <summary>
        /// 输出Label标签
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            if (htmlAttributes == null)
                htmlAttributes = new RouteValueDictionary();
            htmlAttributes["class"] = "tn-form-label";
            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText + ResourceAccessor.GetString("Common_Colon"));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region 下拉列表

        /// <summary>
        /// 联动下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="expression">获取数据集合</param>
        /// <param name="level">显示多少级</param>
        /// <param name="defaultValue">TProperty类型的默认值（如string默认值为"")</param>
        /// <param name="rootItems">获取根级列表数据</param>
        /// <param name="getParentID">获取列表项的ParentID方法</param>
        /// <param name="getChildItems">获取子级列表数据集合方法</param>
        /// <param name="getChildSelectDataUrl">获取子级列表数据的远程地址</param>
        /// <returns>html代码</returns>
        public static MvcHtmlString LinkageDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
                                                                                                                  TProperty defaultValue,
                                                                                                                  int level,
                                                                                                                  Dictionary<TProperty, string> rootItems,
                                                                                                                  Func<TProperty, TProperty> getParentID,
                                                                                                                  Func<TProperty, Dictionary<TProperty, string>> getChildItems,
                                                                                                                  string getChildSelectDataUrl,
                                                                                                                  string optionLabel = "请选择")
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            return LinkageDropDownList(htmlHelper,
                                 ExpressionHelper.GetExpressionText(expression),
                                (TProperty)metadata.Model,
                                defaultValue,
                                 level,
                                rootItems,
                                getParentID,
                                getChildItems,
                                getChildSelectDataUrl,
                                optionLabel);
        }


        /// <summary>
        /// 联动下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="level">显示多少级</param>
        /// <param name="defaultValue">TProperty类型的默认值（如string默认值为"")</param>
        /// <param name="name">下拉列表表单项名</param>
        /// <param name="selectedValue">当前选中值</param>
        /// <param name="rootItems">获取根级列表数据</param>
        /// <param name="getParentId">获取列表项的ParentID方法</param>
        /// <param name="getChildItems">获取子级列表数据集合方法</param>
        /// <param name="getChildSelectDataUrl">获取子级列表数据的远程地址</param>
        /// <returns>html代码</returns>
        public static MvcHtmlString LinkageDropDownList<TProperty>(this HtmlHelper htmlHelper, string name,
                                                                                               TProperty selectedValue,
                                                                                                TProperty defaultValue,
                                                                                                int level,
                                                                                               Dictionary<TProperty, string> rootItems,
                                                                                               Func<TProperty, TProperty> getParentId,
                                                                                               Func<TProperty, Dictionary<TProperty, string>> getChildItems,
                                                                                               string getChildSelectDataUrl,
                                                                                               string optionLabel = "请选择")
        {            
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            //select data init
            Stack<Dictionary<TProperty, string>> stack = new Stack<Dictionary<TProperty, string>>();

            //如果有选中的值，则查找其所在列表前面的所有列表
            IList<TProperty> selectedValues = new List<TProperty>();
            if (selectedValue != null && !selectedValue.Equals(defaultValue))
            {
                TProperty itemId = selectedValue;
                TProperty parentItemId = getParentId(itemId);
                while (!itemId.Equals(defaultValue) && !parentItemId.Equals(defaultValue))
                {
                    stack.Push(getChildItems(parentItemId));
                    selectedValues.Add(itemId);
                    itemId = parentItemId;
                    parentItemId = getParentId(itemId);
                }
                if (rootItems.Count() > 0)
                {
                    TProperty rootId = getParentId(rootItems.First().Key);
                    if (!itemId.Equals(rootId))
                    {
                        stack.Push(rootItems);
                        selectedValues.Add(itemId);
                    }
                }
            }
            else
            {
                TProperty rootItemID = rootItems.Select(n => n.Key).FirstOrDefault();
                stack.Push(rootItems);
            }

            //生成标签
            TagBuilder containerBuilder = new TagBuilder("span");
            containerBuilder.MergeAttribute("plugin", "linkageDropDownList");
            var data = new Dictionary<string, object>();
            data.TryAdd("GetChildSelectDataUrl", getChildSelectDataUrl);
            data.TryAdd("ControlName", name);
            data.TryAdd("Level", level);
            data.TryAdd("OptionLabel", optionLabel);
            data.TryAdd("DefaultValue", defaultValue.ToString());
            containerBuilder.MergeAttribute("data", Json.Encode(data));
            int currentIndex = 0;
            while (stack.Count > 0)
            {
                Dictionary<TProperty, string> dictionary = stack.Pop();
                IEnumerable<SelectListItem> selectList = dictionary.Select(n => new SelectListItem() { Selected = selectedValues.Contains(n.Key), Text = n.Value, Value = n.Key.ToString() });
                containerBuilder.InnerHtml += "\r\n" + htmlHelper.DropDownList(string.Format("{0}_{1}", name, currentIndex), selectList,
                                optionLabel, new { @class = "tn-dropdownlist" });
                currentIndex++;
            }
            containerBuilder.InnerHtml += "\r\n" + htmlHelper.Hidden(name);
            return MvcHtmlString.Create(containerBuilder.ToString());
        }

        #endregion

        #region 复选框

        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="htmlhelper">被扩展对象</param>
        /// <param name="name">名称</param>
        /// <param name="value">复选框对应的值</param>
        /// <param name="isChecked">是否选中</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString SipmleCheckBox(this HtmlHelper htmlhelper, string name, object value, bool isChecked = false, object htmlAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("input");

            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name);

            if (value != null)
                tagBuilder.MergeAttribute("value", value.ToString());

            if (isChecked)
                tagBuilder.MergeAttribute("checked", "checked");

            if (htmlAttributes != null)
            {
                RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tagBuilder.MergeAttributes(attributes, false);
            }

            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        #endregion

        #region PageName&面包屑

        /// <summary>
        /// 设置PageName
        /// </summary>
        public static MvcHtmlString PageName(this HtmlHelper htmlhelper)
        {
            string pageName = htmlhelper.ViewContext.HttpContext.Items["Item_PageName"] as string;
            return GeneratePageName(pageName);

        }


        /// <summary>
        /// 设置PageName
        /// </summary>
        public static MvcHtmlString BreadCrumb(this HtmlHelper htmlhelper)
        {
            Queue<TagBuilder> crumbNodes = htmlhelper.ViewContext.HttpContext.Items["Item_BreadCrumbQueue"] as Queue<TagBuilder>;
            StringBuilder breadCrumbHtml = new StringBuilder();
            TagBuilder outDivBuilder = new TagBuilder("div");
            outDivBuilder.MergeAttribute("class", "tn-breadcrumb");

            if (crumbNodes != null && crumbNodes.Count > 0)
            {
                if (crumbNodes.Count == 1)
                {
                    return GeneratePageName(crumbNodes.Dequeue().InnerHtml);
                }

                bool? isAutoAddSiteHomeNode = htmlhelper.ViewContext.HttpContext.Items["IsAutoAddSiteHomeNode"] as bool?;
                isAutoAddSiteHomeNode = isAutoAddSiteHomeNode ?? false;

                string breadcrumbItemHtml = "<span class=\"tn-breadcrumb-item\">{0}</span>",
                       seperatorHtml = "<span class=\"tn-seperator\">&gt;</span>";

                if (isAutoAddSiteHomeNode.Value)
                {
                    breadCrumbHtml.AppendFormat(breadcrumbItemHtml, string.Format("<a href=\"{0}\"><span>{1}</span>", htmlhelper.ViewContext.HttpContext.Request.Url.Host, "首页"));
                }
                else
                {
                    TagBuilder a = crumbNodes.Dequeue();
                    breadCrumbHtml.AppendFormat(breadcrumbItemHtml, string.IsNullOrEmpty(a.Attributes["href"]) ? a.InnerHtml : a.ToString());
                }

                while (crumbNodes.Count > 0)
                {
                    breadCrumbHtml.Append(seperatorHtml);

                    TagBuilder a = crumbNodes.Dequeue();
                    breadCrumbHtml.AppendFormat(breadcrumbItemHtml, string.IsNullOrEmpty(a.Attributes["href"]) ? a.InnerHtml : a.ToString());
                }

            }

            outDivBuilder.InnerHtml = breadCrumbHtml.ToString();
            return new MvcHtmlString(outDivBuilder.ToString());
        }

        /// <summary>
        /// 生成PageName
        /// </summary>
        /// <param name="pageName">PageName的内容</param>
        /// <returns></returns>
        private static MvcHtmlString GeneratePageName(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                TagBuilder tagBuilder = new TagBuilder("h2");
                tagBuilder.MergeAttribute("class", "tn-pagename");
                tagBuilder.InnerHtml = pageName;
                return MvcHtmlString.Create(tagBuilder.ToString());
            }

            return new MvcHtmlString(string.Empty);
        }

        #endregion

        /// <summary>
        /// 为RouteValueDictionary扩展添加class方法
        /// </summary>
        /// <param name="htmlAttributes">html属性集合</param>
        /// <param name="cssClass">样式名</param>
        /// <returns>RouteValueDictionary</returns>
        private static RouteValueDictionary AddCssClass(this RouteValueDictionary htmlAttributes, string cssClass)
        {
            if (htmlAttributes == null)
                htmlAttributes = new RouteValueDictionary();
            if (htmlAttributes.Any(n => n.Key.ToLower() == "class"))
                htmlAttributes["class"] += " " + cssClass;
            else
                htmlAttributes["class"] = cssClass;

            return htmlAttributes;
        }

        /// <summary>
        /// 获取水印文字并为标签添加水印标注
        /// </summary>
        /// <typeparam name="TModel">模型的类型</typeparam>
        /// <typeparam name="TProperty">模型中对应输出的属性</typeparam>
        /// <param name="expression">获取模型中的对应的属性</param>
        /// <param name="htmlAttributes">html属性集合</param>
        private static void GetWaterMark<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, RouteValueDictionary htmlAttributes)
        {
            //获取水印
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                if (memberExpression.Member is PropertyInfo)
                {
                    Type model = typeof(TModel);
                    string propertyName = memberExpression.Member.Name;

                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        PropertyInfo property = model.GetProperties().FirstOrDefault(p => p.Name == propertyName);

                        if (property != null)
                        {
                            Attribute attr = (Attribute)property.GetCustomAttributes(false).FirstOrDefault(a => a is WaterMarkAttribute);
                            object val = attr != null ? attr.GetType().GetProperty("Content").GetValue(attr, null) : null;

                            if (val != null)
                            {
                                htmlAttributes.Add("watermark", val.ToString());
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 文本和图标的布局方式
    /// </summary>
    public enum TextIconLayout
    {
        /// <summary>
        /// 图标在前，文本在后
        /// </summary>
        IconText,
        /// <summary>
        /// 文本在前，图标在后
        /// </summary>
        TextIcon
    }

    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum ButtonTypes
    {
        /// <summary>
        /// 输入按钮
        /// </summary>
        Button,
        /// <summary>
        /// 重置按钮
        /// </summary>
        Reset,
        /// <summary>
        /// 链接按钮
        /// </summary>
        Link,
        /// <summary>
        /// 提交按钮
        /// </summary>
        Submit,
        /// <summary>
        /// 取消按钮
        /// </summary>
        Cancel
    }

    /// <summary>
    /// 图标类型
    /// </summary>
    public enum IconTypes
    {
        //16*16
        /// <summary>
        /// 上箭头
        /// </summary>
        TriangleUp,
        /// <summary>
        /// 右箭头
        /// </summary>
        TriangleRight,
        /// <summary>
        /// 下
        /// </summary>
        TriangleDown,
        /// <summary>
        /// 左
        /// </summary>
        TriangleLeft,
        /// <summary>
        /// 展开
        /// </summary>
        CollapseOpen,
        /// <summary>
        /// 收起
        /// </summary>
        CollapseClose,
        /// <summary>
        /// 下载
        /// </summary>
        Download,
        /// <summary>
        /// 上传
        /// </summary>
        Upload,
        /// <summary>
        /// 向下、展开
        /// </summary>
        Expand,
        /// <summary>
        /// 向上、收起
        /// </summary>
        Fold,
        /// <summary>
        /// 向后
        /// </summary>
        SlideNext,
        /// <summary>
        /// 向前
        /// </summary>
        SlidePrev,
        /// <summary>
        /// 最先的
        /// </summary>
        First,
        /// <summary>
        /// 最后的
        /// </summary>
        Last,
        /// <summary>
        /// 添加、新建
        /// </summary>
        Add,
        /// <summary>
        /// 编辑、描述
        /// </summary>
        Write,
        /// <summary>
        /// 更新、重建
        /// </summary>
        Update,
        /// <summary>
        /// 设置
        /// </summary>
        Set,
        /// <summary>
        /// 配置
        /// </summary>
        Config,
        /// <summary>
        /// 删除、关闭
        /// </summary>
        Cross,
        /// <summary>
        /// 通过、允许、同意
        /// </summary>
        Accept,
        /// <summary>
        /// 左旋转
        /// </summary>
        RotateLeft,
        /// <summary>
        /// 右旋转
        /// </summary>
        RotateRight,
        /// <summary>
        /// 扩大
        /// </summary>
        Enlarge,
        /// <summary>
        /// 学校
        /// </summary>
        School,
        /// <summary>
        /// 相互关注
        /// </summary>
        Friendly,
        /// <summary>
        /// 悄悄关注
        /// </summary>
        Sneak,
        /// <summary>
        /// 转发
        /// </summary>
        Forwardc,
        /// <summary>
        /// 不通过、否定、拒绝、阻止
        /// </summary>
        Stop,
        ///// <summary>
        ///// 设置
        ///// </summary>
        //Set,
        ///// <summary>
        ///// 配置
        ///// </summary>
        //Config,
        /// <summary>
        /// 精华
        /// </summary>
        Elite,
        /// <summary>
        /// 消息，未读
        /// </summary>
        Email,
        /// <summary>
        /// 消息，已读
        /// </summary>
        EmailOpen,
        /// <summary>
        /// 置顶
        /// </summary>
        Top,
        /// <summary>
        /// 推荐
        /// </summary>
        Flag,
        /// <summary>
        /// 锁定、私有
        /// </summary>
        Lock,
        /// <summary>
        /// 密钥访问
        /// </summary>
        Key,
        /// <summary>
        /// 权限访问
        /// </summary>
        Limit,
        /// <summary>
        /// 积分、钱币
        /// </summary>
        Coins,
        /// <summary>
        /// 热的、急的
        /// </summary>
        Fire,
        /// <summary>
        /// 移动
        /// </summary>
        Move,
        /// <summary>
        /// 个人认证
        /// </summary>
        Approve,
        /// <summary>
        /// 浏览、查看
        /// </summary>
        View,
        /// <summary>
        /// 支持、顶
        /// </summary>
        ThumbUp,
        /// <summary>
        /// 反对、踩
        /// </summary>
        ThumbDown,
        /// <summary>
        /// 分享
        /// </summary>
        Share,
        /// <summary>
        /// 评论、留言
        /// </summary>
        Bubble,
        /// <summary>
        /// 收藏、喜爱 
        /// </summary>
        Favorite,
        /// <summary>
        /// 加星
        /// </summary>
        Star,
        /// <summary>
        /// 订阅
        /// </summary>
        Feed,
        /// <summary>
        /// 双引号、前
        /// </summary>
        QuotesBefore,
        /// <summary>
        /// 双引号、后
        /// </summary>
        QuotesAfter,
        /// <summary>
        /// 主题、话题
        /// </summary>
        Topic,
        /// <summary>
        /// 标签
        /// </summary>
        Label,
        /// <summary>
        /// 用户、好友、成员
        /// </summary>
        User,
        /// <summary>
        /// 添加好友
        /// </summary>
        UserAdd,
        /// <summary>
        /// 特许用户
        /// </summary>
        UserAllow,
        /// <summary>
        /// 阻止用户
        /// </summary>
        UserStop,
        /// <summary>
        /// 用户关系
        /// </summary>
        UserRelation,
        /// <summary>
        /// 用户头像
        /// </summary>
        UserAvatar,
        /// <summary>
        /// 邀请用户
        /// </summary>
        UserInvite, //
        /// <summary>
        /// 用户卡片
        /// </summary>
        UserCard,
        /// <summary>
        /// 群组
        /// </summary>
        Group,
        /// <summary>
        /// 链接
        /// </summary>
        Chain,
        /// <summary>
        /// 个性签名、钢笔
        /// </summary>
        Pen,
        /// <summary>
        /// 男
        /// </summary>
        Creator,
        /// <summary>
        /// 女
        /// </summary>
        Manager,
        /// <summary>
        /// 列表浏览
        /// </summary>
        BrowseList,
        /// <summary>
        /// 详情浏览
        /// </summary>
        BrowseDetail,
        /// <summary>
        /// 中图、中块浏览
        /// </summary>
        BrowseMedium,
        /// <summary>
        /// 小图、小块浏览
        /// </summary>
        BrowseSmall,
        /// <summary>
        /// 幻灯浏览
        /// </summary>
        BrowseSlide,
        /// <summary>
        /// 页面装扮，布局调整
        /// </summary>
        Dress,
        /// <summary>
        /// 跳转
        /// </summary>
        Jump,
        /// <summary>
        /// 放大
        /// </summary>
        Zomm,
        /// <summary>
        /// 照片
        /// </summary>
        Camera,
        /// <summary>
        /// 日期指示
        /// </summary>
        Calendar,
        /// <summary>
        /// 颜色
        /// </summary>
        Color,
        /// <summary>
        /// 创建人、高级管理员
        /// </summary>
        Admin,
        /// <summary>
        /// 管理员、副管理员
        /// </summary>
        Auadmin,
        /// <summary>
        /// 女
        /// </summary>
        Female,
        /// <summary>
        /// 男
        /// </summary>
        Male,
        /// <summary>
        /// //视频
        /// </summary>
        Movie,
        /// <summary>
        /// 声音
        /// </summary>
        Sound,
        /// <summary>
        /// 音乐
        /// </summary>
        Music,
        /// <summary>
        /// 皮肤
        /// </summary>
        Skin,
        /// <summary>
        /// 博客
        /// </summary>
        Blog,
        /// <summary>
        /// 档案
        /// </summary>
        Archive,
        /// <summary>
        /// 论坛、讨论组
        /// </summary>
        Forum,
        /// <summary>
        /// 投票
        /// </summary>
        Vote,
        /// <summary>
        /// 新闻
        /// </summary>
        News,
        /// <summary>
        /// 资讯
        /// </summary>
        Cms,
        /// <summary>
        /// 求职、招聘、职位、经理
        /// </summary>
        Job,
        /// <summary>
        /// 互联网
        /// </summary>
        World,
        /// <summary>
        /// 主页
        /// </summary>
        Home,
        /// <summary>
        /// 疑问
        /// </summary>
        Question,
        /// <summary>
        /// 通知、消息
        /// </summary>
        Notice,
        /// <summary>
        /// 电梯、直达
        /// </summary>
        Escalator,
        /// <summary>
        /// 警告
        /// </summary>
        Alert,
        /// <summary>
        /// 提示
        /// </summary>
        Exclamation,
        /// <summary>
        /// 错误
        /// </summary>
        CrossCircle,
        /// <summary>
        /// 正确
        /// </summary>
        AcceptCircle,
        /// <summary>
        /// 申请
        /// </summary>
        Apply,
        /// <summary>
        /// 注销
        /// </summary>
        Logout,
        /// <summary>
        /// 加入
        /// </summary>
        Join,
        /// <summary>
        /// 退出
        /// </summary>
        Quit,
        /// <summary>
        /// 表情
        /// </summary>
        Emotion,
        /// <summary>
        /// 附件
        /// </summary>
        PaperClip,
        /// <summary>
        /// 文件
        /// </summary>
        Folder,
        /// <summary>
        /// 相册
        /// </summary>
        Album,
        /// <summary>
        /// 活动
        /// </summary>
        Event,
        /// <summary>
        /// 搜索
        /// </summary>
        Find,
        /// <summary>
        /// 图片
        /// </summary>
        Picture,
        ///// <summary>
        ///// 文章
        ///// </summary>
        //Archive,
        ///// <summary>
        ///// 疑问
        ///// </summary>
        //Question,
        ///// <summary>
        ///// 货币
        ///// </summary>
        //Coins,

        ///// <summary>
        ///// 主题
        ///// </summary>
        //Topic,
        ///// <summary>
        ///// 热点主题
        ///// </summary>
        //Fire,
        ///// <summary>
        ///// 引用
        ///// </summary>
        //QuotesBefore,
        ///// <summary>
        ///// 颜色
        ///// </summary>
        //Color,
        /// <summary>
        /// 应用
        /// </summary>
        App,
        /// <summary>
        /// 招贴、交易、买卖、市场
        /// </summary>
        Market,
        /// <summary>
        /// 问答
        /// </summary>
        Answer,
        /// <summary>
        /// 微博
        /// </summary>
        Microblog,
        /// <summary>
        /// 关于、提及
        /// </summary>
        At,
        /// <summary>
        /// 播放
        /// </summary>
        Play,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 探索、发现
        /// </summary>
        Discovery,
        /// <summary>
        /// 礼品
        /// </summary>
        Gift,
        /// <summary>
        /// 功能(常用)
        /// </summary>
        Function,
        /// <summary>
        /// 地图(结构图)
        /// </summary>
        Chart,
        /// <summary>
        /// 时钟(最近访问)
        /// </summary>
        Clock,
        /// <summary>
        /// 排队(待处理)
        /// </summary>
        Pending,
        /// <summary>
        /// 数据表
        /// </summary>
        Datasheet,
        /// <summary>
        /// 系统信息
        /// </summary>
        System,
        /// <summary>
        /// 产品信息
        /// </summary>
        Product,
        /// <summary>
        /// 新增照片
        /// </summary>
        AddPicture,
        /// <summary>
        /// 封禁
        /// </summary>
        Banned,
        /// <summary>
        /// 管制
        /// </summary>
        Moderated,
        /// <summary>
        /// 问答
        /// </summary>
        Ask,
        /// <summary>
        /// 贴吧
        /// </summary>
        Bar,
        /// <summary>
        /// 积分商城
        /// </summary>
        PointMall,
        Wiki,
        //16*16_small
        /// <summary>
        /// small上箭头
        /// </summary>
        SmallTriangleUp,
        /// <summary>
        /// small右箭头
        /// </summary>
        SmallTriangleRight,
        /// <summary>
        /// small下箭头
        /// </summary>
        SmallTriangleDown,
        /// <summary>
        /// small左箭头
        /// </summary>
        SmallTriangleLeft,
        /// <summary>
        /// small展开
        /// </summary>
        SmallCollapseOpen,
        /// <summary>
        /// small收起
        /// </summary>
        SmallCollapseClose,
        /// <summary>
        /// small下载
        /// </summary>
        SmallDownload,
        ///<summary>
        /// small上传
        /// </summary>
        SmallUpload,
        ///<summary>
        /// small编辑、描述
        /// </summary>
        SmallWrite,
        /// <summary>
        /// small更新、重建
        /// </summary>
        SmallUpdate,
        /// <summary>
        /// small设置
        /// </summary>
        SmallSet,
        /// <summary>
        /// small配置
        /// </summary>
        SmallConfig,
        /// <summary>
        /// small添加、新建
        /// </summary>
        SmallAdd,
        /// <summary>
        /// small删除、关闭
        /// </summary>
        SmallCross,
        /// <summary>
        /// small通过、允许、同意
        /// </summary>
        SmallAccept,
        /// <summary>
        /// small不通过、否定、拒绝、阻止
        /// </summary>
        SmallStop,
        /// <summary>
        /// small向下、展开
        /// </summary>
        SmallExpand,
        /// <summary>
        /// small向上、收起
        /// </summary>
        SmallFold,
        /// <summary>
        /// small向后
        /// </summary>
        SmallSlideNext,
        /// <summary>
        /// small向前
        /// </summary>
        SmallSlidePrev,
        /// <summary>
        /// small最先的
        /// </summary>
        SmallSlideFirst,
        /// <summary>
        /// small最后的
        /// </summary>
        SmallSlideLast,
        /// <summary>
        /// small置顶
        /// </summary>
        SmallTop,
        /// <summary>
        /// small微博
        /// </summary>
        SmallMicroblog,
        /// <summary>
        /// small左旋转
        /// </summary>
        SmallRotateLeft,
        /// <summary>
        /// small右旋转
        /// </summary>
        SmallRotateRight,
        /// <summary>
        /// small扩大
        /// </summary>
        SmallEnlarge,
        /// <summary>
        /// small标签
        /// </summary>
        SmallLabel,
        /// <summary>
        /// small搜索、查找
        /// </summary>
        SmallFind,
        /// <summary>
        /// small警告
        /// </summary>
        SmallAlert,
        /// <summary>
        /// small精华
        /// </summary>
        SmallElite,
        /// <summary>
        /// small相互关注
        /// </summary>
        SmallFriendly,
        /// <summary>
        /// small悄悄关注
        /// </summary>
        SmallSneak,
        /// <summary>
        /// small女
        /// </summary>
        SmallFemale,
        /// <summary>
        /// small男
        /// </summary>
        SmallMale,
        /// <summary>
        /// smallAsk
        /// </summary>
        SmallAsk,
        /// <summary>
        /// smallBar
        /// </summary>
        SmallBar,
        /// <summary>
        /// pointMall
        /// </summary>
        SmallPointMall,
        /// <summary>
        /// smallGroup
        /// </summary>
        SmallGroup,
        //32*32_big
        /// <summary>
        /// big上箭头
        /// </summary>
        BigTriangleUp,
        ///<summary>
        /// big右箭头
        /// </summary>
        BigTriangleRight,
        /// <summary>
        /// big下箭头
        /// </summary>
        BigTriangleDown,
        /// <summary>
        /// big左箭头
        /// </summary>
        BigTriangleLeft,
        /// <summary>
        /// big展开
        /// </summary>
        BigCollapseOpen,
        /// <summary>
        /// big收起
        /// </summary>
        BigCollapseClose,
        /// <summary>
        /// big下载
        /// </summary>
        BigDownLoad,
        /// <summary>
        /// big上传
        /// </summary>
        BigUpLoad,
        /// <summary>
        /// big向下、展开
        /// </summary>
        BigExpand,
        /// <summary>
        /// big向上、收起
        /// </summary>
        BigFold,
        /// <summary>
        /// big向后
        /// </summary>
        BigSlideNext,
        /// <summary>
        /// big向前
        /// </summary>
        BigSlidePrev,
        /// <summary>
        /// big最先的
        /// </summary>
        BigFirst,
        /// <summary>
        /// big最后的
        /// </summary>
        BigLast,
        /// <summary>
        /// big编辑、描述
        /// </summary>
        BigWrite,
        /// <summary>
        /// big更新、重建
        /// </summary>
        BigUpdate,
        /// <summary>
        /// big设置
        /// </summary>
        BigSet,
        /// <summary>
        /// big配置
        /// </summary>
        BigConfig,
        /// <summary>
        /// big添加、新建
        /// </summary>
        BigAdd,
        /// <summary>
        /// big删除、关闭
        /// </summary>
        BigCross,
        /// <summary>
        /// big左旋转
        /// </summary>
        BigRotateLeft,
        /// <summary>
        /// big右旋转
        /// </summary>
        BigRotateRight,
        /// <summary>
        /// big扩大
        /// </summary>
        BigEnlarge,
        /// <summary>
        /// big消息、未读
        /// </summary>
        BigEmail,
        /// <summary>
        /// big置顶
        /// </summary>
        BigTop,
        /// <summary>
        /// big锁定、私有
        /// </summary>
        BigLock,
        /// <summary>
        /// big分享
        /// </summary>
        BigShare,
        /// <summary>
        /// big评论、留言
        /// </summary>
        BigBubble,
        /// <summary>
        /// big收藏、喜爱
        /// </summary>
        BigFavorite,
        /// <summary>
        /// big双引号、前
        /// </summary>
        BigQuotesBefore,
        /// <summary>
        /// big双引号、后
        /// </summary>
        BigQuotesAfter,
        /// <summary>
        /// big主题、话题
        /// </summary>
        BigTopic,
        /// <summary>
        /// big用户、好友、成员
        /// </summary>
        BigUser,
        /// <summary>
        /// big群组
        /// </summary>
        BigGroup,
        /// <summary>
        /// big链接
        /// </summary>
        BigChain,
        /// <summary>
        /// big放大
        /// </summary>
        BigZoom,
        /// <summary>
        /// big警告
        /// </summary>
        BigAlert,
        /// <summary>
        /// big提示
        /// </summary>
        BigExclamation,
        /// <summary>
        /// big错误
        /// </summary>
        BigCrossCircle,
        /// <summary>
        /// big正确
        /// </summary>
        BigAcceptCircle,
        /// <summary>
        /// big注销、退出
        /// </summary>
        BigLogout,
        /// <summary>
        /// big主页
        /// </summary>
        BigHome,
        /// <summary>
        /// big表情
        /// </summary>
        BigEmotion,
        /// <summary>
        /// big文件
        /// </summary>
        BigFolder,
        /// <summary>
        /// big视频
        /// </summary>
        BigMoive,
        /// <summary>
        /// big图片
        /// </summary>
        BigPicture,
        /// <summary>
        /// big相册
        /// </summary>
        BigAlbum,
        /// <summary>
        /// big声音
        /// </summary>
        BigSound,
        /// <summary>
        /// big博客
        /// </summary>
        BigBlog,
        /// <summary>
        /// big活动
        /// </summary>
        BigEvent,
        /// <summary>
        /// big投票
        /// </summary>
        BigVote,
        ///  <summary>
        /// big新闻、资讯
        /// </summary>
        BigNews,
        ///  <summary>
        /// big求职、招聘、职位、经理
        /// </summary>
        BigJob,
        ///  <summary>
        /// big应用程序
        /// </summary>
        BigApp,
        ///  <summary>
        /// big关于、提及
        /// </summary>
        BigAt,
        ///  <summary>
        /// big播放
        /// </summary>
        BigPlay,
        ///  <summary>
        /// big暂停
        /// </summary>
        BigPause,
        ///  <summary>
        /// big探索、发现
        /// </summary>
        BigDiscovery,
        ///  <summary>
        /// big皮肤
        /// </summary>
        BigSkin,
        ///  <summary>
        /// big添加照片
        /// </summary>
        BigAddPicture,
        ///  <summary>
        /// big照片
        /// </summary>
        BigCamera,
        ///  <summary>
        /// big音乐
        /// </summary>
        BigMusic,
        /// <summary>
        /// bigAsk
        /// </summary>
        BigAsk,
        /// <summary>
        /// BigBar
        /// </summary>
        BigBar,
        /// <summary>
        /// PointMall
        /// </summary>
        BigPointMall,
        //64*64_large
        /// <summary>
        /// large锁定
        /// </summary>
        LargeLock,
        /// <summary>
        /// large用户
        /// </summary>
        LargeUser,
        /// <summary>
        /// large警告
        /// </summary>
        LargeAlert,
        /// <summary>
        /// large提示
        /// </summary>
        LargeExclamation,
        /// <summary>
        /// large锁定
        /// </summary>
        LargeCrossCircle,
        /// <summary>
        /// large锁定
        /// </summary>
        LargeAcceptCircle
    }

    /// <summary>
    /// 按钮大小
    /// </summary>
    public enum ButtonSizes
    {
        /// <summary>
        /// 默认大小
        /// </summary>
        Default,
        /// <summary>
        /// 大按钮
        /// </summary>
        Large
    }

    /// <summary>
    /// 高亮显示类型
    /// </summary>
    public enum HighlightStyles
    {
        /// <summary>
        /// 默认高亮
        /// 淡蓝色
        /// </summary>
        Default,
        /// <summary>
        /// 主流高亮
        /// 深蓝色，主要用于提交等需要突出显示的按钮
        /// </summary>
        Primary,
        /// <summary>
        /// 非主流高亮
        /// 灰色，主要用于取消等不需要突出显示的按钮
        /// </summary>
        Secondary,
        /// <summary>
        /// 最弱的高亮效果
        /// 不显示背景色，只有鼠标移上去才会显示背景色
        /// </summary>
        Lite,
        /// <summary>
        /// 镂空效果(无底色）
        /// 默认只显示边框色，只有鼠标移上去才会显示背景色
        /// </summary>
        Hollow
    }

    /// <summary>
    /// 录入框宽度类型
    /// </summary>
    public enum InputWidthTypes
    {
        /// <summary>
        /// 短的
        /// </summary>
        Short,
        /// <summary>
        /// 中等的
        /// </summary>
        Medium,
        /// <summary>
        /// 长的
        /// </summary>
        Long,
        /// <summary>
        /// 最长的
        /// </summary>
        Longest
    }

}
