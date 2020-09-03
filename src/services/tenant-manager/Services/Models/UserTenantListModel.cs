// <copyright file="UserTenantListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class UserTenantListModel
    {
        public UserTenantListModel()
        {
        }

        public UserTenantListModel(List<UserTenantModel> models)
        {
            this.Models = models;
        }

        public UserTenantListModel(string batchMethod, List<UserTenantModel> models)
        {
            this.BatchMethod = batchMethod;
            this.Models = models;
        }

        public UserTenantListModel(IdentityGatewayApiListModel identityGatewayApiListModel)
        {
            this.BatchMethod = identityGatewayApiListModel.BatchMethod;
            this.Models = identityGatewayApiListModel.Models.Select(m => new UserTenantModel(m)).ToList();
        }

        [JsonProperty("Method")]
        public string BatchMethod { get; set; }

        [JsonProperty("Models")]
        public List<UserTenantModel> Models { get; set; }
    }
}