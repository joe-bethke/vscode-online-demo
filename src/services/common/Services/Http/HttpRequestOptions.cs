// <copyright file="HttpRequestOptions.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Common.Services.Http
{
    public class HttpRequestOptions
    {
        public bool EnsureSuccess { get; set; } = false;

        public bool AllowInsecureSSLServer { get; set; } = false;

        public int Timeout { get; set; } = 30000;
    }
}