//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet.Common;
using Tunynet.Events;
using Tunynet.Globalization;
using Spacebuilder.Common;
using Tunynet.Utilities;

namespace Spacebuilder.Journals.EventModules
{
    /// <summary>
    /// 处理动态、积分、通知的EventMoudle
    /// </summary>
    public class JournalsEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            EventBus<Journal,AuditEventArgs>.Instance().After += new CommonEventHandler<Journal, AuditEventArgs>(JournalsEventModule_After);
        }

        /// <summary>
        /// 增删改等触发的事件
        /// </summary>
        private void JournalsEventModule_After(Journal repair, AuditEventArgs eventArgs)
        {
            //todo:可以在这里处理动态、通知、积分
        }

    }
}