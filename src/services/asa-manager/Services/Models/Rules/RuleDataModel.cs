// <copyright file="RuleDataModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.Rules
{
    // see https://github.com/Azure/device-telemetry-dotnet/blob/master/WebService/v1/Models/RuleModel.cs
    public class RuleDataModel
    {
        public RuleDataModel()
        {
            this.Conditions = new List<ConditionModel>();
            this.Actions = new List<IActionModel>();
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("GroupId")]
        public string GroupId { get; set; }

        [JsonProperty("Severity")]
        public string Severity { get; set; }

        [JsonProperty("Conditions")]
        public IList<ConditionModel> Conditions { get; set; }

        [JsonProperty("Calculation")]
        public string Calculation { get; set; }

        [JsonProperty("TimePeriod")]
        public long TimePeriod { get; set; }

        [JsonProperty(PropertyName = "Actions")]
        public List<IActionModel> Actions { get; set; }

        [JsonProperty("Deleted")]
        public bool Deleted { get; set; }
    }
}