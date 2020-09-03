// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.AsaManager.Services.External.IotHubManager;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.BlobStorage;
using Mmm.Iot.Common.Services.External.StorageAdapter;

namespace Mmm.Iot.AsaManager.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            IIotHubManagerClient iotHubManager,
            IBlobStorageClient blobStorageClient,
            IStorageAdapterClient storageAdapterClient)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "Storage Adapter", storageAdapterClient },
                { "IoTHub Manager", iotHubManager },
                { "Blob Storage", blobStorageClient },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}