// <copyright file="StorageAccountKeyException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.TenantManager.Services.Exceptions
{
    public class StorageAccountKeyException : Exception
    {
        public StorageAccountKeyException()
            : base()
        {
        }

        public StorageAccountKeyException(string message)
            : base(message)
        {
        }

        public StorageAccountKeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}