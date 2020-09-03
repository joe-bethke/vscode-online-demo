// <copyright file="Template.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Config.Services.External;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.Services.Models
{
    public class Template
    {
        [JsonProperty("Groups")]
        public IEnumerable<DeviceGroup> Groups { get; set; }

        [JsonProperty("Rules")]
        public IEnumerable<RuleApiModel> Rules { get; set; }

        [JsonProperty("Simulations")]
        public IEnumerable<SimulationApiModel> Simulations { get; set; }
    }
}