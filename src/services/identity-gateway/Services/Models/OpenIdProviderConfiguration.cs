// <copyright file="OpenIdProviderConfiguration.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class OpenIdProviderConfiguration : IOpenIdProviderConfiguration
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string host;

        public OpenIdProviderConfiguration()
        {
        }

        public OpenIdProviderConfiguration(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            string forwardedFor = null;
            if (httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].Count > 0)
            {
                forwardedFor = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            // checks for http vs https using _httpContext.Request.IsHttps and creates the url accordingly
            this.host = forwardedFor ?? $"http{(httpContextAccessor.HttpContext.Request.IsHttps ? "s" : string.Empty)}://{httpContextAccessor.HttpContext.Request.Host.ToString()}";
        }

        [JsonProperty("issuer")]
        public virtual string Issuer => this.host;

        [JsonProperty("jwks_uri")]
        public virtual string JwksUri => this.host + "/.well-known/openid-configuration/jwks";

        [JsonProperty("authorization_endpoint")]
        public virtual string AuthorizationEndpoint => this.host + "/connect/authorize";

        [JsonProperty("end_session_endpoint")]
        public virtual string EndSessionEndpoint => this.host + "/connect/logout";

        [JsonProperty("scopes_supported")]
        public virtual IEnumerable<string> ScopesSupported => new List<string> { "openid", "profile" };

        [JsonProperty("claims_supported")]
        public virtual IEnumerable<string> ClaimsSupported => new List<string> { "sub", "name", "tenant", "role" };

        [JsonProperty("grant_types_supported")]
        public virtual IEnumerable<string> GrantTypesSupported => new List<string> { "implicit" };

        [JsonProperty("response_types_supported")]
        public virtual IEnumerable<string> ResponseTypesSupported => new List<string> { "token", "id_token" };

        [JsonProperty("response_modes_supported")]
        public virtual IEnumerable<string> ResponseModesSupported => new List<string> { "query" };
    }
}