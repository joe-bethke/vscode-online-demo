// <copyright file="DeviceProperties.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.IoTHubManager.Services.Helpers;
using Mmm.Iot.IoTHubManager.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IoTHubManager.Services
{
    public class DeviceProperties : IDeviceProperties
    {
        public const string CacheCollectioId = "device-twin-properties";
        public const string CacheKey = "cache";
        private const string WhitelistTagPrefix = "tags.";
        private const string WhitelistReportedPrefix = "reported.";
        private const string TagPrefix = "Tags.";
        private const string ReportedPrefix = "Properties.Reported.";
        private const string FileuploadContainerPrefix = "file-upload";
        private readonly IStorageAdapterClient storageClient;
        private readonly IDevices devices;
        private readonly ILogger logger;
        private readonly string whitelist;
        private readonly long ttl;
        private readonly long rebuildTimeout;
        private readonly string deviceFileAccessDuration;
        private readonly string storageConnectionString;
        private readonly TimeSpan serviceQueryInterval = TimeSpan.FromSeconds(10);
        private DateTime devicePropertiesLastUpdated;

        public DeviceProperties(
            IStorageAdapterClient storageClient,
            AppConfig config,
            ILogger<DeviceProperties> logger,
            IDevices devices)
        {
            this.storageClient = storageClient;
            this.logger = logger;
            this.whitelist = config.IotHubManagerService.DevicePropertiesCache.Whitelist;
            this.ttl = config.IotHubManagerService.DevicePropertiesCache.Ttl;
            this.rebuildTimeout = config.IotHubManagerService.DevicePropertiesCache.RebuildTimeout;
            this.deviceFileAccessDuration = config.Global.PackageSharedAccessExpiryTime;
            this.storageConnectionString = config.Global.StorageAccountConnectionString;
            this.devices = devices;
        }

        public async Task<List<string>> GetListAsync()
        {
            ValueApiModel response = new ValueApiModel();
            try
            {
                response = await this.storageClient.GetAsync(CacheCollectioId, CacheKey);
            }
            catch (ResourceNotFoundException)
            {
                this.logger.LogDebug($"Cache get: cache {CacheCollectioId}:{CacheKey} was not found");
            }
            catch (Exception e)
            {
                throw new ExternalDependencyException(
                    $"Cache get: unable to get device-twin-properties cache", e);
            }

            if (string.IsNullOrEmpty(response?.Data))
            {
                throw new Exception($"StorageAdapter did not return any data for {CacheCollectioId}:{CacheKey}. The DeviceProperties cache has not been created for this tenant yet.");
            }

            DevicePropertyServiceModel properties = new DevicePropertyServiceModel();
            try
            {
                properties = JsonConvert.DeserializeObject<DevicePropertyServiceModel>(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidInputException("Unable to deserialize deviceProperties from CosmosDB", e);
            }

            List<string> result = new List<string>();
            foreach (string tag in properties.Tags)
            {
                result.Add(TagPrefix + tag);
            }

            foreach (string reported in properties.Reported)
            {
                result.Add(ReportedPrefix + reported);
            }

            return result;
        }

        public async Task<bool> TryRecreateListAsync(bool force = false)
        {
            var @lock = new StorageWriteLock<DevicePropertyServiceModel>(
                this.storageClient,
                CacheCollectioId,
                CacheKey,
                (c, b) => c.Rebuilding = b,
                m => this.ShouldCacheRebuild(force, m));

            while (true)
            {
                var locked = await @lock.TryLockAsync();
                if (locked == null)
                {
                    this.logger.LogWarning("Cache rebuilding: lock failed due to conflict. Retry soon");
                    continue;
                }

                if (!locked.Value)
                {
                    return false;
                }

                // Build the cache content
                var twinNamesTask = this.GetValidNamesAsync();

                try
                {
                    Task.WaitAll(twinNamesTask);
                }
                catch (Exception)
                {
                    this.logger.LogWarning("Some underlying service is not ready. Retry after {interval}.", this.serviceQueryInterval);
                    try
                    {
                        await @lock.ReleaseAsync();
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError(e, "Cache rebuilding: Unable to release lock");
                    }

                    await Task.Delay(this.serviceQueryInterval);
                    continue;
                }

                var twinNames = twinNamesTask.Result;
                try
                {
                    var updated = await @lock.WriteAndReleaseAsync(
                        new DevicePropertyServiceModel
                        {
                            Tags = twinNames.Tags,
                            Reported = twinNames.ReportedProperties,
                        });
                    if (updated)
                    {
                        this.devicePropertiesLastUpdated = DateTime.Now;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Cache rebuilding: Unable to write and release lock");
                }

                this.logger.LogWarning("Cache rebuilding: write failed due to conflict. Retry soon");
            }
        }

        public async Task<DevicePropertyServiceModel> UpdateListAsync(
            DevicePropertyServiceModel deviceProperties)
        {
            // To simplify code, use empty set to replace null set
            deviceProperties.Tags = deviceProperties.Tags ?? new HashSet<string>();
            deviceProperties.Reported = deviceProperties.Reported ?? new HashSet<string>();

            string etag = null;
            while (true)
            {
                ValueApiModel model = null;
                try
                {
                    model = await this.storageClient.GetAsync(CacheCollectioId, CacheKey);
                }
                catch (ResourceNotFoundException)
                {
                    this.logger.LogInformation($"Cache updating: cache {CacheCollectioId}:{CacheKey} was not found");
                }

                if (model != null)
                {
                    DevicePropertyServiceModel devicePropertiesFromStorage;

                    try
                    {
                        devicePropertiesFromStorage = JsonConvert.
                            DeserializeObject<DevicePropertyServiceModel>(model.Data);
                    }
                    catch
                    {
                        devicePropertiesFromStorage = new DevicePropertyServiceModel();
                    }

                    devicePropertiesFromStorage.Tags = devicePropertiesFromStorage.Tags ??
                        new HashSet<string>();
                    devicePropertiesFromStorage.Reported = devicePropertiesFromStorage.Reported ??
                        new HashSet<string>();

                    deviceProperties.Tags.UnionWith(devicePropertiesFromStorage.Tags);
                    deviceProperties.Reported.UnionWith(devicePropertiesFromStorage.Reported);
                    etag = model.ETag;

                    // If the new set of deviceProperties are already there in cache, return
                    if (deviceProperties.Tags.Count == devicePropertiesFromStorage.Tags.Count &&
                        deviceProperties.Reported.Count == devicePropertiesFromStorage.Reported.Count)
                    {
                        return deviceProperties;
                    }
                }

                var value = JsonConvert.SerializeObject(deviceProperties);
                try
                {
                    var response = await this.storageClient.UpdateAsync(
                        CacheCollectioId, CacheKey, value, etag);
                    return JsonConvert.DeserializeObject<DevicePropertyServiceModel>(response.Data);
                }
                catch (ConflictingResourceException)
                {
                    this.logger.LogInformation("Cache updating: failed due to conflict. Retry soon");
                }
                catch (Exception e)
                {
                    this.logger.LogInformation(e, "Cache updating: failed");
                    throw new Exception("Cache updating: failed");
                }
            }
        }

        public async Task<List<string>> GetUploadedFilesForDevice(string tenantId, string deviceId)
        {
            List<string> result = null;
            string deviceBlobFileContainer = $"{tenantId}-{FileuploadContainerPrefix}/{deviceId}";

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference($"{tenantId}-{FileuploadContainerPrefix}");

            CloudBlobDirectory blobDirectory = blobContainer.GetDirectoryReference(deviceId);

            // Gets List of Blobs
            BlobContinuationToken continuationToken = null;
            var blobResults = await blobDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.All, null, continuationToken, null, null);
            if (blobResults != null)
            {
                var deviceFileUris = blobResults.Results.Select(file => this.GetBlobSasUri(blobClient, deviceBlobFileContainer, file.Uri.Segments.Last(), this.deviceFileAccessDuration));
                if (deviceFileUris != null)
                {
                    result = deviceFileUris.ToList();
                }
            }

            return result;
        }

        private static void ParseWhitelist(
            string whitelist,
            out DeviceTwinName fullNameWhitelist,
            out DeviceTwinName prefixWhitelist)
        {
            var whitelistItems = whitelist.Split(',').Select(s => s.Trim());

            var tags = whitelistItems
                .Where(s => s.StartsWith(WhitelistTagPrefix, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Substring(WhitelistTagPrefix.Length));

            var reported = whitelistItems
                .Where(s => s.StartsWith(WhitelistReportedPrefix, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Substring(WhitelistReportedPrefix.Length));

            var fixedTags = tags.Where(s => !s.EndsWith("*"));

            var fixedReported = reported.Where(s => !s.EndsWith("*"));

            var regexTags = tags.Where(s => s.EndsWith("*")).Select(s => s.Substring(0, s.Length - 1));

            var regexReported = reported.
                Where(s => s.EndsWith("*")).
                Select(s => s.Substring(0, s.Length - 1));

            fullNameWhitelist = new DeviceTwinName
            {
                Tags = new HashSet<string>(fixedTags),
                ReportedProperties = new HashSet<string>(fixedReported),
            };

            prefixWhitelist = new DeviceTwinName
            {
                Tags = new HashSet<string>(regexTags),
                ReportedProperties = new HashSet<string>(regexReported),
            };
        }

        private string GetBlobSasUri(CloudBlobClient cloudBlobClient, string containerName, string blobName, string timeoutDuration)
        {
            string[] time = timeoutDuration.Split(':');
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(Convert.ToDouble(time[0])).AddMinutes(Convert.ToDouble(time[1])).AddSeconds(Convert.ToDouble(time[2]));
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            // Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blockBlob.GetSharedAccessSignature(sasConstraints);

            // Return the URI string for the container, including the SAS token.
            return blockBlob.Uri + sasBlobToken;
        }

        private async Task<DeviceTwinName> GetValidNamesAsync()
        {
            ParseWhitelist(this.whitelist, out var fullNameWhitelist, out var prefixWhitelist);

            var validNames = new DeviceTwinName
            {
                Tags = fullNameWhitelist.Tags,
                ReportedProperties = fullNameWhitelist.ReportedProperties,
            };

            if (prefixWhitelist.Tags.Any() || prefixWhitelist.ReportedProperties.Any())
            {
                DeviceTwinName allNames = new DeviceTwinName();
                try
                {
                    // Get list of DeviceTwinNames from IOT-hub
                    allNames = await this.devices.GetDeviceTwinNamesAsync();
                }
                catch (Exception e)
                {
                    throw new ExternalDependencyException("Unable to fetch IoT devices", e);
                }

                validNames.Tags.UnionWith(allNames.Tags.
                    Where(s => prefixWhitelist.Tags.Any(s.StartsWith)));

                validNames.ReportedProperties.UnionWith(
                    allNames.ReportedProperties.Where(
                        s => prefixWhitelist.ReportedProperties.Any(s.StartsWith)));
            }

            return validNames;
        }

        private bool ShouldCacheRebuild(bool force, ValueApiModel valueApiModel)
        {
            if (force)
            {
                this.logger.LogInformation("Cache will be rebuilt due to the force flag");
                return true;
            }

            if (valueApiModel == null)
            {
                this.logger.LogInformation("Cache will be rebuilt since no cache was found");
                return true;
            }

            DevicePropertyServiceModel cacheValue = new DevicePropertyServiceModel();
            DateTimeOffset timstamp;
            try
            {
                cacheValue = JsonConvert.DeserializeObject<DevicePropertyServiceModel>(valueApiModel.Data);
                timstamp = DateTimeOffset.Parse(valueApiModel.Metadata["$modified"]);
            }
            catch
            {
                this.logger.LogInformation("DeviceProperties will be rebuilt because the last one is broken.");
                return true;
            }

            if (cacheValue.Rebuilding)
            {
                if (timstamp.AddSeconds(this.rebuildTimeout) < DateTimeOffset.UtcNow)
                {
                    this.logger.LogDebug("Cache will be rebuilt because last rebuilding had timedout");
                    return true;
                }
                else
                {
                    this.logger.LogDebug("Cache rebuilding skipped because it is being rebuilt by other instance");
                    return false;
                }
            }
            else
            {
                if (cacheValue.IsNullOrEmpty())
                {
                    this.logger.LogInformation("Cache will be rebuilt since it is empty");
                    return true;
                }

                if (timstamp.AddSeconds(this.ttl) < DateTimeOffset.UtcNow)
                {
                    this.logger.LogInformation("Cache will be rebuilt because it has expired");
                    return true;
                }
                else
                {
                    this.logger.LogDebug("Cache rebuilding skipped because it has not expired");
                    return false;
                }
            }
        }
    }
}