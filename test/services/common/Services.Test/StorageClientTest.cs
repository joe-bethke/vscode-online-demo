// <copyright file="StorageClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

/* TestFile for Mmm.Iot.Common.Services.External.CosmosDb.StorageClient.cs
   The Following methods did not receive unit tests:
   DeleteDatabaseAsync as it just a wrapper for the DocumentClient.DeleteDatabaseAsync method created by MS
   CreateDatabaseIfNotExistsAsync as it is just a wrapper for DocumentClient.CreateDatabaseIfNotExistsAsync method created by MS
   DeleteCollectionAsync as it is just a wrapper for DocumentClient.DeleteCollectionAsync method by MS
   QueryAllDocumentsAsync due to being unable to mock CreateDocumentQuery as it returns an IOrderedQuerable<document> (cant instantiate interface to return in mock)
   QueryDocumentsAsync due to being unable to mock CreateDocumentQuery as it returns an IOrderedQuerable<document> (cant instantiate interface to return in mock)
   QueryCountAsync to being unable to mock CreateDocumentQuery as it returns an IOrderedQuerable<document> (cant instantiate interface to return in mock)
   SetValuesFromConfig as it is a private method
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.Common.Services.Test
{
    public class StorageClientTest
    {
        private const string DatabaseName = "testDatabase";
        private const string MockCosmosDbConnection = "AccountEndpoint=https://test:443/;AccountKey=abc==;";
        private readonly Mock<AppConfig> mockConfig;
        private readonly StorageClient client;
        private readonly Random rand;

        // StorageClient specific variables
        private readonly Mock<ILogger<StorageClient>> mockILogger;
        private readonly Mock<IDocumentClient> mockDocumentClient;

        public StorageClientTest()
        {
            this.mockILogger = new Mock<ILogger<StorageClient>>();
            this.mockDocumentClient = new Mock<IDocumentClient>();

            this.mockDocumentClient
                .Setup(x => x.CreateDocumentCollectionIfNotExistsAsync(It.IsAny<string>(), It.IsAny<DocumentCollection>(), It.IsAny<RequestOptions>()))
                .ReturnsAsync(new ResourceResponse<DocumentCollection>());
            this.mockDocumentClient
                .Setup(x => x.CreateDatabaseIfNotExistsAsync(It.IsAny<Database>(), null))
                .ReturnsAsync(new ResourceResponse<Database>());
            this.mockDocumentClient
                .Setup(x => x.CreateDocumentAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<RequestOptions>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResourceResponse<Document>(new Document()));

            this.mockConfig = new Mock<AppConfig>();
            this.mockConfig
                .Setup(x => x.Global.CosmosDb.DocumentDbConnectionString)
                .Returns(MockCosmosDbConnection);

            this.client = new StorageClient(
                this.mockConfig.Object,
                this.mockILogger.Object,
                this.mockDocumentClient.Object);

            this.rand = new Random();
        }

        [Fact]
        public async Task CreateCollectionIfNotExistsAsyncSuccessTest()
        {
            // arrange
            var id = this.rand.NextString();

            // act
            var result = await this.client.CreateCollectionIfNotExistsAsync(DatabaseName, id);

            // result will be an empty ResourceResponse<DocumentCollection> so assert if CreateCollectionIfNotExistsAsync returns a value at all
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ReadDocumentAsyncSuccessTest()
        {
            // arrange
            var collectionId = this.rand.NextString();
            var documentId = this.rand.NextString();
            this.mockDocumentClient
                .Setup(x => x.ReadDocumentAsync(It.IsAny<string>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResourceResponse<Document>(new Document()));

            // act
            var result = await this.client.ReadDocumentAsync(DatabaseName, collectionId, documentId);

            // assert that ReadDocumentAsync returns a document, document will be an empty Document Object
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateDocumentAsyncSuccessTest()
        {
            // arrange
            var collectionId = this.rand.NextString();
            var documentId = this.rand.NextString();

            // act
            var result = await this.client.CreateDocumentAsync(DatabaseName, collectionId, documentId);

            // assert that CreateDocumentAsync returns a document, document will be an empty Document object
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteDocumentAsyncSuccessTest()
        {
            // arrange
            var collectionId = this.rand.NextString();
            var documentId = this.rand.NextString();
            this.mockDocumentClient
                .Setup(x => x.DeleteDocumentAsync(It.IsAny<string>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResourceResponse<Document>(new Document()));

            // act
            var result = await this.client.DeleteDocumentAsync(DatabaseName, collectionId, documentId);

            // Check to make sure DeleteDocumentAsync returns a deleted document (document will be an empty Document object)
            Assert.NotNull(result);
        }

        [Fact]
        public void GetDocumentClientReturnsClientSuccessTest()
        {
            // nothing to arrange in this test

            // Act
            var documentClient = this.client.GetDocumentClient();

            // documentClient will be a mock documentClient object and will have all null values, so checking just to see an object gets returned
            Assert.NotNull(documentClient);
        }

        [Theory]
        [InlineData(true)]
        public async Task StatusAsyncReturnsStatusServiceModelTest(bool checkIsHealthyStatus)
        {
            // nothing to arrange

            // Act
            var status = await this.client.StatusAsync();

            // Assert
            if (checkIsHealthyStatus)
            {
                // status,IsHealthy is expected to be false as the documentClient being checked is a mock
                Assert.False(status.IsHealthy);
            }
            else
            {
                // check to make sure StatusAsync returns a value
                Assert.NotNull(status);
            }
        }

        [Fact]
        public async Task UpsertDocumentAsyncReturnsNotNullTest()
        {
            // Arange
            this.mockDocumentClient
                .Setup(x => x.UpsertDocumentAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<RequestOptions>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResourceResponse<Document>(new Document()));
            var collectionId = this.rand.NextString();

            // Act
            var result = await this.client.UpsertDocumentAsync(DatabaseName, collectionId, new object());

            // result will be an empty Document object, assert it is not null
            Assert.NotNull(result);
        }
    }
}