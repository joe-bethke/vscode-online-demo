// <copyright file="DevicePropertiesApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class DevicePropertiesApiModel
    {
        public DevicePropertiesApiModel()
        {
        }

        public DevicePropertiesApiModel(List<string> model)
        {
            this.Items = model;
            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"DevicePropertyList;1" },
                { "$url", $"/v1/deviceproperties" },
            };
        }

        [JsonProperty("Items")]
        public List<string> Items { get; set; }

        [JsonProperty(PropertyName = "$metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}