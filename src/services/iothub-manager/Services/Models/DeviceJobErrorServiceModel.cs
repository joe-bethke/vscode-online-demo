// <copyright file="DeviceJobErrorServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Devices;

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public class DeviceJobErrorServiceModel
    {
        public DeviceJobErrorServiceModel(DeviceJobError error)
        {
            this.Code = error.Code;
            this.Description = error.Description;
        }

        public string Code { get; }

        public string Description { get; }
    }
}