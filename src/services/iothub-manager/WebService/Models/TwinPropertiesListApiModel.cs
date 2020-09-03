// <copyright file="TwinPropertiesListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class TwinPropertiesListApiModel
    {
        public TwinPropertiesListApiModel()
        {
        }

        public TwinPropertiesListApiModel(TwinServiceListModel twins)
        {
            this.Items = new List<TwinPropertiesApiModel>();
            this.ContinuationToken = twins.ContinuationToken;
            foreach (var t in twins.Items)
            {
                this.Items.Add(new TwinPropertiesApiModel(
                    t.DesiredProperties,
                    t.ReportedProperties,
                    t.DeviceId,
                    t.ModuleId));
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
        public List<TwinPropertiesApiModel> Items { get; set; }
    }
}