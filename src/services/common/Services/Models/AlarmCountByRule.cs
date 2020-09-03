// <copyright file="AlarmCountByRule.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Models
{
    public class AlarmCountByRule
    {
        public AlarmCountByRule(
            int count,
            string status,
            DateTimeOffset messageTime,
            Rule rule)
        {
            this.Count = count;
            this.Status = status;
            this.MessageTime = messageTime;
            this.Rule = rule;
        }

        public int Count { get; set; }

        public string Status { get; set; }

        public DateTimeOffset MessageTime { get; set; }

        public Rule Rule { get; set; }
    }
}