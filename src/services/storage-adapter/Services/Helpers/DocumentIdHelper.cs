// <copyright file="DocumentIdHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.StorageAdapter.Services.Helpers
{
    public static class DocumentIdHelper
    {
        public static string GenerateId(string collectionId, string key)
        {
            return $"{collectionId.ToLowerInvariant()}.{key.ToLowerInvariant()}";
        }
    }
}