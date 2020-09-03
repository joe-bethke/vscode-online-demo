// <copyright file="DevicePropertiesController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.IoTHubManager.Services;
using Mmm.Iot.IoTHubManager.WebService.Models;

namespace Mmm.Iot.IoTHubManager.WebService.Controllers
{
    [Route("v1/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class DevicePropertiesController : Controller
    {
        private readonly IDeviceProperties deviceProperties;

        public DevicePropertiesController(IDeviceProperties deviceProperties)
        {
            this.deviceProperties = deviceProperties;
        }

        [HttpGet]
        [Authorize("ReadAll")]
        public async Task<DevicePropertiesApiModel> GetAsync()
        {
            return new DevicePropertiesApiModel(await this.deviceProperties.GetListAsync());
        }
    }
}