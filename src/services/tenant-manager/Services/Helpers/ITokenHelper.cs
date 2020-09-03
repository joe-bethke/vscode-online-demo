// <copyright file="ITokenHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Mmm.Iot.TenantManager.Services.Helpers
{
    public interface ITokenHelper
    {
        Task<string> GetTokenAsync();
    }
}