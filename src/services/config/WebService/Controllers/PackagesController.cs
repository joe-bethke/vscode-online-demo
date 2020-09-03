// <copyright file="PackagesController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.SystemFunctions;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Config.Services;
using Mmm.Iot.Config.Services.Models;
using Mmm.Iot.Config.WebService.Helpers;
using Mmm.Iot.Config.WebService.Models;
using Mmm.Platform.IoT.Common.Services.Models;

namespace Mmm.Iot.Config.WebService.Controllers
{
    [Route("v1/packages")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class PackagesController : Controller
    {
        public const string InactivePackageTag = "reserved.inactive";
        private readonly IStorage storage;

        public PackagesController(IStorage storage)
        {
            this.storage = storage;
        }

        /**
         * This function can be used to get packages with or without parameters
         * PackageType, ConfigType. Without both the query params this will return all
         * the packages. With only packageType the method will return packages of that
         * packageType. If only configType is provided (w/o package type) the method will
         * throw an Exception.
         */
        [HttpGet]
        [Authorize("ReadAll")]
        public async Task<PackageListApiModel> GetFilteredAsync(
            [FromQuery]string packageType,
            [FromQuery]string configType,
            [FromQuery]List<string> tags,
            [FromQuery]string tagOperator = "AND")
        {
            PackageListApiModel packages;
            if (string.IsNullOrEmpty(packageType) && string.IsNullOrEmpty(configType))
            {
                packages = new PackageListApiModel(await this.storage.GetAllPackagesAsync(tags, tagOperator));
            }
            else
            {
                if (string.IsNullOrEmpty(packageType))
                {
                    throw new InvalidInputException("Valid packageType must be provided");
                }

                packages = new PackageListApiModel(await this.storage.GetFilteredPackagesAsync(
                    packageType,
                    configType,
                    tags,
                    tagOperator));
            }

            return packages;
        }

        [HttpGet("{id}")]
        [Authorize("ReadAll")]
        public async Task<PackageApiModel> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidInputException("Valid id must be provided");
            }

            return new PackageApiModel(await this.storage.GetPackageAsync(id));
        }

        [HttpPut("{id:required}/tags/reserved.inactive")]
        [Authorize("TagPackages")]
        public async Task<ActionResult<PackageApiModel>> DeactivatePackageAsync(string id)
        {
            return await this.AddTagAsync(id, InactivePackageTag);
        }

        [HttpDelete("{id:required}/tags/reserved.inactive")]
        [Authorize("TagPackages")]
        public async Task<ActionResult<PackageApiModel>> ActivatePackageAsync(string id)
        {
            return await this.RemoveTagAsync(id, InactivePackageTag);
        }

        [HttpPut("{id:required}/tags/{tag:required:regex(^(?!reserved\\.)[[a-zA-Z0-9\\-\\.]]+$)}")]
        [Authorize("TagPackages")]
        public async Task<ActionResult<PackageApiModel>> AddTagAsync(string id, string tag)
        {
            var package = await this.storage.AddPackageTagAsync(id, tag, this.GetClaimsUserDetails());
            return new PackageApiModel(package);
        }

        [HttpDelete("{id:required}/tags/{tag:required:regex(^(?!reserved\\.)[[a-zA-Z0-9\\.]]+$)}")]
        [Authorize("TagPackages")]
        public async Task<ActionResult<PackageApiModel>> RemoveTagAsync(string id, string tag)
        {
            var package = await this.storage.RemovePackageTagAsync(id, tag, this.GetClaimsUserDetails());
            return new PackageApiModel(package);
        }

        [HttpDelete("{id:required}/tags")]
        [Authorize("TagPackages")]
        public async Task<ActionResult<PackageApiModel>> RemoveTagsAsync(string id)
        {
            var package = await this.GetAsync(id);
            PackageApiModel modifiedPackage = null;
            foreach (var tag in package.Tags)
            {
                modifiedPackage = (await this.RemoveTagAsync(id, tag)).Value;
            }

            return modifiedPackage;
        }

        [HttpPost]
        [Authorize("CreatePackages")]
        public async Task<PackageApiModel> PostAsync(string packageName, string packageType, string configType, string version, List<string> tags, IFormFile package)
        {
            if (string.IsNullOrEmpty(packageType))
            {
                throw new InvalidInputException("Package Type must be provided");
            }

            bool isValidPackageType = Enum.TryParse(packageType, true, out PackageType uploadedPackageType);
            if (!isValidPackageType)
            {
                throw new InvalidInputException($"Provided packageType {packageType} is not valid.");
            }

            if (uploadedPackageType.Equals(PackageType.EdgeManifest) && !string.IsNullOrEmpty(configType))
            {
                throw new InvalidInputException($"Package of type EdgeManifest cannot have parameter " +
                    $"configType.");
            }

            if (configType == null)
            {
                configType = string.Empty;
            }

            if (package == null || package.Length == 0 || string.IsNullOrEmpty(package.FileName))
            {
                throw new InvalidInputException("Package uploaded is missing or invalid.");
            }

            string packageContent;
            using (var streamReader = new StreamReader(package.OpenReadStream()))
            {
                packageContent = await streamReader.ReadToEndAsync();
            }

            if (!PackagesHelper.VerifyPackageType(packageContent, uploadedPackageType))
            {
                throw new InvalidInputException($@"Package uploaded is invalid. Package contents
                            do not match with the given package type {packageType}.");
            }

            var packageToAdd = new PackageApiModel(
                packageContent,
                !string.IsNullOrWhiteSpace(packageName) ? packageName : package.FileName,
                uploadedPackageType,
                version,
                configType,
                tags ?? new List<string>());

            return new PackageApiModel(await this.storage.AddPackageAsync(packageToAdd.ToServiceModel(), this.GetClaimsUserDetails(), this.GetTenantId()));
        }

        [HttpDelete("{id}")]
        [Authorize("DeletePackages")]
        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidInputException("Valid id must be provided");
            }

            await this.storage.DeletePackageAsync(id, this.GetClaimsUserDetails(), this.GetTenantId());
        }

        [HttpPost("UploadFile")]
        [Authorize("CreatePackages")]
        [RequestSizeLimit(200000000)]
        public async Task<UploadFileServiceModel> UploadFileAsync(IFormFile uploadedFile)
        {
            UploadFileServiceModel uploadFileModel = null;
            if (uploadedFile == null || uploadedFile.Length == 0 || string.IsNullOrEmpty(uploadedFile.FileName))
            {
                throw new InvalidInputException("Uploaded file is missing or invalid.");
            }

            using (var stream = uploadedFile.OpenReadStream())
            {
                var tenantId = this.GetTenantId();
                uploadFileModel = await this.storage.UploadToBlobAsync(tenantId, uploadedFile.FileName, stream);
            }

            return uploadFileModel;
        }
    }
}