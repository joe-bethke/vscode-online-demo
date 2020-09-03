// <copyright file="ICorsSetup.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;

namespace Mmm.Iot.Common.Services.Auth
{
    public interface ICorsSetup
    {
        void UseMiddleware(IApplicationBuilder app);
    }
}