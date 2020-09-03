// <copyright file="DeviceJobServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Azure.Devices;

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public class DeviceJobServiceModel
    {
        public DeviceJobServiceModel(DeviceJob deviceJob)
        {
            this.DeviceId = deviceJob.DeviceId;

            switch (deviceJob.Status)
            {
                case Microsoft.Azure.Devices.DeviceJobStatus.Pending:
                    this.Status = DeviceJobStatus.Pending;
                    break;

                case Microsoft.Azure.Devices.DeviceJobStatus.Scheduled:
                    this.Status = DeviceJobStatus.Scheduled;
                    break;

                case Microsoft.Azure.Devices.DeviceJobStatus.Running:
                    this.Status = DeviceJobStatus.Running;
                    break;

                case Microsoft.Azure.Devices.DeviceJobStatus.Completed:
                    this.Status = DeviceJobStatus.Completed;
                    break;

                case Microsoft.Azure.Devices.DeviceJobStatus.Failed:
                    this.Status = DeviceJobStatus.Failed;
                    break;

                case Microsoft.Azure.Devices.DeviceJobStatus.Canceled:
                    this.Status = DeviceJobStatus.Canceled;
                    break;
            }

            this.StartTimeUtc = deviceJob.StartTimeUtc;
            this.EndTimeUtc = deviceJob.EndTimeUtc;
            this.CreatedDateTimeUtc = deviceJob.CreatedDateTimeUtc;
            this.LastUpdatedDateTimeUtc = deviceJob.LastUpdatedDateTimeUtc;

            if (deviceJob.Outcome?.DeviceMethodResponse != null)
            {
                this.Outcome = new MethodResultServiceModel(deviceJob.Outcome.DeviceMethodResponse);
            }

            if (deviceJob.Error != null)
            {
                this.Error = new DeviceJobErrorServiceModel(deviceJob.Error);
            }
        }

        public string DeviceId { get; }

        public DeviceJobStatus Status { get; }

        public DateTime StartTimeUtc { get; }

        public DateTime EndTimeUtc { get; }

        public DateTime CreatedDateTimeUtc { get; }

        public DateTime LastUpdatedDateTimeUtc { get; }

        public MethodResultServiceModel Outcome { get; }

        public DeviceJobErrorServiceModel Error { get; }
    }
}