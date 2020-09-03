// <copyright file="EmptyEntitiesException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.AsaManager.Services.Exceptions
{
    public class EmptyEntitiesException : Exception
    {
        public EmptyEntitiesException()
            : base()
        {
        }

        public EmptyEntitiesException(string message)
            : base(message)
        {
        }

        public EmptyEntitiesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}