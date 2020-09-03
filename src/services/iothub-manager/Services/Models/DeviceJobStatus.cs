// <copyright file="DeviceJobStatus.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public enum DeviceJobStatus
    {
        Pending = 0,
        Scheduled = 1,
        Running = 2,
        Completed = 3,
        Failed = 4,
        Canceled = 5,
    }
}