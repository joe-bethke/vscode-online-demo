// <copyright file="DevicePropertiesControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.IoTHubManager.Services;
using Mmm.Iot.IoTHubManager.WebService.Controllers;
using Mmm.Iot.IoTHubManager.WebService.Models;
using Moq;
using Xunit;

namespace Mmm.Iot.IoTHubManager.WebService.Test.Controllers
{
    public class DevicePropertiesControllerTest : IDisposable
    {
        private readonly DevicePropertiesController controller;
        private readonly Mock<IDeviceProperties> devicePropertiesMock;
        private bool disposedValue = false;

        public DevicePropertiesControllerTest()
        {
            this.devicePropertiesMock = new Mock<IDeviceProperties>();
            this.controller = new DevicePropertiesController(this.devicePropertiesMock.Object);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetPropertiesReturnExpectedResponse()
        {
            // Arrange
            this.devicePropertiesMock.Setup(x => x.GetListAsync()).ReturnsAsync(this.CreateFakeList());
            DevicePropertiesApiModel expectedModel = new DevicePropertiesApiModel(this.CreateFakeList());

            // Act
            DevicePropertiesApiModel model = await this.controller.GetAsync();

            // Assert
            this.devicePropertiesMock.Verify(x => x.GetListAsync(), Times.Once);
            Assert.NotNull(model);
            Assert.Equal(model.Metadata.Keys, expectedModel.Metadata.Keys);
            foreach (string key in model.Metadata.Keys)
            {
                Assert.Equal(model.Metadata[key], expectedModel.Metadata[key]);
            }

            // Assert model and expected model have same items
            Assert.Empty(model.Items.Except(expectedModel.Items));
            Assert.Empty(expectedModel.Items.Except(model.Items));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetPropertiesThrowsException_IfDevicePropertiesThrowsException()
        {
            // Arrange
            this.devicePropertiesMock.Setup(x => x.GetListAsync()).Throws<ExternalDependencyException>();

            // Act - Assert
            await Assert.ThrowsAsync<ExternalDependencyException>(() => this.controller.GetAsync());
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

        private List<string> CreateFakeList()
        {
            return new List<string>
            {
                "property1",
                "property2",
                "property3",
                "property4",
            };
        }
    }
}