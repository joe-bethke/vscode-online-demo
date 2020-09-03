// <copyright file="AzureB2cClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.IdentityGateway.Services.External
{
    public class AzureB2cClient : IAzureB2cClient
    {
        private readonly string serviceUri;

        private readonly IExternalRequestHelper requestHelper;
        private readonly ILogger logger;

        public AzureB2cClient(
            AppConfig config,
            IExternalRequestHelper requestHelper,
            ILogger<AzureB2cClient> logger)
        {
            this.serviceUri = config.Global.AzureB2cBaseUri;
            this.requestHelper = requestHelper;
            this.logger = logger;
        }

        public async Task<StatusResultServiceModel> StatusAsync()
        {
            try
            {
                var response = await this.requestHelper.ProcessRequestAsync(HttpMethod.Get, this.serviceUri);
                if (response.IsSuccessStatusCode)
                {
                    return new StatusResultServiceModel(response.IsSuccessStatusCode, "Alive and well!");
                }
                else
                {
                    return new StatusResultServiceModel(false, $"AzureB2C status check failed with code {response.StatusCode}.");
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Exception in the AzureB2cClient Status Check");
                return new StatusResultServiceModel(false, e.Message);
            }
        }
    }
}