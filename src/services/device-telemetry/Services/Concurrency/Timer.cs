// <copyright file="Timer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Mmm.Iot.DeviceTelemetry.Services.Concurrency
{
    public class Timer : ITimer, IDisposable
    {
        private readonly ILogger logger;
        private bool disposedValue = false;
        private System.Threading.Timer timer;
        private int frequency;

        public Timer(ILogger<Timer> logger)
        {
            this.logger = logger;
            this.frequency = 0;
        }

        public ITimer Setup(Action<object> action, object context, TimeSpan frequency)
        {
            return this.Setup(action, context, (int)frequency.TotalMilliseconds);
        }

        public ITimer Setup(Action<object> action, object context, int frequency)
        {
            this.frequency = frequency;
            this.timer = new System.Threading.Timer(
                new TimerCallback(action),
                context,
                Timeout.Infinite,
                this.frequency);
            return this;
        }

        public ITimer Start()
        {
            return this.StartIn(TimeSpan.Zero);
        }

        public ITimer StartIn(TimeSpan delay)
        {
            string errorMessage = "The timer is not initialized";
            if (this.timer == null)
            {
                this.logger.LogError(new Exception(errorMessage), errorMessage);
                throw new TimerNotInitializedException();
            }

            this.timer.Change((int)delay.TotalMilliseconds, this.frequency);
            return this;
        }

        public void Stop()
        {
            this.timer?.Change(Timeout.Infinite, Timeout.Infinite);
            this.timer?.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.Stop();
                }

                this.disposedValue = true;
            }
        }
    }
}