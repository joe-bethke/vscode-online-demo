// <copyright file="AlarmByRuleApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.DeviceTelemetry.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmByRuleApiModel
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
        private DateTimeOffset created;
        private int count;

        public AlarmByRuleApiModel(
            int count,
            string status,
            DateTimeOffset created,
            Alarm alarm)
        {
            this.Count = count;
            this.Status = status;
            this.created = created;
            this.Rule = new AlarmRuleApiModel(
                alarm.RuleId,
                alarm.RuleSeverity,
                alarm.RuleDescription);

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"AlarmByRule;1" },
                { "$uri", "/" + "v1/alarmsbyrule/" + this.Rule.Id },
            };
        }

        public AlarmByRuleApiModel(
            int count,
            string status,
            DateTimeOffset created,
            AlarmRuleApiModel rule)
        {
            this.count = count;
            this.Status = status;
            this.created = created;
            this.Rule = rule;

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"AlarmByRule;1" },
                { "$uri", "/" + "v1/alarmsbyrule/" + this.Rule.Id },
            };
        }

        [JsonProperty(PropertyName = "Count")]
        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "Created")]
        public string Created
        {
            get { return this.created.ToString(DateFormat); }
            set { this.created = DateTimeOffset.Parse(value); }
        }

        [JsonProperty(PropertyName = "Rule")]
        public AlarmRuleApiModel Rule { get; set; }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata { get; set; }
    }
}