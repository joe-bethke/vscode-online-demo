// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.AsaManager;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.Common.Services.External.TimeSeries;
using Mmm.Iot.DeviceTelemetry.Services.External;

namespace Mmm.Iot.DeviceTelemetry.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            IStorageClient storageClient,
            ITimeSeriesClient timeSeriesClient,
            IAsaManagerClient asaManager,
            IStorageAdapterClient storageAdapter,
            IAppConfigurationClient appConfig)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "Storage Adapter", storageAdapter },
                { "Storage", storageClient },
                { "Asa Manager", asaManager },
                { "Time Series", timeSeriesClient },
                { "App Config", appConfig },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}