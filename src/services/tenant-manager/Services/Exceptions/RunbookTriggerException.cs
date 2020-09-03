// <copyright file="RunbookTriggerException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.TenantManager.Services.Exceptions
{
    public class RunbookTriggerException : Exception
    {
        public RunbookTriggerException()
            : base()
        {
        }

        public RunbookTriggerException(string message)
            : base(message)
        {
        }

        public RunbookTriggerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}