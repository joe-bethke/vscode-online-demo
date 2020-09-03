// <copyright file="IActionSettings.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mmm.Iot.Config.Services.Models.Actions
{
    public interface IActionSettings
    {
        ActionType Type { get; }

        // Note: This should always be initialized as a case-insensitive dictionary
        IDictionary<string, object> Settings { get; set; }

        Task InitializeAsync();
    }
}