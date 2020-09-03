// <copyright file="ExternalServiceClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.Helpers;
using Mmm.Iot.Common.Services.Http;
using Mmm.Iot.Common.Services.Models;
using Moq;
using Newtonsoft.Json;
using Xunit;
using HttpResponse = Mmm.Iot.Common.Services.Http.HttpResponse;

namespace Mmm.Iot.Common.Services.Test
{
    public class ExternalServiceClientTest
    {
        private const string MockServiceUri = @"http://mockclient";
        private const string AzdsRouteKey = "azds-route-as";
        private readonly Mock<IHttpClient> mockHttpClient;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly Mock<ExternalRequestHelper> mockRequestHelper;
        private readonly IExternalServiceClient client;

        public ExternalServiceClientTest()
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

            this.client = new ExternalServiceClient(MockServiceUri, this.mockRequestHelper.Object);
        }

        [Fact]
        public async Task GetHealthyStatusAsyncTest()
        {
            var healthyStatus = new StatusResultServiceModel(true, "all good");
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Content = JsonConvert.SerializeObject(healthyStatus),
            };

            this.mockHttpClient
                .Setup(
                    x => x.SendAsync(
                        It.IsAny<IHttpRequest>(),
                        It.Is<HttpMethod>(method => method == HttpMethod.Get)))
                    .ReturnsAsync(response);

            var result = await this.client.StatusAsync();

            this.mockHttpClient
                .Verify(
                    x => x.SendAsync(
                        It.Is<IHttpRequest>(r => r.Check($"{MockServiceUri}/status")),
                        It.Is<HttpMethod>(method => method == HttpMethod.Get)),
                    Times.Once);

            Assert.Equal(result.IsHealthy, healthyStatus.IsHealthy);
            Assert.Equal(result.Message, healthyStatus.Message);
        }

        [Fact]
        public async Task StatusAsyncReturnsUnhealthyOnExceptionTest()
        {
            this.mockHttpClient
                .Setup(
                    x => x.SendAsync(
                        It.IsAny<IHttpRequest>(),
                        It.Is<HttpMethod>(method => method == HttpMethod.Get)))
                    .ThrowsAsync(new Exception());

            var response = await this.client.StatusAsync();

            Assert.False(response.IsHealthy);
        }
    }
}