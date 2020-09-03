// <copyright file="CloudTableClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Cosmos.Table;
using Mmm.Iot.Common.Services.Config;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public class CloudTableClientFactory : ICloudTableClientFactory
    {
        private readonly AppConfig config;

        public CloudTableClientFactory(AppConfig config)
        {
            this.config = config;
        }

        public CloudTableClient Create()
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.Global.StorageAccountConnectionString);
            return storageAccount.CreateCloudTableClient();
        }
    }
}