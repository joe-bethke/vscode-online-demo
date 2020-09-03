// <copyright file="Condition.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.AsaManager.Services.Models.Rules
{
    public partial class RuleReferenceDataModel
    {
        private struct Condition
        {
            internal string Calculation { get; set; }

            internal string Field { get; set; }

            internal string Operator { get; set; }

            internal string Value { get; set; }
        }
    }
}