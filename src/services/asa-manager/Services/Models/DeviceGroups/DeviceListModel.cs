// <copyright file="DeviceListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.DeviceGroups
{
    public class DeviceListModel
    {
        [JsonProperty("Items")]
        public IEnumerable<DeviceModel> Items { get; set; }

        public string ContinuationToken { get; set; }
    }
}