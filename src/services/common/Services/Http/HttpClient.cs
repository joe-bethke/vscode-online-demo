// <copyright file="HttpClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mmm.Iot.Common.Services.Http
{
    public class HttpClient : IHttpClient
    {
        private readonly ILogger logger;

        public HttpClient(ILogger<HttpClient> logger)
        {
            this.logger = logger;
        }

        public async Task<IHttpResponse> GetAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, HttpMethod.Get);
        }

        public async Task<IHttpResponse> PostAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, HttpMethod.Post);
        }

        public async Task<IHttpResponse> PutAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, HttpMethod.Put);
        }

        public async Task<IHttpResponse> PatchAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, new HttpMethod("PATCH"));
        }

        public async Task<IHttpResponse> DeleteAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, HttpMethod.Delete);
        }

        public async Task<IHttpResponse> HeadAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, HttpMethod.Head);
        }

        public async Task<IHttpResponse> OptionsAsync(IHttpRequest request)
        {
            return await this.SendAsync(request, HttpMethod.Options);
        }

        public async Task<IHttpResponse> SendAsync(IHttpRequest request, HttpMethod httpMethod)
        {
            var clientHandler = new HttpClientHandler();
            using (var client = new System.Net.Http.HttpClient(clientHandler))
            {
                var httpRequest = new HttpRequestMessage
                {
                    Method = httpMethod,
                    RequestUri = request.Uri,
                };

                SetServerSSLSecurity(request, clientHandler);
                SetTimeout(request, client);
                SetContent(request, httpMethod, httpRequest);
                SetHeaders(request, httpRequest);

                this.logger.LogDebug("Sending {method} request to URI {uri} with options {options}", httpMethod, request.Uri, request.Options);

                try
                {
                    using (var response = await client.SendAsync(httpRequest))
                    {
                        if (request.Options.EnsureSuccess)
                        {
                            response.EnsureSuccessStatusCode();
                        }

                        return new HttpResponse
                        {
                            StatusCode = response.StatusCode,
                            IsSuccessStatusCode = response.IsSuccessStatusCode,
                            Headers = response.Headers,
                            Content = await response.Content.ReadAsStringAsync(),
                        };
                    }
                }
                catch (HttpRequestException e)
                {
                    var errorMessage = e.Message;
                    if (e.InnerException != null)
                    {
                        errorMessage += " - " + e.InnerException.Message;
                    }

                    this.logger.LogError(e, "Request failed");

                    return new HttpResponse
                    {
                        StatusCode = 0,
                        IsSuccessStatusCode = false,
                        Content = errorMessage,
                    };
                }
            }
        }

        private static void SetContent(IHttpRequest request, HttpMethod httpMethod, HttpRequestMessage httpRequest)
        {
            if (httpMethod != HttpMethod.Post && httpMethod != HttpMethod.Put)
            {
                return;
            }

            httpRequest.Content = request.Content;
            if (request.ContentType != null && request.Content != null)
            {
                httpRequest.Content.Headers.ContentType = request.ContentType;
            }
        }

        private static void SetHeaders(IHttpRequest request, HttpRequestMessage httpRequest)
        {
            foreach (var header in request.Headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }
        }

        private static void SetServerSSLSecurity(IHttpRequest request, HttpClientHandler clientHandler)
        {
            if (request.Options.AllowInsecureSSLServer)
            {
                clientHandler.ServerCertificateCustomValidationCallback = (HttpRequestMessage a, X509Certificate2 b, X509Chain c, SslPolicyErrors d) => true;
            }
        }

        private static void SetTimeout(
            IHttpRequest request,
            System.Net.Http.HttpClient client)
        {
            client.Timeout = TimeSpan.FromMilliseconds(request.Options.Timeout);
        }
    }
}