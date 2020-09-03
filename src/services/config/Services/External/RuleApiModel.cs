// <copyright file="RuleApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.Services.External
{
    public class RuleApiModel
    {
        [JsonProperty(PropertyName = "ETag")]
        public string ETag { get; set; }

        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "GroupId")]
        public string GroupId { get; set; }

        [JsonProperty(PropertyName = "Severity")]
        public string Severity { get; set; }

        [JsonProperty(PropertyName = "Conditions")]
        public List<ConditionApiModel> Conditions { get; set; }

        // Possible values -["Average", "Instant"]
        [JsonProperty(PropertyName = "Calculation")]
        public string Calculation { get; set; }

        // Possible values -["60000", "300000", "600000"] in milliseconds
        [JsonProperty(PropertyName = "TimePeriod")]
        public string TimePeriod { get; set; }
    }
}