// <copyright file="IStreamAnalyticsHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.Azure.Management.StreamAnalytics;
using Microsoft.Azure.Management.StreamAnalytics.Models;
using Mmm.Iot.Common.Services;

namespace Mmm.Iot.TenantManager.Services.Helpers
{
    public interface IStreamAnalyticsHelper : IStatusOperation
    {
        Task StartAsync(string saJobName);

        Task StopAsync(string saJobName);

        Task<StreamingJob> GetJobAsync(string saJobName);

        bool JobIsActive(StreamingJob job);

        Task<IStreamAnalyticsManagementClient> GetClientAsync();
    }
}