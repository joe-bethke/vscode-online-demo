// <copyright file="TenantReadyController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.TenantManager.Services;

namespace Mmm.Iot.TenantManager.WebService.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class TenantReadyController : Controller
    {
        private readonly ITenantContainer tenantContainer;
        private readonly ILogger logger;

        public TenantReadyController(ITenantContainer tenantContainer, ILogger<TenantReadyController> logger)
        {
            this.tenantContainer = tenantContainer;
            this.logger = logger;
        }

        [HttpGet("{tenantId}")]
        public async Task<bool> GetAsync(string tenantId)
        {
            return await this.tenantContainer.TenantIsReadyAsync(tenantId);
        }
    }
}