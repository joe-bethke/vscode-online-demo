// <copyright file="MethodResultApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class MethodResultApiModel : MethodResultServiceModel
    {
        public MethodResultApiModel(MethodResultServiceModel model)
        {
            this.Status = model.Status;
            this.JsonPayload = model.JsonPayload;
        }
    }
}