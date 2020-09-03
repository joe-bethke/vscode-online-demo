// <copyright file="StatusResultServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public class StatusResultServiceModel
    {
        [JsonConstructor]
        public StatusResultServiceModel(bool isHealthy, string message)
        {
            this.IsHealthy = isHealthy;
            this.Message = message;
        }

        [JsonProperty(PropertyName = "IsHealthy")]
        public bool IsHealthy { get; set; }

        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }
    }
}