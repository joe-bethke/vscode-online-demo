// <copyright file="UserSettingsListModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class UserSettingsListModel
    {
        public UserSettingsListModel()
        {
        }

        public UserSettingsListModel(List<UserSettingsModel> models)
        {
            this.Models = models;
        }

        public UserSettingsListModel(string batchMethod, List<UserSettingsModel> models)
        {
            this.BatchMethod = batchMethod;
            this.Models = models;
        }

        [JsonProperty("Method", Order=10)]
        public string BatchMethod { get; set; }

        [JsonProperty("Models", Order=20)]
        public List<UserSettingsModel> Models { get; set; }
    }
}