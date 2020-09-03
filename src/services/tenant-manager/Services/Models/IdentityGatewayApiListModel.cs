// <copyright file="IdentityGatewayApiListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class IdentityGatewayApiListModel
    {
        public IdentityGatewayApiListModel()
        {
        }

        public IdentityGatewayApiListModel(List<IdentityGatewayApiModel> models)
        {
            this.Models = models;
        }

        public IdentityGatewayApiListModel(string batchMethod, List<IdentityGatewayApiModel> models)
        {
            this.BatchMethod = batchMethod;
            this.Models = models;
        }

        [JsonProperty("Method")]
        public string BatchMethod { get; set; }

        [JsonProperty("Models")]
        public List<IdentityGatewayApiModel> Models { get; set; }
    }
}