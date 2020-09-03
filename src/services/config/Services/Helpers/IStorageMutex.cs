// <copyright file="IStorageMutex.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Mmm.Iot.Config.Services.Helpers
{
    public interface IStorageMutex
    {
        Task<bool> EnterAsync(string collectionId, string key, TimeSpan timeout);

        Task LeaveAsync(string collectionId, string key);
    }
}