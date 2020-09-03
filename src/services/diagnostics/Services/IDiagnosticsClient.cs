// <copyright file="IDiagnosticsClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Mmm.Iot.Diagnostics.Services.Models;

namespace Mmm.Iot.Diagnostics.Services
{
    public interface IDiagnosticsClient
    {
        void LogDiagnosticsEvent(DiagnosticsEventModel diagnosticsEvent);

        void Log(string logMessage);
    }
}