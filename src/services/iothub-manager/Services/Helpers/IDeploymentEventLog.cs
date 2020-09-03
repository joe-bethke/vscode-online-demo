// <copyright file="IDeploymentEventLog.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.IoTHubManager.Services.Models;

namespace Mmm.Iot.IoTHubManager.Services.Helpers
{
    public interface IDeploymentEventLog
    {
        void LogDeploymentCreate(DeploymentServiceModel deployment, string tenantId, string userId);

        void LogDeploymentDelete(string deploymentId, string tenantId, string userId);
    }
}