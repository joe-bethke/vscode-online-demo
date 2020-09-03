// <copyright file="RuleReferenceDataModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.AsaManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.AsaManager.Services.Models.Rules
{
    // Note: all the constants below are meant to be not case sensitive
    public partial class RuleReferenceDataModel
    {
        private const string AsaInstantValue = "";
        private const string AsaAverageValue = ".avg";
        private const string AsaMinimumValue = ".min";
        private const string AsaMaximumValue = ".max";
        private const string AsaCountValue = ".count";

        // Value used by the Rules web service to indicate that
        // a rule doesn't use aggregation and has no time window.
        // See https://github.com/Azure/device-telemetry-dotnet/blob/master/Services/Models/Rule.cs
        private const string SourceNoAggregation = "instant";

        // Used to tell ASA TSQL which aggregation to use
        private const string AsaAggregationNone = "instant";
        private const string AsaAggregationWindowTumbling1Minute = "tumblingwindow1minutes";
        private const string AsaAggregationWindowTumbling5Minutes = "tumblingwindow5minutes";
        private const string AsaAggregationWindowTumbling10Minutes = "tumblingwindow10minutes";
        private const string AsaAggregationWindowTumbling20Minutes = "tumblingwindow20minutes";
        private const string AsaAggregationWindowTumbling30Minutes = "tumblingwindow30minutes";
        private const string AsaAggregationWindowTumbling1Hour = "tumblingwindow1hours";

        // Map from values used in the Device Telemetry web service
        // to the corresponding ASA constant. Some extra values added
        // in case we want to add more options.
        // See https://github.com/Azure/device-telemetry-dotnet/blob/master/Services/Models/Rule.cs
        private static readonly Dictionary<long, string> TimePeriodMap =
            new Dictionary<long, string>
            {
                { 60000, AsaAggregationWindowTumbling1Minute },
                { 300000, AsaAggregationWindowTumbling5Minutes },
                { 600000, AsaAggregationWindowTumbling10Minutes },
                { 1200000, AsaAggregationWindowTumbling20Minutes },
                { 1800000, AsaAggregationWindowTumbling30Minutes },
                { 3600000, AsaAggregationWindowTumbling1Hour },
            };

        // Map from values used in the Device Telemetry web service to the
        // corresponding Javascript field. Some extra values added
        // in case we want to add more options.
        // See https://github.com/Azure/device-telemetry-dotnet/blob/master/Services/Models/Rule.cs
        private static readonly Dictionary<string, string> JsFieldsMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { SourceNoAggregation, AsaInstantValue },
                { "avg", AsaAverageValue },
                { "average", AsaAverageValue },
                { "min", AsaMinimumValue },
                { "minimum", AsaMinimumValue },
                { "max", AsaMaximumValue },
                { "maximum", AsaMaximumValue },
                { "count", AsaCountValue },
            };

        // Map from values used in the Device Telemetry web service to the
        // corresponding Javascript symbols.
        // For flexibility, the keys are case insensitive, and also symbol-to-symbol mapping is supported.
        // See also https://github.com/Azure/device-telemetry-dotnet/blob/master/Services/Models/Condition.cs
        private static readonly Dictionary<string, string> OperatorsMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "GreaterThan", ">" },
                { ">", ">" },
                { "GreaterThanOrEqual", ">=" },
                { ">=", ">=" },
                { "LessThan", "<" },
                { "<", "<" },
                { "LessThanOrEqual", "<=" },
                { "<=", "<=" },
                { "Equals", "=" },
                { "=", "=" },
                { "==", "=" },
            };

        // Internal data structure needed to serialize the model to JSON
        private readonly List<Condition> conditions;

        public RuleReferenceDataModel()
        {
            this.conditions = new List<Condition>();
            this.Fields = new List<string>();
        }

        public RuleReferenceDataModel(RuleModel rule)
            : this()
        {
            if (!rule.Enabled || rule.Deleted)
            {
                return;
            }

            this.Id = rule.Id;
            this.Name = rule.Name;
            this.Description = rule.Description;
            this.GroupId = rule.GroupId;
            this.Severity = rule.Severity;
            this.AggregationWindow = GetAggregationWindowValue(rule.Calculation, rule.TimePeriod);

            this.Fields = new List<string>();
            this.conditions = new List<Condition>();
            foreach (var c in rule.Conditions)
            {
                var condition = new Condition
                {
                    Calculation = rule.Calculation,
                    Field = c.Field,
                    Operator = c.Operator,
                    Value = c.Value,
                };
                this.conditions.Add(condition);
                this.Fields.Add(c.Field);
            }

            if (rule.Actions != null && rule.Actions.Count >= 0)
            {
                this.Actions = rule.Actions;
            }
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("GroupId")]
        public string GroupId { get; set; }

        [JsonProperty("Severity")]
        public string Severity { get; set; }

        [JsonProperty("AggregationWindow")]
        public string AggregationWindow { get; set; }

        [JsonProperty("Fields")]
        public List<string> Fields { get; set; }

        [JsonProperty("Actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<IActionModel> Actions { get; set; }

        [JsonProperty("__rulefilterjs")]
        public string RuleFilterJs => this.ConditionsToJavascript();

        private static string GetFieldName(string field, string calculation)
        {
            // Do not remove. This would be a bug, to be detected at development time.
            if (!JsFieldsMap.ContainsKey(calculation))
            {
                throw new ApplicationException("Unknown calculation: " + calculation);
            }

            return field + JsFieldsMap[calculation];
        }

        private static string GetJsOperator(string op)
        {
            if (OperatorsMap.ContainsKey(op))
            {
                return OperatorsMap[op];
            }

            // This is an overall bug in the solution, to be detected at development time
            throw new ApplicationException("Unknown operator: " + op);
        }

        private static string GetAggregationWindowValue(string calculation, long timePeriod)
        {
            if (string.IsNullOrEmpty(calculation))
            {
                return null;
            }

            if (calculation.ToLowerInvariant() == SourceNoAggregation)
            {
                return AsaAggregationNone;
            }

            // Do not remove. This would be a bug, to be detected at development time.
            if (!TimePeriodMap.ContainsKey(timePeriod))
            {
                throw new ApplicationException("Unknown time period: " + timePeriod);
            }

            return TimePeriodMap[timePeriod];
        }

        private string ConditionsToJavascript()
        {
            if (this.conditions.Count == 0)
            {
                // A rule without conditions will always match
                return "return true;";
            }

            var result = string.Empty;
            foreach (var c in this.conditions)
            {
                // Concatenate conditions with AND
                if (!string.IsNullOrEmpty(result))
                {
                    result += " && ";
                }

                result += $"record.__aggregates." + GetFieldName(c.Field, c.Calculation);
                result += " " + GetJsOperator(c.Operator) + " ";
                result += c.Value;
            }

            return $"return ({result}) ? true : false;";
        }
    }
}