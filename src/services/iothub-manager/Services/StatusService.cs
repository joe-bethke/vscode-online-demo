// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.Common.Services.External.UserManagement;
using Mmm.Iot.IoTHubManager.Services.External;

namespace Mmm.Iot.IoTHubManager.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            IStorageAdapterClient storageAdapter,
            IUserManagementClient userManagement,
            IAppConfigurationClient appConfig)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                 { "Storage Adapter", storageAdapter },
                 { "User Management", userManagement },
                 { "App Config", appConfig },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}