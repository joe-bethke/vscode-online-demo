// <copyright file="AlertingControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.TenantManager.Services;
using Mmm.Iot.TenantManager.Services.Models;
using Mmm.Iot.TenantManager.WebService.Controllers;
using Moq;
using Xunit;

namespace Mmm.Iot.TenantManager.WebService.Test.Controllers
{
    public class AlertingControllerTest : IDisposable
    {
        private const string TenantId = "TenantId";
        private readonly Mock<ILogger<AlertingController>> mockLogger;
        private readonly Mock<IAlertingContainer> mockAlertingContainer;
        private readonly AlertingController controller;
        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpRequest> mockHttpRequest;
        private IDictionary<object, object> contextItems;
        private bool disposedValue = false;

        public AlertingControllerTest()
        {
            this.mockLogger = new Mock<ILogger<AlertingController>>();
            this.mockAlertingContainer = new Mock<IAlertingContainer>();
            this.mockHttpContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest = new Mock<HttpRequest> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest.Setup(m => m.HttpContext).Returns(this.mockHttpContext.Object);
            this.mockHttpContext.Setup(m => m.Request).Returns(this.mockHttpRequest.Object);
            this.controller = new AlertingController(this.mockAlertingContainer.Object, this.mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = this.mockHttpContext.Object,
                },
            };
            this.contextItems = new Dictionary<object, object>
            {
                {
                    RequestExtension.ContextKeyTenantId, TenantId
                },
            };
            this.mockHttpContext.Setup(m => m.Items).Returns(this.contextItems);
        }

        [Fact]
        public async Task AddAlertingAsyncTest()
        {
            // Arrange
            StreamAnalyticsJobModel streamAnalyticsModel = new StreamAnalyticsJobModel()
            {
                TenantId = TenantId,
                StreamAnalyticsJobName = "StreamAnalyticsJob",
                JobState = "Job State",
                IsActive = true,
            };

            this.mockAlertingContainer.Setup(x => x.AddAlertingAsync(It.IsAny<string>()))
                                            .ReturnsAsync(streamAnalyticsModel);

            // Act
            var result = await this.controller.AddAlertingAsync();

            // Assert
            Assert.True(result.IsActive);
            Assert.Equal(result.TenantId, TenantId);
        }

        [Fact]
        public async Task RemoveAlertingAsyncTest()
        {
            // Arrange
            StreamAnalyticsJobModel streamAnalyticsModel = new StreamAnalyticsJobModel()
            {
                TenantId = TenantId,
                StreamAnalyticsJobName = "StreamAnalyticsJob",
                JobState = "Job State",
                IsActive = false,
            };

            this.mockAlertingContainer.Setup(x => x.RemoveAlertingAsync(It.IsAny<string>()))
                                            .ReturnsAsync(streamAnalyticsModel);

            // Act
            var result = await this.controller.RemoveAlertingAsync();

            // Assert
            Assert.False(result.IsActive);
            Assert.Equal(result.TenantId, TenantId);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public async Task GetAlertingAsyncTest(bool createIfNotExists, bool expectCreateNew)
        {
            // Arrange
            StreamAnalyticsJobModel streamAnalyticsModel = new StreamAnalyticsJobModel()
            {
                TenantId = TenantId,
                StreamAnalyticsJobName = "StreamAnalyticsJob",
                JobState = "Job State",
                IsActive = true,
            };
            this.mockAlertingContainer.Setup(x => x.AddAlertingAsync(It.IsAny<string>()))
                                            .ReturnsAsync(streamAnalyticsModel);

            this.mockAlertingContainer.Setup(x => x.GetAlertingAsync(It.IsAny<string>()))
                                            .ReturnsAsync(streamAnalyticsModel);

            // Act
            var result = await this.controller.GetAlertingAsync(createIfNotExists);

            // Assert
            if (createIfNotExists)
            {
                Assert.True(expectCreateNew);
            }
            else
            {
                Assert.False(expectCreateNew);
            }

            Assert.Equal(result.TenantId, TenantId);
        }

        [Fact]
        public async Task StartAsyncTest()
        {
            // Arrange
            StreamAnalyticsJobModel streamAnalyticsModel = new StreamAnalyticsJobModel()
            {
                TenantId = TenantId,
                StreamAnalyticsJobName = "StreamAnalyticsJob",
                JobState = "Job State",
                IsActive = true,
            };

            this.mockAlertingContainer.Setup(x => x.StartAlertingAsync(It.IsAny<string>()))
                                            .ReturnsAsync(streamAnalyticsModel);

            // Act
            var result = await this.controller.StartAsync();

            // Assert
            Assert.True(result.IsActive);
            Assert.Equal(result.TenantId, TenantId);
        }

        [Fact]
        public async Task StopAsyncTest()
        {
            // Arrange
            StreamAnalyticsJobModel streamAnalyticsModel = new StreamAnalyticsJobModel()
            {
                TenantId = TenantId,
                StreamAnalyticsJobName = "StreamAnalyticsJob",
                JobState = "Job State",
                IsActive = false,
            };

            this.mockAlertingContainer.Setup(x => x.StopAlertingAsync(It.IsAny<string>()))
                                            .ReturnsAsync(streamAnalyticsModel);

            // Act
            var result = await this.controller.StopAsync();

            // Assert
            Assert.False(result.IsActive);
            Assert.Equal(result.TenantId, TenantId);
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