//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common
{
    public static class TextLengthSettings
    {
        /// <summary>
        /// 文本主内容长度
        /// </summary>
        public const int TEXT_BODY_MAXLENGTH = 100000;

        /// <summary>
        /// 文本主题内容最小长度
        /// </summary>
        public const int TEXT_BODY_MINLENGTH = 1;

        /// <summary>
        /// 文本名称最大长度
        /// </summary>
        /// <remarks>建议不要超过20<remarks/>
        public const int TEXT_NAME_MAXLENGTH = 20;

        /// <summary>
        /// 文本名称最小长度
        /// </summary>
        public const int TEXT_NAME_MINLENGTH = 1;

        /// <summary>
        /// 文本标题最大长度
        /// </summary>
        /// <remarks>建议不要超过20<remarks/>
        public const int TEXT_SUBJECT_MAXLENGTH = 50;

        /// <summary>
        /// 文本标题最小长度
        /// </summary>
        public const int TEXT_SUBJECT_MINLENGTH = 1;

        /// <summary>
        /// 一般描述的最长字数
        /// </summary>
        /// <remarks>建议不要超过256<remarks/>
        public const int TEXT_DESCRIPTION_MAXLENGTH = 200;
    }
}
