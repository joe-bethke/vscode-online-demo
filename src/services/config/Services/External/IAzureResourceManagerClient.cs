// <copyright file="IAzureResourceManagerClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Mmm.Iot.Config.Services.External
{
    public interface IAzureResourceManagerClient
    {
        Task<bool> IsOffice365EnabledAsync();
    }
}