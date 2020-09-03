// <copyright file="PackageApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public class PackageApiModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("PackageType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType PackageType { get; set; }

        [JsonProperty("ConfigType")]
        public string ConfigType { get; set; }

        [JsonProperty(PropertyName = "DateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Tags")]
        public IList<string> Tags { get; set; }
    }
}