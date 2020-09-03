// <copyright file="IPackageEventLog.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Config.Services.Models;

namespace Mmm.Iot.Config.Services.Helpers
{
    public interface IPackageEventLog
    {
        void LogPackageUpload(PackageServiceModel package, string tenantId, string userId);

        void LogPackageDelete(string packageId, string tenantId, string userId);
    }
}