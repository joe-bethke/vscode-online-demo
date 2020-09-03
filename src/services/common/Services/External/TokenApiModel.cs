// <copyright file="TokenApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.External
{
    public class TokenApiModel
    {
        [JsonProperty(PropertyName = "Audience", Order = 10)]
        public string Audience { get; set; }

        [JsonProperty(PropertyName = "AccessTokenType", Order = 20)]
        public string AccessTokenType { get; set; }

        [JsonProperty(PropertyName = "AccessToken", Order = 30)]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "Authority", Order = 40)]
        public string Authority { get; set; }

        [JsonProperty(PropertyName = "ExpiresOn", Order = 50)]
        public DateTimeOffset ExpiresOn { get; set; }
    }
}