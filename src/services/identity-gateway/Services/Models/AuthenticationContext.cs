// <copyright file="AuthenticationContext.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Mmm.Iot.Common.Services.Config;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class AuthenticationContext : IAuthenticationContext
    {
        private readonly Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext;

        public AuthenticationContext(AppConfig config)
        {
            this.authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext($"https://login.microsoftonline.com/{config.Global.AzureActiveDirectory.TenantId}");
        }

        public Task<AuthenticationResult> AcquireTokenAsync(string resource, ClientCredential clientCredential)
        {
            return this.authContext.AcquireTokenAsync(resource, clientCredential);
        }
    }
}