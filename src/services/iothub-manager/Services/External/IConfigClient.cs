// <copyright file="IConfigClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.Services.External
{
    public interface IConfigClient : IExternalServiceClient
    {
        Task<PackageApiModel> GetPackageAsync(string packageId);

        Task<DeviceGroup> GetDeviceGroupAsync(string deviceGroupId);
    }
}