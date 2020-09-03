// <copyright file="CustomEvent.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public class CustomEvent
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

        public CustomEvent()
        {
        }

        public CustomEvent(
                    string eventSource,
                    string eventType,
                    string eventTime,
                    string eventDescription,
                    string tenantId,
                    string deviceGroup,
                    string packageId,
                    string packageName,
                    string packageType,
                    string userId,
                    string deploymentName,
                    string deploymentId)
        {
            this.EventSource = eventSource;
            this.EventType = eventType;
            this.TenantId = tenantId;
            this.EventDescription = eventDescription;
            this.DeviceGroup = deviceGroup;
            this.PackageId = packageId;
            this.PackageName = packageName;
            this.UserId = userId;
            this.DeploymentName = deploymentName;
            this.EventTime = eventTime;
            this.PackageType = packageType;
            this.DeploymentId = deploymentId;
        }

        [JsonProperty(PropertyName = "eventSource")]
        public string EventSource { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventTime")]
        public string EventTime { get; set; } = DateTimeOffset.UtcNow.ToString(DateFormat);

        [JsonProperty(PropertyName = "eventDescription")]
        public string EventDescription { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "deviceGroup")]
        public string DeviceGroup { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "packageId")]
        public string PackageId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "packageName")]
        public string PackageName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "packageType")]
        public string PackageType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "deploymentId")]
        public string DeploymentId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "deploymentName")]
        public string DeploymentName { get; set; } = string.Empty;
    }
}