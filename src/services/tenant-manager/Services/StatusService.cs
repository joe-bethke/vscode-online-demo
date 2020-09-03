// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.Services.External.TableStorage;
using Mmm.Iot.TenantManager.Services.External;
using Mmm.Iot.TenantManager.Services.Helpers;

namespace Mmm.Iot.TenantManager.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            ILogger<StatusService> logger,
            IIdentityGatewayClient identityGatewayClient,
            IDeviceGroupsConfigClient deviceGroupsConfigClient,
            IStorageClient cosmosClient,
            ITableStorageClient tableStorageClient,
            IRunbookHelper runbookHelper,
            IAppConfigurationClient appConfigClient)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "CosmosDb", cosmosClient },
                { "Tenant Runbooks", runbookHelper },
                { "Table Storage", tableStorageClient },
                { "Identity Gateway", identityGatewayClient },
                { "Config", deviceGroupsConfigClient },
                { "App Config", appConfigClient },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}