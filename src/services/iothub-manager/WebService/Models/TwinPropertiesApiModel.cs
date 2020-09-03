// <copyright file="TwinPropertiesApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class TwinPropertiesApiModel
    {
        public TwinPropertiesApiModel()
        {
            this.Reported = new Dictionary<string, JToken>();
            this.Desired = new Dictionary<string, JToken>();
        }

        public TwinPropertiesApiModel(Dictionary<string, JToken> desired, Dictionary<string, JToken> reported)
            : this(desired, reported, string.Empty, string.Empty)
        {
        }

        public TwinPropertiesApiModel(
            Dictionary<string, JToken> desired,
            Dictionary<string, JToken> reported,
            string deviceId,
            string moduleId)
        {
            this.Desired = desired;
            this.Reported = reported;
            this.DeviceId = deviceId;
            this.ModuleId = moduleId;
        }

        [JsonProperty(PropertyName = "Reported", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JToken> Reported { get; set; }

        [JsonProperty(PropertyName = "Desired", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JToken> Desired { get; set; }

        [JsonProperty(PropertyName = "DeviceId", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "ModuleId", NullValueHandling = NullValueHandling.Ignore)]
        public string ModuleId { get; set; }
    }
}