// <copyright file="ConfigurationControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.IdentityGateway.Services.Helpers;
using Mmm.Iot.IdentityGateway.Services.Models;
using Mmm.Iot.IdentityGateway.WebService.Controllers;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Mmm.Iot.IdentityGateway.WebService.Test.Controllers
{
    public class ConfigurationControllerTest : IDisposable
    {
        private const string SomePublicKey = "-----BEGIN PUBLIC KEY-----\r\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAryQICCl6NZ5gDKrnSztO\r\n3Hy8PEUcuyvg/ikC+VcIo2SFFSf18a3IMYldIugqqqZCs4/4uVW3sbdLs/6PfgdX\r\n7O9D22ZiFWHPYA2k2N744MNiCD1UE+tJyllUhSblK48bn+v1oZHCM0nYQ2NqUkvS\r\nj+hwUU3RiWl7x3D2s9wSdNt7XUtW05a/FXehsPSiJfKvHJJnGOX0BgTvkLnkAOTd\r\nOrUZ/wK69Dzu4IvrN4vs9Nes8vbwPa/ddZEzGR0cQMt0JBkhk9kU/qwqUseP1QRJ\r\n5I1jR4g8aYPL/ke9K35PxZWuDp3U0UPAZ3PjFAh+5T+fc7gzCs9dPzSHloruU+gl\r\nFQIDAQAB\r\n-----END PUBLIC KEY-----";
        private const string SomeUri = "http://azureb2caseuri.com";
        private const string SomeIssuer = "http://someissuer";
        private bool disposedValue = false;
        private ConfigurationController controller;
        private Mock<HttpContext> mockHttpContext;
        private Mock<AppConfig> mockAppConfig;
        private Mock<OpenIdProviderConfiguration> mockOpenIdProviderConfiguration;
        private Mock<IRsaHelpers> mockRsaHelpers;
        private JsonWebKeySet someJwks = new JsonWebKeySet();

        public ConfigurationControllerTest()
        {
            this.InitializeController();
            this.SetupDefaultBehaviors();
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetOpenIdProviderConfigurationReturnsExpectedIssuer()
        {
            // Arrange
            // Act
            var result = this.controller.GetOpenIdProviderConfiguration() as OkObjectResult;
            var openIdProviderConfiguration = result.Value as IOpenIdProviderConfiguration;

            // Assert
            Assert.Equal(SomeIssuer, openIdProviderConfiguration.Issuer);
            Assert.Equal(ConfigurationController.ContentType, result.ContentTypes[0]);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public void GetJsonWebKeySetReturnsExpctedJwks()
        {
            // Arrange
            // Act
            var result = this.controller.GetJsonWebKeySet();
            var jwks = JsonConvert.DeserializeObject<JsonWebKeySet>(result.Content);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(this.someJwks.AdditionalData, jwks.AdditionalData);
            Assert.Equal(this.someJwks.Keys, jwks.Keys);
            Assert.Equal(ConfigurationController.ContentType, result.ContentType.ToLowerInvariant());
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.controller.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private void InitializeController()
        {
            this.mockAppConfig = new Mock<AppConfig> { DefaultValue = DefaultValue.Mock };
            this.mockHttpContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            this.mockOpenIdProviderConfiguration = new Mock<OpenIdProviderConfiguration> { DefaultValue = DefaultValue.Mock };
            this.mockRsaHelpers = new Mock<IRsaHelpers> { DefaultValue = DefaultValue.Mock };
            this.controller = new ConfigurationController(this.mockAppConfig.Object, this.mockOpenIdProviderConfiguration.Object, this.mockRsaHelpers.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = this.mockHttpContext.Object,
                },
            };
        }

        private void SetupDefaultBehaviors()
        {
            this.mockAppConfig.Setup(m => m.Global.AzureB2cBaseUri).Returns(SomeUri);
            this.mockAppConfig.Setup(m => m.IdentityGatewayService.PublicKey).Returns(SomePublicKey);
            this.mockOpenIdProviderConfiguration.SetupGet(m => m.Issuer).Returns(SomeIssuer);
            this.mockRsaHelpers.Setup(m => m.GetJsonWebKey(It.IsAny<string>())).Returns(this.someJwks);
        }
    }
}