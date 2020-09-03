// <copyright file="JobServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Devices;

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public class JobServiceModel
    {
        public JobServiceModel()
        {
        }

        public JobServiceModel(JobResponse jobResponse, IEnumerable<DeviceJob> deviceJobs = null)
        {
            this.JobId = jobResponse.JobId;
            this.QueryCondition = jobResponse.QueryCondition;
            this.CreatedTimeUtc = jobResponse.CreatedTimeUtc;
            this.StartTimeUtc = jobResponse.StartTimeUtc;
            this.MaxExecutionTimeInSeconds = jobResponse.MaxExecutionTimeInSeconds;

            switch (jobResponse.Type)
            {
                case Microsoft.Azure.Devices.JobType.ScheduleDeviceMethod:
                case Microsoft.Azure.Devices.JobType.ScheduleUpdateTwin:
                    this.Type = (JobType)jobResponse.Type;
                    break;
                default:
                    this.Type = JobType.Unknown;
                    break;
            }

            switch (jobResponse.Status)
            {
                case Microsoft.Azure.Devices.JobStatus.Completed:
                case Microsoft.Azure.Devices.JobStatus.Failed:
                case Microsoft.Azure.Devices.JobStatus.Cancelled:
                    // If job is complete return end time
                    this.EndTimeUtc = jobResponse.EndTimeUtc;
                    this.Status = (JobStatus)jobResponse.Status;
                    break;
                case Microsoft.Azure.Devices.JobStatus.Enqueued:
                case Microsoft.Azure.Devices.JobStatus.Queued:
                case Microsoft.Azure.Devices.JobStatus.Running:
                case Microsoft.Azure.Devices.JobStatus.Scheduled:
                    // IoT Hub will return a date of 12/30/9999 if job hasn't completed yet
                    this.EndTimeUtc = null;
                    this.Status = (JobStatus)jobResponse.Status;
                    break;
                default:
                    this.Status = JobStatus.Unknown;
                    break;
            }

            if (jobResponse.CloudToDeviceMethod != null)
            {
                this.MethodParameter = new MethodParameterServiceModel(jobResponse.CloudToDeviceMethod);
            }

            if (jobResponse.UpdateTwin != null)
            {
                this.UpdateTwin = new TwinServiceModel(jobResponse.UpdateTwin);
            }

            this.FailureReason = jobResponse.FailureReason;
            this.StatusMessage = jobResponse.StatusMessage;

            if (jobResponse.DeviceJobStatistics != null)
            {
                this.ResultStatistics = new JobStatistics(jobResponse.DeviceJobStatistics);
            }

            this.Devices = deviceJobs?.Select(j => new DeviceJobServiceModel(j));
        }

        public string JobId { get; set; }

        public string QueryCondition { get; set; }

        public DateTime? CreatedTimeUtc { get; set; }

        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        public long MaxExecutionTimeInSeconds { get; set; }

        public JobType Type { get; set; }

        public JobStatus Status { get; set; }

        public MethodParameterServiceModel MethodParameter { get; set; }

        public TwinServiceModel UpdateTwin { get; set; }

        public string FailureReason { get; set; }

        public string StatusMessage { get; set; }

        public JobStatistics ResultStatistics { get; set; }

        public IEnumerable<DeviceJobServiceModel> Devices { get; }

        public static Microsoft.Azure.Devices.JobType? ToJobTypeAzureModel(JobType? jobType)
        {
            if (!jobType.HasValue)
            {
                return null;
            }

            switch (jobType.Value)
            {
                case JobType.ScheduleDeviceMethod:
                case JobType.ScheduleUpdateTwin:
                    return (Microsoft.Azure.Devices.JobType)jobType.Value;
                default:
                    return (Microsoft.Azure.Devices.JobType)JobType.Unknown;
            }
        }

        public static Microsoft.Azure.Devices.JobStatus? ToJobStatusAzureModel(JobStatus? jobStatus)
        {
            if (!jobStatus.HasValue)
            {
                return null;
            }

            switch (jobStatus.Value)
            {
                case JobStatus.Enqueued:
                case JobStatus.Running:
                case JobStatus.Completed:
                case JobStatus.Failed:
                case JobStatus.Cancelled:
                case JobStatus.Scheduled:
                case JobStatus.Queued:
                    return (Microsoft.Azure.Devices.JobStatus)jobStatus.Value;
                default:
                    return Microsoft.Azure.Devices.JobStatus.Unknown;
            }
        }
    }
}