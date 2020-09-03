// <copyright file="UserApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.External
{
    public class UserApiModel
    {
        [JsonProperty(PropertyName = "Id", Order = 10)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Email", Order = 20)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "Name", Order = 30)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "AllowedActions", Order = 40)]
        public List<string> AllowedActions { get; set; }
    }
}