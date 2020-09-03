// <copyright file="StatusController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.DeviceTelemetry.WebService.Controllers
{
    [Route("v1/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public sealed class StatusController : ControllerBase
    {
        private readonly IStatusService statusService;

        public StatusController(IStatusService statusService)
        {
            this.statusService = statusService;
        }

        [HttpGet]
        public async Task<StatusApiModel> GetAsync()
        {
            try
            {
                return new StatusApiModel(await this.statusService.GetStatusAsync(), "Device Telemetry");
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while attempting to get the service status", e);
            }
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return new StatusCodeResult(200);
        }
    }
}