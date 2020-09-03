// <copyright file="IAlertingContainer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.TenantManager.Services.Models;

namespace Mmm.Iot.TenantManager.Services
{
    public interface IAlertingContainer
    {
        Task<StreamAnalyticsJobModel> AddAlertingAsync(string tenantId);

        Task<StreamAnalyticsJobModel> RemoveAlertingAsync(string tenantId);

        Task<StreamAnalyticsJobModel> GetAlertingAsync(string tenantId);

        Task<StreamAnalyticsJobModel> StartAlertingAsync(string tenantId);

        Task<StreamAnalyticsJobModel> StopAlertingAsync(string tenantId);

        bool SaJobExists(StreamAnalyticsJobModel model);
    }
}