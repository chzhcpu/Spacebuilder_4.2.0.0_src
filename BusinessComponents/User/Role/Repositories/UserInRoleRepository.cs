//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户角色与用户关联Repository
    /// </summary>
    public class UserInRoleRepository : Repository<UserInRole>, IUserInRoleRepository
    {

        /// <summary>
        /// 把用户加入到一组角色中
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleNames">赋予用户的用户角色</param>
        public void AddUserToRoles(long userId, List<string> roleNames)
        {
            Database dao = CreateDAO();

            dao.OpenSharedConnection();
            RemoveUserRoles(userId);
            var sqlInsert = Sql.Builder;
            UserInRole userInRole = new UserInRole();
            userInRole.UserId = userId;
            foreach (var roleName in roleNames)
            {
                userInRole.RoleName = roleName;
                dao.Insert(userInRole);
            }
            dao.CloseSharedConnection();

            //增加版本
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            foreach (var roleName in roleNames)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("RoleName", roleName);
            }
        }

        /// <summary>
        /// 删除用户的一个角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名</param>
        public void Delete(long userId, string roleName)
        {
            var sql = Sql.Builder;
            sql.Append("delete from tn_UsersInRoles")
                .Where("UserId= @0 and RoleName=@1", userId, roleName);
            CreateDAO().Execute(sql);

            //增加版本
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            RealTimeCacheHelper.IncreaseAreaVersion("RoleName", roleName);
        }


        /// <summary>
        /// 获取用户的角色名称
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="onlyPublic">是否仅获取对外公开的角色</param>
        /// <returns>用户的所有角色，如果该用户没有用户角色返回空集合</returns>
        public IEnumerable<string> GetRoleNamesOfUser(long userId, bool onlyPublic = false)
        {
            string cacheKeyUserInRole = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId) + onlyPublic;
            List<string> roleNames = cacheService.Get<List<string>>(cacheKeyUserInRole);

            var sqlRole = Sql.Builder;
            if (roleNames == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("RoleName")
                    .From("tn_UsersInRoles")
                    .Where("UserId = @0", userId);
                roleNames = CreateDAO().Fetch<string>(sql);

                cacheService.Add(cacheKeyUserInRole, roleNames, CachingExpirationType.UsualObjectCollection);
            }
            return roleNames;
        }

        /// <summary>
        /// 查询拥有管理员角色的用户Id集合
        /// </summary>
        /// <param name="administratorRoleName">管理员角色名称</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfRole(string administratorRoleName)
        {
            string cacheKeyUserInRole = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "RoleName", administratorRoleName);
            List<long> userIds = cacheService.Get<List<long>>(cacheKeyUserInRole);

            var sqlRole = Sql.Builder;
            if (userIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("UserId")
                    .From("tn_UsersInRoles")
                    .Where("RoleName = @0", administratorRoleName);
                userIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKeyUserInRole, userIds, CachingExpirationType.UsualObjectCollection);
            }
            return userIds;
        }


        /// <summary>
        /// 移除用户的所有角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void RemoveUserRoles(long userId)
        {
            var sqlDelete = Sql.Builder;
            sqlDelete.Append("Delete from tn_UsersInRoles where UserId = @0", userId);
            CreateDAO().Execute(sqlDelete);
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }
    }
}
