// <copyright file="ResourceNotFoundException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
            : base()
        {
        }

        public ResourceNotFoundException(string message)
            : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}