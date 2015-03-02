//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 设置管理器
    /// </summary>
    /// <typeparam name="TSettingsEntity"></typeparam>
    public class SettingManager<TSettingsEntity> : ISettingsManager<TSettingsEntity> where TSettingsEntity : class, IEntity, new()
    {
        public ISettingsRepository<TSettingsEntity> repository { get; set; }

        public TSettingsEntity Get()
        {
            return repository.Get();
        }

        public void Save(TSettingsEntity settings)
        {
            repository.Save(settings);
        }
    }
}