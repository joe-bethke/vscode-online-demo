// <copyright file="UserContainer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Common.Services.External.TableStorage;

namespace Mmm.Iot.IdentityGateway.Services
{
    public abstract class UserContainer
    {
        public UserContainer()
        {
        }

        public UserContainer(ITableStorageClient tableStorageClient)
        {
            this.TableStorageClient = tableStorageClient;
        }

        public abstract string TableName { get; }

        protected ITableStorageClient TableStorageClient { get; private set; }
    }
}