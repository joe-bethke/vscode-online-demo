// <copyright file="DeviceGroupController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Config.Services;
using Mmm.Iot.Config.WebService.Models;

namespace Mmm.Iot.Config.WebService.Controllers
{
    [Route("v1/devicegroups")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class DeviceGroupController : Controller
    {
        private readonly IStorage storage;

        public DeviceGroupController(IStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        [Authorize("ReadAll")]
        public async Task<DeviceGroupListApiModel> GetAllAsync()
        {
            return new DeviceGroupListApiModel(await this.storage.GetAllDeviceGroupsAsync());
        }

        [HttpGet("{id}")]
        [Authorize("ReadAll")]
        public async Task<DeviceGroupApiModel> GetAsync(string id)
        {
            return new DeviceGroupApiModel(await this.storage.GetDeviceGroupAsync(id));
        }

        [HttpPost]
        [Authorize("CreateDeviceGroups")]
        public async Task<DeviceGroupApiModel> CreateAsync([FromBody] DeviceGroupApiModel input)
        {
            return new DeviceGroupApiModel(await this.storage.CreateDeviceGroupAsync(input.ToServiceModel()));
        }

        [HttpPut("{id}")]
        [Authorize("UpdateDeviceGroups")]
        public async Task<DeviceGroupApiModel> UpdateAsync(string id, [FromBody] DeviceGroupApiModel input)
        {
            return new DeviceGroupApiModel(await this.storage.UpdateDeviceGroupAsync(id, input.ToServiceModel(), input.ETag));
        }

        [HttpDelete("{id}")]
        [Authorize("DeleteDeviceGroups")]
        public async Task DeleteAsync(string id)
        {
            await this.storage.DeleteDeviceGroupAsync(id);
        }
    }
}