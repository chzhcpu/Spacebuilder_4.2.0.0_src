using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;

namespace SpecialTopic.Topic
{
    public static class UserBlockServiceExtension
    {
        public static bool BlockTopic(this UserBlockService blockservice, long userId, long blockedGroupId, string blockedGroupName)
        {
            throw new Exception();
            return false;
        }

        public static bool IsBlockedTopic(this UserBlockService block, long userId, long checkingTopicId)
        {
            throw new Exception();
            return false;
        }

        public static IEnumerable<UserBlockedObject> GetBlockedTopics(this UserBlockService blockservice,long userId)
        {
            throw new  Exception();
            return null;
        }
    }
}