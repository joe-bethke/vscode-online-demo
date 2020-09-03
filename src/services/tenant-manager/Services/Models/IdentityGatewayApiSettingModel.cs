// <copyright file="IdentityGatewayApiSettingModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class IdentityGatewayApiSettingModel
    {
        public string UserId { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string Value { get; set; }

        public string SettingKey { get; set; }
    }
}