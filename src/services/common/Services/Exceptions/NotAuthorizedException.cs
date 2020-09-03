// <copyright file="NotAuthorizedException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
            : base()
        {
        }

        public NotAuthorizedException(string message)
            : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}