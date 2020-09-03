// <copyright file="AlarmListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.DeviceTelemetry.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmListApiModel
    {
        public AlarmListApiModel(List<Alarm> alarms)
        {
            this.Items = new List<AlarmApiModel>();
            if (alarms != null)
            {
                foreach (Alarm alarm in alarms)
                {
                    this.Items.Add(new AlarmApiModel(alarm));
                }
            }
        }

        [JsonProperty(PropertyName = "Items")]
        public List<AlarmApiModel> Items { get; set; }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", $"Alarms;1" },
            { "$uri", "/" + "v1/alarms" },
        };
    }
}