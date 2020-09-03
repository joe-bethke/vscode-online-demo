// <copyright file="AlertingController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.TenantManager.Services;
using Mmm.Iot.TenantManager.Services.Models;

namespace Mmm.Iot.TenantManager.WebService.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class AlertingController : Controller
    {
        private readonly IAlertingContainer alertingContainer;
        private readonly ILogger logger;

        public AlertingController(IAlertingContainer alertingContainer, ILogger<AlertingController> logger)
        {
            this.alertingContainer = alertingContainer;
            this.logger = logger;
        }

        [HttpPost]
        [Authorize("EnableAlerting")]
        public async Task<StreamAnalyticsJobModel> AddAlertingAsync()
        {
            return await this.alertingContainer.AddAlertingAsync(this.GetTenantId());
        }

        [HttpDelete]
        [Authorize("DisableAlerting")]
        public async Task<StreamAnalyticsJobModel> RemoveAlertingAsync()
        {
            return await this.alertingContainer.RemoveAlertingAsync(this.GetTenantId());
        }

        [HttpGet]
        [Authorize("ReadAll")]
        public async Task<StreamAnalyticsJobModel> GetAlertingAsync([FromQuery] bool createIfNotExists = false)
        {
            string tenantId = this.GetTenantId();
            StreamAnalyticsJobModel model = await this.alertingContainer.GetAlertingAsync(tenantId);
            if (!this.alertingContainer.SaJobExists(model) && createIfNotExists)
            {
                // If the tenant does not have an sa job, start creating it
                this.logger.LogInformation("The tenant does not already have alerting enabled and the createIfNotExists parameter was set to true. Creating a stream analytics job now. TenantId: {tenantId}", tenantId);
                return await this.alertingContainer.AddAlertingAsync(tenantId);
            }
            else
            {
                return model;
            }
        }

        [HttpPost("start")]
        [Authorize("EnableAlerting")]
        public async Task<StreamAnalyticsJobModel> StartAsync()
        {
            return await this.alertingContainer.StartAlertingAsync(this.GetTenantId());
        }

        [HttpPost("stop")]
        [Authorize("DisableAlerting")]
        public async Task<StreamAnalyticsJobModel> StopAsync()
        {
            return await this.alertingContainer.StopAlertingAsync(this.GetTenantId());
        }
    }
}