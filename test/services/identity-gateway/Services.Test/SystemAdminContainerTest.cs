// <copyright file="SystemAdminContainerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.External.TableStorage;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.IdentityGateway.Services.Models;
using Mmm.Iot.IdentityGateway.Services.Test.Helpers.Builders;
using Moq;
using Newtonsoft.Json;
using TestStack.Dossier;
using TestStack.Dossier.EquivalenceClasses;
using TestStack.Dossier.Lists;
using Xunit;

namespace Mmm.Iot.IdentityGateway.Services.Test
{
    public class SystemAdminContainerTest
    {
        private const int DynamicTableEntityCount = 100;
        private readonly Mock<ILogger<SystemAdminContainer>> logger;
        private SystemAdminContainer systemAdminContainer;
        private Mock<ITableStorageClient> mockTableStorageClient;
        private Random random = new Random();
        private SystemAdminInput someSystemAdminInput = Builder<SystemAdminInput>.CreateNew().Build();
        private IList<DynamicTableEntity> dynamicTableEntities;
        private AnonymousValueFixture any = new AnonymousValueFixture();

        public SystemAdminContainerTest()
        {
            this.logger = new Mock<ILogger<SystemAdminContainer>>();
            this.mockTableStorageClient = new Mock<ITableStorageClient> { DefaultValue = DefaultValue.Mock };
            this.systemAdminContainer = new SystemAdminContainer(this.mockTableStorageClient.Object, this.logger.Object);
        }

        public static IEnumerable<object[]> GetRoleLists()
        {
            yield return new object[] { null };
            yield return new object[] { string.Empty };
            yield return new object[] { " " };
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void CreateAsyncReturnsExpectedTableEntity()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<SystemAdminModel>(
                    It.Is<string>(n => n == this.systemAdminContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((SystemAdminModel)null);

            this.mockTableStorageClient
                .Setup(m => m.InsertAsync(
                    It.Is<string>(n => n == this.systemAdminContainer.TableName),
                    It.Is<SystemAdminModel>(t => t.PartitionKey == this.someSystemAdminInput.UserId)))
                .ReturnsAsync(new SystemAdminModel(this.someSystemAdminInput));

            // Act
            var result = await this.systemAdminContainer.CreateAsync(this.someSystemAdminInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.InsertAsync(
                        It.Is<string>(n => n == this.systemAdminContainer.TableName),
                        It.Is<SystemAdminModel>(u => u.PartitionKey == this.someSystemAdminInput.UserId)),
                    Times.Once);

            // Assert
            this.AssertSystemAdminMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAsyncReturnsExpectedTableEntity()
        {
            // Arrange
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .Set(dte => dte.Properties, new Dictionary<string, EntityProperty> { { "Name", new EntityProperty(this.any.String()) } })
                .TheLast(1)
                .Set(dte => dte.Properties, new Dictionary<string, EntityProperty> { { "Name", new EntityProperty(this.someSystemAdminInput.Name) } })
                .BuildList();

            this.mockTableStorageClient
                .Setup(m => m.QueryAsync<SystemAdminModel>(
                    It.Is<string>(n => n == this.systemAdminContainer.TableName),
                    It.IsAny<TableQuery<SystemAdminModel>>(),
                    It.Is<CancellationToken>(t => t == default(CancellationToken))))
                .ReturnsAsync(this.dynamicTableEntities
                    .Where(dte => dte.Properties["Name"].StringValue == this.someSystemAdminInput.Name)
                    .Select(e => new SystemAdminModel(e))
                    .ToList());

            // Act
            var result = await this.systemAdminContainer.GetAsync(this.someSystemAdminInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.systemAdminContainer.TableName),
                        It.IsAny<TableQuery<SystemAdminModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("getsystemadmin", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Equal(this.dynamicTableEntities.Count(dte => dte.Properties["Name"].StringValue == this.someSystemAdminInput.Name), result.Models.Count());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void DeleteReturnsExpectedUserTenant()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<SystemAdminModel>(
                    It.Is<string>(n => n == this.systemAdminContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new SystemAdminModel(this.someSystemAdminInput));

            // Act
            var result = await this.systemAdminContainer.DeleteAsync(this.someSystemAdminInput);

            // Assert
            this.AssertSystemAdminMatchesInput(result);
        }

        private void AssertSystemAdminMatchesInput(SystemAdminModel systemAdmin)
        {
            Assert.NotNull(systemAdmin);
            systemAdmin = new SystemAdminModel(this.someSystemAdminInput);
            Assert.Equal(systemAdmin.Name, systemAdmin.Name);
            Assert.Equal(systemAdmin.PartitionKey, systemAdmin.PartitionKey);
        }
    }
}