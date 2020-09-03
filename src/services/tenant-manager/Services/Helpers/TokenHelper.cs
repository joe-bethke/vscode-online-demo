// <copyright file="TokenHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;

namespace Mmm.Iot.TenantManager.Services.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private AppConfig config;

        public TokenHelper(AppConfig config)
        {
            this.config = config;
        }

        public async Task<string> GetTokenAsync()
        {
            string keyVaultAppId = this.config.Global.AzureActiveDirectory.AppId;
            string aadTenantId = this.config.Global.AzureActiveDirectory.TenantId;
            string keyVaultAppKey = this.config.Global.AzureActiveDirectory.AppSecret;

            // Retrieve a token from Azure AD using the application id and password.
            try
            {
                var authContext = new AuthenticationContext(string.Format("https://login.microsoftonline.com/{0}", aadTenantId));
                var credential = new ClientCredential(keyVaultAppId, keyVaultAppKey);
                AuthenticationResult token = await authContext.AcquireTokenAsync("https://management.core.windows.net/", credential);

                if (token == null)
                {
                    throw new NoAuthorizationException("The authentication context returned a null value while aquiring the authentication token.");
                }

                return token.AccessToken;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to retrieve authentication access token in GetServicePrincipalToken().", e);
            }
        }
    }
}