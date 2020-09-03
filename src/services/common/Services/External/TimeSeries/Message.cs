// <copyright file="Message.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.External.TimeSeries
{
    public class Message
    {
        public Message()
        {
            this.DeviceId = string.Empty;
            this.Time = DateTimeOffset.UtcNow;
            this.Data = null;
        }

        public Message(
            string deviceId,
            long time,
            JObject data)
        {
            this.DeviceId = deviceId;
            this.Time = DateTimeOffset.FromUnixTimeMilliseconds(time);
            this.Data = data;
        }

        public string DeviceId { get; set; }

        public DateTimeOffset Time { get; set; }

        public JObject Data { get; set; }
    }
}