// <copyright file="DeviceModelRef.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.Config.Services.External
{
    public class DeviceModelRef
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Count")]
        public int Count { get; set; }
    }
}