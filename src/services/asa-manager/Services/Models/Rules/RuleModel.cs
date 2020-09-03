// <copyright file="RuleModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.Rules
{
    // see https://github.com/Azure/device-telemetry-dotnet/blob/master/WebService/v1/Models/RuleModel.cs
    public class RuleModel
    {
        public RuleModel()
        {
            this.Conditions = new List<ConditionModel>();
            this.Actions = new List<IActionModel>();
        }

        public RuleModel(string id, RuleDataModel data)
        {
            this.Id = id;
            this.Name = data.Name;
            this.Enabled = data.Enabled;
            this.Description = data.Description;
            this.GroupId = data.GroupId;
            this.Severity = data.Severity;
            this.Conditions = data.Conditions;
            this.Calculation = data.Calculation;
            this.Actions = data.Actions;
            this.Deleted = data.Deleted;
            this.TimePeriod = data.TimePeriod;
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

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

        public override bool Equals(object obj)
        {
            if (!(obj is RuleModel x))
            {
                return false;
            }

            if (this.Conditions.Count != x.Conditions.Count
                || this.Actions.Count != x.Actions.Count)
            {
                return false;
            }

            if (this.Conditions.Except(x.Conditions).Any())
            {
                return false;
            }

            for (int i = 0; i < this.Actions.Count; i++)
            {
                {
                    if (!this.Actions[i].Equals(x.Actions[i]))
                    {
                        return false;
                    }
                }
            }

            // Compare all other parameters if conditions and actions are equal
            return string.Equals(this.Id, x.Id)
                   && string.Equals(this.Name, x.Name)
                   && string.Equals(this.Description, x.Description)
                   && this.Enabled == x.Enabled
                   && string.Equals(this.GroupId, x.GroupId)
                   && string.Equals(this.Severity, x.Severity)
                   && this.Deleted == x.Deleted;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id != null ? this.Id.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Deleted.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.GroupId != null ? this.GroupId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Severity != null ? this.Severity.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Conditions != null ? this.Conditions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Actions != null ? this.Actions.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}