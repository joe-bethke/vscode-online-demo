// <copyright file="StorageMutex.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External;
using Mmm.Iot.Common.Services.External.StorageAdapter;

namespace Mmm.Iot.Config.Services.Helpers
{
    public class StorageMutex : IStorageMutex
    {
        private const string LastModifiedKey = "$modified";
        private readonly IStorageAdapterClient storageClient;
        private readonly ILogger logger;

        public StorageMutex(
            IStorageAdapterClient storageClient,
            ILogger<StorageMutex> logger)
        {
            this.storageClient = storageClient;
            this.logger = logger;
        }

        public async Task<bool> EnterAsync(string collectionId, string key, TimeSpan timeout)
        {
            string etag = null;

            while (true)
            {
                try
                {
                    var model = await this.storageClient.GetAsync(collectionId, key);
                    etag = model.ETag;

                    // Mutex was captured by some other instance, return `false` except the state was not updated for a long time
                    // The motivation of timeout check is to recovery from stale state due to instance crash
                    if (Convert.ToBoolean(model.Data))
                    {
                        if (model.Metadata.ContainsKey(LastModifiedKey) && DateTimeOffset.TryParse(model.Metadata[LastModifiedKey], out var lastModified))
                        {
                            // Timestamp retrieved successfully, nothing to do
                            this.logger.LogInformation($"Mutex {collectionId}.{key} was occupied. Last modified = {lastModified}");
                        }
                        else
                        {
                            // Treat it as timeout if the timestamp could not be retrieved
                            lastModified = DateTimeOffset.MinValue;
                            this.logger.LogInformation("Mutex {collectionId}.{key} was occupied. Last modified could not be retrieved");
                        }

                        if (DateTimeOffset.UtcNow < lastModified + timeout)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"Mutex {collectionId}.{key} was NOT occupied");
                    }
                }
                catch (ResourceNotFoundException)
                {
                    // Mutex is not initialized, treat it as released
                    this.logger.LogInformation($"Mutex {collectionId}.{key} was not found");
                }

                try
                {
                    // In case there is no such a mutex, the `etag` will be null. It will cause
                    // a new mutex created, and the operation will be synchronized
                    await this.storageClient.UpdateAsync(collectionId, key, "true", etag);

                    // Successfully enter the mutex, return `true`
                    return true;
                }
                catch (ConflictingResourceException)
                {
                    // Etag does not match. Restart the whole process
                }
            }
        }

        public async Task LeaveAsync(string collectionId, string key)
        {
            await this.storageClient.UpdateAsync(collectionId, key, "false", "*");
        }
    }
}