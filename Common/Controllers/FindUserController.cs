//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.Email;

using Tunynet.UI;
using Tunynet.Utilities;
using System.Text.RegularExpressions;
using Tunynet.Common.Configuration;
using Spacebuilder.Search;
using System.Collections;
using DevTrends.MvcDonutCaching;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 找人Controller
    /// </summary>
    [Themed(PresentAreaKeysOfBuiltIn.Channel, IsApplication = false)]
    [TitleFilter(IsAppendSiteName = true, TitlePart = "找人")]
    [AnonymousBrowseCheck]
    public class FindUserController : Controller
    {
        public IPageResourceManager pageResourceManager { get; set; }
        public FollowService followService { get; set; }
        public UserService userService { get; set; }
        public OnlineUserService onlineUserService { get; set; }
        public RecommendService recommendService { get; set; }
        private TagService tagService = new TagService(TenantTypeIds.Instance().Tag());

        private static readonly int pageSize = 6;

        /// <summary>
        /// 公共页头
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public ActionResult _Header(string menu = "Home")
        {
            ViewData["menu"] = menu;
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            pageResourceManager.InsertTitlePart("首页");
            ISettingsManager<UserSettings> userSettingsManager = DIContainer.Resolve<ISettingsManager<UserSettings>>();
            bool enableTrackAnonymous = userSettingsManager.Get().EnableTrackAnonymous;
            ViewData["enableTrackAnonymous"] = enableTrackAnonymous;
            return View();
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        //[DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult Ranking(SortBy_User? sortBy = SortBy_User.PreWeekHitTimes, int pageIndex = 1)
        {
            PagingDataSet<User> users = userService.GetPagingUsers(sortBy, pageIndex, 20);
            IUser CurrentUser = UserContext.CurrentUser;

            ViewData["sortBy"] = sortBy;

            //查询用户标签
            IEnumerable<long> itemInTagIds = tagService.GetItemInTagIdsByItemIds(users.Select(n => n.UserId));
            IEnumerable<ItemInTag> itemInTags = tagService.GetItemInTags(itemInTagIds);
            Dictionary<long, List<string>> userTagNameDic = new Dictionary<long, List<string>>();
            foreach (ItemInTag itemInTag in itemInTags)
            {
                if (userTagNameDic.ContainsKey(itemInTag.ItemId))
                {
                    List<string> tagNames = userTagNameDic[itemInTag.ItemId];
                    tagNames.Add(itemInTag.TagName);
                }
                else
                {
                    List<string> tagNames = new List<string>();
                    tagNames.Add(itemInTag.TagName);
                    userTagNameDic.Add(itemInTag.ItemId, tagNames);
                }

            }
            ViewData["userTagNameDic"] = userTagNameDic;

            pageResourceManager.InsertTitlePart("用户排行");

            return View(users);
        }

        /// <summary>
        /// 侧边栏快捷搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult _QuickSearch()
        {
            return View();
        }

        /// <summary>
        /// 用户TopN列表
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Stable")]
        public ActionResult _TopUsers(SortBy_User? sortBy, int topNumber = 20)
        {
            IEnumerable<IUser> users = userService.GetTopUsers(topNumber, sortBy ?? SortBy_User.PreWeekHitTimes);
            ViewData["sortBy"] = sortBy;
            return PartialView(users);
        }


        /// <summary>
        /// 在线用户的推荐用户
        /// </summary>
        /// <param name="topNumber">个数</param>
        /// <param name="recommendTypeId">推荐类型</param>
        /// <returns>推荐用户</returns>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _RecommendUser(int topNumber = 5, string recommendTypeId = "00001101")
        {
            IEnumerable<RecommendItem> recommendUsers = recommendService.GetTops(topNumber, recommendTypeId);
            userService.GetUsers(recommendUsers.Select(n => n.ItemId));
            return View(recommendUsers);
        }



        #region 可能感兴趣的人

        /// <summary>
        /// 可能感兴趣的人
        /// </summary>
        /// <returns></returns>
        public ActionResult Interested()
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return Redirect(SiteUrls.Instance().Login());
            }

            // 没有感兴趣的人时，推荐人气用户，需去除已关注用户和自己
            //根据点击数取热门用户
            IEnumerable<IUser> topUsers = userService.GetTopUsers(100, SortBy_User.HitTimes);

            //已关注用户
            IEnumerable<long> followedUserIds = followService.GetTopFollowedUserIds(CurrentUser.UserId, 1000);

            //黑名单用户
            IEnumerable<long> stopUserIds = new PrivacyService().GetStopedUsers(CurrentUser.UserId).Select(n => n.Key);

            //去除已关注用户和加黑名单用户
            IEnumerable<IUser> hotUsers = topUsers.Where(n => !followedUserIds.Contains(n.UserId) && n.UserId != CurrentUser.UserId && !stopUserIds.Contains(n.UserId)).Take(Math.Min(8, topUsers.Count()));
            ViewData["userName"] = CurrentUser.UserName;
            pageResourceManager.InsertTitlePart("可能感兴趣的人");

            return View(hotUsers);
        }

        /// <summary>
        /// 可能感兴趣的人-有共同关注的人
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <returns></returns>
        public ActionResult _InterestedWithFollows(int pageIndex = 1)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return null;
            }

            Dictionary<long, IEnumerable<long>> followedUserIdDic = null;
            Dictionary<long, User> followedUserDic = null;

            FollowUserSearcher searcher = (FollowUserSearcher)SearcherFactory.GetSearcher(FollowUserSearcher.CODE);
            PagingDataSet<User> users = searcher.SearchInterestedWithFollows(CurrentUser.UserId, pageIndex, pageSize, out followedUserIdDic, out followedUserDic);

            ViewData["followedUserIdDic"] = followedUserIdDic;
            ViewData["followedUserDic"] = followedUserDic;

            return View(users);
        }

        /// <summary>
        /// 使用了相同标签
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <returns></returns>
        public ActionResult _InterestedWithTags(int pageIndex = 1, string view = null)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return null;
            }

            Dictionary<long, IEnumerable<string>> tagNameDic = null;

            UserSearcher searcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> users = searcher.SearchInterestedWithTags(CurrentUser.UserId, pageIndex, pageSize, out tagNameDic);

            ViewData["tagNameDic"] = tagNameDic;

            if (string.IsNullOrEmpty(view))
            {
                return View(users);
            }
            else
            {
                return View(view, users);
            }
        }

        /// <summary>
        /// 供职于同一公司
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <returns></returns>
        public ActionResult _InterestedWithCompanys(int pageIndex = 1, string view = null)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return null;
            }

            Dictionary<long, IEnumerable<string>> companyNameDic = null;

            UserSearcher searcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> users = searcher.SearchInterestedWithCompanys(CurrentUser.UserId, pageIndex, pageSize, out companyNameDic);

            ViewData["companyNameDic"] = companyNameDic;

            List<User> userList = new List<User>();
            foreach (var user in users)
            {
                if (companyNameDic.ContainsKey(user.UserId) && companyNameDic[user.UserId].Count() > 0)
                {
                    userList.Add(user);
                }
            }
            int pageCount = users.PageCount;
            users = new PagingDataSet<User>(userList);
            ViewData["pageCount"] = pageCount;
            if (string.IsNullOrEmpty(view))
            {
                return View(users);
            }
            else
            {
                return View(view, users);
            }
        }

        /// <summary>
        /// 毕业于同一学校
        /// </summary>
        /// <param name="pageIndex">分页页码</param>
        /// <returns></returns>
        public ActionResult _InterestedWithSchools(int pageIndex = 1, string view = null)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return null;
            }

            Dictionary<long, IEnumerable<string>> schoolNameDic = null;

            UserSearcher searcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> users = searcher.SearchInterestedWithSchools(CurrentUser.UserId, pageIndex, pageSize, out schoolNameDic);

            ViewData["schoolNameDic"] = schoolNameDic;

            if (string.IsNullOrEmpty(view))
            {
                return View(users);
            }
            else
            {
                return View(view, users);
            }
        }

        /// <summary>
        /// 混合查找可能感兴趣的人-找人首页
        /// </summary>
        /// <param name="pageSize"></param>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _InterestedWithAll(int pageSize = 6, int pageIndex = 1, string view = null)
        {
            IUser CurrentUser = UserContext.CurrentUser;
            if (CurrentUser == null)
            {
                return null;
            }

            //查询同一公司的(优先级最高)
            Dictionary<long, IEnumerable<string>> companyNameDic = null;
            UserSearcher userSearcher = (UserSearcher)SearcherFactory.GetSearcher(UserSearcher.CODE);
            PagingDataSet<User> usersWithCompanys = userSearcher.SearchInterestedWithCompanys(CurrentUser.UserId, 1, 30, out companyNameDic);

            //存储感兴趣的人
            List<User> userList = usersWithCompanys.ToList();
            //存储感兴趣的类型
            Dictionary<long, string> interestedDic = usersWithCompanys.ToDictionary(key => key.UserId, value => "WithCompanys");

            //查询有共同关注用户的(优先级第二)
            Dictionary<long, IEnumerable<long>> followedUserIdDic = null;
            Dictionary<long, User> followedUserDic = null;
            FollowUserSearcher followUserSearcher = (FollowUserSearcher)SearcherFactory.GetSearcher(FollowUserSearcher.CODE);
            PagingDataSet<User> usersWithFollows = followUserSearcher.SearchInterestedWithFollows(CurrentUser.UserId, 1, 30, out followedUserIdDic, out followedUserDic);
            foreach (User user in usersWithFollows)
            {
                if (!interestedDic.ContainsKey(user.UserId))
                {
                    userList.Add(user);
                    interestedDic.Add(user.UserId, "WithFollows");
                }
            }

            //查询同一个学校的(优先级第三)
            Dictionary<long, IEnumerable<string>> schoolNameDic = null;
            PagingDataSet<User> usersWithSchools = userSearcher.SearchInterestedWithSchools(CurrentUser.UserId, 1, 30, out schoolNameDic);
            foreach (User user in usersWithSchools)
            {
                if (!interestedDic.ContainsKey(user.UserId))
                {
                    userList.Add(user);
                    interestedDic.Add(user.UserId, "WithSchools");
                }
            }

            //查询相同标签的(优先级最低)
            Dictionary<long, IEnumerable<string>> tagNameDic = null;
            PagingDataSet<User> usersWithTags = userSearcher.SearchInterestedWithTags(CurrentUser.UserId, 1, 30, out tagNameDic);
            foreach (User user in usersWithTags)
            {
                if (!interestedDic.ContainsKey(user.UserId))
                {
                    userList.Add(user);
                    interestedDic.Add(user.UserId, "WithTags");
                }
            }


            ViewData["followedUserIdDic"] = followedUserIdDic;
            ViewData["followedUserDic"] = followedUserDic;
            ViewData["companyNameDic"] = companyNameDic;
            ViewData["schoolNameDic"] = schoolNameDic;
            ViewData["tagNameDic"] = tagNameDic;
            ViewData["interestedDic"] = interestedDic;

            //不指定View，则返回固定的页面，随机显示
            if (string.IsNullOrEmpty(view))
            {
                //随机从用户列表中取pageSize对应的对象组装成新的集合
                List<User> randomUserList = null;
                if (userList.Count <= pageSize)
                {
                    randomUserList = userList;
                }
                else
                {
                    Dictionary<int, User> randomUserDic = new Dictionary<int, User>();
                    Random random = new Random();

                    while (randomUserDic.Count < pageSize)
                    {
                        int index = random.Next(0, userList.Count);
                        if (!randomUserDic.ContainsKey(index))
                        {
                            User user = userList[index];
                            randomUserDic.Add(index, user);
                        }
                    }

                    randomUserList = randomUserDic.OrderBy(n => n.Key).Select(n => n.Value).ToList();
                }

                return View(randomUserList);
            }
            //指定view，分页显示
            else
            {
                ViewData["pageCount"] = userList.Count / pageSize + 1;
                return View(view, userList.Skip(pageSize * (pageIndex - 1)).Take(pageSize));
            }
        }

        /// <summary>
        /// 混合查找可能感兴趣的人-侧边栏
        /// </summary>
        /// <param name="pageSize"></param>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult _InterestedWithAllSide(int pageSize = 5)
        {
            ViewData["pageSize"] = pageSize;

            return _InterestedWithAll(pageSize);
        }

        #endregion 可能感兴趣的人

        #region 在线用户
        /// <summary>
        /// 在线用户局部页
        /// </summary>
        [DonutOutputCache(CacheProfile = "Frequently")]
        public ActionResult OnlineUser(string viewName = "OnlineUser", bool ignoreAnonymousUsers = true, int topNumber = 0, int pageIndex = 1, int pageSize = 100)
        {
            Dictionary<string, OnlineUser> loggerUsers = onlineUserService.GetLoggedUsers();
            IList<OnlineUser> anonymousUsers = onlineUserService.GetAnonymousUsers();
            IEnumerable<OnlineUser> onlineUsers = Enumerable.Empty<OnlineUser>();
            onlineUsers = onlineUsers.Union(loggerUsers.Values.ToList());
            if (!ignoreAnonymousUsers)
            {
                onlineUsers = onlineUsers.Union(anonymousUsers);
            }
            if (topNumber != 0)
            {
                onlineUsers = onlineUsers.Take(topNumber);
            }
            PagingDataSet<OnlineUser> users = new PagingDataSet<OnlineUser>(onlineUsers)
            {
                TotalRecords = onlineUsers.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return View(viewName, users);
        }

        #endregion
    }
}
