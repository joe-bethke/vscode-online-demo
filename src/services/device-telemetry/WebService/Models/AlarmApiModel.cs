// <copyright file="AlarmApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.DeviceTelemetry.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmApiModel
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
        private DateTimeOffset dateCreated;
        private DateTimeOffset dateModified;

        public AlarmApiModel(Alarm alarm)
        {
            if (alarm != null)
            {
                this.ETag = alarm.ETag;
                this.Id = alarm.Id;
                this.dateCreated = alarm.DateCreated;
                this.dateModified = alarm.DateModified;
                this.Description = alarm.Description;
                this.GroupId = alarm.GroupId;
                this.DeviceId = alarm.DeviceId;
                this.Status = alarm.Status;
                this.Rule = new AlarmRuleApiModel(
                    alarm.RuleId,
                    alarm.RuleSeverity,
                    alarm.RuleDescription);

                this.Metadata = new Dictionary<string, string>
                {
                    { "$type", $"Alarm;1" },
                    { "$uri", "/" + "v1/alarms/" + this.Id },
                };
            }
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty(PropertyName = "ETag")]
        public string ETag { get; set; }

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "DateCreated")]
        public string DateCreated => this.dateCreated.ToString(DateFormat);

        [JsonProperty(PropertyName = "DateModified")]
        public string DateModified => this.dateModified.ToString(DateFormat);

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "GroupId")]
        public string GroupId { get; set; }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "Rule")]
        public AlarmRuleApiModel Rule { get; set; }
    }
}