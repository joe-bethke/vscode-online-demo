// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.TableStorage;
using Mmm.Iot.IdentityGateway.Services.External;

namespace Mmm.Iot.IdentityGateway.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config,
            ITableStorageClient tableStorage,
            IAzureB2cClient b2cClient)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
                { "Table Storage", tableStorage },
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}