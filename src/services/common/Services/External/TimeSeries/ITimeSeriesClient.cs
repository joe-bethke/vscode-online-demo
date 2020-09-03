// <copyright file="ITimeSeriesClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services.External.TimeSeries
{
    public interface ITimeSeriesClient : IStatusOperation
    {
        Task<MessageList> QueryEventsAsync(
            DateTimeOffset? from,
            DateTimeOffset? to,
            string order,
            int skip,
            int limit,
            string[] deviceIds);

        Task<MessageList> QueryEventsAsync(
            int limit,
            string deviceId);
    }
}