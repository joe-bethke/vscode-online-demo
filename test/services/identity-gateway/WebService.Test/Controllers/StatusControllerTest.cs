// <copyright file="StatusControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.IdentityGateway.WebService.Controllers;
using Moq;
using Xunit;

namespace Mmm.Iot.IdentityGateway.WebService.Test.Controllers
{
    public class StatusControllerTest
    {
        private readonly StatusController controller;
        private readonly Mock<IStatusService> statusServiceMock;
        private readonly Mock<StatusServiceModel> statusServiceModelMock;

        public StatusControllerTest()
        {
            this.statusServiceMock = new Mock<IStatusService>();
            this.controller = new StatusController(this.statusServiceMock.Object);
            this.statusServiceModelMock = new Mock<StatusServiceModel>();
        }

        [Fact]
        public async Task GetAsyncTest()
        {
            // Arrange
            StatusServiceModel statusServiceModel = new StatusServiceModel(true, "Is Alive");
            statusServiceModel.Dependencies.Add("Table Storage", new StatusResultServiceModel(true, "Is Alive"));
            statusServiceModel.Dependencies.Add("AzureB2C", new StatusResultServiceModel(true, "Is Alive"));

            this.statusServiceMock.Setup(x => x.GetStatusAsync()).Returns(Task.FromResult(statusServiceModel));

            // Act
            var result = await this.controller.GetAsync();

            // Assert
            Assert.True(result.Status.IsHealthy);
            Assert.Equal("2", result.Dependencies.Count.ToString());
            foreach (StatusResultApiModel dependency in result.Dependencies.Values)
            {
                Assert.True(dependency.IsHealthy);
            }
        }
    }
}