// <copyright file="TimeSeriesClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Http;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.External.TimeSeries
{
    public class TimeSeriesClient : ITimeSeriesClient
    {
        private const string TsiDateFormat = "yyyy-MM-ddTHH:mm:ssZ";
        private const string TimeSeriesApiVersionPrefix = "api-version";
        private const string TimeSeriesTimeoutPrefix = "timeout";
        private const string EventsKey = "events";
        private const string AvailabilityKey = "availability";
        private const string SearchSpanKey = "searchSpan";
        private const string PredicateKey = "predicate";
        private const string PredicateStringKey = "predicateString";
        private const string TopKey = "top";
        private const string SortKey = "sort";
        private const string SortInputKey = "input";
        private const string BuiltInPropKey = "builtInProperty";
        private const string BuiltInPropValue = "$ts";
        private const string SortOrderKey = "order";
        private const string CountKey = "count";
        private const string FromKey = "from";
        private const string ToKey = "to";
        private const int ClockCalibrationInSeconds = 5;
        private const string DeviceIdKey = "iothub-connection-device-id";
        private const string AadClientIdKey = "ApplicationClientId";
        private const string AadClientSecretKey = "ApplicationClientSecret";
        private const string AadTenantKey = "Tenant";
        private const string TsiStorageTypeKey = "tsi";
        private readonly string authority;
        private readonly string applicationId;
        private readonly string applicationSecret;
        private readonly string tenant;
        private readonly string fqdn;
        private readonly string host;
        private readonly string apiVersion;
        private readonly bool? timeSeriesEnabled = null;
        private readonly string timeout;
        private readonly IHttpClient httpClient;
        private readonly ILogger logger;
        private AuthenticationResult token;

        public TimeSeriesClient(
            IHttpClient httpClient,
            AppConfig config,
            ILogger<TimeSeriesClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.authority = config.DeviceTelemetryService.TimeSeries.Authority;
            this.applicationId = config.Global.AzureActiveDirectory.AppId;
            this.applicationSecret = config.Global.AzureActiveDirectory.AppSecret;
            this.tenant = config.Global.AzureActiveDirectory.TenantId;
            this.fqdn = config.DeviceTelemetryService.TimeSeries.TsiDataAccessFqdn;
            this.host = config.DeviceTelemetryService.TimeSeries.Audience;
            this.apiVersion = config.DeviceTelemetryService.TimeSeries.ApiVersion;
            this.timeout = config.DeviceTelemetryService.TimeSeries.Timeout;
            if (!string.IsNullOrEmpty(config.DeviceTelemetryService.Messages.TelemetryStorageType))
            {
                this.timeSeriesEnabled = config.DeviceTelemetryService.Messages.TelemetryStorageType.Equals(TsiStorageTypeKey, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                this.timeSeriesEnabled = null;
            }
        }

        public async Task<StatusResultServiceModel> StatusAsync()
        {
            var result = new StatusResultServiceModel(false, "TimeSeries check failed");

            if (this.timeSeriesEnabled == true)
            {
                // Acquire an access token.
                string accessToken = string.Empty;
                try
                {
                    accessToken = await this.AcquireAccessTokenAsync();

                    // Prepare request
                    HttpRequest request = this.PrepareRequest(
                        AvailabilityKey,
                        accessToken,
                        new[] { TimeSeriesTimeoutPrefix + "=" + this.timeout });

                    var response = await this.httpClient.GetAsync(request);

                    // Return status
                    if (!response.IsError)
                    {
                        result.IsHealthy = true;
                        result.Message = "Alive and well!";
                    }
                    else
                    {
                        result.Message = $"Status code: {response.StatusCode}; Response: {response.Content}";
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, result.Message);
                }
            }
            else
            {
                result.IsHealthy = true;
                result.Message = "Service disabled not in use";
            }

            return result;
        }

        public async Task<MessageList> QueryEventsAsync(
            DateTimeOffset? from,
            DateTimeOffset? to,
            string order,
            int skip,
            int limit,
            string[] deviceIds)
        {
            // Acquire an access token.
            string accessToken = await this.AcquireAccessTokenAsync();

            // Prepare request
            HttpRequest request = this.PrepareRequest(
                EventsKey,
                accessToken,
                new[] { TimeSeriesTimeoutPrefix + "=" + this.timeout });

            request.SetContent(
                this.PrepareInput(from, to, order, skip, limit, deviceIds));

            this.logger.LogInformation("Making query to time series at URI {requestUri} with body {requestContent}", request.Uri, request.Content);

            var response = await this.httpClient.PostAsync(request);
            var messages = JsonConvert.DeserializeObject<ValueListApiModel>(response.Content);

            return messages.ToMessageList(skip);
        }

        public async Task<MessageList> QueryEventsAsync(
            int limit,
            string deviceId)
        {
            // Acquire an access token.
            string accessToken = await this.AcquireAccessTokenAsync();

            // Prepare request
            HttpRequest request = this.PrepareRequest(
                EventsKey,
                accessToken,
                new[] { TimeSeriesTimeoutPrefix + "=" + this.timeout });

            request.SetContent(this.PrepareInput(limit, deviceId));

            this.logger.LogInformation("Making query to time series at URI {requestUri} with body {requestContent}", request.Uri, request.Content);

            var response = await this.httpClient.PostAsync(request);
            var messages = JsonConvert.DeserializeObject<ValueListApiModel>(response.Content);

            return messages.ToMessageList(0);
        }

        private async Task<string> AcquireAccessTokenAsync()
        {
            // Return existing token unless it is near expiry or null
            if (this.token != null)
            {
                // Add buffer time to renew token, built in buffer for AAD is 5 mins
                if (DateTimeOffset.UtcNow.AddSeconds(ClockCalibrationInSeconds) < this.token.ExpiresOn)
                {
                    return this.token.AccessToken;
                }
            }

            if (string.IsNullOrEmpty(this.applicationId) ||
                string.IsNullOrEmpty(this.applicationSecret) ||
                string.IsNullOrEmpty(this.tenant))
            {
                throw new InvalidConfigurationException(
                    $"Active Directory properties '{AadClientIdKey}', '{AadClientSecretKey}' " +
                    $"and '{AadTenantKey}' are not set.");
            }

            var authenticationContext = new AuthenticationContext(
                this.authority + this.tenant,
                TokenCache.DefaultShared);

            try
            {
                AuthenticationResult tokenResponse = await authenticationContext.AcquireTokenAsync(
                    resource: this.host,
                    clientCredential: new ClientCredential(
                        clientId: this.applicationId,
                        clientSecret: this.applicationSecret));

                this.token = tokenResponse;

                return this.token.AccessToken;
            }
            catch (Exception e)
            {
                var msg = "Unable to retrieve token with Active Directory properties" +
                          $"'{AadClientIdKey}', '{AadClientSecretKey}' and '{AadTenantKey}'.";
                throw new InvalidConfigurationException(msg, e);
            }
        }

        private JObject PrepareInput(
            int limit,
            string deviceId)
        {
            var result = new JObject();

            // Add the predicate for devices
            if (deviceId != null)
            {
                var predicateStringObject = new JObject
                {
                    new JProperty(PredicateStringKey, string.Join(" OR ", $"[{DeviceIdKey}].String='{deviceId}'")),
                };
                result.Add(PredicateKey, predicateStringObject);
            }

            // Add the limit top clause
            JObject builtInPropObject = new JObject(new JProperty(BuiltInPropKey, BuiltInPropValue));
            JArray sortArray = new JArray(new JObject
                {
                    { SortInputKey, builtInPropObject },
                    { SortOrderKey, "desc" },
                });

            JObject topObject = new JObject
            {
                { SortKey, sortArray },
                { CountKey, limit },
            };

            result.Add(TopKey, topObject);

            return result;
        }

        private JObject PrepareInput(
            DateTimeOffset? from,
            DateTimeOffset? to,
            string order,
            int skip,
            int limit,
            string[] deviceIds)
        {
            var result = new JObject();

            // Add the search span clause
            // End of the interval is exclusive
            if (!to.HasValue)
            {
                to = DateTimeOffset.UtcNow;
            }

            if (!from.HasValue)
            {
                from = DateTimeOffset.MinValue;
            }

            result.Add(SearchSpanKey, new JObject(
                new JProperty(FromKey, from.Value.ToString(TsiDateFormat)),
                new JProperty(ToKey, to.Value.ToString(TsiDateFormat))));

            // Add the predicate for devices
            if (deviceIds != null && deviceIds.Length > 0)
            {
                var devicePredicates = new List<string>();
                foreach (var deviceId in deviceIds)
                {
                    devicePredicates.Add($"[{DeviceIdKey}].String='{deviceId}'");
                }

                var predicateStringObject = new JObject
                {
                    new JProperty(PredicateStringKey, string.Join(" OR ", devicePredicates)),
                };
                result.Add(PredicateKey, predicateStringObject);
            }

            // Add the limit top clause
            JObject builtInPropObject = new JObject(new JProperty(BuiltInPropKey, BuiltInPropValue));
            JArray sortArray = new JArray(new JObject
                {
                    { SortInputKey, builtInPropObject },
                    { SortOrderKey, order },
                });

            JObject topObject = new JObject
            {
                { SortKey, sortArray },
                { CountKey, skip + limit },
            };

            result.Add(TopKey, topObject);

            return result;
        }

        private HttpRequest PrepareRequest(
            string path,
            string accessToken,
            string[] queryArgs = null)
        {
            string args = TimeSeriesApiVersionPrefix + "=" + this.apiVersion;
            if (queryArgs != null && queryArgs.Any())
            {
                args += "&" + string.Join("&", queryArgs);
            }

            Uri uri = new UriBuilder("https", this.fqdn)
            {
                Path = path,
                Query = args,
            }.Uri;
            HttpRequest request = new HttpRequest(uri);
            request.Headers.Add("x-ms-client-application-name", this.applicationId);
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            return request;
        }
    }
}