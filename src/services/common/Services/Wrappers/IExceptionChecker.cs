// <copyright file="IExceptionChecker.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Wrappers
{
    public interface IExceptionChecker
    {
        bool IsConflictException(Exception exception);

        bool IsPreconditionFailedException(Exception exception);

        bool IsNotFoundException(Exception exception);
    }
}