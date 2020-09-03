// <copyright file="DeviceGroupCondition.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Common.Services.Models
{
    public class DeviceGroupCondition
    {
        public string Key { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }
    }
}