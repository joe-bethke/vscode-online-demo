// <copyright file="IotHubKeyException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.TenantManager.Services.Exceptions
{
    public class IotHubKeyException : Exception
    {
        public IotHubKeyException()
            : base()
        {
        }

        public IotHubKeyException(string message)
            : base(message)
        {
        }

        public IotHubKeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}