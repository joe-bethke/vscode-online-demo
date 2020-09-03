// <copyright file="KeyValueDocument.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;
using Microsoft.Azure.Documents;
using Mmm.Iot.StorageAdapter.Services.Helpers;

[assembly: InternalsVisibleTo("Mmm.Iot.StorageAdapter.Services.Test")]

namespace Mmm.Iot.StorageAdapter.Services
{
    internal sealed class KeyValueDocument : Resource
    {
        public KeyValueDocument(string collectionId, string key, string data)
        {
            this.Id = DocumentIdHelper.GenerateId(collectionId, key);
            this.CollectionId = collectionId;
            this.Key = key;
            this.Data = data;
        }

        public string CollectionId { get; }

        public string Key { get; }

        public string Data { get; }
    }
}