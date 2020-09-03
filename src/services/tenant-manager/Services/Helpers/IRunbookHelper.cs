// <copyright file="IRunbookHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using Mmm.Iot.Common.Services;

namespace Mmm.Iot.TenantManager.Services.Helpers
{
    public interface IRunbookHelper : IStatusOperation
    {
        Task<HttpResponseMessage> CreateIotHub(string tenantId, string iotHubName, string dpsName);

        Task<HttpResponseMessage> DeleteIotHub(string tenantId, string iotHubName, string dpsName);

        Task<HttpResponseMessage> CreateAlerting(string tenantId, string saJobName, string iotHubName);

        Task<HttpResponseMessage> DeleteAlerting(string tenantId, string saJobName);
    }
}