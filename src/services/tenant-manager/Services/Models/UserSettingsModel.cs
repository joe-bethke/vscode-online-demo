// <copyright file="UserSettingsModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Cosmos.Table;

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class UserSettingsModel : TableEntity
    {
        public UserSettingsModel()
        {
        }

        public UserSettingsModel(string userId, string settingKey, string value)
        {
            this.PartitionKey = userId;
            this.RowKey = settingKey;
            this.Value = value;
        }

        public string Value { get; set; }

        // Define aliases for the partition and row keys
        public string UserId
        {
            get
            {
                return this.PartitionKey;
            }
        }

        public string SettingKey
        {
            get
            {
                return this.RowKey;
            }
        }
    }
}