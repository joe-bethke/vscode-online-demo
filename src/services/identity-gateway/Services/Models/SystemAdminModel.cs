// <copyright file="SystemAdminModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Azure.Cosmos.Table;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class SystemAdminModel : TableEntity
    {
        public SystemAdminModel()
        {
        }

        public SystemAdminModel(SystemAdminInput systemAdminInput)
        {
            if (systemAdminInput != null)
            {
                this.PartitionKey = systemAdminInput.UserId;
                this.RowKey = systemAdminInput.UserId;
                this.Name = systemAdminInput.Name;
            }
        }

        public SystemAdminModel(string userId, string name)
        {
            this.PartitionKey = userId;
            this.RowKey = userId;
            this.Name = name;
        }

        public SystemAdminModel(DynamicTableEntity tableEntity)
        {
            if (tableEntity != null)
            {
                this.PartitionKey = tableEntity.PartitionKey;
                this.RowKey = tableEntity.RowKey;
                this.Name = tableEntity.Properties["Name"]?.StringValue;
            }
        }

        public string Name { get; set; }

        public static explicit operator SystemAdminModel(DynamicTableEntity v) => new SystemAdminModel(v);
    }
}