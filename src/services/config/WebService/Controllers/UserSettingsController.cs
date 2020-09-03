// <copyright file="UserSettingsController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Config.Services;

namespace Mmm.Iot.Config.WebService.Controllers
{
    [Route("v1")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class UserSettingsController : Controller
    {
        private readonly IStorage storage;

        public UserSettingsController(IStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet("user-settings/{id}")]
        [Authorize("ReadAll")]
        public async Task<object> GetUserSettingAsync(string id)
        {
            return await this.storage.GetUserSetting(id);
        }

        [HttpPut("user-settings/{id}")]
        [Authorize("ReadAll")]
        public async Task<object> SetUserSettingAsync(string id, [FromBody] object setting)
        {
            return await this.storage.SetUserSetting(id, setting);
        }
    }
}