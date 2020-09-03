// <copyright file="UserTenantContainerTest.cs" company="3M">
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
using TestStack.Dossier.Lists;
using Xunit;

namespace Mmm.Iot.IdentityGateway.Services.Test
{
    public class UserTenantContainerTest
    {
        private const int DynamicTableEntityCount = 100;
        private readonly Mock<ILogger<UserTenantContainer>> logger;
        private UserTenantContainer userTenantContainer;
        private Mock<ITableStorageClient> mockTableStorageClient;
        private Random random = new Random();
        private UserTenantInput someUserTenantInput = Builder<UserTenantInput>.CreateNew().Build();
        private IList<DynamicTableEntity> dynamicTableEntities;
        private AnonymousValueFixture any = new AnonymousValueFixture();

        public UserTenantContainerTest()
        {
            this.logger = new Mock<ILogger<UserTenantContainer>>();
            this.mockTableStorageClient = new Mock<ITableStorageClient> { DefaultValue = DefaultValue.Mock };
            this.userTenantContainer = new UserTenantContainer(this.mockTableStorageClient.Object, this.logger.Object);
        }

        public static IEnumerable<object[]> GetRoleLists()
        {
            yield return new object[] { null };
            yield return new object[] { string.Empty };
            yield return new object[] { " " };
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAllReturnsExpectedUserTenantList()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomRolesProperty()
                .TheLast(this.random.Next(0, DynamicTableEntityCount))
                .Set(dte => dte.PartitionKey, this.someUserTenantInput.UserId)
                .BuildList();

            this.mockTableStorageClient
                .Setup(m => m.QueryAsync(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<TableQuery<UserTenantModel>>(),
                    It.Is<CancellationToken>(t => t == default(CancellationToken))))
                .ReturnsAsync(this.dynamicTableEntities
                    .Where(dte => dte.PartitionKey == this.someUserTenantInput.UserId)
                    .Select(e => new UserTenantModel(e))
                    .ToList());

            // Act
            var result = await this.userTenantContainer.GetAllAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<TableQuery<UserTenantModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("gettenants", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Equal(this.dynamicTableEntities.Count(dte => dte.PartitionKey == this.someUserTenantInput.UserId), result.Models.Count);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAllUsersReturnsExpectedUserTenantList()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomRolesProperty()
                .TheLast(this.random.Next(0, DynamicTableEntityCount))
                .Set(dte => dte.RowKey, this.someUserTenantInput.Tenant)
                .BuildList();

            // Act
            var result = await this.userTenantContainer.GetAllAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<TableQuery<UserTenantModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("gettenants", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Equal(this.dynamicTableEntities.Count(dte => dte.PartitionKey == this.someUserTenantInput.UserId), result.Models.Count);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAllReturnsEmptyUserTenantList()
        {
            // Arrange
            this.dynamicTableEntities = new List<DynamicTableEntity>();

            // Act
            var result = await this.userTenantContainer.GetAllAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<TableQuery<UserTenantModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("gettenants", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Empty(result.Models);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetReturnsExpectedUserTenant()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomRolesProperty()
                .TheLast(1)
                .Set(dte => dte.PartitionKey, this.someUserTenantInput.UserId)
                .Set(dte => dte.RowKey, this.someUserTenantInput.Tenant)
                .BuildList();

            // Act
            var result = await this.userTenantContainer.GetAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserTenantModel>(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            // Assert
            this.AssertUserTenantMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetReturnsEmptyUserTenant()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserTenantModel>(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserTenantModel)null);

            // Act
            var result = await this.userTenantContainer.GetAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserTenantModel>(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void CreateReturnsExpectedUserTenant()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserTenantModel>(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserTenantModel)null);

            this.mockTableStorageClient
                .Setup(m => m.InsertAsync(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.Is<UserTenantModel>(t => t.TenantId == this.someUserTenantInput.Tenant && t.UserId == this.someUserTenantInput.UserId)))
                .ReturnsAsync(new UserTenantModel(this.someUserTenantInput));

            // Act
            var result = await this.userTenantContainer.CreateAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserTenantModel>(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            this.mockTableStorageClient
                .Verify(
                    m => m.InsertAsync(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.Is<UserTenantModel>(u => u.TenantId == this.someUserTenantInput.Tenant && u.UserId == this.someUserTenantInput.UserId)),
                    Times.Once);

            // Assert
            this.AssertUserTenantMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void CreateHandlesNullUserIdAndReturnsExpectedUserTenant()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserTenantModel>(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserTenantModel)null);

            this.someUserTenantInput.UserId = null;

            this.mockTableStorageClient
                .Setup(m => m.InsertAsync(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.Is<UserTenantModel>(u => u.TenantId == this.someUserTenantInput.Tenant)))
                .ReturnsAsync(new UserTenantModel(this.someUserTenantInput));

            // Act
            var result = await this.userTenantContainer.CreateAsync(this.someUserTenantInput);

            this.mockTableStorageClient
                .Verify(
                    m => m.RetrieveAsync<UserTenantModel>(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<string>(),
                        It.IsAny<string>()),
                    Times.Once);

            this.mockTableStorageClient
                .Verify(
                    m => m.InsertAsync(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.Is<UserTenantModel>(u => u.TenantId == this.someUserTenantInput.Tenant && u.UserId == this.someUserTenantInput.UserId)),
                    Times.Once);

            // Assert
            this.AssertUserTenantMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void CreateThrowsWhenUserTenantAlreadyExist()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserTenantModel>(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new UserTenantModel(this.someUserTenantInput));

            // Act
            Func<Task> a = async () => await this.userTenantContainer.CreateAsync(this.someUserTenantInput);

            // Assert
            await Assert.ThrowsAsync<StorageException>(a);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void UpdateReturnsExpectedUserTenant()
        {
            // Arrange
            this.someUserTenantInput = Builder<UserTenantInput>.CreateNew().Set(uti => uti.Roles, JsonConvert.SerializeObject(new[] { "someRole", "someOtherRole" })).Build();

            // Act
            var result = await this.userTenantContainer.UpdateAsync(this.someUserTenantInput);

            // Assert
            this.AssertUserTenantMatchesInput(result);
        }

        [Theory]
        [Trait(Constants.Type, Constants.UnitTest)]
        [MemberData(nameof(GetRoleLists))]
        public async void UpdateDoesNotThrowWhenUserTenantRoleListIsNullOrEmptyOrWhitespace(string roles)
        {
            // Arrange
            this.someUserTenantInput.Roles = roles;

            // Act
            // Assert
            await this.userTenantContainer.UpdateAsync(this.someUserTenantInput);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void DeleteReturnsExpectedUserTenant()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserTenantModel>(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new UserTenantModel(this.someUserTenantInput));

            // Act
            var result = await this.userTenantContainer.DeleteAsync(this.someUserTenantInput);

            // Assert
            this.AssertUserTenantMatchesInput(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void DeleteThrowsWhenUserTenantDoesNotExist()
        {
            // Arrange
            this.mockTableStorageClient
                .Setup(m => m.RetrieveAsync<UserTenantModel>(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((UserTenantModel)null);

            // Act
            Func<Task> a = async () => await this.userTenantContainer.DeleteAsync(this.someUserTenantInput);

            // Assert
            await Assert.ThrowsAsync<StorageException>(a);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void DeleteAllReturnsExpectedUserTenantList()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomRolesProperty()
                .TheLast(this.random.Next(0, DynamicTableEntityCount))
                .Set(dte => dte.RowKey, this.someUserTenantInput.Tenant)
                .BuildList();

            // Act
            var result = await this.userTenantContainer.DeleteAllAsync(this.someUserTenantInput);

            // Assert
            Assert.Equal("delete", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
            Assert.Equal(this.dynamicTableEntities.Count(dte => dte.PartitionKey == this.someUserTenantInput.UserId), result.Models.Count);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async void GetAllUsersReturnsExpectedDistincyUserList()
        {
            // Arrange
            this.dynamicTableEntities = DynamicTableEntityBuilder
                .CreateListOfSize(DynamicTableEntityCount)
                .All()
                .WithRandomRolesProperty()
                .TheLast(this.random.Next(0, DynamicTableEntityCount))
                .Set(dte => dte.PartitionKey, this.someUserTenantInput.UserId)
                .BuildList();

            this.mockTableStorageClient
                .Setup(m => m.QueryAsync(
                    It.Is<string>(n => n == this.userTenantContainer.TableName),
                    It.IsAny<TableQuery<UserTenantModel>>(),
                    It.Is<CancellationToken>(t => t == default(CancellationToken))))
                .ReturnsAsync(this.dynamicTableEntities
                    .Where(dte => !string.IsNullOrWhiteSpace(dte.PartitionKey))
                    .Select(e => new UserTenantModel(e))
                    .ToList());

            // Act
            var result = await this.userTenantContainer.GetAllUsersAsync();

            this.mockTableStorageClient
                .Verify(
                    m => m.QueryAsync(
                        It.Is<string>(n => n == this.userTenantContainer.TableName),
                        It.IsAny<TableQuery<UserTenantModel>>(),
                        It.Is<CancellationToken>(t => t == default(CancellationToken))),
                    Times.Once);

            // Assert
            Assert.Equal("getallusers", result.BatchMethod.ToLowerInvariant());
            Assert.NotNull(result.Models);
        }

        private void AssertUserTenantMatchesInput(UserTenantModel userTenant)
        {
            Assert.NotNull(userTenant);
            userTenant = new UserTenantModel(this.someUserTenantInput);
            Assert.Equal(userTenant.Name, userTenant.Name);
            Assert.Equal(userTenant.RoleList, userTenant.RoleList);
            Assert.Equal(userTenant.Roles, userTenant.Roles);
            Assert.Equal(userTenant.TenantId, userTenant.TenantId);
            Assert.Equal(userTenant.Type, userTenant.Type);
            Assert.Equal(userTenant.UserId, userTenant.UserId);
        }
    }
}