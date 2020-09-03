// <copyright file="DeviceJobApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class DeviceJobApiModel
    {
        public DeviceJobApiModel()
        {
        }

        public DeviceJobApiModel(DeviceJobServiceModel serviceModel)
        {
            this.DeviceId = serviceModel.DeviceId;
            this.Status = serviceModel.Status;
            this.StartTimeUtc = serviceModel.StartTimeUtc;
            this.EndTimeUtc = serviceModel.EndTimeUtc;
            this.CreatedDateTimeUtc = serviceModel.CreatedDateTimeUtc;
            this.LastUpdatedDateTimeUtc = serviceModel.LastUpdatedDateTimeUtc;

            if (serviceModel.Outcome != null)
            {
                this.Outcome = new MethodResultApiModel(serviceModel.Outcome);
            }

            if (serviceModel.Error != null)
            {
                this.Error = new DeviceJobErrorApiModel(serviceModel.Error);
            }
        }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "Status")]
        public DeviceJobStatus Status { get; set; }

        [JsonProperty(PropertyName = "StartTimeUtc")]
        public DateTime StartTimeUtc { get; set; }

        [JsonProperty(PropertyName = "EndTimeUtc")]
        public DateTime EndTimeUtc { get; set; }

        [JsonProperty(PropertyName = "CreatedDateTimeUtc")]
        public DateTime CreatedDateTimeUtc { get; set; }

        [JsonProperty(PropertyName = "LastUpdatedDateTimeUtc")]
        public DateTime LastUpdatedDateTimeUtc { get; set; }

        [JsonProperty(PropertyName = "Outcome", NullValueHandling = NullValueHandling.Ignore)]
        public MethodResultApiModel Outcome { get; set; }

        [JsonProperty(PropertyName = "Error", NullValueHandling = NullValueHandling.Ignore)]
        public DeviceJobErrorApiModel Error { get; set; }
    }
}