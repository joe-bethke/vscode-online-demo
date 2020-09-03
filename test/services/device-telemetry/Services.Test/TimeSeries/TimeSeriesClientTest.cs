// <copyright file="TimeSeriesClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.TimeSeries;
using Mmm.Iot.Common.Services.Http;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.DeviceTelemetry.Services.Test.TimeSeries
{
    public class TimeSeriesClientTest
    {
        private readonly Mock<ILogger<TimeSeriesClient>> logger;
        private readonly Mock<IHttpClient> httpClient;
        private Mock<AppConfig> config;
        private TimeSeriesClient client;

        public TimeSeriesClientTest()
        {
            this.logger = new Mock<ILogger<TimeSeriesClient>>();
            this.config = new Mock<AppConfig> { DefaultValue = DefaultValue.Mock };
            this.httpClient = new Mock<IHttpClient>();
            this.client = new TimeSeriesClient(
                this.httpClient.Object,
                this.config.Object,
                this.logger.Object);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task QueryThrowsInvalidConfiguration_WhenConfigValuesAreNull()
        {
            // Arrange
            this.SetupClientWithNullConfigValues();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidConfigurationException>(() =>
                 this.client.QueryEventsAsync(null, null, "desc", 0, 1000, new string[0]));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task PingReturns_ServiceDisabled_WhenConfigValuesAreNull()
        {
            // Arrange
            this.SetupClientWithNullConfigValues();

            // Act
            var result = await this.client.StatusAsync();

            // Assert
            Assert.True(result.IsHealthy);
            Assert.Contains("Service disabled not in use", result.Message);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task QueryThrows_IfInvalidAuthParams()
        {
            // Arrange
            this.SetupClientWithConfigValues();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidConfigurationException>(() =>
                this.client.QueryEventsAsync(null, null, "desc", 0, 1000, new string[0]));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task QueryThrowsInvalidConfigurationForTopMessage_WhenConfigValuesAreNull()
        {
            // Arrange
            this.SetupClientWithNullConfigValues();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidConfigurationException>(() =>
                 this.client.QueryEventsAsync(1000, string.Empty));
        }

        private void SetupClientWithNullConfigValues()
        {
            this.config = new Mock<AppConfig> { DefaultValue = DefaultValue.Mock };
            this.client = new TimeSeriesClient(
                this.httpClient.Object,
                this.config.Object,
                this.logger.Object);
        }

        private void SetupClientWithConfigValues()
        {
            this.config.Setup(f => f.DeviceTelemetryService.TimeSeries.TsiDataAccessFqdn).Returns("test123");
            this.config.Setup(f => f.DeviceTelemetryService.TimeSeries.Audience).Returns("test123");
            this.config.Setup(f => f.DeviceTelemetryService.TimeSeries.ApiVersion).Returns("2016-12-12-test");
            this.config.Setup(f => f.DeviceTelemetryService.TimeSeries.Timeout).Returns("PT20S");
            this.config.Setup(f => f.Global.AzureActiveDirectory.TenantId).Returns("test123");
            this.config.Setup(f => f.Global.AzureActiveDirectory.AppId).Returns("test123");
            this.config.Setup(f => f.Global.AzureActiveDirectory.AppSecret).Returns("test123");
            this.config.Setup(f => f.DeviceTelemetryService.TimeSeries.Authority).Returns("https://login.testing.net/");
            this.config.Setup(f => f.DeviceTelemetryService.Messages.TelemetryStorageType).Returns("tsi");

            this.client = new TimeSeriesClient(
                this.httpClient.Object,
                this.config.Object,
                this.logger.Object);
        }
    }
}