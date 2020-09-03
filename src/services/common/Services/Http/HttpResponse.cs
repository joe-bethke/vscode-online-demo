// <copyright file="HttpResponse.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mmm.Iot.Config.Services.Test")]
[assembly: InternalsVisibleTo("Mmm.Iot.DeviceTelemetry.Services.Test")]
[assembly: InternalsVisibleTo("Mmm.Iot.StorageAdapter.Services.Test")]
[assembly: InternalsVisibleTo("Mmm.Iot.IoTHubManager.Services.Test")]

namespace Mmm.Iot.Common.Services.Http
{
    public class HttpResponse : IHttpResponse
    {
        private const int Error429TooManyRequests = 429;

        public HttpResponse()
        {
        }

        public HttpResponse(
            HttpStatusCode statusCode,
            string content,
            HttpResponseHeaders headers)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
            this.Content = content;
        }

        public HttpStatusCode StatusCode { get; set; }

        public HttpResponseHeaders Headers { get; set; }

        public string Content { get; set; }

        public bool IsSuccess
        {
            get => (int)this.StatusCode >= 200 && (int)this.StatusCode <= 299;
            set { throw new System.NotImplementedException(); }
        }

        public bool IsError => (int)this.StatusCode >= 400 || (int)this.StatusCode == 0;

        public bool IsIncomplete
        {
            get
            {
                var c = (int)this.StatusCode;
                return (c >= 100 && c <= 199) || (c >= 300 && c <= 399);
            }
        }

        public bool IsNonRetriableError => this.IsError && !this.IsRetriableError;

        public bool IsRetriableError => this.StatusCode == HttpStatusCode.NotFound ||
                                        this.StatusCode == HttpStatusCode.RequestTimeout ||
                                        (int)this.StatusCode == Error429TooManyRequests;

        public bool IsBadRequest => (int)this.StatusCode == 400;

        public bool IsUnauthorized => (int)this.StatusCode == 401;

        public bool IsForbidden => (int)this.StatusCode == 403;

        public bool IsNotFound => (int)this.StatusCode == 404;

        public bool IsTimeout => (int)this.StatusCode == 408;

        public bool IsConflict => (int)this.StatusCode == 409;

        public bool IsServerError => (int)this.StatusCode >= 500;

        public bool IsServiceUnavailable => (int)this.StatusCode == 503;

        public bool IsSuccessStatusCode { get; set; }
    }
}