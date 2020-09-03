// <copyright file="MessagesTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.Services.External.TimeSeries;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Mmm.Iot.DeviceTelemetry.Services.Test
{
    public class MessagesTest
    {
        private const int SKIP = 0;
        private const int LIMIT = 1000;

        private readonly Mock<IStorageClient> storageClient;
        private readonly Mock<ITimeSeriesClient> timeSeriesClient;
        private readonly Mock<ILogger<Messages>> logger;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<IAppConfigurationClient> appConfigHelper;
        private readonly IMessages messages;

        public MessagesTest()
        {
            var servicesConfig = new AppConfig()
            {
                DeviceTelemetryService = new DeviceTelemetryServiceConfig
                {
                    Messages = new MessagesConfig
                    {
                        Database = "database",
                        TelemetryStorageType = "tsi",
                    },
                },
            };
            this.storageClient = new Mock<IStorageClient>();
            this.timeSeriesClient = new Mock<ITimeSeriesClient>();
            this.httpContextAccessor = new Mock<IHttpContextAccessor>();
            this.appConfigHelper = new Mock<IAppConfigurationClient>();
            this.logger = new Mock<ILogger<Messages>>();
            this.messages = new Messages(
                servicesConfig,
                this.storageClient.Object,
                this.timeSeriesClient.Object,
                this.logger.Object,
                this.httpContextAccessor.Object,
                this.appConfigHelper.Object);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task InitialListIsEmptyAsync()
        {
            // Arrange
            this.ThereAreNoMessagesInStorage();
            var devices = new string[] { "device1" };

            // Act
            var list = await this.messages.ListAsync(null, null, "asc", SKIP, LIMIT, devices);

            // Assert
            Assert.Empty(list.Messages);
            Assert.Empty(list.Properties);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetListWithValuesAsync()
        {
            // Arrange
            this.ThereAreSomeMessagesInStorage();
            var devices = new string[] { "device1" };

            // Act
            var list = await this.messages.ListAsync(null, null, "asc", SKIP, LIMIT, devices);

            // Assert
            Assert.NotEmpty(list.Messages);
            Assert.NotEmpty(list.Properties);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetTopDeviceListWithValuesAsync()
        {
            // Arrange
            this.RecentDeviceMessagesInStorage();
            var devices = new string[] { "device1" };

            // Act
            var list = await this.messages.ListTopDeviceMessagesAsync(LIMIT, devices[0]);

            // Assert
            Assert.NotEmpty(list.Messages);
            Assert.NotEmpty(list.Properties);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task ThrowsOnInvalidInput()
        {
            // Arrange
            var xssString = "<body onload=alert('test1')>";
            var xssList = new List<string>
            {
                "<body onload=alert('test1')>",
                "<IMG SRC=j&#X41vascript:alert('test2')>",
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.messages.ListAsync(null, null, xssString, 0, LIMIT, xssList.ToArray()));
        }

        private void ThereAreNoMessagesInStorage()
        {
            this.timeSeriesClient.Setup(x => x.QueryEventsAsync(null, null, It.IsAny<string>(), SKIP, LIMIT, It.IsAny<string[]>()))
                .ReturnsAsync(new MessageList());
        }

        private void ThereAreSomeMessagesInStorage()
        {
            var sampleMessages = new List<Message>();
            var sampleProperties = new List<string>();

            var data = new JObject
            {
                { "data.sample_unit", "mph" },
                { "data.sample_speed", "10" },
            };

            sampleMessages.Add(new Message("id1", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), data));
            sampleMessages.Add(new Message("id2", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), data));

            sampleProperties.Add("data.sample_unit");
            sampleProperties.Add("data.sample_speed");

            this.timeSeriesClient.Setup(x => x.QueryEventsAsync(null, null, It.IsAny<string>(), SKIP, LIMIT, It.IsAny<string[]>()))
                .ReturnsAsync(new MessageList(sampleMessages, sampleProperties));
        }

        private void RecentDeviceMessagesInStorage()
        {
            var sampleMessages = new List<Message>();
            var sampleProperties = new List<string>();

            var data = new JObject
            {
                { "data.sample_unit", "mph" },
                { "data.sample_speed", "10" },
            };

            sampleMessages.Add(new Message("id1", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), data));
            sampleMessages.Add(new Message("id2", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), data));

            sampleProperties.Add("data.sample_unit");
            sampleProperties.Add("data.sample_speed");

            this.timeSeriesClient.Setup(x => x.QueryEventsAsync(LIMIT, It.IsAny<string>()))
                .ReturnsAsync(new MessageList(sampleMessages, sampleProperties));
        }
    }
}