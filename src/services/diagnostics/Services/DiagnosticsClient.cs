// <copyright file="DiagnosticsClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Diagnostics.Services.Exceptions;
using Mmm.Iot.Diagnostics.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Diagnostics.Services
{
    public class DiagnosticsClient : IDiagnosticsClient
    {
        private readonly ILogger logger;

        public DiagnosticsClient(ILogger<DiagnosticsClient> logger)
        {
            this.logger = logger;
        }

        public void LogDiagnosticsEvent(DiagnosticsEventModel diagnosticsEvent)
        {
            if (diagnosticsEvent.IsEmpty())
            {
                throw new BadDiagnosticsEventException("The given DiagnosticsEventModel was null or had no content. Empty events will not be logged.");
            }

            try
            {
                string eventLog = JsonConvert.SerializeObject(diagnosticsEvent, Formatting.Indented);
                this.Log(eventLog);
            }
            catch (JsonSerializationException jse)
            {
                throw new BadDiagnosticsEventException(jse.Message, jse);
            }
        }

        public virtual void Log(string logMessage)
        {
            this.logger.LogInformation(logMessage);
        }
    }
}