// <copyright file="CreateTenantModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class CreateTenantModel
    {
        public CreateTenantModel()
        {
        }

        public CreateTenantModel(string tenantGuid)
        {
            this.TenantId = tenantGuid;
            this.Message = $"Your tenant is currently being deployed. This may take several minutes. You can check if your tenant is fully deployed using GET /api/tenantready";
        }

        public string TenantId { get; set; }

        public string Message { get; set; }
    }
}