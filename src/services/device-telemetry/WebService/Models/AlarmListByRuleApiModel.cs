// <copyright file="AlarmListByRuleApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.DeviceTelemetry.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.DeviceTelemetry.WebService.Models
{
    public class AlarmListByRuleApiModel : AlarmListApiModel
    {
        public AlarmListByRuleApiModel(List<Alarm> alarms)
            : base(alarms)
        {
        }

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public new Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", $"AlarmsByRule;1" },
            { "$uri", "/" + "v1/alarmsbyrule" },
        };
    }
}