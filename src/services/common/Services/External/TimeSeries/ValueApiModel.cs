// <copyright file="ValueApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.External.TimeSeries
{
    public class ValueApiModel
    {
        [JsonProperty("schemaRid")]
        public long? SchemaRowId { get; set; }

        [JsonProperty("schema")]
        public SchemaModel Schema { get; set; }

        [JsonProperty("$ts")]
        public string Timestamp { get; set; }

        [JsonProperty("values")]
        public List<JValue> Values { get; set; }
    }
}