// <copyright file="SystemAdminContainer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.External.TableStorage;
using Mmm.Iot.IdentityGateway.Services.Models;

namespace Mmm.Iot.IdentityGateway.Services
{
    public class SystemAdminContainer : UserContainer
    {
        private readonly ILogger logger;

        public SystemAdminContainer(ITableStorageClient tableStorageClient, ILogger<SystemAdminContainer> logger)
            : base(tableStorageClient)
        {
            this.logger = logger;
        }

        public SystemAdminContainer(ILogger<SystemAdminContainer> logger)
        {
            this.logger = logger;
        }

        public override string TableName => "systemAdmin";

        public string PartitionKey => "PartitionKey";

        public string GetSystemAdminBatchMethod => "GetSystemAdmin";

        public virtual async Task<SystemAdminModel> CreateAsync(SystemAdminInput systemAdminInput)
        {
            SystemAdminModel systemAdmin = new SystemAdminModel(systemAdminInput);
            return await this.TableStorageClient.InsertAsync(this.TableName, systemAdmin);
        }

        public virtual async Task<SystemAdminListModel> GetAsync(SystemAdminInput input)
        {
            TableQuery<SystemAdminModel> query = new TableQuery<SystemAdminModel>().Where(TableQuery.GenerateFilterCondition(this.PartitionKey, QueryComparisons.Equal, input.UserId));
            List<SystemAdminModel> result = await this.TableStorageClient.QueryAsync<SystemAdminModel>(this.TableName, query);
            return new SystemAdminListModel(this.GetSystemAdminBatchMethod, result);
        }

        public virtual async Task<SystemAdminListModel> GetAllAsync()
        {
            TableQuery<SystemAdminModel> query = new TableQuery<SystemAdminModel>().Where(TableQuery.GenerateFilterCondition(this.PartitionKey, QueryComparisons.NotEqual, string.Empty));
            List<SystemAdminModel> result = await this.TableStorageClient.QueryAsync<SystemAdminModel>(this.TableName, query);
            return new SystemAdminListModel(this.GetSystemAdminBatchMethod, result);
        }

        public virtual async Task<SystemAdminModel> DeleteAsync(SystemAdminInput input)
        {
            SystemAdminListModel model = await this.GetAsync(input);
            if (model == null)
            {
                string errorMessage = $"Super user  does not exist with {input.UserId}";
                StorageException exception = new StorageException(errorMessage);
                this.logger.LogError(exception, errorMessage);
                throw exception;
            }
            else
            {
                return await this.TableStorageClient.DeleteAsync<SystemAdminModel>(this.TableName, model.Models.FirstOrDefault());
            }
        }
    }
}