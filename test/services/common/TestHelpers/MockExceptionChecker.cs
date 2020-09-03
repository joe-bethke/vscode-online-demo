// <copyright file="MockExceptionChecker.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Wrappers;

namespace Mmm.Iot.Common.TestHelpers
{
    public class MockExceptionChecker : IExceptionChecker
    {
        public bool IsConflictException(Exception exception)
        {
            return exception is ConflictingResourceException;
        }

        public bool IsPreconditionFailedException(Exception exception)
        {
            return exception is ConflictingResourceException;
        }

        public bool IsNotFoundException(Exception exception)
        {
            return exception is ResourceNotFoundException;
        }
    }
}