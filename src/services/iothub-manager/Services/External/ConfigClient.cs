// <copyright file="ConfigClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.Services.External
{
    public class ConfigClient : ExternalServiceClient, IConfigClient
    {
        private readonly IExternalRequestHelper requestHelper;
        private readonly string serviceUri;

        public ConfigClient(AppConfig config, IExternalRequestHelper requestHelper)
            : base(config.ExternalDependencies.ConfigServiceUrl, requestHelper)
        {
            this.serviceUri = config.ExternalDependencies.ConfigServiceUrl;
            this.requestHelper = requestHelper;
        }

        public string RequestUrl(string path)
        {
            return $"{this.serviceUri}/{path}";
        }

        public async Task<PackageApiModel> GetPackageAsync(string packageId)
        {
            string url = this.RequestUrl($"packages/{packageId}");
            return await this.requestHelper.ProcessRequestAsync<PackageApiModel>(HttpMethod.Get, url);
        }

        public async Task<DeviceGroup> GetDeviceGroupAsync(string deviceGroupId)
        {
            string url = this.RequestUrl($"devicegroups/{deviceGroupId}");
            return await this.requestHelper.ProcessRequestAsync<DeviceGroup>(HttpMethod.Get, url);
        }
    }
}