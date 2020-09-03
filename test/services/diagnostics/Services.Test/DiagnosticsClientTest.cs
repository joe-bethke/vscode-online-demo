// <copyright file="DiagnosticsClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.Diagnostics.Services.Exceptions;
using Mmm.Iot.Diagnostics.Services.Models;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Mmm.Iot.Diagnostics.Services.Test
{
    public class DiagnosticsClientTest
    {
        private readonly Random rand;
        private readonly Mock<ILogger<DiagnosticsClient>> mockLogger;
        private readonly Mock<DiagnosticsClient> mockClient;
        private DiagnosticsClient client;

        public DiagnosticsClientTest()
        {
            this.mockLogger = new Mock<ILogger<DiagnosticsClient>>();
            this.mockClient = new Mock<DiagnosticsClient>(this.mockLogger.Object);
            this.client = this.mockClient.Object;
            this.rand = new Random();
        }

        [Fact]
        public void LogDiagnosticsEventTest()
        {
            DiagnosticsEventModel eventModel = new DiagnosticsEventModel
            {
                EventType = this.rand.NextString(),
                SessionId = this.rand.NextString(),
                EventProperties = new
                    {
                        id = this.rand.NextString(),
                        test = this.rand.NextString(),
                    },
            };

            this.mockClient
                .Setup(x => x.Log(
                    It.IsAny<string>()))
                .Verifiable();

            this.client.LogDiagnosticsEvent(eventModel);

            this.mockClient
                .Verify(
                    x => x.Log(
                        It.Is<string>(m => m == JsonConvert.SerializeObject(eventModel, Formatting.Indented))),
                    Times.Once);
        }

        [Fact]
        public void LogDiagnosticsEventThrowsWhenLoggingFailsTest()
        {
            DiagnosticsEventModel eventModel = new DiagnosticsEventModel
            {
                EventType = this.rand.NextString(),
                SessionId = this.rand.NextString(),
                EventProperties = new
                    {
                        id = this.rand.NextString(),
                        test = this.rand.NextString(),
                    },
            };

            this.mockClient
                .Setup(x => x.Log(
                    It.IsAny<string>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => this.client.LogDiagnosticsEvent(eventModel));
        }

        [Fact]
        public void LogDiagnosticsEventThrowsWhenEmptyTest()
        {
            DiagnosticsEventModel blankModel = new DiagnosticsEventModel();

            Assert.Throws<BadDiagnosticsEventException>(() => this.client.LogDiagnosticsEvent(blankModel));
        }
    }
}