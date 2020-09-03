// <copyright file="StreamAnalyticsJobModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.TenantManager.Services.Models
{
    public class StreamAnalyticsJobModel
    {
        public string TenantId { get; set; }

        public string StreamAnalyticsJobName { get; set; }

        public string JobState { get; set; }

        public bool IsActive { get; set; }
    }
}