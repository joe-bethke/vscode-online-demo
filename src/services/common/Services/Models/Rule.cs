// <copyright file="Rule.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.Common.Services.Models
{
    public class Rule : IComparable<Rule>
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

        public Rule()
        {
        }

        // Comes from the StorageAdapter document and not the serialized rule
        [JsonIgnore]
        public string ETag { get; set; } = string.Empty;

        // Comes from the StorageAdapter document and not the serialized rule
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string DateCreated { get; set; } = DateTimeOffset.UtcNow.ToString(DateFormat);

        public string DateModified { get; set; } = DateTimeOffset.UtcNow.ToString(DateFormat);

        public bool Enabled { get; set; } = false;

        public string Description { get; set; } = string.Empty;

        public string GroupId { get; set; } = string.Empty;

        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityType Severity { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CalculationType Calculation { get; set; }

        // Possible values -[60000, 300000, 600000] in milliseconds
        public long TimePeriod { get; set; } = 0;

        public IList<Condition> Conditions { get; set; } = new List<Condition>();

        public IList<IAction> Actions { get; set; } = new List<IAction>();

        public bool Deleted { get; set; } = false;

        public int CompareTo(Rule other)
        {
            if (other == null)
            {
                return 1;
            }

            return DateTimeOffset.Parse(other.DateCreated)
                .CompareTo(DateTimeOffset.Parse(this.DateCreated));
        }

        public void Validate()
        {
            InputValidator.Validate(this.Id);
        }
    }
}