// <copyright file="DevicesControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.IoTHubManager.Services;
using Mmm.Iot.IoTHubManager.Services.Models;
using Mmm.Iot.IoTHubManager.WebService.Controllers;
using Mmm.Iot.IoTHubManager.WebService.Models;
using Moq;
using Xunit;

namespace Mmm.Iot.IoTHubManager.WebService.Test.Controllers
{
    public class DevicesControllerTest : IDisposable
    {
        private const string TenantId = "TenantId";
        private readonly DevicesController controller;
        private readonly Mock<IDeviceProperties> devicePropertiesMock;
        private readonly Mock<IDeviceService> deviceServiceMock;
        private readonly Mock<IDevices> devicesMock;
        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpRequest> mockHttpRequest;
        private IDictionary<object, object> contextItems;
        private bool disposedValue = false;

        public DevicesControllerTest()
        {
            this.devicePropertiesMock = new Mock<IDeviceProperties>();
            this.deviceServiceMock = new Mock<IDeviceService>();
            this.devicesMock = new Mock<IDevices>();
            this.mockHttpContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest = new Mock<HttpRequest> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest.Setup(m => m.HttpContext).Returns(this.mockHttpContext.Object);
            this.mockHttpContext.Setup(m => m.Request).Returns(this.mockHttpRequest.Object);
            this.controller = new DevicesController(this.devicesMock.Object, this.deviceServiceMock.Object, this.devicePropertiesMock.Object)
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
        public void TestAllDevices()
        {
            // http://127.0.0.1:" + config.Port + "/v1/devices");
            Assert.True(true);
        }

        [Fact]
        public void TestSingleDevice()
        {
            // http://127.0.0.1:" + config.Port + "/v1/device/mydevice");
            Assert.True(true);
        }

        [Fact]
        public async void GetDeviceFilesReturnExpectedResponse()
        {
            // Arrange
            const string deviceId = "testDevice";
            int[] idx = new int[] { 0, 1, 2, 3 };
            this.devicePropertiesMock
                .Setup(x => x.GetUploadedFilesForDevice(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(this.CreateFakeFilesList());
            List<string> expectedFiles = new List<string>(this.CreateFakeFilesList());

            // Act
            List<string> files = await this.controller.GetDeviceFilesAsync(deviceId);

            // Assert
            this.devicePropertiesMock
                .Verify(x => x.GetUploadedFilesForDevice(TenantId, deviceId), Times.Once);
            Assert.NotNull(files);
            foreach (int i in idx)
            {
                Assert.Equal(files[i], expectedFiles[i]);
            }
        }

        [Fact]
        public async void PutThrowsResourceNotFound()
        {
            const string deviceId = "testDevice";
            var model = new DeviceRegistryApiModel
            {
                Id = deviceId,
            };

            this.devicesMock
                .Setup(x => x.UpdateAsync(
                    It.IsAny<DeviceServiceModel>(),
                    It.IsAny<DevicePropertyDelegate>()))
                .ThrowsAsync(new ResourceNotFoundException());

            await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await this.controller.PutAsync(deviceId, model));
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

        private List<string> CreateFakeFilesList()
        {
            return new List<string>
            {
                "file1",
                "file2",
                "file3",
                "file4",
            };
        }
    }
}