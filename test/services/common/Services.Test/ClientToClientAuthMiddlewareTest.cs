// <copyright file="ClientToClientAuthMiddlewareTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Mmm.Iot.Common.Services.Auth;
using Mmm.Iot.Common.TestHelpers;
using Moq;
using Xunit;

namespace Mmm.Iot.Common.Services.Test
{
    public class ClientToClientAuthMiddlewareTest
    {
        private readonly Mock<ILogger<ClientToClientAuthMiddleware>> mockLogger;
        private readonly Mock<HttpContext> mockHttpContext;
        private readonly Mock<RequestDelegate> mockRequestDelegate;
        private ClientToClientAuthMiddleware middleware;

        public ClientToClientAuthMiddlewareTest()
        {
            this.mockHttpContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            this.mockLogger = new Mock<ILogger<ClientToClientAuthMiddleware>>();
            this.mockRequestDelegate = new Mock<RequestDelegate>();
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task SetsTenant()
        {
            // Arrange
            var x = new RequestDelegate(async context =>
            {
                await context.Response.WriteAsync("Hello from 2nd delegate.");

                // Assert
                Assert.Contains(RequestExtension.ContextKeyTenantId, context.Items.Keys);
            });
            this.middleware = new ClientToClientAuthMiddleware(x, this.mockLogger.Object);

            // Act
            await this.middleware.Invoke(new DefaultHttpContext());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task SetsTenantWithValue()
        {
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[RequestExtension.HeaderKeyTenantId] = "test_tenant";

            // Arrange
            var x = new RequestDelegate(async context =>
            {
                await context.Response.WriteAsync("Hello from 2nd delegate.");

                // Assert
                Assert.Contains(RequestExtension.ContextKeyTenantId, context.Items.Keys);
                Assert.Equal(httpContext.Request.Headers[RequestExtension.HeaderKeyTenantId].First(), context.Items[RequestExtension.ContextKeyTenantId]);
            });
            this.middleware = new ClientToClientAuthMiddleware(x, this.mockLogger.Object);
            await this.middleware.Invoke(httpContext);
        }
    }
}