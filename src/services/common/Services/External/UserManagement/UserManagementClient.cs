// <copyright file="UserManagementClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Helpers;

namespace Mmm.Iot.Common.Services.External.UserManagement
{
    public class UserManagementClient : ExternalServiceClient, IUserManagementClient
    {
        private const string DefaultUserId = "default";

        public UserManagementClient(AppConfig config, IExternalRequestHelper requestHelper)
            : base(config.ExternalDependencies.AuthServiceUrl, requestHelper)
        {
        }

        public async Task<IEnumerable<string>> GetAllowedActionsAsync(string userObjectId, IEnumerable<string> roles)
        {
            string url = $"{this.ServiceUri}/users/{userObjectId}/allowedActions";
            return await this.RequestHelper.ProcessRequestAsync<IEnumerable<string>>(HttpMethod.Post, url, roles);
        }

        public async Task<string> GetTokenAsync()
        {
            // Note: The DEFAULT_USER_ID is set to any value. The user management service doesn't
            // currently use the user ID information, but if this API is updated in the future, we
            // will need to grab the user ID from the request JWT token and pass in here.
            var url = $"{this.ServiceUri}/users/{DefaultUserId}/token";
            TokenApiModel tokenModel = await this.RequestHelper.ProcessRequestAsync<TokenApiModel>(HttpMethod.Get, url);
            return tokenModel.AccessToken;
        }
    }
}