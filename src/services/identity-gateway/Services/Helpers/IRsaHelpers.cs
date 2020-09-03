// <copyright file="IRsaHelpers.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Mmm.Iot.IdentityGateway.Services.Helpers
{
    public interface IRsaHelpers
    {
        RSA DecodeRsa(string privateRsaKey);

        JsonWebKeySet GetJsonWebKey(string key);
    }
}