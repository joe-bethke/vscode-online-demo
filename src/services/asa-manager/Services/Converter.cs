// <copyright file="Converter.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.AsaManager.Services.Exceptions;
using Mmm.Iot.AsaManager.Services.Models;
using Mmm.Iot.Common.Services.External.BlobStorage;
using Mmm.Iot.Common.Services.External.StorageAdapter;

namespace Mmm.Iot.AsaManager.Services
{
    public abstract class Converter : IConverter
    {
        private const string ReferenceDataDateFormat = "yyyy-MM-dd";
        private const string ReferenceDataTimeFormat = "HH-mm";
        private const string PathPrefix = "alertinginput";

        private readonly IBlobStorageClient blobStorageClient;
        private readonly string dateTimeFormat = $"{ReferenceDataDateFormat}/{ReferenceDataTimeFormat}";

        public Converter(
            IBlobStorageClient blobStorageClient,
            IStorageAdapterClient storageAdapterClient,
            ILogger<Converter> logger)
        {
            this.blobStorageClient = blobStorageClient;
            this.StorageAdapterClient = storageAdapterClient;
            this.Logger = logger;
        }

        public abstract string Entity { get; }

        public abstract string FileExtension { get; }

        protected ILogger Logger { get; private set; }

        protected IStorageAdapterClient StorageAdapterClient { get; private set; }

        public abstract Task<ConversionApiModel> ConvertAsync(string tenantId, string operationId = null);

        public string GetBlobFilePath()
        {
            string formattedDateTime = DateTimeOffset.UtcNow.ToString(this.dateTimeFormat);
            return $"{PathPrefix}/{formattedDateTime}/{this.Entity}.{this.FileExtension}";
        }

        protected async Task<string> WriteFileContentToBlobAsync(string fileContent, string tenantId, string operationId = null)
        {
            if (string.IsNullOrEmpty(fileContent))
            {
                string errorMessage = $"The temporary file content was null or empty for {this.Entity}. Blank files will not be written to Blob storage. OperationId: {operationId}. TenantId: {tenantId}";
                this.Logger.LogError(new Exception(errorMessage), errorMessage);
                throw new BlankFileContentException($"The temporary file content serialized from the converted {this.Entity} queried from storage adapter was null or empty. Empty files will not be written to Blob storage.");
            }

            string tempFilePath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempFilePath, fileContent);
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Unable to convert {entity} to blob storage file format. OperationId: {operationId}. TenantId: {tenantId}", this.Entity, operationId, tenantId);
                throw new Exception($"Unable to convert {this.Entity} to blob storage file format.", e);
            }

            string blobFilePath = this.GetBlobFilePath();
            try
            {
                await this.blobStorageClient.CreateBlobAsync(tenantId, tempFilePath, blobFilePath);
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Unable to create {entity} blob for tenant. OperationId: {operationId}. TenantId: {tenantId}", this.Entity, operationId, tenantId);
            }

            return blobFilePath;
        }
    }
}