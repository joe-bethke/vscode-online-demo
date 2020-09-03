// <copyright file="StatusApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Common.Services.Runtime;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public sealed class StatusApiModel
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

        public StatusApiModel(StatusServiceModel model, string name)
        {
            this.Status = new StatusResultApiModel(model.Status);
            this.Dependencies = new Dictionary<string, StatusResultApiModel>();
            this.Name = name;
            foreach (KeyValuePair<string, StatusResultServiceModel> pair in model.Dependencies)
            {
                this.Dependencies.Add(pair.Key, new StatusResultApiModel(pair.Value));
            }

            this.Properties = model.Properties;
        }

        [JsonProperty(PropertyName = "Name", Order = 10)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Status", Order = 20)]
        public StatusResultApiModel Status { get; set; }

        [JsonProperty(PropertyName = "CurrentTime", Order = 30)]
        public string CurrentTime => DateTimeOffset.UtcNow.ToString(DateFormat);

        [JsonProperty(PropertyName = "StartTime", Order = 40)]
        public string StartTime => Uptime.Start.ToString(DateFormat);

        [JsonProperty(PropertyName = "UpTime", Order = 50)]
        public long UpTime => Convert.ToInt64(Uptime.Duration.TotalSeconds);

        [JsonProperty(PropertyName = "UID", Order = 60)]
        public string UID => Uptime.ProcessId;

        [JsonProperty(PropertyName = "Properties", Order = 70)]
        public StatusServicePropertiesModel Properties { get; set; }

        [JsonProperty(PropertyName = "Dependencies", Order = 80)]
        public Dictionary<string, StatusResultApiModel> Dependencies { get; set; }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "Status;" + "0" },
            { "$uri", "/status" },
        };
    }
}