// <copyright file="TableStorageClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.TableStorage;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.Common.Services.Test
{
    public class TableStorageClientTest
    {
        private const string MockTableName = "mocktable";

        private readonly Mock<AppConfig> mockConfig;
        private readonly Mock<ICloudTableClientFactory> mockFactory;
        private readonly Mock<CloudTableClient> mockTableClient;
        private readonly Mock<CloudTable> mockTable;
        private readonly ITableStorageClient client;
        private readonly Uri mockTableUri = new Uri("http://mocktableuri.c");

        public TableStorageClientTest()
        {
            this.mockConfig = new Mock<AppConfig>();
            this.mockTable = new Mock<CloudTable>(this.mockTableUri);

            this.mockTableClient = new Mock<CloudTableClient>(this.mockTableUri, new StorageCredentials());
            this.mockTableClient
                .Setup(x => x.GetTableReference(
                    It.IsAny<string>()))
                .Returns(this.mockTable.Object);

            this.mockFactory = new Mock<ICloudTableClientFactory>();
            this.mockFactory
                .Setup(x => x.Create())
                .Returns(this.mockTableClient.Object);

            this.client = new TableStorageClient(this.mockFactory.Object);
        }

        public static TableEntity CreateEntity(string rowKey = null, string partitionKey = null, string eTag = null)
        {
            Random rand = new Random();
            return new TableEntity
            {
                RowKey = rowKey ?? rand.NextString(),
                PartitionKey = partitionKey ?? rand.NextString(),
                ETag = eTag ?? rand.NextString(),
            };
        }

        public static IEnumerable<object[]> EntityOperations()
        {
            yield return new object[] { TableOperationType.Insert };
            yield return new object[] { TableOperationType.InsertOrMerge };
            yield return new object[] { TableOperationType.InsertOrReplace };
            yield return new object[] { TableOperationType.Delete };
        }

        [Fact]
        public async Task StatusAsyncReturnsHealthyStatusTest()
        {
            this.mockTableClient
                .Setup(x => x.ListTablesSegmentedAsync(
                    It.IsAny<TableContinuationToken>()))
                .Verifiable();

            var response = await this.client.StatusAsync();

            this.mockTableClient
                .Verify(
                    x => x.ListTablesSegmentedAsync(
                        It.IsAny<TableContinuationToken>()),
                    Times.Once);

            Assert.True(response.IsHealthy);
        }

        [Fact]
        public async Task StatusAsyncReturnsUnhealthyOnExceptionTest()
        {
            this.mockTableClient
                .Setup(x => x.ListTablesSegmentedAsync(
                    It.IsAny<TableContinuationToken>()))
                .ThrowsAsync(new Exception());

            var response = await this.client.StatusAsync();

            this.mockTableClient
                .Verify(
                    x => x.ListTablesSegmentedAsync(
                        It.IsAny<TableContinuationToken>()),
                    Times.Once);
            Assert.False(response.IsHealthy);
        }

        [Fact]
        public async Task QueryReturnsExpectedResultTest()
        {
            var entities = new List<TableEntity>
            {
                CreateEntity(),
                CreateEntity(),
                CreateEntity(),
            };
            var query = new TableQuery<TableEntity>();

            // mock a table query segment with the table results
            var ctor = typeof(TableQuerySegment<TableEntity>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);
            var mockQuerySegment = ctor.Invoke(new object[] { entities }) as TableQuerySegment<TableEntity>;

            this.mockTable
                .Setup(x => x.ExecuteQuerySegmentedAsync(
                    It.IsAny<TableQuery<TableEntity>>(),
                    It.IsAny<TableContinuationToken>()))
                .ReturnsAsync(mockQuerySegment);

            var response = await this.client.QueryAsync(MockTableName, query);

            this.mockTable
                .Verify(
                    x => x.ExecuteQuerySegmentedAsync(
                        It.Is<TableQuery<TableEntity>>(q => q.Equals(query)),
                        It.IsAny<TableContinuationToken>()),
                    Times.AtLeastOnce);

            Assert.Equal(entities, response);
        }

        [Fact]
        public async Task RetrieveReturnsExpectedEntityTest()
        {
            var entity = CreateEntity();
            var result = new TableResult
            {
                Result = entity,
            };

            this.mockTable
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<TableOperation>()))
                .ReturnsAsync(result);

            var response = await this.client.RetrieveAsync<TableEntity>(
                MockTableName,
                entity.PartitionKey,
                entity.RowKey);

            this.mockTable
                .Verify(
                    x => x.ExecuteAsync(
                        It.Is<TableOperation>(o => o.OperationType == TableOperationType.Retrieve)),
                    Times.Once);

            Assert.Equal(entity.RowKey, response.RowKey);
            Assert.Equal(entity.PartitionKey, response.PartitionKey);
        }

        [Theory]
        [MemberData(nameof(EntityOperations))]
        public async Task EntityOperationReturnsExpectedEntityTest(TableOperationType operationType)
        {
            var entity = CreateEntity();
            var result = new TableResult
            {
                Result = entity,
            };

            this.mockTable
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<TableOperation>()))
                .ReturnsAsync(result);

            var response = await this.GetResponseByTableOperationAsync(operationType, entity);

            this.mockTable
                .Verify(
                    x => x.ExecuteAsync(
                        It.Is<TableOperation>(op => op.Entity == entity && op.OperationType == operationType)),
                    Times.Once);

            Assert.Equal(entity.RowKey, response.RowKey);
            Assert.Equal(entity.PartitionKey, response.PartitionKey);
        }

        [Theory]
        [MemberData(nameof(EntityOperations))]
        public async Task EntityOperationThrowsWhenUnableToCastResultTest(TableOperationType operationType)
        {
            this.mockTable
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<TableOperation>()))
                .ReturnsAsync(
                    new TableResult
                    {
                        Result = new
                        {
                            UnusualResultKey = "value",
                        },
                    });

            await Assert.ThrowsAsync<Exception>(async () => await this.GetResponseByTableOperationAsync(operationType, CreateEntity()));

            this.mockTable
                .Verify(
                    x => x.ExecuteAsync(
                        It.Is<TableOperation>(o => o.OperationType == operationType)),
                    Times.Once);
        }

        [Theory]
        [MemberData(nameof(EntityOperations))]
        public async Task EntityOperationThrowsExceptionWhenTableThrowsStorageExceptionTest(TableOperationType operationType)
        {
            this.mockTable
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<TableOperation>()))
                .ThrowsAsync(new StorageException());

            await Assert.ThrowsAsync<Exception>(async () => await this.GetResponseByTableOperationAsync(operationType, CreateEntity()));

            this.mockTable
                .Verify(
                    x => x.ExecuteAsync(
                        It.Is<TableOperation>(o => o.OperationType == operationType)),
                    Times.Once);
        }

        private async Task<TableEntity> GetResponseByTableOperationAsync(TableOperationType operationType, TableEntity entity)
        {
            switch (operationType)
            {
                case TableOperationType.Insert:
                    return await this.client.InsertAsync(MockTableName, entity);
                case TableOperationType.InsertOrMerge:
                    return await this.client.InsertOrMergeAsync(MockTableName, entity);
                case TableOperationType.InsertOrReplace:
                    return await this.client.InsertOrReplaceAsync(MockTableName, entity);
                case TableOperationType.Delete:
                    return await this.client.DeleteAsync(MockTableName, entity);
                default:
                    throw new NotImplementedException($"TableStorageClient does not have a test implementation for operation type {operationType}.");
            }
        }
    }
}