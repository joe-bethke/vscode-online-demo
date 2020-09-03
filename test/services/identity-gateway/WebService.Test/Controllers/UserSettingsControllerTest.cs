// <copyright file="UserSettingsControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.IdentityGateway.Services;
using Mmm.Iot.IdentityGateway.Services.Models;
using Mmm.Iot.IdentityGateway.WebService.Controllers;
using Moq;
using Xunit;

namespace Mmm.Iot.IdentityGateway.WebService.Test.Controllers
{
    public class UserSettingsControllerTest : IDisposable
    {
        private const string SomeUserId = "someUserId";
        private const string SomeSub = "someSub";
        private const string SomeTenantId = "someTenantId";
        private const string SomeSetting = "someSetting";
        private const string SomeValue = "someValue";
        private Mock<HttpRequest> mockHttpRequest;
        private bool disposedValue = false;
        private Mock<UserSettingsContainer> mockUserSettingsContainer;
        private Mock<ILogger<UserSettingsContainer>> logger;
        private UserSettingsController controller;
        private Mock<HttpContext> mockHttpContext;
        private UserSettingsListModel someUserSettingsList = new UserSettingsListModel();
        private UserSettingsModel someUserSettings = new UserSettingsModel();
        private IDictionary<object, object> contextItems;

        public UserSettingsControllerTest()
        {
            this.InitializeController();
            this.SetupDefaultBehaviors();
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetAllReturnsExpectedUserSettingsList()
        {
            // Arrange
            // Act
            var result = await this.controller.GetAllAsync(SomeUserId);

            // Assert
            Assert.Equal(this.someUserSettingsList, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UserClaimsGetAllReturnsExpectedUserSettingsList()
        {
            // Arrange
            // Act
            var result = await this.controller.UserClaimsGetAllAsync();

            // Assert
            Assert.Equal(this.someUserSettingsList, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.GetAsync(SomeUserId, SomeSetting);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UserClaimsGetReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.UserClaimsGetAsync(SomeSetting);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetUserActiveDeviceGroupExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.GetUserActiveDeviceGroupAsync();

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task PostReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.PostAsync(SomeUserId, SomeSetting, SomeValue);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UserClaimsPostReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.UserClaimsPostAsync(SomeSetting, SomeValue);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task PutReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.PutAsync(SomeUserId, SomeSetting, SomeValue);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UserClaimsPutReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.UserClaimsPutAsync(SomeSetting, SomeValue);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UserActiveDeviceGroupPutReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.PutUserActiveDeviceGroupAsync(SomeValue);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task DeleteReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.DeleteAsync(SomeUserId, SomeSetting);

            // Assert
            Assert.Equal(this.someUserSettings, result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task UserClaimsDeleteReturnsExpectedUserSettings()
        {
            // Arrange
            // Act
            var result = await this.controller.UserClaimsDeleteAsync(SomeSetting);

            // Assert
            Assert.Equal(this.someUserSettings, result);
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
            this.logger = new Mock<ILogger<UserSettingsContainer>>();
            this.mockUserSettingsContainer = new Mock<UserSettingsContainer>(this.logger.Object);
            this.mockHttpContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest = new Mock<HttpRequest> { DefaultValue = DefaultValue.Mock };
            this.controller = new UserSettingsController(this.mockUserSettingsContainer.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = this.mockHttpContext.Object,
                },
            };
        }

        private void SetupDefaultBehaviors()
        {
            this.mockUserSettingsContainer.Setup(m => m.GetAllAsync(It.IsAny<UserSettingsInput>())).ReturnsAsync(this.someUserSettingsList);
            this.mockUserSettingsContainer.Setup(m => m.GetAsync(It.IsAny<UserSettingsInput>())).ReturnsAsync(this.someUserSettings);
            this.mockUserSettingsContainer.Setup(m => m.CreateAsync(It.IsAny<UserSettingsInput>())).ReturnsAsync(this.someUserSettings);
            this.mockUserSettingsContainer.Setup(m => m.UpdateAsync(It.IsAny<UserSettingsInput>())).ReturnsAsync(this.someUserSettings);
            this.mockUserSettingsContainer.Setup(m => m.DeleteAsync(It.IsAny<UserSettingsInput>())).ReturnsAsync(this.someUserSettings);
            this.mockHttpRequest.Setup(m => m.HttpContext).Returns(this.mockHttpContext.Object);
            this.mockHttpContext.Setup(m => m.Request).Returns(this.mockHttpRequest.Object);
            this.contextItems = new Dictionary<object, object>
            {
                {
                    RequestExtension.ContextKeyUserClaims,
                    new List<Claim> { new Claim(RequestExtension.UserObjectIdClaimType, SomeSub) }
                },
                {
                    RequestExtension.ContextKeyTenantId, SomeTenantId
                },
            };
            this.mockHttpContext.Setup(m => m.Items).Returns(this.contextItems);
        }
    }
}