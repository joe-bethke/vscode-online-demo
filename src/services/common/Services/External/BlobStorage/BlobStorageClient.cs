// <copyright file="BlobStorageClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services.External.BlobStorage
{
    public class BlobStorageClient : IBlobStorageClient
    {
        private readonly BlobServiceClient client;

        public BlobStorageClient(AppConfig config)
        {
            this.client = new BlobServiceClient(config.Global.StorageAccountConnectionString);
        }

        public async Task<StatusResultServiceModel> StatusAsync()
        {
            try
            {
                var accountInfo = await this.client.GetAccountInfoAsync();
                if (accountInfo != null)
                {
                    return new StatusResultServiceModel(true, "Alive and well!");
                }
                else
                {
                    return new StatusResultServiceModel(false, $"Unable to retrieve Storage Account Information from the Blob Storage Client Container.");
                }
            }
            catch (Exception e)
            {
                return new StatusResultServiceModel(false, $"Table Storage check failed: {e.Message}");
            }
        }

        public async Task DeleteBlobContainerAsync(string blobContainerName)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(blobContainerName);
            await containerClient.DeleteAsync();
        }

        public async Task CreateBlobContainerIfNotExistsAsync(string blobContainerName)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(blobContainerName);
            await containerClient.CreateIfNotExistsAsync();
        }

        public async Task CreateBlobAsync(string blobContainerName, string contentFileName, string blobFileName)
        {
            // Get the client for writing to this container
            BlobClient createBlobClient = this.client.GetBlobContainerClient(blobContainerName).GetBlobClient(blobFileName);

            try
            {
                FileStream uploadStream = File.OpenRead(contentFileName);
                await createBlobClient.UploadAsync(uploadStream);
                uploadStream.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Unable to upload the contents of temporary local file {tempFileName} to blob storage.", e);
            }
        }
    }
}