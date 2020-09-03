// <copyright file="NoAuthorizationException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class NoAuthorizationException : Exception
    {
        public NoAuthorizationException()
            : base()
        {
        }

        public NoAuthorizationException(string message)
            : base(message)
        {
        }

        public NoAuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}