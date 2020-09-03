// <copyright file="StatusServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public class StatusServiceModel
    {
        public StatusServiceModel(bool isHealthy, string message)
        {
            this.Status = new StatusResultServiceModel(isHealthy, message);
            this.Dependencies = new Dictionary<string, StatusResultServiceModel>();
            this.Properties = new StatusServicePropertiesModel();
        }

        [JsonProperty(PropertyName = "Status")]
        public StatusResultServiceModel Status { get; set; }

        [JsonProperty(PropertyName = "Properties")]
        public StatusServicePropertiesModel Properties { get; set; }

        [JsonProperty(PropertyName = "Dependencies")]
        public Dictionary<string, StatusResultServiceModel> Dependencies { get; set; }
    }
}