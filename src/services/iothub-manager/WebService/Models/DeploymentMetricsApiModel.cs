// <copyright file="DeploymentMetricsApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.IoTHubManager.Services;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.WebService.Models
{
    public class DeploymentMetricsApiModel
    {
        private const string AppliedMetricsKey = "appliedCount";
        private const string TargetedMetricsKey = "targetedCount";
        private const string SuccessfulMetricsKey = "reportedSuccessfulCount";
        private const string FailedMetricsKey = "reportedFailedCount";
        private const string PendingMetricsKey = "pendingCount";

        public DeploymentMetricsApiModel(DeploymentMetricsServiceModel metricsServiceModel)
        {
            this.SystemMetrics = new Dictionary<string, long>();

            this.SystemMetrics[AppliedMetricsKey] = 0;
            this.SystemMetrics[TargetedMetricsKey] = 0;

            if (metricsServiceModel == null)
            {
                return;
            }

            this.CustomMetrics = metricsServiceModel.CustomMetrics;
            this.SystemMetrics = metricsServiceModel.SystemMetrics != null && metricsServiceModel.SystemMetrics.Count > 0 ?
                metricsServiceModel.SystemMetrics : this.SystemMetrics;
            this.DeviceStatuses = metricsServiceModel.DeviceStatuses;

            if (metricsServiceModel.DeviceMetrics != null)
            {
                this.SystemMetrics[SuccessfulMetricsKey] =
                    metricsServiceModel.DeviceMetrics[DeploymentStatus.Succeeded];
                this.SystemMetrics[FailedMetricsKey] =
                    metricsServiceModel.DeviceMetrics[DeploymentStatus.Failed];
                this.SystemMetrics[PendingMetricsKey] =
                    metricsServiceModel.DeviceMetrics[DeploymentStatus.Pending];
            }

            if (this.CustomMetrics != null)
            {
                // Override System metrics if custom metric contain same metrics
                if (this.CustomMetrics.ContainsKey(SuccessfulMetricsKey))
                {
                    this.SystemMetrics[SuccessfulMetricsKey] =
                        this.CustomMetrics[SuccessfulMetricsKey];
                    this.CustomMetrics.Remove(SuccessfulMetricsKey);
                }

                if (this.CustomMetrics.ContainsKey(FailedMetricsKey))
                {
                    this.SystemMetrics[FailedMetricsKey] =
                        this.CustomMetrics[FailedMetricsKey];
                    this.CustomMetrics.Remove(FailedMetricsKey);
                }

                if (this.CustomMetrics.ContainsKey(PendingMetricsKey))
                {
                    this.SystemMetrics[PendingMetricsKey] =
                        this.CustomMetrics[PendingMetricsKey];
                    this.CustomMetrics.Remove(PendingMetricsKey);
                }
            }
        }

        [JsonProperty(PropertyName = "SystemMetrics")]
        public IDictionary<string, long> SystemMetrics { get; set; }

        [JsonProperty(PropertyName = "CustomMetrics")]
        public IDictionary<string, long> CustomMetrics { get; set; }

        [JsonProperty(PropertyName = "DeviceStatuses")]
        public IDictionary<string, DeploymentStatus> DeviceStatuses { get; set; }
    }
}