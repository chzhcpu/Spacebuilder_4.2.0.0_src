//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.CMS
{
    /// <summary>
    /// 内容模型标识
    /// </summary>
    public class ContentTypeKeys
    {
        #region Instance

        private static volatile ContentTypeKeys _instance = null;
        private static readonly object lockObject = new object();

        public static ContentTypeKeys Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ContentTypeKeys();
                    }
                }
            }
            return _instance;
        }

        private ContentTypeKeys()
        { }

        #endregion Instance


        /// <summary>
        /// 文章
        /// </summary>
        /// <returns></returns>
        public string News()
        {
            return "News";
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <returns></returns>
        public string NewsLink()
        {
            return "NewsLink";
        }
    }
}