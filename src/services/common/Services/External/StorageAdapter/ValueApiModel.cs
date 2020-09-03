// <copyright file="ValueApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.External.StorageAdapter
{
    public class ValueApiModel
    {
        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("ETag")]
        public string ETag { get; set; }
    }
}