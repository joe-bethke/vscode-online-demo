// <copyright file="DocumentClientExceptionChecker.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Net;
using Microsoft.Azure.Documents;
using Mmm.Iot.Common.Services.Wrappers;

namespace Mmm.Iot.StorageAdapter.Services.Wrappers
{
    public class DocumentClientExceptionChecker : IExceptionChecker
    {
        public bool IsConflictException(Exception exception)
        {
            var ex = exception as DocumentClientException;
            return ex != null && ex.StatusCode == HttpStatusCode.Conflict;
        }

        public bool IsPreconditionFailedException(Exception exception)
        {
            var ex = exception as DocumentClientException;
            return ex != null && ex.StatusCode == HttpStatusCode.PreconditionFailed;
        }

        public bool IsNotFoundException(Exception exception)
        {
            var ex = exception as DocumentClientException;
            return ex != null && ex.StatusCode == HttpStatusCode.NotFound;
        }
    }
}