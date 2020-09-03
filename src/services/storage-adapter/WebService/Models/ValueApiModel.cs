// <copyright file="ValueApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Globalization;
using Mmm.Iot.StorageAdapter.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.StorageAdapter.WebService.Models
{
    public class ValueApiModel
    {
        public ValueApiModel(ValueServiceModel model)
        {
            this.Key = model.Key;
            this.Data = model.Data;
            this.ETag = model.ETag;

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"Value;1" },
                { "$modified", model.Timestamp.ToString(CultureInfo.InvariantCulture) },
                { "$uri", $"/v1/collections/{model.CollectionId}/values/{model.Key}" },
            };
        }

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