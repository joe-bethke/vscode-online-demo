// <copyright file="TenantReadyControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.TenantManager.Services;
using Mmm.Iot.TenantManager.WebService.Controllers;
using Moq;
using Xunit;

namespace Mmm.Iot.TenantManager.WebService.Test.Controllers
{
    public class TenantReadyControllerTest : IDisposable
    {
        private const string TenantId = "TenantId";
        private readonly Mock<ILogger<TenantReadyController>> mockLogger;
        private readonly Mock<ITenantContainer> mockTenantContainer;
        private readonly TenantReadyController controller;
        private bool disposedValue = false;

        public TenantReadyControllerTest()
        {
            this.mockLogger = new Mock<ILogger<TenantReadyController>>();
            this.mockTenantContainer = new Mock<ITenantContainer>();
            this.controller = new TenantReadyController(this.mockTenantContainer.Object, this.mockLogger.Object);
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            // Arrange
            this.mockTenantContainer.Setup(x => x.TenantIsReadyAsync(It.IsAny<string>()))
                                            .ReturnsAsync(true);

            // Act
            var result = await this.controller.GetAsync(TenantId);

            // Assert
            Assert.True(result);
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