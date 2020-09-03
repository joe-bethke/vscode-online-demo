// <copyright file="AzureResourceManagerClientTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.UserManagement;
using Mmm.Iot.Common.Services.Http;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.Config.Services.External;
using Moq;
using Xunit;

namespace Mmm.Iot.Config.Services.Test
{
    public class AzureResourceManagerClientTest
    {
        private const string MockSubscriptionId = @"123456abcd";
        private const string MockResourceGroup = @"example-name";
        private const string MockArmEndpointUrl = @"https://management.azure.com";
        private const string MockApiVersion = @"2016-06-01";
        private readonly string logicAppTestConnectionUrl;
        private readonly Mock<IHttpClient> mockHttpClient;
        private readonly Mock<IUserManagementClient> mockUserManagementClient;
        private readonly AzureResourceManagerClient client;

        public AzureResourceManagerClientTest()
        {
            this.mockHttpClient = new Mock<IHttpClient>();
            this.mockUserManagementClient = new Mock<IUserManagementClient>();
            this.client = new AzureResourceManagerClient(
                this.mockHttpClient.Object,
                new AppConfig
                {
                    ConfigService = new ConfigServiceConfig
                    {
                        ConfigServiceActions = new ConfigServiceActionsConfig
                        {
                            SubscriptionId = MockSubscriptionId,
                            SolutionName = MockResourceGroup,
                            ArmEndpointUrl = MockArmEndpointUrl,
                            ManagementApiVersion = MockApiVersion,
                        },
                    },
                },
                this.mockUserManagementClient.Object);

            this.logicAppTestConnectionUrl = $"{MockArmEndpointUrl}" +
                                        $"/subscriptions/{MockSubscriptionId}/" +
                                        $"resourceGroups/{MockResourceGroup}/" +
                                        "providers/Microsoft.Web/connections/" +
                                        "office365-connector/extensions/proxy/testconnection?" +
                                        $"api-version={MockApiVersion}";
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetOffice365IsEnabled_ReturnsTrueIfEnabled()
        {
            // Arrange
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
            };

            this.mockHttpClient
                .Setup(x => x.GetAsync(It.IsAny<IHttpRequest>()))
                .ReturnsAsync(response);

            // Act
            var result = await this.client.IsOffice365EnabledAsync();

            // Assert
            this.mockHttpClient
                .Verify(
                    x => x.GetAsync(
                        It.Is<IHttpRequest>(r => r.Check(
                        this.logicAppTestConnectionUrl))), Times.Once);

            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetOffice365IsEnabled_ReturnsFalseIfDisabled()
        {
            // Arrange
            var response = new HttpResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccessStatusCode = false,
            };

            this.mockHttpClient
                .Setup(x => x.GetAsync(It.IsAny<IHttpRequest>()))
                .ReturnsAsync(response);

            // Act
            var result = await this.client.IsOffice365EnabledAsync();

            // Assert
            this.mockHttpClient
                .Verify(
                    x => x.GetAsync(
                        It.Is<IHttpRequest>(r => r.Check(
                        this.logicAppTestConnectionUrl))), Times.Once);

            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.Type, Constants.UnitTest)]
        public async Task GetOffice365IsEnabled_ThrowsIfNotAuthorizd()
        {
            // Arrange
            this.mockUserManagementClient
                .Setup(x => x.GetTokenAsync())
                .ThrowsAsync(new NotAuthorizedException());

            // Act & Assert
            await Assert.ThrowsAsync<NotAuthorizedException>(async () => await this.client.IsOffice365EnabledAsync());
        }
    }
}