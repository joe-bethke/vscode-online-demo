// <copyright file="MessageApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Mmm.Iot.Common.Services.External.TimeSeries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class MessageApiModel
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
        private DateTimeOffset time;

        public MessageApiModel(
            string deviceId,
            DateTimeOffset time,
            JObject data)
        {
            this.DeviceId = deviceId;
            this.time = time;
            this.Data = data;
        }

        public MessageApiModel(Message message)
        {
            if (message != null)
            {
                this.DeviceId = message.DeviceId;
                this.time = message.Time;
                this.Data = message.Data;
            }
        }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "Time")]
        public string Time => this.time.ToString(DateFormat);

        [JsonProperty(PropertyName = "Data")]
        public JObject Data { get; set; }
    }
}