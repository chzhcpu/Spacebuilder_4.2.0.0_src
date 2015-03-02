//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Documents;
using Tunynet.Common;
using Tunynet.Utilities;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 资讯索引文档
    /// </summary>
    public class CmsIndexDocument
    {
        #region 索引字段

        public static readonly string ContentItemId = "ContentItemId";
        public static readonly string IsEssential = "IsEssential";
        public static readonly string Summary = "Summary";
        public static readonly string UserId = "UserId";
        public static readonly string Title = "Title";
        public static readonly string Body = "Body";
        public static readonly string Author = "Author";
        public static readonly string Tag = "Tag";
        public static readonly string FolderName = "FolderName";
        public static readonly string ContentFolderId = "ContentFolderId";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string AuditStatus = "AuditStatus";

        #endregion

        /// <summary>
        /// contentItem转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="contentItem">资讯实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(ContentItem contentItem)
        {
            Document doc = new Document();

            //索引资讯基本信息
            doc.Add(new Field(CmsIndexDocument.ContentItemId, contentItem.ContentItemId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(CmsIndexDocument.IsEssential, contentItem.IsEssential ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(CmsIndexDocument.Summary, contentItem.Summary ?? "", Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(CmsIndexDocument.UserId, contentItem.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(CmsIndexDocument.Title, contentItem.Title.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(CmsIndexDocument.Body, HtmlUtility.StripHtml(contentItem.AdditionalProperties.Get<string>("Body", string.Empty), true, false).ToLower(), Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(CmsIndexDocument.Author, contentItem.Author, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(CmsIndexDocument.DateCreated, DateTools.DateToString(contentItem.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(CmsIndexDocument.AuditStatus, ((int)contentItem.AuditStatus).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            //索引资讯tag
            foreach (string tagName in contentItem.TagNames)
            {
                doc.Add(new Field(CmsIndexDocument.Tag, tagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            //索引资讯用户分类名称            
            if (contentItem.ContentFolder != null)
            {
                doc.Add(new Field(CmsIndexDocument.FolderName, contentItem.ContentFolder.FolderName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            //索引资讯站点分类Id
                doc.Add(new Field(CmsIndexDocument.ContentFolderId, contentItem.ContentFolderId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            return doc;
        }

        /// <summary>
        /// contentItem批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="contentItems">资讯实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<ContentItem> contentItems)
        {
            List<Document> docs = new List<Document>();
            foreach (ContentItem contentItem in contentItems)
            {
                Document doc = Convert(contentItem);
                docs.Add(doc);
            }

            return docs;
        }


    }
}