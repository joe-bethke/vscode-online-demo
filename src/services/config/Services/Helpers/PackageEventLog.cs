// <copyright file="PackageEventLog.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.ApplicationInsights;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Config.Services.Models;

namespace Mmm.Iot.Config.Services.Helpers
{
    public class PackageEventLog : CustomEventLogHelper, IPackageEventLog
    {
        public PackageEventLog(
            AppConfig config,
            TelemetryClient telemetry,
            ApplicationInsightsHelper applicationInsightsHelper)
            : base(config, telemetry, applicationInsightsHelper)
        {
        }

        public void LogPackageUpload(PackageServiceModel package, string tenantId, string userId)
        {
            var eventInfo = new CustomEvent
            {
                EventSource = Convert.ToString(EventSource.Config),
                EventType = Convert.ToString(EventType.PackageUpload),
                EventTime = DateTimeOffset.UtcNow.ToString(AppInsightDateFormat),
                EventDescription = $@"Package {package.Name} of {package.PackageType} uploaded successfully by User {userId}",
                TenantId = tenantId,
                PackageId = package.Id,
                PackageName = package.Name,
                PackageType = package.PackageType.ToString(),
                UserId = userId,
            };

            this.LogCustomEvent($"{tenantId.Substring(0, 8)}-{package.Id}", package.Name, eventInfo);
        }

        public void LogPackageDelete(string packageId, string tenantId, string userId)
        {
            var eventInfo = new CustomEvent
            {
                EventSource = Convert.ToString(EventSource.Config),
                EventType = Convert.ToString(EventType.PackageDelete),
                EventTime = DateTimeOffset.UtcNow.ToString(AppInsightDateFormat),
                EventDescription = $@"Package {packageId} deleted by User {userId}",
                TenantId = tenantId,
                PackageId = packageId,
                UserId = userId,
            };

            this.LogCustomEvent($"{tenantId.Substring(0, 8)}-{packageId}", packageId, eventInfo);
        }
    }
}