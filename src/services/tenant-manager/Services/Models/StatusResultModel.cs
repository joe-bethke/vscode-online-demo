// <copyright file="StatusResultModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.TenantManager.WebService.Models
{
    public class StatusResultModel
    {
        public StatusResultModel()
        {
        }

        public StatusResultModel(StatusResultServiceModel servicemodel)
        {
            this.IsHealthy = servicemodel.IsHealthy;
            this.Message = servicemodel.Message;
        }

        public bool IsHealthy { get; set; }

        public string Message { get; set; }
    }
}