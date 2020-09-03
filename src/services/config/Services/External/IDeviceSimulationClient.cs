// <copyright file="IDeviceSimulationClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.External;

namespace Mmm.Iot.Config.Services.External
{
    public interface IDeviceSimulationClient : IExternalServiceClient
    {
        Task<SimulationApiModel> GetDefaultSimulationAsync();

        Task UpdateSimulationAsync(SimulationApiModel model);
    }
}