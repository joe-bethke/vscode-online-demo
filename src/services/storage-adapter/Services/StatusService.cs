// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.CosmosDb;

namespace Mmm.Iot.StorageAdapter.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            IStorageClient cosmos,
            IAppConfigurationClient appConfig)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "CosmosDb", cosmos },
                { "App Config", appConfig },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}