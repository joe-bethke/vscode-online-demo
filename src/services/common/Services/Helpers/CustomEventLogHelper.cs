// <copyright file="CustomEventLogHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Helpers
{
    public class CustomEventLogHelper : ICustomEventLogHelper
    {
        public const string AppInsightDateFormat = "yyyy-MM-dd HH:mm:ss";

        private readonly ApplicationInsightsHelper applicationInsightsHelper;
        private readonly TelemetryClient telemetryClient;

        public CustomEventLogHelper(AppConfig config, TelemetryClient telemetry, ApplicationInsightsHelper applicationInsightsHelper)
        {
            this.telemetryClient = telemetry;
            this.telemetryClient.InstrumentationKey = config.Global.InstrumentationKey;
            this.applicationInsightsHelper = applicationInsightsHelper;
            this.applicationInsightsHelper.Initialize(telemetry);
        }

        public void LogCustomEvent(string logDescription, string logHeaderField, CustomEvent eventInfo)
        {
            string serializedEvent = JsonConvert.SerializeObject(
                eventInfo,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });

            var logEvent = new Dictionary<string, string>
            {
                { logHeaderField, serializedEvent },
            };

            this.applicationInsightsHelper.LogCustomEvent(logDescription, logEvent);
        }
    }
}