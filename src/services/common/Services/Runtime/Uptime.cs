// <copyright file="Uptime.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Runtime
{
    public static class Uptime
    {
        public static DateTimeOffset Start { get; } = DateTimeOffset.UtcNow;

        public static TimeSpan Duration => DateTimeOffset.UtcNow.Subtract(Start);

        public static string ProcessId { get; } = "WebService." + Guid.NewGuid();
    }
}