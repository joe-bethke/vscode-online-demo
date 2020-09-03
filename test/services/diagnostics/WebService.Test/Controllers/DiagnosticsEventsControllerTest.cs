// <copyright file="DiagnosticsEventsControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.Diagnostics.Services;
using Mmm.Iot.Diagnostics.Services.Exceptions;
using Mmm.Iot.Diagnostics.Services.Models;
using Mmm.Iot.Diagnostics.WebService.Controllers;
using Moq;
using Xunit;

namespace Mmm.Iot.Diagnostics.WebService.Test.Controllers
{
    public class DiagnosticsEventsControllerTest : IDisposable
    {
        private const int PostResponseStatusCode = 201;

        private readonly Random rand;
        private readonly Mock<IDiagnosticsClient> mockClient;
        private DiagnosticsEventsController controller;
        private bool disposedValue = false;

        public DiagnosticsEventsControllerTest()
        {
            this.mockClient = new Mock<IDiagnosticsClient>();
            this.controller = new DiagnosticsEventsController(this.mockClient.Object);
            this.rand = new Random();
        }

        [Fact]
        public void PostTest()
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
                .Setup(x => x.LogDiagnosticsEvent(
                    It.IsAny<DiagnosticsEventModel>()))
                .Verifiable();

            StatusCodeResult response = this.controller.Post(eventModel);

            this.mockClient
                .Verify(
                    x => x.LogDiagnosticsEvent(
                        It.Is<DiagnosticsEventModel>(m => m.EventType == eventModel.EventType && m.SessionId == eventModel.SessionId && m.EventProperties == eventModel.EventProperties)),
                    Times.Once);

            Assert.Equal(PostResponseStatusCode, response.StatusCode);
        }

        [Fact]
        public void PostThrowsWhenBodyNullTest()
        {
            Assert.Throws<BadRequestException>(() => this.controller.Post(null));
        }

        [Fact]
        public void PostThrowsBadRequestWhenBadDiagnosticsEventExceptionTest()
        {
            this.mockClient
                .Setup(x => x.LogDiagnosticsEvent(
                    It.IsAny<DiagnosticsEventModel>()))
                .Throws(new BadDiagnosticsEventException());

            Assert.Throws<BadRequestException>(() => this.controller.Post(new DiagnosticsEventModel()));
        }

        [Fact]
        public void PostThrowsWhenClientFailsTest()
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
                .Setup(x => x.LogDiagnosticsEvent(
                    It.IsAny<DiagnosticsEventModel>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => this.controller.Post(eventModel));
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
                    this.controller.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}