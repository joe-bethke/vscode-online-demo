// <copyright file="AppInsightsExceptionHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Kubernetes.Debugging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Mmm.Iot.StorageAdapter.Services.Helpers
{
    public static class AppInsightsExceptionHelper
    {
        private static TelemetryConfiguration configuration;
        private static TelemetryClient client = null;
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        public static void Initialize(string instrumentationKey)
        {
            configuration = new TelemetryConfiguration(instrumentationKey);
            var observer = new ApplicationInsightsKubernetesDiagnosticObserver(DiagnosticLogLevel.Trace);
            ApplicationInsightsKubernetesDiagnosticSource.Instance.Observable.SubscribeWithAdapter(observer);

            configuration.AddApplicationInsightsKubernetesEnricher(applyOptions: null);

            // client = new TelemetryClient();
            // client.InstrumentationKey = instrumentationKey;
        }

        public static void LogException(Exception exception, Dictionary<string, string> traceDetails)
        {
            try
            {
                ExceptionTelemetry telemetry = new ExceptionTelemetry(exception);

                Type exceptionType = exception.GetType();
                if (exceptionType != null)
                {
                    foreach (PropertyInfo property in exceptionType.GetProperties())
                    {
                        telemetry.Properties[$"{exceptionType.Name}.{property.Name}"] = JsonConvert.SerializeObject(property.GetValue(exception), jsonSettings);
                    }

                    foreach (KeyValuePair<string, string> entry in traceDetails)
                    {
                        telemetry.Properties[entry.Key] = entry.Value;
                    }

                    telemetry.Message = exception.Message;
                    telemetry.Exception = exception;
                    client.TrackException(telemetry);
                    client.Flush();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void LogTrace(string message, int severity, Dictionary<string, string> traceDetails)
        {
            try
            {
                client.TrackTrace(message, (SeverityLevel)severity, traceDetails);
                client.Flush();
            }
            catch (Exception)
            {
            }
        }

        public static void LogCustomEvent(string message, Dictionary<string, string> traceDetails)
        {
            try
            {
                client.TrackEvent(message, traceDetails);
                client.Flush();
            }
            catch (Exception)
            {
            }
        }
    }
}