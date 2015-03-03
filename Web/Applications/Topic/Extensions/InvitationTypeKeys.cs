//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace SpecialTopic.Topic
{
    /// <summary>
    /// 扩展InvitationTypeKeys
    /// </summary>
    public static class InvitationTypeKeysExtension
    {
        /// <summary>
        /// 邀请加入专题
        /// </summary>
        /// <param name="invitationTypeKeys">invitationTypeKeys</param>
        public static string InviteJoinTopic(this InvitationTypeKeys invitationTypeKeys)
        {
            return "InviteJoinTopic";
        }

        /// <summary>
        /// 申请加入专题
        /// </summary>
        /// <param name="invitationTypeKeys">invitationTypeKeys</param>
        public static string ApplyJoinTopic(this InvitationTypeKeys invitationTypeKeys)
        {
            return "ApplyJoinTopic";
        }
    }
}