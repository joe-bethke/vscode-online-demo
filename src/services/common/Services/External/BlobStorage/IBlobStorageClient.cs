// <copyright file="IBlobStorageClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Mmm.Iot.Common.Services.External.BlobStorage
{
    public interface IBlobStorageClient : IStatusOperation
    {
        Task DeleteBlobContainerAsync(string blobContainerName);

        Task CreateBlobContainerIfNotExistsAsync(string blobContainerName);

        Task CreateBlobAsync(string blobContainerName, string contentFileName, string blobFileName);
    }
}