// <copyright file="StorageAdapterClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Http;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Newtonsoft.Json;
using Xunit;
using HttpResponse = Mmm.Iot.Common.Services.Http.HttpResponse;

namespace Mmm.Iot.Common.Services.Test
{
    public class StorageAdapterClientTest
    {
        private const string MockServiceUri = @"http://mockstorageadapter";
        private const string AzdsRouteKey = "azds-route-as";
        private readonly Mock<IHttpClient> mockHttpClient;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly Mock<ExternalRequestHelper> mockRequestHelper;
        private readonly Mock<AppConfig> mockConfig;
        private readonly StorageAdapterClient client;
        private readonly Random rand;

        public StorageAdapterClientTest()
        {
            this.mockHttpClient = new Mock<IHttpClient>();
            this.mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            this.mockHttpContextAccessor
                .Setup(t => t.HttpContext.Request.HttpContext.Items)
                .Returns(new Dictionary<object, object>() { { "TenantID", "test_tenant" } });
            this.mockHttpContextAccessor
                .Setup(t => t.HttpContext.Request.Headers)
                .Returns(new HeaderDictionary() { { AzdsRouteKey, "mockDevSpace" } });
            this.mockRequestHelper = new Mock<ExternalRequestHelper>(
                this.mockHttpClient.Object,
                this.mockHttpContextAccessor.Object);

            this.mockConfig = new Mock<AppConfig>();
            this.mockConfig
                .Setup(x => x.ExternalDependencies.StorageAdapterServiceUrl)
                .Returns(MockServiceUri);

            this.client = new StorageAdapterClient(
                this.mockConfig.Object,
                this.mockRequestHelper.Object);

            this.rand = new Random();
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etag = this.rand.NextString();

            var method = HttpMethod.Get;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(new ValueApiModel
                {
                    Key = key,
                    Data = data,
                    ETag = etag,
                }),
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            var result = await this.client.GetAsync(collectionId, key);

            this.mockHttpClient
                .Verify(
                    x => x.SendAsync(
                        It.Is<IHttpRequest>(r => r.Check($"{MockServiceUri}/collections/{collectionId}/values/{key}")),
                        It.Is<HttpMethod>(m => m == method)),
                    Times.Once);

            Assert.Equal(result.Key, key);
            Assert.Equal(result.Data, data);
            Assert.Equal(result.ETag, etag);
        }

        [Fact]
        public async Task GetAsyncNotFoundTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();

            var method = HttpMethod.Get;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccessStatusCode = false,
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await this.client.GetAsync(collectionId, key));
        }

        [Fact]
        public async Task GetAllAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var models = new[]
            {
                new ValueApiModel
                {
                    Key = this.rand.NextString(),
                    Data = this.rand.NextString(),
                    ETag = this.rand.NextString(),
                },
                new ValueApiModel
                {
                    Key = this.rand.NextString(),
                    Data = this.rand.NextString(),
                    ETag = this.rand.NextString(),
                },
                new ValueApiModel
                {
                    Key = this.rand.NextString(),
                    Data = this.rand.NextString(),
                    ETag = this.rand.NextString(),
                },
            };
            var method = HttpMethod.Get;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(new ValueListApiModel { Items = models }),
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            var result = await this.client.GetAllAsync(collectionId);
            this.mockHttpClient
                .Verify(
                    x => x.SendAsync(
                        It.Is<IHttpRequest>(r => r.Check($"{MockServiceUri}/collections/{collectionId}/values")),
                        It.Is<HttpMethod>(m => m == method)),
                    Times.Once);

            Assert.Equal(result.Items.Count(), models.Length);
            foreach (var item in result.Items)
            {
                var model = models.Single(m => m.Key == item.Key);
                Assert.Equal(model.Data, item.Data);
                Assert.Equal(model.ETag, item.ETag);
            }
        }

        [Fact]
        public async Task CreateAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etag = this.rand.NextString();

            var method = HttpMethod.Post;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(new ValueApiModel
                {
                    Key = key,
                    Data = data,
                    ETag = etag,
                }),
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            var result = await this.client.CreateAsync(collectionId, data);
            this.mockHttpClient
                .Verify(
                    x => x.SendAsync(
                        It.Is<IHttpRequest>(r => r.Check<ValueApiModel>($"{MockServiceUri}/collections/{collectionId}/values", m => m.Data == data)),
                        It.Is<HttpMethod>(m => m == method)),
                    Times.Once);

            Assert.Equal(result.Key, key);
            Assert.Equal(result.Data, data);
            Assert.Equal(result.ETag, etag);
        }

        [Fact]
        public async Task UpdateAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etagOld = this.rand.NextString();
            var etagNew = this.rand.NextString();

            var method = HttpMethod.Put;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(new ValueApiModel
                {
                    Key = key,
                    Data = data,
                    ETag = etagNew,
                }),
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            var result = await this.client.UpdateAsync(collectionId, key, data, etagOld);
            this.mockHttpClient
                .Verify(
                    x => x.SendAsync(
                        It.Is<IHttpRequest>(r => r.Check<ValueApiModel>($"{MockServiceUri}/collections/{collectionId}/values/{key}", m => m.Data == data && m.ETag == etagOld)),
                        It.Is<HttpMethod>(m => m == method)),
                    Times.Once);

            Assert.Equal(result.Key, key);
            Assert.Equal(result.Data, data);
            Assert.Equal(result.ETag, etagNew);
        }

        [Fact]
        public async Task UpdateAsyncConflictTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();
            var data = this.rand.NextString();
            var etag = this.rand.NextString();

            var method = HttpMethod.Put;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.Conflict,
                IsSuccessStatusCode = false,
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            await Assert.ThrowsAsync<ConflictingResourceException>(async () =>
                await this.client.UpdateAsync(collectionId, key, data, etag));
        }

        [Fact]
        public async Task DeleteAsyncTest()
        {
            var collectionId = this.rand.NextString();
            var key = this.rand.NextString();

            var method = HttpMethod.Delete;
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
            };

            this.mockHttpClient
                .Setup(x => x.SendAsync(
                    It.IsAny<IHttpRequest>(),
                    It.IsAny<HttpMethod>()))
                .ReturnsAsync(response);

            await this.client.DeleteAsync(collectionId, key);
            this.mockHttpClient
                .Verify(
                    x => x.SendAsync(
                        It.Is<IHttpRequest>(r => r.Check($"{MockServiceUri}/collections/{collectionId}/values/{key}")),
                        It.Is<HttpMethod>(m => m == method)),
                    Times.Once);
        }
    }
}