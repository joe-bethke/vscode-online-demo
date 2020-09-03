// <copyright file="UserListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class UserListModel
    {
        public UserListModel()
        {
        }

        public UserListModel(List<UserModel> models)
        {
            this.Models = models;
        }

        public UserListModel(string batchMethod, List<UserModel> models)
        {
            this.BatchMethod = batchMethod;
            this.Models = models;
        }

        [JsonProperty("Method", Order = 10)]
        public string BatchMethod { get; set; }

        [JsonProperty("Models", Order = 20)]
        public List<UserModel> Models { get; set; }
    }
}