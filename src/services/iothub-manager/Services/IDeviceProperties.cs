// <copyright file="IDeviceProperties.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.Services
{
    public interface IDeviceProperties
    {
        Task<List<string>> GetListAsync();

        Task<DevicePropertyServiceModel> UpdateListAsync(
            DevicePropertyServiceModel devicePropertyServiceModel);

        Task<bool> TryRecreateListAsync(bool force = false);

        Task<List<string>> GetUploadedFilesForDevice(string tenantId, string deviceId);
    }
}