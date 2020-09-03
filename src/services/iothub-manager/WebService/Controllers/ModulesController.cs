// <copyright file="ModulesController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.IoTHubManager.Services;
using Mmm.Iot.IoTHubManager.WebService.Models;

namespace Mmm.Iot.IoTHubManager.WebService.Controllers
{
    [Route("v1/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class ModulesController : Controller
    {
        private const string ContinuationTokenName = "x-ms-continuation";
        private readonly IDevices devices;

        public ModulesController(IDevices devices)
        {
            this.devices = devices;
        }

        [HttpGet]
        [Authorize("ReadAll")]
        public async Task<TwinPropertiesListApiModel> GetModuleTwinsAsync([FromQuery] string query)
        {
            string continuationToken = string.Empty;
            if (this.Request.Headers.ContainsKey(ContinuationTokenName))
            {
                continuationToken = this.Request.Headers[ContinuationTokenName].FirstOrDefault();
            }

            return new TwinPropertiesListApiModel(
                await this.devices.GetModuleTwinsByQueryAsync(query, continuationToken));
        }

        [HttpPost("query")]
        [Authorize("ReadAll")]
        public async Task<TwinPropertiesListApiModel> QueryModuleTwinsAsync([FromBody] string query)
        {
            return await this.GetModuleTwinsAsync(query);
        }

        [HttpGet("{deviceId}/{moduleId}")]
        [Authorize("ReadAll")]
        public async Task<TwinPropertiesApiModel> GetModuleTwinAsync(string deviceId, string moduleId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                throw new InvalidInputException("deviceId must be provided");
            }

            if (string.IsNullOrWhiteSpace(moduleId))
            {
                throw new InvalidInputException("moduleId must be provided");
            }

            var twin = await this.devices.GetModuleTwinAsync(deviceId, moduleId);
            return new TwinPropertiesApiModel(
                twin.DesiredProperties,
                twin.ReportedProperties,
                deviceId,
                moduleId);
        }
    }
}