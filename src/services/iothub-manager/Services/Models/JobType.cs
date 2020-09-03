// <copyright file="JobType.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public enum JobType
    {
        Unknown = 0,
        ScheduleDeviceMethod = 3,
        ScheduleUpdateTwin = 4,
    }
}