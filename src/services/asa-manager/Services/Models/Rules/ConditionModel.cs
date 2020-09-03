// <copyright file="ConditionModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.Rules
{
    public class ConditionModel
    {
        [JsonProperty("Field")]
        public string Field { get; set; }

        [JsonProperty("Operator")]
        public string Operator { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ConditionModel x))
            {
                return false;
            }

            return string.Equals(this.Field, x.Field)
                   && string.Equals(this.Operator, x.Operator)
                   && string.Equals(this.Value, x.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Field != null ? this.Field.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.Operator != null ? this.Operator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Value != null ? this.Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}