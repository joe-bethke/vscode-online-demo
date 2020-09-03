// <copyright file="IHttpResponse.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http.Headers;

namespace Mmm.Iot.Common.Services.Http
{
    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        HttpResponseHeaders Headers { get; }

        string Content { get; }

        bool IsSuccess { get; }

        bool IsError { get; }

        bool IsIncomplete { get; }

        bool IsNonRetriableError { get; }

        bool IsRetriableError { get; }

        bool IsBadRequest { get; }

        bool IsUnauthorized { get; }

        bool IsForbidden { get; }

        bool IsNotFound { get; }

        bool IsTimeout { get; }

        bool IsConflict { get; }

        bool IsServerError { get; }

        bool IsServiceUnavailable { get; }

        bool IsSuccessStatusCode { get; }
    }
}