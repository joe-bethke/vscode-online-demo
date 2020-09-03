// <copyright file="DeviceGroupDataModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.DeviceGroups
{
    public class DeviceGroupDataModel
    {
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("Conditions")]
        public IEnumerable<DeviceGroupConditionModel> Conditions { get; set; }
    }
}