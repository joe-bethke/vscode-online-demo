// <copyright file="ResourceOutOfDateException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class ResourceOutOfDateException : Exception
    {
        public ResourceOutOfDateException()
            : base()
        {
        }

        public ResourceOutOfDateException(string message)
            : base(message)
        {
        }

        public ResourceOutOfDateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}