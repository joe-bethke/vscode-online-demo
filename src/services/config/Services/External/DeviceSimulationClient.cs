// <copyright file="DeviceSimulationClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.Helpers;

namespace Mmm.Iot.Config.Services.External
{
    public class DeviceSimulationClient : ExternalServiceClient, IDeviceSimulationClient
    {
        private const int DefaultSimulationId = 1;

        public DeviceSimulationClient(AppConfig config, IExternalRequestHelper requestHelper)
            : base(config.ExternalDependencies.DeviceSimulationServiceUrl, requestHelper)
        {
        }

        public async Task<SimulationApiModel> GetDefaultSimulationAsync()
        {
            return await this.RequestHelper.ProcessRequestAsync<SimulationApiModel>(
                HttpMethod.Get,
                $"{this.ServiceUri}/simulations/{DefaultSimulationId}");
        }

        public async Task UpdateSimulationAsync(SimulationApiModel model)
        {
            await this.RequestHelper.ProcessRequestAsync(
                HttpMethod.Put,
                $"{this.ServiceUri}/simulations/{model.Id}");
        }
    }
}