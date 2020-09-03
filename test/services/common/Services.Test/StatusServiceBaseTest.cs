// <copyright file="StatusServiceBaseTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Mmm.Iot.Common.Services.Auth;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Common.Services.Test.Models;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.Common.Services.Test
{
    public class StatusServiceBaseTest
    {
        private readonly Mock<AppConfig> mockAppConfig = new Mock<AppConfig>(MockBehavior.Default);
        private readonly StatusServiceTest statusService;

        public StatusServiceBaseTest()
        {
            this.mockAppConfig
                .Setup(t => t.Global.AuthRequired)
                .Returns(true);
            this.mockAppConfig
                .Setup(t => t.ASPNETCORE_URLS)
                .Returns("some_url");
            this.statusService = new StatusServiceTest(this.mockAppConfig.Object);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task CallsDependencies()
        {
            // Arrange

            // Act
            var ping = this.statusService.Ping();
            var status = await this.statusService.GetStatusAsync();

            // Assert
            Assert.Equal(200, ((StatusCodeResult)ping).StatusCode);
            Assert.Contains("Test Service 1", status.Dependencies.Keys);
            Assert.Contains("Test Service 2", status.Dependencies.Keys);
            Assert.Contains("Test Service 3", status.Dependencies.Keys);
            Assert.True(status.Dependencies.Values.First().IsHealthy);
            Assert.False(status.Dependencies["Test Service 3"].IsHealthy);
        }
    }
}