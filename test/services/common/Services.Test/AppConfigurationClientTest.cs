// <copyright file="AppConfigurationClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.AppConfiguration;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.Common.Services.Test
{
    public class AppConfigurationClientTest
    {
        private const string MockConnectionString = @"Endpoint=https://abc.azconfig.io;Id=1:/1;Secret=1234";
        private readonly Mock<ConfigurationClient> client;
        private readonly Mock<AppConfig> mockConfig;
        private readonly Mock<Response> mockResponse;
        private readonly ConfigurationSetting configurationSetting;
        private readonly AppConfigurationClient appConfigClient;
        private readonly Mock<IConfigurationClientFactory> mockFactory;
        private readonly Random rand;
        private Dictionary<string, AppConfigCacheValue> cache = new Dictionary<string, AppConfigCacheValue>();

        public AppConfigurationClientTest()
        {
            this.mockConfig = new Mock<AppConfig>();
            this.mockConfig.Object.AppConfigurationConnectionString = MockConnectionString;
            this.client = new Mock<ConfigurationClient>(MockConnectionString);
            this.mockResponse = new Mock<Response>();
            this.configurationSetting = new ConfigurationSetting("test", "test");
            this.rand = new Random();
            this.mockFactory = new Mock<IConfigurationClientFactory>();
            this.mockFactory
                .Setup(x => x.Create())
                .Returns(this.client.Object);
            this.appConfigClient = new AppConfigurationClient(this.mockConfig.Object, this.mockFactory.Object);
        }

        [Fact]
        public async Task SetAppConfigByKeyAndValueTest()
        {
            string key = this.rand.NextString();
            string value = this.rand.NextString();
            Response<ConfigurationSetting> response = Response.FromValue(ConfigurationModelFactory.ConfigurationSetting("test", "test"), this.mockResponse.Object);
            this.client.Setup(c => c.SetConfigurationSettingAsync(It.IsAny<ConfigurationSetting>(), true, It.IsAny<CancellationToken>()))
            .Returns(Task<Response>.FromResult(response));

            await this.appConfigClient.SetValueAsync(key, value);
            Assert.True(true);
        }

        [Fact]
        public void GetAppConfigValueByKeyTest()
        {
            string key = this.rand.NextString();
            string value = this.rand.NextString();
            Response<ConfigurationSetting> response = Response.FromValue(ConfigurationModelFactory.ConfigurationSetting("test", "test"), this.mockResponse.Object);
            this.client.Setup(c => c.GetConfigurationSetting(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(response);

            string result = this.appConfigClient.GetValue(key);
            Assert.Equal(result, response.Value.Value);
        }

        [Fact]
        public async Task GetAppConfigStatusReturnsHealthyTest()
        {
            this.mockConfig.Setup(x => x.Global.Location).Returns("eastus");

            Response<ConfigurationSetting> response = Response.FromValue(ConfigurationModelFactory.ConfigurationSetting("test", "test"), this.mockResponse.Object);
            this.client.Setup(c => c.GetConfigurationSetting(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(response);

            StatusResultServiceModel result = await this.appConfigClient.StatusAsync();
            Assert.True(result.IsHealthy);
        }

        [Fact]
        public async Task GetAppConfigStatusReturnsUnhealthyOnExceptionTest()
        {
            Response<ConfigurationSetting> response = Response.FromValue(ConfigurationModelFactory.ConfigurationSetting(string.Empty, string.Empty), this.mockResponse.Object);
            this.client.Setup(c => c.GetConfigurationSettingAsync(string.Empty, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

            StatusResultServiceModel result = await this.appConfigClient.StatusAsync();
            Assert.False(result.IsHealthy);
        }

        [Fact]
        public async Task DeleteAppConfigKeyAsyncTest()
        {
            string key = this.rand.NextString();
            Response<string> response = Response.FromValue(key, this.mockResponse.Object);
            this.client
                .Setup(x => x.DeleteConfigurationSettingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task<Response>.FromResult(this.mockResponse.Object));

            await this.appConfigClient.DeleteKeyAsync(key);
            Assert.True(true);
        }
    }
}