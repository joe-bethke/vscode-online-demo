// <copyright file="JobUpdateTwinApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class JobUpdateTwinApiModel
    {
        public JobUpdateTwinApiModel()
        {
            this.Tags = new Dictionary<string, JToken>();
            this.Properties = new TwinPropertiesApiModel();
        }

        public JobUpdateTwinApiModel(string deviceId, TwinServiceModel deviceTwin)
        {
            if (deviceTwin != null)
            {
                this.ETag = deviceTwin.ETag;
                this.DeviceId = deviceId;
                this.Properties = new TwinPropertiesApiModel(deviceTwin.DesiredProperties, deviceTwin.ReportedProperties);
                this.Tags = deviceTwin.Tags;
                this.IsSimulated = deviceTwin.IsSimulated;
            }
        }

        [JsonProperty(PropertyName = "ETag")]
        public string ETag { get; set; }

        [JsonProperty(PropertyName = "DeviceId")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "Properties")]
        public TwinPropertiesApiModel Properties { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public Dictionary<string, JToken> Tags { get; set; }

        [JsonProperty(PropertyName = "IsSimulated")]
        public bool IsSimulated { get; set; }

        public TwinServiceModel ToServiceModel()
        {
            return new TwinServiceModel(
                etag: this.ETag,
                deviceId: this.DeviceId,
                desiredProperties: this.Properties.Desired,
                reportedProperties: this.Properties.Reported,
                tags: this.Tags,
                isSimulated: this.IsSimulated);
        }
    }
}