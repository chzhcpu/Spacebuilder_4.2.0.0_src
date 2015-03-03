//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;

using Tunynet.UI;
using Tunynet.Utilities;
using System.Text;
using spt.Utils;

namespace SpecialTopic.Topic.Controllers
{
    [Themed(PresentAreaKeysOfExtension.TopicSpace, IsApplication = true)]
    [AnonymousBrowseCheck]
    [TitleFilter(IsAppendSiteName = true)]
    [TopicSpaceAuthorize(RequireManager = true)]
    public class TopicSpaceSettingsController : Controller
    {

        public IPageResourceManager pageResourceManager { get; set; }
        public CategoryService categoryService { get; set; }
        public TopicService topicService { get; set; }
        public Authorizer authorizer { get; set; }

        public PaperService paperService { get; set; }

        private TagService tagService = new TagService(TenantTypeIds.Instance().Topic());

        
        //这个多个地方用到
        /// <summary>
        /// 右侧导航菜单
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public ActionResult _TopicSettingRightMenu(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            PagingDataSet<TopicMemberApply> applys = topicService.GetTopicMemberApplies(topic.TopicId, TopicMemberApplyStatus.Pending);
            long totalRecords = applys.TotalRecords;
            ViewData["totalRecords"] = totalRecords;
            return View(topic);
        }

        /// <summary>
        /// 管理专题成员申请页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageMemberApplies(string spaceKey, TopicMemberApplyStatus? applyStatus, int pageIndex = 1, int pageSize = 20)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(topic.TopicName);
            pageResourceManager.InsertTitlePart("管理专题成员申请页");
            
            //已修改
            PagingDataSet<TopicMemberApply> topicMemberApplies = topicService.GetTopicMemberApplies(topic.TopicId, applyStatus, pageSize, pageIndex);
            ViewData["topicId"] = topic.TopicId;
            TempData["TopicMenu"] = TopicMenu.ManageMember;

            return View(topicMemberApplies);
        }

        /// <summary>
        /// 接受/拒绝专题加入申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveMemberApply(string spaceKey, IList<long> applyIds, bool isApproved)
        {
            
            
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            topicService.ApproveTopicMemberApply(applyIds, isApproved);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }


        /// <summary>
        /// 删除专题加入申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMemberApply(string spaceKey, long id)
        {    
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            topicService.DeleteTopicMemberApply(id);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        /// <summary>
        /// 管理专题成员页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageMembers(string spaceKey, int pageIndex = 1, int pageSize = 20)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(topic.TopicName);
            pageResourceManager.InsertTitlePart("管理专题成员页");

            PagingDataSet<TopicMember> topicMembers = topicService.GetTopicMembers(topic.TopicId, true, SortBy_TopicMember.DateCreated_Asc, pageSize, pageIndex);
            ViewData["topic"] = topic;
            TempData["TopicMenu"] = TopicMenu.ManageMember;

            return View(topicMembers);
        }
                 
        /// <summary>
        /// 创建更换群主模式框
        /// </summary>
        /// <param name="topicId">专题Id</param>
        /// <param name="userId">群主名称</param>
        /// <returns>更换群主</returns>
        [HttpGet]
        public ActionResult _ChangeTopicOwner(string spaceKey, string returnUrl)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return Content(string.Empty);
            
            
            
            ViewData["returnUrl"] = WebUtility.UrlDecode(returnUrl);
            return View(topic);
        }

        
        
        /// <summary>
        /// 更换群主
        /// </summary>
        /// <param name="topic">编辑专题对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ChangeTopicOwner(string spaceKey)
        {
            string returnUrl = Request.QueryString.Get<string>(WebUtility.UrlDecode("returnUrl"));
            
            var userIds = Request.Form.Gets<long>("UserId", new List<long>());
            long userId = userIds.FirstOrDefault();
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return Content(string.Empty);
            if (userId == 0)
            {
                Tunynet.Utilities.WebUtility.SetStatusCodeForError(Response);
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Hint, "您没有选择群主");
                ViewData["returnUrl"] = returnUrl;
                return View(topic);
            }
            if (topic.UserId == userId)
            {
                Tunynet.Utilities.WebUtility.SetStatusCodeForError(Response);
                ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Hint, "您没有更换群主");
                ViewData["returnUrl"] = returnUrl;
                return View(topic);
            }
            if (!authorizer.Topic_SetManager(topic))
            {
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有更换群主的权限"));
            }

            topicService.ChangeTopicOwner(topic.TopicId, userId);
            return Json(new StatusMessageData(StatusMessageType.Success, "更换群主操作成功"));
        }

        /// <summary>
        ///  设置/取消 专题管理员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetManager(string spaceKey, long userId, bool isManager)
        {
            StatusMessageData message = null;
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();

            if (!authorizer.Topic_SetManager(topic))
                return Json(new StatusMessageData(StatusMessageType.Error, "您没有设置管理员的权限"));

            bool result = topicService.SetManager(topic.TopicId, userId, isManager);
            if (result)
            {
                message = new StatusMessageData(StatusMessageType.Success, "操作成功！");
            }
            else
            {
                message = new StatusMessageData(StatusMessageType.Error, "操作失败！");
            }
            return Json(message);
        }

        
        
        /// <summary>
        /// 批量移除专题成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMember(string spaceKey, List<long> userIds)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            foreach (var userId in userIds)
            {
                if (!authorizer.Topic_DeleteMember(topic, userId))
                {
                    return Json(new StatusMessageData(StatusMessageType.Error, "您没有删除专题成员的权限"));
                }
            }




            topicService.DeleteTopicMember(topic.TopicId, userIds);
            return Json(new StatusMessageData(StatusMessageType.Success, "操作成功"));
        }

        /// <summary>
        /// 删除专题logo
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DeleteTopicLogo(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "没有该专题！"));
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            
            //已修改
            //这个功能属于编辑专题，在编辑专题已做权限验证，这边还需要做验证吗？
            
            topicService.DeleteLogo(topic.TopicId);
            return Json(new StatusMessageData(StatusMessageType.Success, "删除专题Logo成功！"));
        }

        [HttpGet]
        public ActionResult BatchAddPapers(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            if (topic == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(topic.TopicName);
            pageResourceManager.InsertTitlePart("添加专题文章");
            BatchDois batchdois = new BatchDois();
            batchdois.TopicId = topic.TopicId;
            batchdois.TopicName = topic.TopicName;

            return View(batchdois);

        }
        /// <summary>
        /// 处理批量添加的doi
        /// </summary>
        /// <param name="spaceKey">topicKey</param>
        /// <param name="batchdois">批量doi</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        public ActionResult BatchAddPapers(string spaceKey,BatchDois batchdois)
        {
            long topicId = topicService.GetTopicIdByTopicKey(spaceKey);
            string doisstring = batchdois.dois;
            doisstring = doisstring.ToLower().Replace(" ", "");
            doisstring = doisstring.Replace("doi:","");
            string[] dois = doisstring.Split(new string[] { "<br/>" },System.StringSplitOptions.RemoveEmptyEntries);

            string doiQueryStr = "";
            if (dois.Count() < 1)
            {
                return View(batchdois);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string doi in dois)
                {
                    PaperEntity paper = paperService.GetPaperByPaperDOI(doi);
                    if (paper==null)
                    {
                        sb.Append(doi);
                        sb.Append(" or ");
                    }
                    else 
                    {
                        if (!paperService.TopicPaperExist(topicId, paper.PaperId))
                        {
                            paperService.AddPaperToTopic(paper.PaperId, topicId);
                        }
                    }
                }
                doiQueryStr = sb.ToString();
                doiQueryStr = doiQueryStr.Substring(0, doiQueryStr.Length - 4);
            }
            PubMedService pmService = new PubMedService();
            List<PubMedItem> pubmedItemList= pmService.GetCitationsFromDOIs(doiQueryStr);

            foreach (PubMedItem pubItem in pubmedItemList)
            {
                //todo:1 add pubitem to paperrepository
                //PaperEntity p = pubItem.AsPaperEntity();
                //paperService.Create(p);
                //todo:2 add pubitem to topicpaperrepository
                //paperService.AddPaperToTopic(
                paperService.AddPaperToTopic(topicId,pubItem.AsPaperEntity());
            }


            return View(batchdois);

        }


        /// <summary>
        /// 编辑专题页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTopic(string spaceKey)
        {
            TopicEntity topic = topicService.Get(spaceKey);
            
            //已修改
            if (topic == null)
                return HttpNotFound();
            pageResourceManager.InsertTitlePart(topic.TopicName);
            pageResourceManager.InsertTitlePart("编辑专题");

            
            //编辑的时候需要显示已添加的标签
            IEnumerable<string> tags = topic.TagNames;
            TopicEditModel topicEditModel = topic.AsEditModel();
            ViewData["tags"] = tags;
            TempData["TopicMenu"] = TopicMenu.TopicSettings;

            return View(topicEditModel);
        }

        /// <summary>
        /// 编辑专题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTopic(string spaceKey, TopicEditModel topicEditModel)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return Json(new StatusMessageData(StatusMessageType.Error, "您尚未登录！"));
            System.IO.Stream stream = null;
            HttpPostedFileBase topicLogo = Request.Files["TopicLogo"];

            if (topicLogo != null && !string.IsNullOrEmpty(topicLogo.FileName))
            {
                TenantLogoSettings tenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(TenantTypeIds.Instance().Topic());
                if (!tenantLogoSettings.ValidateFileLength(topicLogo.ContentLength))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, string.Format("文件大小不允许超过{0}", Formatter.FormatFriendlyFileSize(tenantLogoSettings.MaxLogoLength * 1024)));
                    return View(topicEditModel);
                }

                LogoSettings logoSettings = DIContainer.Resolve<ISettingsManager<LogoSettings>>().Get();
                if (!logoSettings.ValidateFileExtensions(topicLogo.FileName))
                {
                    ViewData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Error, "不支持的文件类型，仅支持" + logoSettings.AllowedFileExtensions);
                    return View(topicEditModel);
                }
                stream = topicLogo.InputStream;
                topicEditModel.Logo = topicLogo.FileName;
            }

            TopicEntity topic = topicEditModel.AsTopicEntity();


            //设置分类
            if (topicEditModel.CategoryId > 0)
            {
                categoryService.ClearCategoriesFromItem(topic.TopicId, 0, TenantTypeIds.Instance().Topic());
                categoryService.AddItemsToCategory(new List<long>() { topic.TopicId }, topicEditModel.CategoryId);
            }

            
            //已修改
            //设置标签
            string relatedTags = Request.Form.Get<string>("RelatedTags");
            if (!string.IsNullOrEmpty(relatedTags))
            {
                tagService.ClearTagsFromItem(topic.TopicId, topic.TopicId);
                tagService.AddTagsToItem(relatedTags, topic.TopicId, topic.TopicId);
            }
            if (stream != null)
            {
                topicService.UploadLogo(topic.TopicId, stream);
            }

            topicService.Update(currentUser.UserId, topic);
            TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Success, "更新成功！");
            return Redirect(SiteUrls.Instance().EditTopic(topic.TopicKey));
        }

        //[HttpGet]
        public ActionResult _Menu_Manage(string spaceKey)
        {
            long topicId = TopicIdToTopicKeyDictionary.GetTopicId(spaceKey);
            TopicEntity topic = topicService.Get(topicId);
            if (topic == null)
                return Content(string.Empty);

            int currentNavigationId = RouteData.Values.Get<int>("CurrentNavigationId", 0);

            NavigationService navigationService = new NavigationService();
            Navigation navigation = navigationService.GetNavigation(PresentAreaKeysOfExtension.TopicSpace, currentNavigationId, topic.TopicId);

            IEnumerable<Navigation> navigations = new List<Navigation>();
            if (navigation != null)
            {
                if (navigation.Depth >= 1 && navigation.Parent != null)
                {
                    navigations = navigation.Parent.Children;
                }
                else if (navigation.Depth == 0)
                {
                    navigations = navigation.Children;
                }
            }

            ViewData["MemberApplyCount"] = topicService.GetMemberApplyCount(topic.TopicId);

            return View(navigations);
        }
    }


    public class BatchDois
    {
        public long TopicId { get; set; }
        public string TopicName { get; set; }
        public string dois { get; set; }
    }
}