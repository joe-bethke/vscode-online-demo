// <copyright file="AlarmIdListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmIdListApiModel
    {
        public AlarmIdListApiModel()
        {
            this.Items = null;
        }

        public AlarmIdListApiModel(List<string> items)
        {
            this.Items = items;
        }

        [JsonProperty(PropertyName = "Items")]
        public List<string> Items { get; set; }
    }
}