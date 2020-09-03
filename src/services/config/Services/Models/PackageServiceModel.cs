// <copyright file="PackageServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.Config.Services.Models
{
    public class PackageServiceModel : Audit
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("Type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType PackageType { get; set; }

        public string ConfigType { get; set; }

        public string Content { get; set; }

        public string DateCreated { get; set; }

        public string ETag { get; set; }

        public IList<string> Tags { get; set; }

        public string Version { get; set; }
    }
}