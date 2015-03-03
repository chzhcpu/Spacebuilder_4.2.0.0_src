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

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 专题索引文档
    /// </summary>
    public class TopicIndexDocument
    {
        #region 索引字段

        public static readonly string TopicId = "TopicId";
        public static readonly string TopicName = "TopicName";
        public static readonly string Description = "Description";
        public static readonly string IsPublic = "IsPublic";
        public static readonly string AreaCode = "AreaCode";
        public static readonly string UserId = "UserId";
        public static readonly string AuditStatus = "AuditStatus";
        public static readonly string DateCreated = "DateCreated";
        public static readonly string Tag = "Tag";
        public static readonly string CategoryName = "CategoryName";
        public static readonly string CategoryId = "CategoryId";
        public static readonly string MemberCount = "MemberCount";
        public static readonly string GrowthValue = "GrowthValue";

        #endregion

        /// <summary>
        /// TopicEntity转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="TopicEntity">专题实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(TopicEntity group)
        {
            Document doc = new Document();

            //索引专题基本信息
            doc.Add(new Field(TopicIndexDocument.TopicId, group.TopicId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.TopicName, group.TopicName, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(TopicIndexDocument.Description, group.Description, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(TopicIndexDocument.IsPublic, group.IsPublic==true ? "1" : "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.AreaCode, group.AreaCode, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.UserId, group.UserId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.AuditStatus, ((int)group.AuditStatus).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.DateCreated, DateTools.DateToString(group.DateCreated, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.MemberCount,group.MemberCount.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(TopicIndexDocument.GrowthValue, group.GrowthValue.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            if (group.Category != null)
            {
                doc.Add(new Field(TopicIndexDocument.CategoryName, group.Category.CategoryName, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(TopicIndexDocument.CategoryId, group.Category.CategoryId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            }

            //索引专题tag
            foreach (string tagName in group.TagNames)
            {
                doc.Add(new Field(TopicIndexDocument.Tag, tagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            return doc;
        }

        /// <summary>
        /// group批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="groups">日志实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<TopicEntity> groups)
        {
            List<Document> docs = new List<Document>();
            foreach (TopicEntity group in groups)
            {
                Document doc = Convert(group);
                docs.Add(doc);
            }

            return docs;
        }


    }
}