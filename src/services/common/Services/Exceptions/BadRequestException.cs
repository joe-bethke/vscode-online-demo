// <copyright file="BadRequestException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}