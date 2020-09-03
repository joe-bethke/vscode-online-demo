// <copyright file="ITimer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.DeviceTelemetry.Services.Concurrency
{
    public interface ITimer
    {
        ITimer Start();

        ITimer StartIn(TimeSpan delay);

        void Stop();

        ITimer Setup(Action<object> action, object context, TimeSpan frequency);

        ITimer Setup(Action<object> action, object context, int frequency);
    }
}