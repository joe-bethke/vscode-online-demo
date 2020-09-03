// <copyright file="DeploymentStatus.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.IoTHubManager.Services
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DeploymentStatus
    {
        Pending,
        Succeeded,
        Failed,
        Unknown,
    }
}