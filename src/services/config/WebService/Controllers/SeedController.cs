// <copyright file="SeedController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Config.Services;

namespace Mmm.Iot.Config.WebService.Controllers
{
    [Route("v1/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class SeedController : Controller
    {
        private readonly ISeed seed;

        public SeedController(ISeed seed)
        {
            this.seed = seed;
        }

        [HttpPost]
        [Authorize("ReadAll")]
        public async Task PostAsync()
        {
            await this.seed.TrySeedAsync();
        }
    }
}