// <copyright file="DeviceGroupListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mmm.Iot.Config.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.WebService.Models
{
    public class DeviceGroupListApiModel
    {
        public DeviceGroupListApiModel(IEnumerable<DeviceGroup> models)
        {
            this.Items = models.Select(m => new DeviceGroupApiModel(m));

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"DeviceGroupList;1" },
                { "$url", $"/v1/devicegroups" },
            };
        }

        public IEnumerable<DeviceGroupApiModel> Items { get; set; }

        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}