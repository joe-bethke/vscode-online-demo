// <copyright file="TimeSeriesParseException.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Exceptions
{
    public class TimeSeriesParseException : Exception
    {
        public TimeSeriesParseException()
            : base()
        {
        }

        public TimeSeriesParseException(string message)
            : base(message)
        {
        }

        public TimeSeriesParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}