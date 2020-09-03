// <copyright file="AuthState.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class AuthState
    {
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("tenant")]
        public string Tenant { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("invitation")]
        public string Invitation { get; set; }
    }
}