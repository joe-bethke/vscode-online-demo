// <copyright file="IMessages.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.External.TimeSeries;

namespace Mmm.Iot.DeviceTelemetry.Services
{
    public interface IMessages
    {
        Task<MessageList> ListAsync(
            DateTimeOffset? from,
            DateTimeOffset? to,
            string order,
            int skip,
            int limit,
            string[] devices);

        Task<MessageList> ListTopDeviceMessagesAsync(
            int limit,
            string deviceId);
    }
}