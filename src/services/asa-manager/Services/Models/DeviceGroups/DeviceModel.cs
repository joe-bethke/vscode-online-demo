// <copyright file="DeviceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.DeviceGroups
{
    public class DeviceModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}