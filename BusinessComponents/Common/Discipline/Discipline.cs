//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-5-7</createdate>
//<author>bianchx</author>
//<email>bianchx@tunynet.com</email>
//<log date="2012-5-7 9:45:06" version="0.5">新建</log>
//--------------------------------------------------------------
//</TunynetCopyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 地区实体类
    /// </summary>
    [TableName("tn_Disciplines")]
    [PrimaryKey("DisciplineCode", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class Discipline : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Discipline New()
        {
            Discipline discipline = new Discipline()
            {
                Name = string.Empty,
                PostCode = string.Empty
            };
            return discipline;
        }

        #region 需持久化属性

        /// <summary>
        ///地区编码
        /// </summary>
        public string DisciplineCode { get; set; }

        /// <summary>
        ///父级地区编码
        /// </summary>
        public string ParentCode { get; set; }

        /// <summary>
        ///地区名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///邮政编码
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///子地区个数
        /// </summary>
        public int ChildCount { get; set; }


        #endregion

        List<Discipline> children;
        /// <summary>
        /// 子地区列表
        /// </summary>
        [Ignore]
        public IEnumerable<Discipline> Children
        {
            get
            {
                if (this.children == null)
                    children = new List<Discipline>();
                return children.ToReadOnly();
            }
        }

        public void AppendChild(Discipline discipline)
        {
            if (children == null)
                children = new List<Discipline>();

            children.Add(discipline);
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.DisciplineCode; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
