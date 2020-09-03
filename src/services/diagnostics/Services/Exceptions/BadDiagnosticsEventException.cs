// <copyright file="BadDiagnosticsEventException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Diagnostics.Services.Exceptions
{
    public class BadDiagnosticsEventException : Exception
    {
        public BadDiagnosticsEventException()
            : base()
        {
        }

        public BadDiagnosticsEventException(string message)
            : base(message)
        {
        }

        public BadDiagnosticsEventException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}