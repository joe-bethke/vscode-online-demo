// <copyright file="ExternalServiceClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services.External
{
    public class ExternalServiceClient : IExternalServiceClient
    {
        public ExternalServiceClient()
        {
        }

        public ExternalServiceClient(string serviceUri, IExternalRequestHelper requestHelper)
        {
            this.ServiceUri = serviceUri;
            this.RequestHelper = requestHelper;
        }

        protected string ServiceUri { get; private set; }

        protected IExternalRequestHelper RequestHelper { get; private set; }

        public virtual async Task<StatusResultServiceModel> StatusAsync()
        {
            try
            {
                var status = await this.RequestHelper.ProcessStatusAsync(this.ServiceUri);
                return status.Status;
            }
            catch (Exception e)
            {
                return new StatusResultServiceModel(false, $"Unable to get the status of external service client at {this.ServiceUri}/status. {e.Message}");
            }
        }
    }
}