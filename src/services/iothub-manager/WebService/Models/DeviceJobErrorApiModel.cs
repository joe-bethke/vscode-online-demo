// <copyright file="DeviceJobErrorApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class DeviceJobErrorApiModel
    {
        public DeviceJobErrorApiModel()
        {
        }

        public DeviceJobErrorApiModel(DeviceJobErrorServiceModel error)
        {
            this.Code = error.Code;
            this.Description = error.Description;
        }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}