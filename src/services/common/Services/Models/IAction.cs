// <copyright file="IAction.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Common.Services.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.Common.Services.Models
{
    [JsonConverter(typeof(ActionConverter))]
    public interface IAction
    {
        [JsonConverter(typeof(StringEnumConverter))]
        ActionType Type { get; }

        // Note: Parameters should always be initialized as a case-insensitive dictionary
        IDictionary<string, object> Parameters { get; }
    }
}