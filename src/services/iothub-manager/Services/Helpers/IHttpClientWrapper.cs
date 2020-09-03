// <copyright file="IHttpClientWrapper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Mmm.Iot.IoTHubManager.Services.Helpers
{
    public interface IHttpClientWrapper
    {
        Task PostAsync(string uri, string description, object content = null);
    }
}