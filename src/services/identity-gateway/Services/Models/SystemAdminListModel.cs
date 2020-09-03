// <copyright file="SystemAdminListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class SystemAdminListModel
    {
        public SystemAdminListModel()
        {
        }

        public SystemAdminListModel(IEnumerable<SystemAdminModel> models)
        {
            this.Models = models;
        }

        public SystemAdminListModel(string batchMethod, IEnumerable<SystemAdminModel> models)
        {
            this.BatchMethod = batchMethod;
            this.Models = models;
        }

        [JsonProperty("Method", Order = 10)]
        public string BatchMethod { get; set; }

        [JsonProperty("Models", Order = 20)]
        public IEnumerable<SystemAdminModel> Models { get; set; }
    }
}