// <copyright file="AlarmStatusApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmStatusApiModel
    {
        public AlarmStatusApiModel()
        {
            this.Status = null;
        }

        public AlarmStatusApiModel(string status)
        {
            this.Status = status;
        }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
    }
}