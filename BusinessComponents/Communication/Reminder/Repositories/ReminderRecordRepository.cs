//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Threading;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 提醒记录数据访问
    /// </summary>
    public class ReminderRecordRepository : Repository<ReminderRecord>, IReminderRecordRepository
    {
        private static ReaderWriterLockSlim RWLock = new System.Threading.ReaderWriterLockSlim();
        /// <summary>
        /// 创建提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderMode">提醒方式</param>
        /// <param name="reminderInfoType">提醒信息类型</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        public void CreateRecords(long userId, int reminderMode, int reminderInfoType, IEnumerable<long> objectIds)
        {
            Database dao = CreateDAO();

            dao.OpenSharedConnection();
            var sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_ReminderRecords")
               .Where("UserId=@0", userId)
               .Where("ReminderModeId=@0", reminderMode)
               .Where("ReminderInfoTypeId=@0", reminderInfoType);

            IEnumerable<object> oldObjectIds_object = dao.FetchFirstColumn(sql);
            IEnumerable<long> oldObjectIds = oldObjectIds_object.Cast<long>();
            foreach (var objectId in objectIds)
            {
                if (!oldObjectIds.Contains(objectId))
                {
                    ReminderRecord record = ReminderRecord.New();
                    record.UserId = userId;
                    record.ReminderModeId = reminderMode;
                    record.ReminderInfoTypeId = reminderInfoType;
                    record.ObjectId = objectId;
                    dao.Insert(record);
                }
            }

            dao.CloseSharedConnection();
        }

        /// <summary>
        /// 更新提醒记录（只更新最后提醒时间）
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        public void UpdateRecoreds(long userId, int reminderModeId, int reminderInfoTypeId, IEnumerable<long> objectIds)
        {
            Database dao = CreateDAO();
            dao.OpenSharedConnection();

            RWLock.EnterWriteLock();

            foreach (var objectId in objectIds)
            {
                int count = dao.FirstOrDefault<int>(Sql.Builder.Select("count(*)").From("tn_ReminderRecords").Where("UserId = @0 and ReminderModeId = @1 and ReminderInfoTypeId = @2 and ObjectId=@3", userId, reminderModeId, reminderInfoTypeId, objectId));
                if (count > 0)
                    dao.Execute(Sql.Builder.Append("Update tn_ReminderRecords set LastReminderTime = @0 where UserId = @1 and ReminderModeId = @2 and ReminderInfoTypeId = @3 and ObjectId = @4", DateTime.UtcNow, userId, reminderModeId, reminderInfoTypeId, objectId));
                else
                {
                    ReminderRecord record = ReminderRecord.New();
                    record.UserId = userId;
                    record.ReminderModeId = reminderModeId;
                    record.ReminderInfoTypeId = reminderInfoTypeId;
                    record.ObjectId = objectId;
                    dao.Insert(record);
                }
            }
            RWLock.ExitWriteLock();
            dao.CloseSharedConnection();
        }

        //方法是否正确
        /// <summary>
        /// 删除用户数据（删除用户的时候调用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            List<Sql> sql_Deletes = new List<Sql>();
            sql_Deletes.Add(PetaPoco.Sql.Builder.Append("delete from tn_ReminderRecords where UserId = @0", userId));
            sql_Deletes.Add(PetaPoco.Sql.Builder.Append("delete from tn_UserReminderSettings where UserId = @0", userId));
            CreateDAO().Execute(sql_Deletes);
        }

        /// <summary>
        /// 清除垃圾提醒记录
        /// </summary>
        /// <param name="storageDay">保留天数</param>
        public void DeleteTrashRecords(int storageDay)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("Delete from tn_ReminderRecords where DateCreated < @0 ", DateTime.UtcNow.AddDays(-storageDay));
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 获取用户所有的提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        public IEnumerable<ReminderRecord> GetRecords(long userId, int reminderModeId, int reminderInfoTypeId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("*")
                .From("tn_ReminderRecords")
                .Where("UserId = @0", userId)
                .Where("ReminderModeId =@0", reminderModeId)
                .Where("ReminderInfoTypeId =@0", reminderInfoTypeId);

            IEnumerable<ReminderRecord> records = CreateDAO().Fetch<ReminderRecord>(sql);

            return records;
        }
    }
}