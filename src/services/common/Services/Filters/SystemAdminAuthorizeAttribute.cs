// <copyright file="SystemAdminAuthorizeAttribute.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace Mmm.Iot.Common.Services.Filters
{
    public class SystemAdminAuthorizeAttribute : TypeFilterAttribute
    {
        public SystemAdminAuthorizeAttribute()
            : base(typeof(SystemAdminAuthorizeActionFilterAttribute))
        {
        }
    }
}