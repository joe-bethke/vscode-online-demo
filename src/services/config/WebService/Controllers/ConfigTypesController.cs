// <copyright file="ConfigTypesController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Config.Services;
using Mmm.Iot.Config.WebService.Models;

namespace Mmm.Iot.Config.WebService.Controllers
{
    [Route("v1/configtypes")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class ConfigTypesController
    {
        private readonly IStorage storage;

        public ConfigTypesController(IStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        [Authorize("ReadAll")]
        public async Task<ConfigTypeListApiModel> GetAllConfigTypesAsync()
        {
            return new ConfigTypeListApiModel(await this.storage.GetConfigTypesListAsync());
        }
    }
}