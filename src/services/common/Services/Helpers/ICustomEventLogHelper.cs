// <copyright file="ICustomEventLogHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services.Helpers
{
    public interface ICustomEventLogHelper
    {
        void LogCustomEvent(string logDescription, string logHeaderField, CustomEvent eventInfo);
    }
}