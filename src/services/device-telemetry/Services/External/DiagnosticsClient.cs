// <copyright file="DiagnosticsClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Http;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;
using HttpRequest = Mmm.Iot.Common.Services.Http.HttpRequest;

namespace Mmm.Iot.DeviceTelemetry.Services.External
{
    public class DiagnosticsClient : IDiagnosticsClient
    {
        private const string TenantHeader = "ApplicationTenantID";
        private const string TenantId = "TenantID";
        private const int RetrySleepMilliseconds = 500;
        private readonly IHttpClient httpClient;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string serviceUrl;
        private readonly int maxRetries;

        public DiagnosticsClient(IHttpClient httpClient, AppConfig config, ILogger<DiagnosticsClient> logger, IHttpContextAccessor contextAccessor)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.serviceUrl = config.ExternalDependencies.DiagnosticsServiceUrl;
            this.maxRetries = config.ExternalDependencies.DiagnosticsMaxLogRetries;
            if (string.IsNullOrEmpty(this.serviceUrl))
            {
                logger.LogError("Cannot log to diagnostics service, diagnostics url not provided");
                this.CanLogToDiagnostics = false;
            }
            else
            {
                this.CanLogToDiagnostics = true;
            }

            this.httpContextAccessor = contextAccessor;
        }

        public bool CanLogToDiagnostics { get; }

        /**
         * Logs event with given event name and empty event properties
         * to diagnostics event endpoint.
         */
        public async Task LogEventAsync(string eventName)
        {
            await this.LogEventAsync(eventName, new Dictionary<string, object>());
        }

        /**
         * Logs event with given event name and event properties
         * to diagnostics event endpoint.
         */
        public async Task LogEventAsync(string eventName, Dictionary<string, object> eventProperties)
        {
            var request = new HttpRequest();
            try
            {
                request.SetUriFromString($"{this.serviceUrl}/diagnosticsevents");

                string tenantId = this.httpContextAccessor.HttpContext.Request.GetTenant();
                request.Headers.Add(TenantHeader, tenantId);
                DiagnosticsRequestModel model = new DiagnosticsRequestModel
                {
                    EventType = eventName,
                    EventProperties = eventProperties,
                };
                request.SetContent(JsonConvert.SerializeObject(model));
                await this.PostHttpRequestWithRetryAsync(request);
            }
            catch (Exception e)
            {
                this.logger.LogWarning(e, "Cannot log to diagnostics service, diagnostics url not provided");
            }
        }

        public async Task<StatusResultServiceModel> StatusAsync()
        {
            var isHealthy = false;
            var message = "Diagnostics check failed";
            var request = new HttpRequest();
            try
            {
                request.SetUriFromString($"{this.serviceUrl}/status");
                string tenantId = this.httpContextAccessor.HttpContext.Request.GetTenant();
                request.Headers.Add(TenantHeader, tenantId);
                var response = await this.httpClient.GetAsync(request);

                if (response.IsError)
                {
                    message = "Status code: " + response.StatusCode + "; Response: " + response.Content;
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
                    message = data["Message"].ToString();
                    isHealthy = Convert.ToBoolean(data["IsHealthy"]);
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, message);
            }

            return new StatusResultServiceModel(isHealthy, message);
        }

        private async Task PostHttpRequestWithRetryAsync(HttpRequest request)
        {
            int retries = 0;
            bool requestSucceeded = false;
            while (!requestSucceeded && retries < this.maxRetries)
            {
                try
                {
                    IHttpResponse response = await this.httpClient.PostAsync(request);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        retries++;
                        this.LogAndSleepOnFailure(retries, response.Content);
                    }
                    else
                    {
                        requestSucceeded = true;
                    }
                }
                catch (Exception e)
                {
                    retries++;
                    this.LogAndSleepOnFailure(retries, e.Message);
                }
            }
        }

        private void LogAndSleepOnFailure(int retries, string errorMessage)
        {
            if (retries < this.maxRetries)
            {
                int retriesLeft = this.maxRetries - retries;
                string logString = $"";
                string errorText = $"Failed to log to diagnostics: {errorMessage}. {retriesLeft} retries remaining";
                this.logger.LogWarning(new Exception(errorText), errorText);
                Thread.Sleep(RetrySleepMilliseconds);
            }
            else
            {
                string errorText = $"Failed to log to diagnostics: {errorMessage}. Reached max retries and will not log.";
                this.logger.LogError(new Exception(errorText), errorText);
            }
        }
    }
}