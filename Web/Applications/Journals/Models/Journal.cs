//<TunynetCopyright>
//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet;

namespace Spacebuilder.Journals
{
    /// <summary>
    /// 杂志实体类
    /// </summary>
    [PetaPoco.TableName("j_journal")]
    [PetaPoco.PrimaryKey("id", autoIncrement = true)]
    public class Journal : IEntity
    {
        /*
          ,[journal],[abbr] ,[issn],[eissn],[publisher],[publishersite]
      ,[jdescrip],[jsite] ,[subject],[subcat]
         */
        public long id { get; set; }
        public string journal { get; set; }
        public string abbr { get; set; }
        public string issn { get; set; }
        public string eissn { get; set; }
        public string publisher { get; set; }
        public string publishersite { get; set; }

        public string jdescrip { get; set; }
        public string jsite { get; set; }
        public string subject { get; set; }
        public string subcat { get; set; }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion


        public static Journal New()
        {
            Journal j = new Journal();
            
            //todo:

            return j;
                 
        }
    }
}