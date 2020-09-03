// <copyright file="DeviceGroupListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.DeviceGroups
{
    public class DeviceGroupListModel
    {
        [JsonProperty("Items")]
        public IEnumerable<DeviceGroupModel> Items { get; set; }
    }
}