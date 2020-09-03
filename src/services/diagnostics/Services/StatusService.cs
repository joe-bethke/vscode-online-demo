// <copyright file="StatusService.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;

namespace Mmm.Iot.Diagnostics.Services
{
    public class StatusService : StatusServiceBase
    {
        public StatusService(
            AppConfig config)
                : base(config)
        {
            this.Dependencies = new Dictionary<string, IStatusOperation>
            {
            };
        }

        public override IDictionary<string, IStatusOperation> Dependencies { get; set; }
    }
}