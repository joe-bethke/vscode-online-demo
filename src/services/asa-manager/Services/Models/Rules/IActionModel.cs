// <copyright file="IActionModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.AsaManager.Services.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.AsaManager.Services.Models.Rules
{
    [JsonConverter(typeof(ActionConverter))]
    public interface IActionModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("Type")]
        ActionType Type { get; set; }

        // Dictionary should always be initialized as a case-insensitive dictionary
        [JsonProperty("Parameters")]
        IDictionary<string, object> Parameters { get; set; }
    }
}