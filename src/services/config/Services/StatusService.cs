// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.AsaManager;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.Config.Services.External;

namespace Mmm.Iot.Config.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            IAsaManagerClient asaManager,
            IStorageAdapterClient storageAdapter,
            IDeviceTelemetryClient deviceTelemetry)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "Storage Adapter", storageAdapter },
                { "Device Telemetry", deviceTelemetry },
                { "Asa Manager", asaManager },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}