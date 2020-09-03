// <copyright file="ResourceNotSupportedException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class ResourceNotSupportedException : Exception
    {
        public ResourceNotSupportedException()
        {
        }

        public ResourceNotSupportedException(string message)
            : base(message)
        {
        }

        public ResourceNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}