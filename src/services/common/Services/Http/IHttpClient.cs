// <copyright file="IHttpClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;

namespace Mmm.Iot.Common.Services.Http
{
    public interface IHttpClient
    {
        Task<IHttpResponse> GetAsync(IHttpRequest request);

        Task<IHttpResponse> PostAsync(IHttpRequest request);

        Task<IHttpResponse> PutAsync(IHttpRequest request);

        Task<IHttpResponse> PatchAsync(IHttpRequest request);

        Task<IHttpResponse> DeleteAsync(IHttpRequest request);

        Task<IHttpResponse> HeadAsync(IHttpRequest request);

        Task<IHttpResponse> OptionsAsync(IHttpRequest request);

        Task<IHttpResponse> SendAsync(IHttpRequest request, HttpMethod method);
    }
}