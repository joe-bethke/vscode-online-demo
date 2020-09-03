// <copyright file="MockTelemetryChannel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights.Channel;

namespace Mmm.Iot.Common.TestHelpers
{
    public class MockTelemetryChannel : ITelemetryChannel
    {
        private readonly ConcurrentBag<ITelemetry> sentTelemtries = new ConcurrentBag<ITelemetry>();

        public bool IsFlushed { get; private set; }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

        public void Send(ITelemetry item)
        {
            this.sentTelemtries.Add(item);
        }

        public void Flush()
        {
            this.IsFlushed = true;
        }

        public void Dispose()
        {
        }
    }
}