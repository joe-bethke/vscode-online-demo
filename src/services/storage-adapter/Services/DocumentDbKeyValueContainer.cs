// <copyright file="DocumentDbKeyValueContainer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.AppConfiguration;
using Mmm.Iot.Common.Services.External.CosmosDb;
using Mmm.Iot.Common.Services.Wrappers;
using Mmm.Iot.StorageAdapter.Services.Helpers;
using Mmm.Iot.StorageAdapter.Services.Models;
using Index = Microsoft.Azure.Documents.Index;

namespace Mmm.Iot.StorageAdapter.Services
{
    public class DocumentDbKeyValueContainer : IKeyValueContainer, IDisposable
    {
        private const string CollectionIdKeyFormat = "tenant:{0}:{1}-collection";
        private readonly IAppConfigurationClient appConfigClient;
        private readonly AppConfig appConfig;
        private readonly IExceptionChecker exceptionChecker;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private IStorageClient client;
        private bool disposedValue;

        public DocumentDbKeyValueContainer(
            IStorageClient client,
            IExceptionChecker exceptionChecker,
            AppConfig appConfig,
            IAppConfigurationClient appConfigHelper,
            ILogger<DocumentDbKeyValueContainer> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            this.disposedValue = false;
            this.client = client;
            this.exceptionChecker = exceptionChecker;
            this.appConfig = appConfig;
            this.appConfigClient = appConfigHelper;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        public virtual string DocumentDataType
        {
            get
            {
                return "pcs";
            }
        }

        public virtual string DocumentDatabaseSuffix
        {
            get
            {
                return "storage";
            }
        }

        public virtual string DocumentDbDatabaseId
        {
            get
            {
                return $"{this.DocumentDataType}-{this.DocumentDatabaseSuffix}";
            }
        }

        public virtual string DocumentDbCollectionId
        {
            get
            {
                string tenantId = this.TenantId;
                string key = string.Format(CollectionIdKeyFormat, this.TenantId, this.DocumentDataType);
                try
                {
                    return this.appConfigClient.GetValue(key);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Unable to get the CollectionId from App Config. Key: {key}. TenantId: {tenantId}", key, this.TenantId);
                    throw;
                }
            }
        }

        public string TenantId
        {
            get
            {
                return this.httpContextAccessor.HttpContext.Request.GetTenant();
            }
        }

        private string CollectionLink
        {
            get
            {
                return $"/dbs/{this.DocumentDbDatabaseId}/colls/{this.DocumentDbCollectionId}";
            }
        }

        public async Task<ValueServiceModel> GetAsync(string collectionId, string key)
        {
            try
            {
                var docId = DocumentIdHelper.GenerateId(collectionId, key);
                var response = await this.client.ReadDocumentAsync(this.DocumentDbDatabaseId, this.DocumentDbCollectionId, docId);
                return new ValueServiceModel(response);
            }
            catch (Exception ex)
            {
                if (!this.exceptionChecker.IsNotFoundException(ex))
                {
                    throw;
                }

                const string message = "The resource requested doesn't exist.";
                this.logger.LogInformation(message + " {collection ID {collectionId}, key {key}", collectionId, key);
                throw new ResourceNotFoundException(message);
            }
        }

        public async Task<IEnumerable<ValueServiceModel>> GetAllAsync(string collectionId)
        {
            var query = await this.client.QueryAllDocumentsAsync(
                this.DocumentDbDatabaseId,
                this.DocumentDbCollectionId);
            return await Task
                .FromResult(query
                    .Select(doc => new ValueServiceModel(doc))
                    .Where(model => model.CollectionId == collectionId));
        }

        public async Task<ValueServiceModel> CreateAsync(string collectionId, string key, ValueServiceModel input)
        {
            try
            {
                var response = await this.client.CreateDocumentAsync(
                    this.DocumentDbDatabaseId,
                    this.DocumentDbCollectionId,
                    new KeyValueDocument(
                        collectionId,
                        key,
                        input.Data));
                return new ValueServiceModel(response);
            }
            catch (Exception ex)
            {
                if (!this.exceptionChecker.IsConflictException(ex))
                {
                    throw;
                }

                const string message = "There is already a value with the key specified.";
                this.logger.LogInformation(message + " {collection ID {collectionId}, key {key}", collectionId, key);
                throw new ConflictingResourceException(message);
            }
        }

        public async Task<ValueServiceModel> UpsertAsync(string collectionId, string key, ValueServiceModel input)
        {
            try
            {
                var response = await this.client.UpsertDocumentAsync(
                    this.DocumentDbDatabaseId,
                    this.DocumentDbCollectionId,
                    new KeyValueDocument(
                        collectionId,
                        key,
                        input.Data));
                return new ValueServiceModel(response);
            }
            catch (Exception ex)
            {
                if (!this.exceptionChecker.IsPreconditionFailedException(ex))
                {
                    throw;
                }

                const string message = "ETag mismatch: the resource has been updated by another client.";
                this.logger.LogInformation(message + " {collection ID {collectionId}, key {key}, ETag {eTag}", collectionId, key, input.ETag);
                throw new ConflictingResourceException(message);
            }
        }

        public async Task DeleteAsync(string collectionId, string key)
        {
            try
            {
                string documentId = DocumentIdHelper.GenerateId(collectionId, key);
                await this.client.DeleteDocumentAsync(this.DocumentDbDatabaseId, this.DocumentDbCollectionId, documentId);
            }
            catch (Exception ex)
            {
                if (!this.exceptionChecker.IsNotFoundException(ex))
                {
                    throw;
                }

                this.logger.LogDebug("Key {key} does not exist, nothing to do");
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    (this.client as IDisposable)?.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}