// <copyright file="DeleteTenantModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class DeleteTenantModel
    {
        public DeleteTenantModel()
        {
        }

        public DeleteTenantModel(string tenantGuid, Dictionary<string, bool> deletionRecord, bool ensuredDeployment)
        {
            this.TenantId = tenantGuid;
            this.EnsuredDeployment = ensuredDeployment;
            this.DeletionRecord = deletionRecord;
        }

        public bool FullyDeleted
        {
            get
            {
                return this.DeletionRecord.All(item => item.Value);
            }
        }

        public Dictionary<string, bool> DeletionRecord { get; set; }

        public bool EnsuredDeployment { get; set; }

        public string TenantId { get; set; }
    }
}