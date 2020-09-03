// <copyright file="PackageControllerTest.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.TestHelpers;
using Mmm.Iot.Config.Services;
using Mmm.Iot.Config.Services.Models;
using Mmm.Iot.Config.WebService.Controllers;
using Mmm.Iot.Config.WebService.Models;
using Mmm.Platform.IoT.Common.Services.Models;
using Moq;
using TestStack.Dossier;
using TestStack.Dossier.EquivalenceClasses;
using Xunit;

namespace Mmm.Iot.Config.WebService.Test.Controllers
{
    public class PackageControllerTest : IDisposable
    {
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
        private const string TenantId = "TenantId";
        private readonly Mock<IStorage> mockStorage;
        private readonly PackagesController controller;
        private readonly Random rand;
        private readonly AnonymousValueFixture any;
        private readonly List<Claim> claims;
        private Mock<HttpContext> mockHttpContext;
        private Mock<HttpRequest> mockHttpRequest;
        private IDictionary<object, object> contextItems;
        private bool disposedValue = false;
        private List<string> tags = new List<string>
        {
            "alpha",
            "namespace.alpha",
            "alphanum3ric",
            "namespace.alphanum3ric",
            "1234567890",
            "namespace.1234567890",
            null,
            string.Empty,
        };

        private string packageId = "myId";

        public PackageControllerTest()
        {
            this.mockStorage = new Mock<IStorage>();
            this.mockHttpContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest = new Mock<HttpRequest> { DefaultValue = DefaultValue.Mock };
            this.mockHttpRequest.Setup(m => m.HttpContext).Returns(this.mockHttpContext.Object);
            this.mockHttpContext.Setup(m => m.Request).Returns(this.mockHttpRequest.Object);
            this.claims = new List<Claim>();
            this.claims.Add(new Claim("sub", "Admin"));
            this.claims.Add(new Claim("email", "TenantId@mmm.com"));
            this.controller = new PackagesController(this.mockStorage.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = this.mockHttpContext.Object,
                },
            };
            this.contextItems = new Dictionary<object, object>
            {
                {
                    RequestExtension.ContextKeyTenantId, TenantId
                },
                {
                    RequestExtension.ContextKeyUserClaims, this.claims
                },
            };
            this.mockHttpContext.Setup(m => m.Items).Returns(this.contextItems);
            this.rand = new Random();
            this.any = new AnonymousValueFixture();
        }

        [Theory]
        [Trait(Constants.Type, Constants.UnitTest)]
        [InlineData("EdgeManifest", "filename", true, false)]
        [InlineData("EdgeManifest", "filename", false, true)]
        [InlineData("EdgeManifest", "filename", false, true, true)]
        [InlineData("DeviceConfiguration", "filename", true, false, true)]
        [InlineData("EdgeManifest", "", true, true)]
        [InlineData("BAD_TYPE", "filename", true, true)]
        public async Task PostAsyncExceptionVerificationTest(
            string type,
            string filename,
            bool isValidFileProvided,
            bool expectException,
            bool shouldHaveConfig = false)
        {
            // Arrange
            IFormFile file = null;
            if (isValidFileProvided)
            {
                bool isEdgePackage = (type == "EdgeManifest") ? true : false;
                file = this.CreateSampleFile(filename, isEdgePackage);
            }

            Enum.TryParse(type, out PackageType pckgType);

            this.mockStorage.Setup(x => x.AddPackageAsync(
                                    It.Is<PackageServiceModel>(p => p.PackageType.ToString().Equals(type) &&
                                                        p.Name.Equals(filename)),
                                    It.IsAny<string>(),
                                    It.IsAny<string>()))
                            .ReturnsAsync(new PackageServiceModel()
                            {
                                Name = filename,
                                PackageType = pckgType,
                            });

            var configType = shouldHaveConfig ? "customconfig" : null;

            try
            {
                // Act
                var package = await this.controller.PostAsync(string.Empty, type, configType, string.Empty, new List<string>(), file);

                // Assert
                Assert.False(expectException);
                Assert.Equal(filename, package.Name);
                Assert.Equal(type, package.PackageType.ToString());
            }
            catch (Exception)
            {
                Assert.True(expectException);
            }
        }

        [Fact]
        public async Task GetPackageTest()
        {
            // Arrange
            const string id = "packageId";
            const string name = "packageName";
            const PackageType type = PackageType.EdgeManifest;
            const string content = "{}";
            string dateCreated = DateTime.UtcNow.ToString(DateFormat);

            this.mockStorage
                .Setup(x => x.GetPackageAsync(id))
                .ReturnsAsync(new PackageServiceModel()
                {
                    Id = id,
                    Name = name,
                    Content = content,
                    PackageType = type,
                    ConfigType = string.Empty,
                    DateCreated = dateCreated,
                });

            // Act
            var pkg = await this.controller.GetAsync(id);

            // Assert
            this.mockStorage
                .Verify(x => x.GetPackageAsync(id), Times.Once);

            Assert.Equal(id, pkg.Id);
            Assert.Equal(name, pkg.Name);
            Assert.Equal(type, pkg.PackageType);
            Assert.Equal(content, pkg.Content);
            Assert.Equal(dateCreated, pkg.DateCreated);
        }

        [Fact]
        public async Task GetAllPackageTest()
        {
            // Arrange
            const string id = "packageId";
            const string name = "packageName";
            const PackageType type = PackageType.EdgeManifest;
            string config = string.Empty;
            const string content = "{}";
            string dateCreated = DateTime.UtcNow.ToString(DateFormat);

            int[] idx = new int[] { 0, 1, 2 };
            var packages = idx.Select(i => new PackageServiceModel()
            {
                Id = id + i,
                Name = name + i,
                Content = content + i,
                PackageType = type,
                ConfigType = config + i,
                DateCreated = dateCreated,
            }).ToList();

            this.mockStorage
                .Setup(x => x.GetAllPackagesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .ReturnsAsync(packages);

            // Act
            var resultPackages = await this.controller.GetFilteredAsync(null, null, null);

            // Assert
            this.mockStorage
                .Verify(x => x.GetAllPackagesAsync(null, "AND"), Times.Once);

            foreach (int i in idx)
            {
                var pkg = resultPackages.Items.ElementAt(i);
                Assert.Equal(id + i, pkg.Id);
                Assert.Equal(name + i, pkg.Name);
                Assert.Equal(type, pkg.PackageType);
                Assert.Equal(content + i, pkg.Content);
                Assert.Equal(dateCreated, pkg.DateCreated);
            }
        }

        [Fact]
        public async Task ListPackagesTest()
        {
            // Arrange
            const string id = "packageId";
            const string name = "packageName";
            const PackageType type = PackageType.DeviceConfiguration;
            const string content = "{}";
            string dateCreated = DateTime.UtcNow.ToString(DateFormat);

            int[] idx = new int[] { 0, 1, 2 };
            var packages = idx.Select(i => new PackageServiceModel()
            {
                Id = id + i,
                Name = name + i,
                Content = content + i,
                PackageType = type + i,
                ConfigType = (i == 0) ? ConfigType.Firmware.ToString() : i.ToString(),
                DateCreated = dateCreated,
            }).ToList();

            this.mockStorage
                .Setup(x => x.GetFilteredPackagesAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(packages);

            // Act
            var resultPackages = await this.controller.GetFilteredAsync(
                                                    PackageType.DeviceConfiguration.ToString(),
                                                    ConfigType.Firmware.ToString(),
                                                    null,
                                                    null);

            // Assert
            this.mockStorage.Verify(
                x => x.GetFilteredPackagesAsync(
                    PackageType.DeviceConfiguration.ToString(),
                    ConfigType.Firmware.ToString(),
                    null,
                    null),
                Times.Once);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        [Theory]
        [Trait(Constants.Type, Constants.UnitTest)]
        [InlineData("filename", true, false)]
        [InlineData("filename", false, true)]
        public async Task UploadFileVerificationTest(string filename, bool isValidFileProvided, bool expectException)
        {
            // Arrange
            IFormFile file = null;

            UploadFileServiceModel uploadFileModel = new UploadFileServiceModel();
            uploadFileModel.CheckSum = new CheckSumModel();
            uploadFileModel.SoftwarePackageURL = "filename";
            uploadFileModel.CheckSum.MD5 = "checkSum";

            if (isValidFileProvided)
            {
                file = this.CreateSampleFile(filename, false);
            }

            this.mockStorage.Setup(x => x.UploadToBlobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
                            .ReturnsAsync(uploadFileModel);
            try
            {
                // Act
                var uploadedFileName = await this.controller.UploadFileAsync(file);

                // Assert
                Assert.False(expectException);
                Assert.Equal(filename, uploadedFileName.SoftwarePackageURL);
                Assert.Equal("checkSum", uploadedFileName.CheckSum.MD5);
            }
            catch (Exception)
            {
                Assert.True(expectException);
            }
        }

        [Fact]
        public async Task PackageTagsCanBeAdded()
        {
            // Arrange
            var tag = this.any.String();
            this.mockStorage.Setup(m => m.AddPackageTagAsync(this.packageId, tag, It.IsAny<string>())).ReturnsAsync(new PackageServiceModel { Tags = new List<string> { tag } });

            // Act
            var result = await this.controller.AddTagAsync(this.packageId, tag);

            // Assert
            Assert.IsType<ActionResult<PackageApiModel>>(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Value);
            Assert.Contains(tag, result.Value.Tags);
        }

        [Fact]
        public async Task PackageTagsCanBeRemoved()
        {
            // Arrange
            var tag = this.any.String();
            this.mockStorage.Setup(m => m.RemovePackageTagAsync(this.packageId, tag, It.IsAny<string>())).ReturnsAsync(new PackageServiceModel { Tags = this.tags.Except(new[] { tag }).ToList() });

            // Act
            var result = await this.controller.RemoveTagAsync(this.packageId, tag);

            // Assert
            Assert.IsType<ActionResult<PackageApiModel>>(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Value);
            Assert.DoesNotContain(tag, result.Value.Tags);
        }

        [Fact]
        public async Task AllPackageTagsCanBeRemoved()
        {
            // Arrange
            this.mockStorage.Setup(m => m.GetPackageAsync(this.packageId)).ReturnsAsync(new PackageServiceModel { Tags = this.tags });
            this.mockStorage.Setup(m => m.RemovePackageTagAsync(this.packageId, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string pid, string t, string userId) =>
            {
                this.tags = this.tags.Except(new[] { t }.ToList()).ToList();
                return new PackageServiceModel { Tags = this.tags };
            });

            // Act
            var result = await this.controller.RemoveTagsAsync(this.packageId);

            // Assert
            Assert.IsType<ActionResult<PackageApiModel>>(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Tags);
        }

        [Fact]
        public async Task PackagesCanBeActivated()
        {
            // Arrange
            this.tags.Add(PackagesController.InactivePackageTag);
            this.mockStorage.Setup(m => m.RemovePackageTagAsync(this.packageId, PackagesController.InactivePackageTag, It.IsAny<string>())).ReturnsAsync((string pid, string t, string userId) =>
            {
                this.tags = this.tags.Except(new[] { t }.ToList()).ToList();
                return new PackageServiceModel { Tags = this.tags };
            });

            // Act
            var result = await this.controller.ActivatePackageAsync(this.packageId);

            // Assert
            Assert.IsType<ActionResult<PackageApiModel>>(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Value);
            Assert.DoesNotContain(PackagesController.InactivePackageTag, result.Value.Tags);
        }

        [Fact]
        public async Task PackagesCanBeDeactivated()
        {
            // Arrange
            this.mockStorage.Setup(m => m.AddPackageTagAsync(this.packageId, PackagesController.InactivePackageTag, It.IsAny<string>())).ReturnsAsync((string pid, string t, string userId) =>
            {
                this.tags.Add(t);
                return new PackageServiceModel { Tags = this.tags };
            });

            // Act
            var result = await this.controller.DeactivatePackageAsync(this.packageId);

            // Assert
            Assert.IsType<ActionResult<PackageApiModel>>(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Value);
            Assert.Contains(PackagesController.InactivePackageTag, result.Value.Tags);
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

        private FormFile CreateSampleFile(string filename, bool isEdgePackage)
        {
            var admPackage = "{\"id\":\"dummy\",\"content\":{\"deviceContent\":{}}}";
            var edgePackage = "{\"id\":\"dummy\",\"content\":{\"modulesContent\":{}}}";

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var package = isEdgePackage ? edgePackage : admPackage;

            writer.Write(package);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, package.Length, "file", filename);
        }
    }
}