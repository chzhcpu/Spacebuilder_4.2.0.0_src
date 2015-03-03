//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;

using Spacebuilder.Common;
using System.Web.Mvc;


namespace SpecialTopic.Topic
{
    /// <summary>
    /// 编辑专题实体
    /// </summary>
    public class TopicEditModel
    {
        /// <summary>
        ///TopicId
        /// </summary>
        public long TopicId { get; set; }

        /// <summary>
        ///专题名称
        /// </summary>
        [Display(Name = "名称")]
        [WaterMark(Content = "在此输入专题名称")]
        [Required(ErrorMessage = "请输入专题名称")]
        [StringLength(60, ErrorMessage = "最多允许输入60个字")]
        [DataType(DataType.Text)]
        public string TopicName { get; set; }

        /// <summary>
        ///专题标识（个性网址的关键组成部分）
        /// </summary>
        [Required(ErrorMessage = "请输入专题标识")]
        //[StringLength(16, MinimumLength = 4, ErrorMessage = "请输入4-16个字")]
        //[DataType(DataType.Url)]
        [Remote("ValidateTopicKey", "ChannelTopic", "Topic", ErrorMessage = "此专题Key已存在", AdditionalFields = "TopicId")]
        public string TopicKey { get; set; }

        /// <summary>
        ///专题介绍
        /// </summary>
        [Display(Name = "简介")]
        [StringLength(300, ErrorMessage = "最多可以输入300个字")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        ///所在地区
        /// </summary>        
        public string AreaCode { get; set; }

        /// <summary>
        ///logo名称（带部分路径）
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        ///是否公开
        /// </summary>
        [Required(ErrorMessage = "请选择专题类型")]
        public bool IsPublic { get; set; }

        /// <summary>
        ///加入方式
        /// </summary>
        [Required(ErrorMessage = "请选择加入方式")]
        public TopicJoinWay JoinWay { get; set; }

        /// <summary>
        ///是否允许成员邀请（一直允许群管理员邀请）
        /// </summary>
        public bool EnableMemberInvite { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        [WaterMark(Content = "如1+1=？")]
        [StringLength(36, ErrorMessage = "最多输入36个汉字")]
        [DataType(DataType.Text)]
        public string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        [StringLength(36, ErrorMessage = "最多输入36个汉字")]
        [DataType(DataType.Text)]
        public string Answer { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        [Required(ErrorMessage = "请选择类别")]
        public long CategoryId { get; set; }

        /// <summary>
        /// 相关用户Id集合
        /// </summary>
        public string RelatedUserIds { get; set; }

        /// <summary>
        /// 相关标签集合
        /// </summary>
        public string[] RelatedTags { get; set; }

        /// <summary>
        /// 转换成groupEntity类型
        /// </summary>
        /// <returns></returns>
        public TopicEntity AsTopicEntity()
        {
            CategoryService categoryService = new CategoryService();
            TopicEntity topicEntity = null;

            //创建专题
            if (this.TopicId == 0)
            {
                topicEntity = TopicEntity.New();
                topicEntity.UserId = UserContext.CurrentUser.UserId;
                topicEntity.DateCreated = DateTime.UtcNow;
                topicEntity.TopicKey = this.TopicKey;
            }
            //编辑专题
            else
            {
                TopicService topicService = new TopicService();
                topicEntity = topicService.Get(this.TopicId);
            }
            topicEntity.IsPublic = this.IsPublic;
            topicEntity.TopicName = this.TopicName;
            if (Logo != null)
            {
                topicEntity.Logo = this.Logo;
            }
            topicEntity.Description = Formatter.FormatMultiLinePlainTextForStorage(this.Description == null ? string.Empty : this.Description, true);
            topicEntity.AreaCode = this.AreaCode??string.Empty;
            topicEntity.JoinWay = this.JoinWay;
            topicEntity.EnableMemberInvite = this.EnableMemberInvite;
            //topickey 去掉空格，变为小写字母
            topicEntity.TopicKey = this.TopicKey.ToLower().Replace(" ","-");

            if (JoinWay == TopicJoinWay.ByQuestion)
            {
                topicEntity.Question = this.Question;
                topicEntity.Answer = this.Answer;
            }
            return topicEntity;
        }
    }

    /// <summary>
    /// 专题实体的扩展类
    /// </summary>
    public static class TopicEntityExtensions
    {
        /// <summary>
        /// 将数据库中的信息转换成EditModel实体
        /// </summary>
        /// <param name="topicEntity"></param>
        /// <returns></returns>
        public static TopicEditModel AsEditModel(this TopicEntity topicEntity)
        {
            return new TopicEditModel
            {
                TopicId = topicEntity.TopicId,
                IsPublic = topicEntity.IsPublic,
                TopicName = topicEntity.TopicName,
                TopicKey = topicEntity.TopicKey,
                Logo = topicEntity.Logo,
                Description = Formatter.FormatMultiLinePlainTextForEdit(topicEntity.Description, true),
                AreaCode = topicEntity.AreaCode,
                JoinWay = topicEntity.JoinWay,
                EnableMemberInvite = topicEntity.EnableMemberInvite,
                CategoryId = topicEntity.Category == null ? 0 : topicEntity.Category.CategoryId,
                Question = topicEntity.Question,
                Answer = topicEntity.Answer
            };
        }
    }
}
