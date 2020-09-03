// <copyright file="IOpenIdProviderConfiguration.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public interface IOpenIdProviderConfiguration
    {
        string Issuer { get; }

        string JwksUri { get; }

        string AuthorizationEndpoint { get; }

        string EndSessionEndpoint { get; }

        IEnumerable<string> ScopesSupported { get; }

        IEnumerable<string> ClaimsSupported { get; }

        IEnumerable<string> GrantTypesSupported { get; }

        IEnumerable<string> ResponseTypesSupported { get; }

        IEnumerable<string> ResponseModesSupported { get; }
    }
}