// <copyright file="StatusServiceBase.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services
{
    public abstract class StatusServiceBase : IStatusService
    {
        private AppConfig config;

        public StatusServiceBase(AppConfig config)
        {
            this.config = config;
        }

        public abstract IDictionary<string, IStatusOperation> Dependencies { get; set; }

        public async Task<StatusServiceModel> GetStatusAsync()
        {
            var result = new StatusServiceModel(true, "Alive and well!");
            var errors = new List<string>();

            // Loop over the IStatusOperation classes and get each status - set service status based on each response
            foreach (var dependency in this.Dependencies)
            {
                var service = dependency.Value;
                var serviceResult = await service.StatusAsync();
                this.SetServiceStatus(dependency.Key, serviceResult, result, errors);
            }

            if (errors.Count > 0)
            {
                result.Status.Message = string.Join("; ", errors);
            }

            result.Properties.AuthRequired = this.config.Global.AuthRequired;
            result.Properties.Endpoint = this.config.ASPNETCORE_URLS;

            return result;
        }

        public IActionResult Ping()
        {
            return new StatusCodeResult(200);
        }

        private void SetServiceStatus(string dependencyName, StatusResultServiceModel serviceResult, StatusServiceModel result, IList<string> errors)
        {
            if (!serviceResult.IsHealthy)
            {
                errors.Add(dependencyName + " check failed");
                result.Status.IsHealthy = false;
            }

            result.Dependencies.Add(dependencyName, serviceResult);
        }
    }
}