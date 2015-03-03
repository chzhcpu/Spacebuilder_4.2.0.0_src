using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Documents;
using Tunynet.Common;
using Tunynet.Utilities;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// Journal 索引文档
    /// </summary>
    public class JournalIndexDocument
    {
        #region 索引字段
        public static readonly string id = "id";
        public static readonly string journalname = "journalname";
        public static readonly string abbr = "abbr";
        public static readonly string publisher = "publisher";
        public static readonly string jdescrip = "jdescrip";
        public static readonly string subcat = "subcat";

        #endregion

        /// <summary>
        /// BlogThread转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="journal">日志实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static Document Convert(Journal journal)
        {
            Document doc = new Document();

            //索引日志基本信息
            doc.Add(new Field(JournalIndexDocument.id, journal.id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(JournalIndexDocument.journalname, journal.journal.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(JournalIndexDocument.abbr, journal.abbr, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(JournalIndexDocument.publisher, journal.publisher.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(JournalIndexDocument.jdescrip, journal.jdescrip, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(JournalIndexDocument.subcat, journal.subcat , Field.Store.YES, Field.Index.NOT_ANALYZED));
            
            /*
            //索引日志tag
            foreach (string tagName in journal.TagNames)
            {
                doc.Add(new Field(BlogIndexDocument.Tag, tagName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
            }

            //索引日志用户分类名称
            IEnumerable<string> ownerCategoryNames = journal.OwnerCategoryNames;
            if (ownerCategoryNames != null)
            {
                foreach (string ownerCategoryName in ownerCategoryNames)
                {
                    doc.Add(new Field(BlogIndexDocument.OwnerCategoryName, ownerCategoryName.ToLower(), Field.Store.YES, Field.Index.ANALYZED));
                }
            }

            //索引日志站点分类ID
            long? siteCategoryId = journal.SiteCategoryId;
            if (siteCategoryId.HasValue)
            {
                doc.Add(new Field(BlogIndexDocument.SiteCategoryId, siteCategoryId.Value.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            }
            */
            return doc;
        }

        /// <summary>
        /// Journal批量转换成<see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="journals">Journals实体</param>
        /// <returns>Lucene.Net.Documents.Document</returns>
        public static IEnumerable<Document> Convert(IEnumerable<Journal> journals)
        {
            List<Document> docs = new List<Document>();
            foreach (Journal j in journals)
            {
                Document doc = Convert(j);
                docs.Add(doc);
            }

            return docs;
        }

    }
}