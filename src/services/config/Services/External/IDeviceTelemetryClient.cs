// <copyright file="IDeviceTelemetryClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.External;

namespace Mmm.Iot.Config.Services.External
{
    public interface IDeviceTelemetryClient : IExternalServiceClient
    {
        Task UpdateRuleAsync(RuleApiModel rule, string etag);
    }
}