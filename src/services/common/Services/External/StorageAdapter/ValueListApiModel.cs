// <copyright file="ValueListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.External.StorageAdapter
{
    public class ValueListApiModel
    {
        public IList<ValueApiModel> Items { get; set; }

        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}