// <copyright file="DiagnosticsRequestModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.Services.External
{
    public class DiagnosticsRequestModel
    {
        [JsonProperty(PropertyName = "EventType", Order = 10)]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "EventProperties", Order = 20)]
        public Dictionary<string, object> EventProperties { get; set; }
    }
}