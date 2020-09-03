// <copyright file="BlankFileContentException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.AsaManager.Services.Exceptions
{
    public class BlankFileContentException : Exception
    {
        public BlankFileContentException()
            : base()
        {
        }

        public BlankFileContentException(string message)
            : base(message)
        {
        }

        public BlankFileContentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}