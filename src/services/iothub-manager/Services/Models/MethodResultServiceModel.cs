// <copyright file="MethodResultServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Devices;

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public class MethodResultServiceModel
    {
        public MethodResultServiceModel()
        {
        }

        public MethodResultServiceModel(CloudToDeviceMethodResult result)
        {
            this.Status = result.Status;
            this.JsonPayload = result.GetPayloadAsJson();
        }

        public int Status { get; set; }

        public string JsonPayload { get; set; }
    }
}