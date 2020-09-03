// <copyright file="RequestExtensionTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class RequestExtensionTest
    {
        private readonly Mock<ILogger> mockLogger;
        private HttpRequest httpRequest;

        public RequestExtensionTest()
        {
            this.httpRequest = new DefaultHttpContext().Request;
            this.mockLogger = new Mock<ILogger>();
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetListOfClaims()
        {
            // Arrange
            this.SetupAsUser();

            // Act
            var claims = this.httpRequest.GetCurrentUserClaims();

            // Assert
            Assert.Equal(3, claims.Count());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void NoClaimsMakesEmptyList()
        {
            // Arrange

            // Act
            var claims = this.httpRequest.GetCurrentUserClaims();

            // Assert
            Assert.Empty(claims);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetRequiresAuth(bool value)
        {
            // Arrange
            this.httpRequest.SetAuthRequired(value);

            // Act
            var required = this.httpRequest.GetAuthRequired();

            // Assert
            Assert.Equal(value, required);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetExternalRequest(bool value)
        {
            // Arrange
            this.httpRequest.SetExternalRequest(value);

            // Act
            var external = this.httpRequest.IsExternalRequest();

            // Assert
            Assert.Equal(value, external);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetUserObjectId()
        {
            // Arrange
            this.SetupAsUser();

            // Act
            var sub = this.httpRequest.GetCurrentUserObjectId();

            // Assert
            Assert.Equal("test_user", sub);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetRoleClaim()
        {
            // Arrange
            this.SetupAsUser();

            // Act
            var roles = this.httpRequest.GetCurrentUserRoleClaim();

            // Assert
            Assert.Contains("admin", roles);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetAllowedActions()
        {
            // Arrange
            this.httpRequest.SetCurrentUserAllowedActions(new List<string>() { "can_do_this", "and_this" });

            // Act
            var actions = this.httpRequest.GetCurrentUserAllowedActions();

            // Assert
            Assert.Equal(2, actions.Count());
            Assert.Contains("can_do_this", actions);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetsTenant()
        {
            // Arrange
            this.SetupAsUser();

            // Act
            this.httpRequest.SetTenant(this.mockLogger.Object);

            // Assert
            Assert.Equal("test_tenant", this.httpRequest.GetTenant());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetsTenantAsUserCallNoTenant()
        {
            // Arrange
            this.httpRequest.SetExternalRequest(true);

            // Act
            this.httpRequest.SetTenant(this.mockLogger.Object);

            // Assert
            Assert.Null(this.httpRequest.GetTenant());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetsTenantAsServiceCall()
        {
            // Arrange
            this.SetupAsService();

            // Act
            this.httpRequest.SetTenant(this.mockLogger.Object);

            // Assert
            Assert.Equal("test_tenant", this.httpRequest.GetTenant());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void SetsTenantAsServiceCallNoTenant()
        {
            // Arrange
            this.httpRequest.SetExternalRequest(false);

            // Act
            this.httpRequest.SetTenant(this.mockLogger.Object);

            // Assert
            Assert.Null(this.httpRequest.GetTenant());
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetTenantWhenNone()
        {
            // Arrange

            // Act
            this.httpRequest.SetTenant(this.mockLogger.Object);

            // Assert
            Assert.Null(this.httpRequest.GetTenant());
        }

        private void SetupAsUser()
        {
            this.httpRequest.SetCurrentUserClaims(new List<Claim>()
            {
                new Claim(RequestExtension.UserObjectIdClaimType, "test_user"),
                new Claim(RequestExtension.ClaimKeyTenantId, "test_tenant"),
                new Claim(RequestExtension.RoleClaimType, "admin"),
            });
            this.httpRequest.SetAuthRequired(true);
            this.httpRequest.SetExternalRequest(true);
        }

        private void SetupAsService()
        {
            this.httpRequest.Headers[RequestExtension.HeaderKeyTenantId] = "test_tenant";
            this.httpRequest.SetAuthRequired(true);
            this.httpRequest.SetExternalRequest(false);
        }
    }
}