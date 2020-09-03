// <copyright file="StatusResultApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public class StatusResultApiModel
    {
        public StatusResultApiModel(StatusResultServiceModel serviceModel)
        {
            this.IsHealthy = serviceModel.IsHealthy;
            this.Message = serviceModel.Message;
        }

        [JsonProperty(PropertyName = "IsHealthy", Order = 10)]
        public bool IsHealthy { get; set; }

        [JsonProperty(PropertyName = "Message", Order = 20)]
        public string Message { get; set; }
    }
}