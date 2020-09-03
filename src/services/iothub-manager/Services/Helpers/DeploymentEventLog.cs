// <copyright file="DeploymentEventLog.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.ApplicationInsights;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.Services.Helpers
{
    public class DeploymentEventLog : CustomEventLogHelper, IDeploymentEventLog
    {
        public DeploymentEventLog(
            AppConfig config,
            TelemetryClient telemetry,
            ApplicationInsightsHelper applicationInsightsHelper)
            : base(config, telemetry, applicationInsightsHelper)
        {
        }

        public void LogDeploymentCreate(DeploymentServiceModel deployment, string tenantId, string userId)
        {
            var eventInfo = new CustomEvent
            {
                EventSource = Convert.ToString(EventSource.IotHubManager),
                EventType = Convert.ToString(EventType.DeploymentCreate),
                EventTime = DateTimeOffset.UtcNow.ToString(AppInsightDateFormat),
                EventDescription = $@"Deployment created by User {userId} for package {deployment.PackageName} targeting {deployment.DeviceGroupName} Devicegroup",
                TenantId = tenantId,
                PackageId = deployment.PackageId,
                PackageName = deployment.PackageName,
                DeviceGroup = deployment.DeviceGroupName,
                PackageType = deployment.PackageType.ToString(),
                UserId = userId,
                DeploymentId = deployment.Id,
                DeploymentName = deployment.Name,
            };

            this.LogCustomEvent($"{tenantId.Substring(0, 8)}-{deployment.Id}", deployment.Name, eventInfo);
            this.LogCustomEvent($"{tenantId.Substring(0, 8)}-{deployment.PackageId}", deployment.Name, eventInfo);
        }

        public void LogDeploymentDelete(string deploymentId, string tenantId, string userId)
        {
            var eventInfo = new CustomEvent
            {
                EventSource = Convert.ToString(EventSource.IotHubManager),
                EventType = Convert.ToString(EventType.DeploymentDelete),
                EventTime = DateTimeOffset.UtcNow.ToString(AppInsightDateFormat),
                EventDescription = $@"Deployment {deploymentId} deleted by User {userId}",
                TenantId = tenantId,
                UserId = userId,
                DeploymentId = deploymentId,
            };

            this.LogCustomEvent($"{tenantId.Substring(0, 8)}-{deploymentId}", deploymentId, eventInfo);
        }
    }
}