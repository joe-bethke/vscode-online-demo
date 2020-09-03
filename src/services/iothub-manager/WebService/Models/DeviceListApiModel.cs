// <copyright file="DeviceListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class DeviceListApiModel
    {
        public DeviceListApiModel()
        {
        }

        public DeviceListApiModel(DeviceServiceListModel devices)
        {
            this.Items = new List<DeviceRegistryApiModel>();
            this.ContinuationToken = devices.ContinuationToken;
            foreach (var d in devices.Items)
            {
                this.Items.Add(new DeviceRegistryApiModel(d));
            }
        }

        [JsonProperty(PropertyName = "$metadata")]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "DeviceList;1" },
            { "$uri", "/" + "v1/devices" },
        };

        [JsonProperty(PropertyName = "ContinuationToken")]
        public string ContinuationToken { get; set; }

        [JsonProperty(PropertyName = "Items")]
        public List<DeviceRegistryApiModel> Items { get; set; }
    }
}