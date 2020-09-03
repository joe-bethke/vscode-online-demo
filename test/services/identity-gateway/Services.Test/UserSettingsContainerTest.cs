// <copyright file="UserSettingsContainerTest.cs" company="3M">
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
using TestStack.Dossier;
using TestStack.Dossier.Lists;
using Xunit;

namespace Mmm.Iot.IdentityGateway.Services.Test
{
    public class UserSettingsContainerTest
    {
        private const int DynamicTableEntityCount = 100;
        private readonly Mock<ILogger<UserSettingsContainer>> logger;
        private UserSettingsContainer userSettingsContainer;
        private Mock<ITableStorageClient> mockTableStorageClient;
        private Random random = new Random();
        private UserSettingsInput someUserSettingsInput = Builder<UserSettingsInput>.CreateNew().Build();
        private IList<DynamicTableEntity> dynamicTableEntities;
        private string newUserId = Guid.NewGuid().ToString();

        public UserSettingsContainerTest()
        {
            this.logger = new Mock<ILogger<UserSettingsContainer>>();
            this.mockTableStorageClient = new Mock<ITableStorageClient> { DefaultValue = DefaultValue.Mock };
            this.userSettingsContainer = new UserSettingsContainer(this.mockTableStorageClient.Object, this.logger.Object);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAllReturnsExpectedUserSettingsList()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomValueProperty()
                .TheLast(this.random.Next(0, DynamicTableEntityCount))
                .Set(dte => dte.PartitionKey, this.someUserSettingsInput.UserId)
                .BuildList();

            this.mockTableStorageClient
                .Setup(m => m.QueryAsync(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<TableQuery<UserSettingsModel>>(),
                    It.Is<CancellationToken>(t => t == default(CancellationToken))))
                .ReturnsAsync(this.dynamicTableEntities
                    .Where(dte => dte.PartitionKey == this.someUserSettingsInput.UserId)
                    .Select(e => new UserSettingsModel(e))
                    .ToList());

            // Act
            var result = await this.userSettingsContainer.GetAllAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<TableQuery<UserSettingsModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("get", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Equal(this.dynamicTableEntities.Count(dte => dte.PartitionKey == this.someUserSettingsInput.UserId), result.Models.Count);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void UpdateUserIdAsync()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomValueProperty()
                .TheLast(this.random.Next(0, DynamicTableEntityCount))
                .Set(dte => dte.PartitionKey, this.someUserSettingsInput.UserId)
                .BuildList();

            this.mockTableStorageClient
                .Setup(m => m.QueryAsync(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<TableQuery<UserSettingsModel>>(),
                    It.Is<CancellationToken>(t => t == default(CancellationToken))))
                .ReturnsAsync(this.dynamicTableEntities
                    .Where(dte => dte.PartitionKey == this.someUserSettingsInput.UserId)
                    .Select(e => new UserSettingsModel(e))
                    .ToList());

            // Act
            var result = await this.userSettingsContainer.UpdateUserIdAsync(this.someUserSettingsInput.UserId, this.newUserId);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<TableQuery<UserSettingsModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("get", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Equal(this.dynamicTableEntities.Count(dte => dte.PartitionKey == this.someUserSettingsInput.UserId), result.Models.Count);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAllReturnsEmptyUserSettingsList()
        {
            // Arrange
            this.dynamicTableEntities = new List<DynamicTableEntity>();

            this.mockTableStorageClient
                .Setup(m => m.QueryAsync(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<TableQuery<UserSettingsModel>>(),
                    It.Is<CancellationToken>(t => t == default(CancellationToken))))
                .ReturnsAsync(this.dynamicTableEntities
                    .Where(dte => dte.PartitionKey == this.someUserSettingsInput.UserId)
                    .Select(e => new UserSettingsModel(e))
                    .ToList());

            // Act
            var result = await this.userSettingsContainer.GetAllAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<TableQuery<UserSettingsModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("get", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Empty(result.Models);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetReturnsExpectedUserSettings()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomValueProperty()
                .TheLast(1)
                .Set(dte => dte.PartitionKey, this.someUserSettingsInput.UserId)
                .Set(dte => dte.RowKey, this.someUserSettingsInput.SettingKey)
                .BuildList();

            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserSettingsModel>(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((string tableName, string partitionKey, string rowKey) =>
                {
                    return new UserSettingsModel(
                        this.dynamicTableEntities.FirstOrDefault(dte =>
                        {
                            return dte.PartitionKey == partitionKey && dte.RowKey == rowKey;
                        }));
                });

            // Act
            var result = await this.userSettingsContainer.GetAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<ITableEntity>(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            // Assert
            this.AssertUserSettingsMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetReturnsEmptyUserSettings()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserSettingsModel>(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserSettingsModel)null);

            // Act
            var result = await this.userSettingsContainer.GetAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserSettingsModel>(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void CreateReturnsExpectedUserSettings()
        {
            // mock intial check to see if setting already exists during CreateAsync
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserSettingsModel>(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserSettingsModel)null);

            // mock call to insert new setting
            this.mockTableStorageClient
                .Setup(m => m.InsertAsync(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.Is<UserSettingsModel>(u => u.PartitionKey == this.someUserSettingsInput.UserId && u.RowKey == this.someUserSettingsInput.SettingKey && u.Value == this.someUserSettingsInput.Value)))
                .ReturnsAsync(new UserSettingsModel(this.someUserSettingsInput));

            // Act
            var result = await this.userSettingsContainer.CreateAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserSettingsModel>(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            this.mockTableStorageClient
                .Verify(
                    m => m.InsertAsync(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.Is<UserSettingsModel>(u => u.PartitionKey == this.someUserSettingsInput.UserId && u.RowKey == this.someUserSettingsInput.SettingKey && u.Value == this.someUserSettingsInput.Value)),
                    Times.Once);

            // Assert
            this.AssertUserSettingsMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void CreateThrowsWhenUserSettingsAlreadyExist()
        {
            // Arrange
            // mock the initial check to see if setting already exists
            // no need to mock the Insert call as the exception should be thrown before it is invoked
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserSettingsModel>(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new UserSettingsModel());

            // Act
            Func<Task> a = async () => await this.userSettingsContainer.CreateAsync(this.someUserSettingsInput);

            // Assert
            await Assert.ThrowsAsync<StorageException>(a);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void UpdateReturnsExpectedUserSettings()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.InsertOrReplaceAsync(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.Is<UserSettingsModel>(u => u.PartitionKey == this.someUserSettingsInput.UserId && u.RowKey == this.someUserSettingsInput.SettingKey && u.Value == this.someUserSettingsInput.Value)))
                .ReturnsAsync(new UserSettingsModel(this.someUserSettingsInput));

            // Act
            var result = await this.userSettingsContainer.UpdateAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.InsertOrReplaceAsync(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.Is<UserSettingsModel>(u => u.PartitionKey == this.someUserSettingsInput.UserId && u.RowKey == this.someUserSettingsInput.SettingKey && u.Value == this.someUserSettingsInput.Value)),
                    Times.Once);

            // Assert
            this.AssertUserSettingsMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void DeleteReturnsExpectedUserSettings()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserSettingsModel>(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new UserSettingsModel(this.someUserSettingsInput));

            this.mockTableStorageClient
                .Setup(m => m.DeleteAsync(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.Is<UserSettingsModel>(u => u.PartitionKey == this.someUserSettingsInput.UserId && u.RowKey == this.someUserSettingsInput.SettingKey && u.Value == this.someUserSettingsInput.Value)))
                .ReturnsAsync(new UserSettingsModel(this.someUserSettingsInput));

            // Act
            var result = await this.userSettingsContainer.DeleteAsync(this.someUserSettingsInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserSettingsModel>(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            this.mockTableStorageClient
                .Verify(
                    m => m.DeleteAsync(
                        It.Is<string>(n => n == this.userSettingsContainer.TableName),
                        It.Is<UserSettingsModel>(u => u.PartitionKey == this.someUserSettingsInput.UserId && u.RowKey == this.someUserSettingsInput.SettingKey && u.Value == this.someUserSettingsInput.Value)),
                    Times.Once);

            // Assert
            this.AssertUserSettingsMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void DeleteThrowsWhenUserSettingsDoesNotExist()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserSettingsModel>(
                    It.Is<string>(n => n == this.userSettingsContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserSettingsModel)null);

            // Act
            Func<Task> a = async () => await this.userSettingsContainer.DeleteAsync(this.someUserSettingsInput);

            // Assert
            await Assert.ThrowsAsync<StorageException>(a);
        }

        private void AssertUserSettingsMatchesInput(UserSettingsModel userSettings)
        {
            Assert.NotNull(userSettings);
            userSettings = new UserSettingsModel(this.someUserSettingsInput);
            Assert.Equal(userSettings.SettingKey, userSettings.SettingKey);
            Assert.Equal(userSettings.UserId, userSettings.UserId);
            Assert.Equal(userSettings.Value, userSettings.Value);
        }
    }
}