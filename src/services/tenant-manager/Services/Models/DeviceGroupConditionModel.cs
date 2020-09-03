// <copyright file="DeviceGroupConditionModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class DeviceGroupConditionModel
    {
        public string Field { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }
    }
}