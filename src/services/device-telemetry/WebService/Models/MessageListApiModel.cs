// <copyright file="MessageListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services.External.TimeSeries;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class MessageListApiModel
    {
        private readonly List<MessageApiModel> items = new List<MessageApiModel>();
        private readonly List<string> properties = new List<string>();

        public MessageListApiModel(MessageList data)
        {
            if (data == null)
            {
                return;
            }

            foreach (Message message in data.Messages)
            {
                this.items.Add(new MessageApiModel(message));
            }

            foreach (string s in data.Properties)
            {
                this.properties.Add(s);
            }
        }

        [JsonProperty(PropertyName = "Items")]
        public List<MessageApiModel> Items
        {
            get { return this.items; }
        }

        [JsonProperty(PropertyName = "Properties")]
        public List<string> Properties
        {
            get { return this.properties; }
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public IDictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "MessageList;1" },
            { "$uri", "/" + "v1/messages" },
        };
    }
}