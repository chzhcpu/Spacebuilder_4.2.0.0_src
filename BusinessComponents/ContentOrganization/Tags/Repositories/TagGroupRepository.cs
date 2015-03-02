//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 标签分组仓储的具体实现类
    /// </summary>
    public class TagGroupRepository : Repository<TagGroup>, ITagGroupRepository
    {



        /// <summary>
        /// 根据标签获取标签分组
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<TagGroup> GetGroupsOfTag(string tagName, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
            cacheKey.Append("tagName-" + tagName);

            List<long> tagGroupIds = cacheService.Get<List<long>>(cacheKey.ToString());

            if (tagGroupIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("GroupId")
                   .From("tn_TagsInGroups")
                   .Where("TagName = @0 and TenantTypeId = @1", tagName, tenantTypeId);

                tagGroupIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey.ToString(), tagGroupIds, CachingExpirationType.ObjectCollection);
            }

            return PopulateEntitiesByEntityIds(tagGroupIds);
        }

        /// <summary>
        /// 获取全部标签分组
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<TagGroup> GetGroups(string tenantTypeId)
        {
            string cacheKey = RealTimeCacheHelper.GetAreaVersion("TenantTypeId", tenantTypeId) + "::AllTagGroups";
            List<long> tagGroupIds = cacheService.Get<List<long>>(cacheKey);
            if (tagGroupIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("GroupId")
                   .From("tn_TagGroups");

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId = @0", tenantTypeId);

                tagGroupIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey, tagGroupIds, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds(tagGroupIds);
        }
    }
}