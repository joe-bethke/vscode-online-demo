// <copyright file="ITableStorageClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public interface ITableStorageClient : IStatusOperation
    {
        Task<T> InsertAsync<T>(
            string tableName,
            T entity)
            where T : ITableEntity;

        Task<T> InsertOrReplaceAsync<T>(
            string tableName,
            T entity)
            where T : ITableEntity;

        Task<T> InsertOrMergeAsync<T>(
            string tableName,
            T entity)
            where T : ITableEntity;

        Task<T> RetrieveAsync<T>(
            string tableName,
            string partitionKey,
            string rowKey)
            where T : ITableEntity;

        Task<T> DeleteAsync<T>(
            string tableName,
            T entity)
            where T : ITableEntity;

        Task<List<T>> QueryAsync<T>(
            string tableName,
            TableQuery<T> query,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : ITableEntity, new();
    }
}