// <copyright file="IDeployments.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.Services
{
    public interface IDeployments
    {
        Task<DeploymentServiceModel> CreateAsync(DeploymentServiceModel model, string userId, string tenantId);

        Task<DeploymentServiceListModel> ListAsync();

        Task<DeploymentServiceListModel> ListFromStorageAsync();

        Task<DeploymentServiceModel> GetAsync(string id, bool includeDeviceStatus, bool isLatest);

        Task DeleteAsync(string deploymentId, string userId, string tenantId, bool isDelete);

        Task<PackageApiModel> GetPackageAsync(string packageId);

        Task<DeviceGroup> GetDeviceGroupAsync(string deviceGroupId);

        Task ReactivateDeploymentAsyc(string deploymentId, string userId, string tenantId);

        Task<DeviceServiceListModel> GetDeviceListAsync(string deploymentId, string query, bool isLatest);
    }
}