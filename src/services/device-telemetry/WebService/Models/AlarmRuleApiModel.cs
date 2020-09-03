// <copyright file="AlarmRuleApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmRuleApiModel
    {
        public AlarmRuleApiModel(
            string id,
            string severity,
            string description)
        {
            this.Id = id;
            this.Severity = severity;
            this.Description = description;

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"Rule;1" },
                { "$uri", "/" + "v1/rules/" + id },
            };
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Severity")]
        public string Severity { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }
}