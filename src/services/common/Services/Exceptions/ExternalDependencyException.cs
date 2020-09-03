// <copyright file="ExternalDependencyException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class ExternalDependencyException : Exception
    {
        public ExternalDependencyException()
            : base()
        {
        }

        public ExternalDependencyException(string message)
            : base(message)
        {
        }

        public ExternalDependencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ExternalDependencyException(Exception innerException)
            : base("An unexpected error happened while using an external dependency.", innerException)
        {
        }
    }
}