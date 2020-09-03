// <copyright file="DocumentDbKeyValueContainerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.StorageAdapter.Services.Models;
using Moq;
using Xunit;

namespace Mmm.Iot.StorageAdapter.Services.Test
{
    public class DocumentDbKeyValueContainerTest
    {
        private const string MockDatabaseId = "mockdb";
        private const string MockCollectionId = "mockcoll";
        private const string MockTenantId = "mocktenant";
        private const string AppConfigConnectionString = "";
        private readonly Mock<IStorageClient> mockClient;
        private readonly Mock<IHttpContextAccessor> mockContextAccessor;
        private readonly Mock<DocumentDbKeyValueContainer> mockContainer;
        private readonly DocumentDbKeyValueContainer container;
        private readonly Random rand = new Random();

        public DocumentDbKeyValueContainerTest()
        {
            var config = new AppConfig();
            config.StorageAdapterService = new StorageAdapterServiceConfig { DocumentDbRus = 400 };

            this.mockContextAccessor = new Mock<IHttpContextAccessor>();
            DefaultHttpContext context = new DefaultHttpContext();
            context.Items.Add("TenantID", MockTenantId);
            this.mockContextAccessor
                .Setup(t => t.HttpContext)
                .Returns(context);

            this.mockClient = new Mock<IStorageClient>();
            var database = new Mock<ResourceResponse<Database>>();

            // mock a specific tenant
            Mock<IAppConfigurationClient> mockAppConfigClient = new Mock<IAppConfigurationClient>();
            mockAppConfigClient
                .Setup(m => m.GetValue(It.IsAny<string>()))
                .Returns(MockCollectionId);

            // Mock service returns dummy data
            this.mockContainer = new Mock<DocumentDbKeyValueContainer>(
                this.mockClient.Object,
                new MockExceptionChecker(),
                config,
                mockAppConfigClient.Object,
                new Mock<ILogger<DocumentDbKeyValueContainer>>().Object,
                this.mockContextAccessor.Object);
            this.mockContainer
                .Setup(t => t.DocumentDbDatabaseId)
                .Returns(MockDatabaseId);
            this.mockContainer
                .Setup(t => t.DocumentDbCollectionId)
                .Returns(MockCollectionId);
            this.container = this.mockContainer.Object;
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etag = this.rand.NextString();
            var timestamp = this.rand.NextDateTimeOffset();
            var documentId = $"{collectionId.ToLowerInvariant()}.{key.ToLowerInvariant()}";

            var document = new Document();
            document.SetPropertyValue("CollectionId", collectionId);
            document.SetPropertyValue("Key", key);
            document.SetPropertyValue("Data", data);
            document.SetETag(etag);
            document.SetTimestamp(timestamp);
            var response = new ResourceResponse<Document>(document);

            this.mockClient
                .Setup(x => x.ReadDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(response);

            var result = await this.container.GetAsync(collectionId, key);

            Assert.Equal(result.CollectionId, collectionId);
            Assert.Equal(result.Key, key);
            Assert.Equal(result.Data, data);
            Assert.Equal(result.ETag, etag);
            Assert.Equal(result.Timestamp, timestamp);

            this.mockClient
                .Verify(
                    x => x.ReadDocumentAsync(
                        It.Is<string>(s => s == MockDatabaseId),
                        It.Is<string>(s => s == MockCollectionId),
                        It.Is<string>(s => s == documentId)),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetAsyncNotFoundTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();

            this.mockClient
                .Setup(x => x.ReadDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new ResourceNotFoundException());

            await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await this.container.GetAsync(collectionId, key));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetAllAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var documents = new List<Document>();
            foreach (int i in Enumerable.Range(0, 3))
            {
                var key = this.rand.NextString();
                var data = this.rand.NextString();
                var doc = new Document();
                doc.Id = $"{collectionId}.{key}";
                doc.SetPropertyValue("CollectionId", collectionId);
                doc.SetPropertyValue("Key", key);
                doc.SetPropertyValue("Data", data);
                doc.SetETag(this.rand.NextString());
                doc.SetTimestamp(this.rand.NextDateTimeOffset());
                documents.Add(doc);
            }

            this.mockClient
                .Setup(x => x.QueryAllDocumentsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(documents);

            var result = (await this.container.GetAllAsync(collectionId)).ToList();

            Assert.Equal(result.Count(), documents.Count);
            foreach (var model in result)
            {
                string documentId = $"{collectionId}.{model.Key}";
                var doc = documents.Single(d => documentId == d.Id);
                Assert.Equal(doc.Id, documentId);
            }

            this.mockClient
                .Verify(
                    x => x.QueryAllDocumentsAsync(
                        It.Is<string>(s => s == MockDatabaseId),
                        It.Is<string>(s => s == MockCollectionId)),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task CreateAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etag = this.rand.NextString();
            var timestamp = this.rand.NextDateTimeOffset();

            var document = new Document();
            document.SetPropertyValue("CollectionId", collectionId);
            document.SetPropertyValue("Key", key);
            document.SetPropertyValue("Data", data);
            document.SetETag(etag);
            document.SetTimestamp(timestamp);
            var response = new ResourceResponse<Document>(document);

            this.mockClient
                .Setup(x => x.CreateDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<KeyValueDocument>()))
                .ReturnsAsync(response);

            var result = await this.container.CreateAsync(collectionId, key, new ValueServiceModel
            {
                Data = data,
            });

            Assert.Equal(result.CollectionId, collectionId);
            Assert.Equal(result.Key, key);
            Assert.Equal(result.Data, data);
            Assert.Equal(result.ETag, etag);
            Assert.Equal(result.Timestamp, timestamp);

            this.mockClient
                .Verify(
                    x => x.CreateDocumentAsync(
                        It.Is<string>(s => s == MockDatabaseId),
                        It.Is<string>(s => s == MockCollectionId),
                        It.Is<KeyValueDocument>(d => d.Id == $"{collectionId.ToLowerInvariant()}.{key.ToLowerInvariant()}")),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task CreateAsyncConflictTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();

            this.mockClient
                .Setup(x => x.CreateDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<KeyValueDocument>()))
                .ThrowsAsync(new ConflictingResourceException());

            await Assert.ThrowsAsync<ConflictingResourceException>(async () =>
                await this.container.CreateAsync(collectionId, key, new ValueServiceModel
                {
                    Data = data,
                }));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UpsertAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etagOld = this.rand.NextString();
            var etagNew = this.rand.NextString();
            var timestamp = this.rand.NextDateTimeOffset();

            var document = new Document();
            document.SetPropertyValue("CollectionId", collectionId);
            document.SetPropertyValue("Key", key);
            document.SetPropertyValue("Data", data);
            document.SetETag(etagNew);
            document.SetTimestamp(timestamp);
            var response = new ValueServiceModel(document);

            this.mockClient
                .Setup(x => x.UpsertDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<KeyValueDocument>()))
                .ReturnsAsync(document);

            var result = await this.container.UpsertAsync(collectionId, key, new ValueServiceModel
            {
                Data = data,
                ETag = etagOld,
            });

            Assert.Equal(result.CollectionId, collectionId);
            Assert.Equal(result.Key, key);
            Assert.Equal(result.Data, data);
            Assert.Equal(result.ETag, etagNew);
            Assert.Equal(result.Timestamp, timestamp);

            this.mockClient
                .Verify(
                    x => x.UpsertDocumentAsync(
                        It.Is<string>(s => s == MockDatabaseId),
                        It.Is<string>(s => s == MockCollectionId),
                        It.Is<KeyValueDocument>(d => d.Id == $"{collectionId.ToLowerInvariant()}.{key.ToLowerInvariant()}")),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UpsertAsyncConflictTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etag = this.rand.NextString();

            this.mockClient
                .Setup(x => x.UpsertDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<KeyValueDocument>()))
                .ThrowsAsync(new ConflictingResourceException());

            await Assert.ThrowsAsync<ConflictingResourceException>(async () =>
                await this.container.UpsertAsync(collectionId, key, new ValueServiceModel
                {
                    Data = data,
                    ETag = etag,
                }));
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task DeleteAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var documentId = $"{collectionId.ToLowerInvariant()}.{key.ToLowerInvariant()}";

            this.mockClient
                .Setup(x => x.DeleteDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((Document)null);

            await this.container.DeleteAsync(collectionId, key);

            this.mockClient
                .Verify(
                    x => x.DeleteDocumentAsync(
                        It.Is<string>(s => s == MockDatabaseId),
                        It.Is<string>(s => s == MockCollectionId),
                        It.Is<string>(s => s == documentId)),
                    Times.Once);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task DeleteAsyncNotFoundTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();

            this.mockClient
                .Setup(x => x.DeleteDocumentAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new ResourceNotFoundException());

            await this.container.DeleteAsync(collectionId, key);
        }
    }
}