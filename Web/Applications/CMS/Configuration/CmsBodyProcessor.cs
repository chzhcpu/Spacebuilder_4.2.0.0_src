//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CodeKicker.BBCode;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet.Utilities;
using System;
using System.Text.RegularExpressions;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯正文解析器
    /// </summary>
    public class CmsBodyProcessor : IBodyProcessor
    {

        public string Process(string body, string tenantTypeId, long associateId, long userId)
        {
            if (string.IsNullOrEmpty(body) || !body.Contains("[attach:"))
                return body;

            List<long> attachmentIds = new List<long>();

            Regex rg = new Regex(@"\[attach:(?<id>[\d]+)\]", RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(body);

            if (matches != null)
            {
                foreach (Match m in matches)
                {
                    if (m.Groups["id"] == null || string.IsNullOrEmpty(m.Groups["id"].Value))
                        continue;
                    long attachmentId = 0;
                    long.TryParse(m.Groups["id"].Value, out attachmentId);
                    if (attachmentId > 0 && !attachmentIds.Contains(attachmentId))
                        attachmentIds.Add(attachmentId);
                }
            }

            IEnumerable<ContentAttachment> attachments = new ContentAttachmentService().Gets(attachmentIds);
            if (attachments != null && attachments.Count() > 0)
            {
                IList<BBTag> bbTags = new List<BBTag>();
                string htmlTemplate = "<div class=\"tn-annexinlaid\"><a href=\"{3}\" rel=\"nofollow\">{0}</a>（<em>{1}</em>,<em>下载次数：{2}</em>）<a href=\"{3}\" rel=\"nofollow\">下载</a> </div>";

                //解析文本中附件
                foreach (var attachment in attachments)
                {
                    bbTags.Add(AddBBTag(htmlTemplate, attachment));
                }

                body = HtmlUtility.BBCodeToHtml(body, bbTags);
            }
            return body;
        }

        /// <summary>
        /// 添加BBTag实体
        /// </summary>
        /// <param name="htmlTemplate">html模板</param>
        /// <param name="attachment">带替换附件</param>
        /// <returns></returns>
        private BBTag AddBBTag(string htmlTemplate, ContentAttachment attachment)
        {

            BBAttribute bbAttribute = new BBAttribute("attachTemplate", "",
                                                      n =>
                                                      {
                                                          return string.Format(htmlTemplate,
                                                                               attachment.FriendlyFileName,
                                                                               attachment.FriendlyFileLength,
                                                                               attachment.DownloadCount,
                                                                              SiteUrls.Instance().ContentAttachmentUrl(attachment.AttachmentId));
                                                      },
                                                      HtmlEncodingMode.UnsafeDontEncode);

            return new BBTag("attach:" + attachment.AttachmentId, "${attachTemplate}", "", false, BBTagClosingStyle.LeafElementWithoutContent, null, bbAttribute);
        }
    }
}