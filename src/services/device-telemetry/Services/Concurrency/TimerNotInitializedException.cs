// <copyright file="TimerNotInitializedException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.DeviceTelemetry.Services.Concurrency
{
    public class TimerNotInitializedException : Exception
    {
        public TimerNotInitializedException()
            : base("Timer object not initialized. Call 'Setup()' first.")
        {
        }
    }
}