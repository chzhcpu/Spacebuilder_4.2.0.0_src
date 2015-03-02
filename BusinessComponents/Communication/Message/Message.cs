//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-04-17</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-04-17" version="0.5">创建</log>
//--------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Utilities;
using System.Text.RegularExpressions;

namespace Tunynet.Common
{  /// <summary>
    /// 私信实体
    /// </summary>
    [TableName("tn_Messages")]
    [PrimaryKey("MessageId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "ReceiverUserId")]
    [Serializable]
    public class Message : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Message New()
        {
            Message message = new Message()
            {
                Sender = string.Empty,
                Receiver = string.Empty,
                Subject = string.Empty,
                Body = string.Empty,
                IP = WebUtility.GetIP(),
                IsRead = false,
                MessageType = Common.MessageType.Common,
                DateCreated = DateTime.UtcNow
            };
            return message;
        }

        #region 需持久化属性

        /// <summary>
        ///MessageId
        /// </summary>
        public long MessageId { get; protected set; }

        /// <summary>
        ///发件人UserId
        /// </summary>
        public long SenderUserId { get; set; }

        /// <summary>
        ///发件人的DisplayName
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        ///收件人UserId
        /// </summary>
        public long ReceiverUserId { get; set; }

        /// <summary>
        ///收件人DisplayName
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        ///私信标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///私信内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        ///私信来源IP
        /// </summary>
        public string IP { get; protected set; }

        /// <summary>
        ///发布日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion 需持久化属性

        /// <summary>
        /// 私信类型
        /// </summary>
        [Ignore]
        public MessageType MessageType { get; set; }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.MessageId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员

        #region 扩展属性

        /// <summary>
        /// 将私信中的图片和音频转换成实际的信息
        /// </summary>
        [Ignore]
        public string ResolveBody
        {
            get
            {
                string resolveBody = this.Body;
                Regex regex = new Regex("{\"AttachmentPath\":\"(?<url>(.|\n)*)\"Type\":(?<type>[0-9]*)}", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(resolveBody);
                foreach (Match match in matches)
                {
                    string url = match.Groups["url"].Value;
                    string type = match.Groups["type"].Value;
                    //将图片显示出来
                    if (type == "17")
                    {
                        int length = url.LastIndexOf("--") > 0 ? url.LastIndexOf("--") : url.LastIndexOf("/") + 23;
                        string image = string.Format("<a rel=\"fancybox\" href=\"{0}\" ><img alt=\"{1}\" src=\"{2}\" /></a>", url.Substring(0, length), "消息图片" + new Random().Next(99, 9999), url);
                        resolveBody = resolveBody.Replace(match.Value, image);
                    }
                    //将音频显示出来
                    else if (type == "18")
                    {
                        string audio = string.Format("<a href=\"javascript:;\" data-url=\"{0}\" class=\"voice_player_btn\"><span class=\"before\">&nbsp;</span><span class=\"middle\"><span class=\"speaker speaker_animate\">&nbsp;</span></span><span class=\"after\">&nbsp;</span></a>", url);
                        resolveBody = resolveBody.Replace(match.Value, audio);
                    }
                }
                //将表情显示出来
                EmotionService emotionService = new EmotionService();
                resolveBody = emotionService.EmoticonTransforms(resolveBody);
                return resolveBody;
            }
        }


        /// <summary>
        /// 将私信中的图片和音频转换[音频]、[图片]等内容
        /// </summary>
        [Ignore]
        public string FormatResolveBody
        {
            get
            {
                string resolveBody = this.Body;
                Regex regex = new Regex("{\"AttachmentPath\":\"(?<url>(.|\n)*)\"Type\":(?<type>[0-9]*)}", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(resolveBody);
                foreach (Match match in matches)
                {
                    string url = match.Groups["url"].Value;
                    string type = match.Groups["type"].Value;
                    //将图片显示出来
                    if (type == "17")
                    {
                        string image = "[图片]";
                        resolveBody = resolveBody.Replace(match.Value, image);
                    }
                    //将音频显示出来
                    else if (type == "18")
                    {
                        string audio = "[音频]";
                        resolveBody = resolveBody.Replace(match.Value, audio);
                    }
                }
                //将表情显示出来
                EmotionService emotionService = new EmotionService();
                resolveBody = emotionService.EmoticonTransforms(resolveBody);
                return resolveBody;
            }
        }

        #endregion
    }
}