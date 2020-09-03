// <copyright file="AlarmsTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.DeviceTelemetry.Services.Test
{
    public class AlarmsTest
    {
        private const string TenantInfoKey = "tenant";
        private const string AlarmsCollectionKey = "alarms-collection";
        private const string TenantId = "test_tenant";
        private readonly Mock<IStorageClient> storageClient;
        private readonly Mock<ILogger<Alarms>> logger;
        private readonly IAlarms alarms;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<IAppConfigurationClient> appConfigHelper;

        public AlarmsTest()
        {
            var servicesConfig = new AppConfig
            {
                DeviceTelemetryService = new DeviceTelemetryServiceConfig
                {
                    Alarms = new AlarmsConfig
                    {
                        Database = "database",
                        MaxDeleteRetries = 3,
                    },
                },
            };
            this.storageClient = new Mock<IStorageClient>();
            this.httpContextAccessor = new Mock<IHttpContextAccessor>();
            this.appConfigHelper = new Mock<IAppConfigurationClient>();
            this.httpContextAccessor.Setup(t => t.HttpContext.Request.HttpContext.Items).Returns(new Dictionary<object, object>()
                { { "TenantID", TenantId } });
            this.appConfigHelper.Setup(t => t.GetValue($"{TenantInfoKey}:{TenantId}:{AlarmsCollectionKey}")).Returns("collection");
            this.logger = new Mock<ILogger<Alarms>>();
            this.alarms = new Alarms(servicesConfig, this.storageClient.Object, this.logger.Object, this.httpContextAccessor.Object, this.appConfigHelper.Object);
        }

        /**
         * Test basic functionality of delete alarms by id.
         */
        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void BasicDelete()
        {
            // Arrange
            List<string> ids = new List<string> { "id1", "id2", "id3", "id4" };
            Document d1 = new Document
            {
                Id = "test",
            };
            this.storageClient
                .Setup(x => x.DeleteDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(d1));

            // Act
            this.alarms.Delete(ids);

            // Assert
            for (int i = 0; i < ids.Count; i++)
            {
                this.storageClient.Verify(x => x.DeleteDocumentAsync("database", "collection", ids[i]), Times.Once);
            }
        }

        /**
         * Verify if delete alarm by id fails once it will retry
        */
        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task DeleteSucceedsTransientExceptionAsync()
        {
            // Arrange
            List<string> ids = new List<string> { "id1" };
            Document d1 = new Document
            {
                Id = "test",
            };
            this.storageClient
                .SetupSequence(x => x.DeleteDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Throws(new Exception())
                .Returns(Task.FromResult(d1));

            // Act
            await this.alarms.Delete(ids);

            // Assert
            this.storageClient.Verify(x => x.DeleteDocumentAsync("database", "collection", ids[0]), Times.Exactly(2));
        }

        /**
         * Verify that after 3 failures to delete an alarm an
         * exception will be thrown.
         */
        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task DeleteFailsAfter3ExceptionsAsync()
        {
            // Arrange
            List<string> ids = new List<string> { "id1" };
            Document d1 = new Document
            {
                Id = "test",
            };

            this.storageClient
                .SetupSequence(x => x.DeleteDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception());

            // Act
            await Assert.ThrowsAsync<ExternalDependencyException>(async () => await this.alarms.Delete(ids));

            // Assert
            this.storageClient.Verify(x => x.DeleteDocumentAsync("database", "collection", ids[0]), Times.Exactly(3));
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
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.alarms.DeleteAsync(xssString));
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.alarms.Delete(xssList));
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.alarms.UpdateAsync(xssString, xssString));
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.alarms.GetCountByRuleAsync(xssString, DateTimeOffset.MaxValue, DateTimeOffset.MaxValue, xssList.ToArray()));
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.alarms.ListAsync(null, null, xssString, 0, 1, xssList.ToArray()));
            await Assert.ThrowsAsync<InvalidInputException>(async () => await this.alarms.ListByRuleAsync(xssString, DateTimeOffset.MaxValue, DateTimeOffset.MaxValue, xssString, 0, 1, xssList.ToArray()));
        }
    }
}