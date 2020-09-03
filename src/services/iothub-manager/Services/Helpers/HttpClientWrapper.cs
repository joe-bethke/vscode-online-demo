// <copyright file="HttpClientWrapper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Http;

namespace Mmm.Iot.IoTHubManager.Services.Helpers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly ILogger logger;
        private readonly IHttpClient client;

        public HttpClientWrapper(
            ILogger<HttpClientWrapper> logger,
            IHttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task PostAsync(
            string uri,
            string description,
            object content = null)
        {
            var request = new HttpRequest();
            request.SetUriFromString(uri);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("User-Agent", "Config");
            if (uri.ToLowerInvariant().StartsWith("https:"))
            {
                request.Options.AllowInsecureSSLServer = true;
            }

            if (content != null)
            {
                request.SetContent(content);
            }

            IHttpResponse response;

            try
            {
                response = await this.client.PostAsync(request);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Request to URI {uri} failed", uri);
                throw new ExternalDependencyException($"Failed to post {description}");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                string errorMessage = $"Request to URI {uri} failed with response {response}";
                this.logger.LogError(new Exception(errorMessage), errorMessage);
                throw new ExternalDependencyException($"Unable to post {description}");
            }
        }
    }
}