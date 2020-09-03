// <copyright file="ApplicationInsightsHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace Mmm.Iot.Common.Services.Helpers
{
    public class ApplicationInsightsHelper
    {
        private TelemetryClient client;

        public ApplicationInsightsHelper()
        {
        }

        public void Initialize(TelemetryClient telemetryClient)
        {
            this.client = telemetryClient;
        }

        public void LogCustomEvent(string message, Dictionary<string, string> traceDetails)
        {
            try
            {
                this.client.TrackEvent(message, traceDetails);
                this.client.Flush();
            }
            catch (Exception)
            {
            }
        }
    }
}