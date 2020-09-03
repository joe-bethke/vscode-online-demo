// <copyright file="EventSource.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.Common.Services.Models
{
    public enum EventSource
    {
        Config,
        DeviceTelemetry,
        IotHubManager,
        StorageAdapter,
        TenantManager,
    }
}