// <copyright file="StreamAnalyticsHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.StreamAnalytics;
using Microsoft.Azure.Management.StreamAnalytics.Models;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.TenantManager.Services.Helpers
{
    public class StreamAnalyticsHelper : IStreamAnalyticsHelper
    {
        private readonly AppConfig config;
        private readonly ITokenHelper tokenHelper;

        public StreamAnalyticsHelper(AppConfig config, ITokenHelper tokenHelper)
        {
            this.config = config;
            this.tokenHelper = tokenHelper;
        }

        public async Task<StatusResultServiceModel> StatusAsync()
        {
            try
            {
                IStreamAnalyticsManagementClient client = await this.GetClientAsync();
                return new StatusResultServiceModel(true, "Alive and well!");
            }
            catch (Exception e)
            {
                return new StatusResultServiceModel(false, $"Unable to get the status of Stream Analytics: {e.Message}");
            }
        }

        public async Task<StreamingJob> GetJobAsync(string saJobName)
        {
            IStreamAnalyticsManagementClient client = await this.GetClientAsync();
            try
            {
                return await client.StreamingJobs.GetAsync(this.config.Global.ResourceGroup, saJobName);
            }
            catch (CloudException ce)
            {
                if (ce.Message.ToLower().Contains("was not found"))
                {
                    // Ensure that this cloud exception is for a resource not found exception
                    throw new ResourceNotFoundException($"The stream analytics job {saJobName} does not exist or could not be found.", ce);
                }
                else
                {
                    // otherwise, just throw the cloud exception.
                    throw ce;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An Unknown Exception occurred while attempting to get the stream analytics job {saJobName}", e);
            }
        }

        public bool JobIsActive(StreamingJob job)
        {
            return job.JobState == "Running" || job.JobState == "Starting";
        }

        public async Task StartAsync(string saJobName)
        {
            IStreamAnalyticsManagementClient client = await this.GetClientAsync();
            try
            {
                await client.StreamingJobs.BeginStartAsync(this.config.Global.ResourceGroup, saJobName);
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to start the Stream Analytics Job {saJobName}.", e);
            }
        }

        public async Task StopAsync(string saJobName)
        {
            IStreamAnalyticsManagementClient client = await this.GetClientAsync();
            try
            {
                await client.StreamingJobs.BeginStopAsync(this.config.Global.ResourceGroup, saJobName);
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to stop the Stream Analytics Job {saJobName}.", e);
            }
        }

        public virtual async Task<IStreamAnalyticsManagementClient> GetClientAsync()
        {
            try
            {
                string authToken = string.Empty;
                try
                {
                    authToken = await this.tokenHelper.GetTokenAsync();
                    if (string.IsNullOrEmpty(authToken))
                    {
                        throw new Exception("Auth Token from tokenHelper returned a null response.");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to get an authorization token for creating a Stream Analytics Management Client.", e);
                }

                TokenCredentials credentials = new TokenCredentials(authToken);
                StreamAnalyticsManagementClient client = new StreamAnalyticsManagementClient(credentials)
                {
                    SubscriptionId = this.config.Global.SubscriptionId,
                };
                return client;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to get a new Stream Analytics Management Client.", e);
            }
        }
    }
}