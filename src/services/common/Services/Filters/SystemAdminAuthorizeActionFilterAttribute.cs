// <copyright file="SystemAdminAuthorizeActionFilterAttribute.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Mmm.Iot.Common.Services;
using Mmm.Iot.Common.Services.Exceptions;

namespace Mmm.Iot.Common.Services.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate)]
    public class SystemAdminAuthorizeActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isAuthorized = this.IsValidAuthorization(context.HttpContext);

            if (!isAuthorized)
            {
                throw new NotAuthorizedException($"Current user is not authorized to perform this action");
            }
            else
            {
                await next();
            }
        }

        private bool IsValidAuthorization(HttpContext httpContext)
        {
            if (!httpContext.Request.GetAuthRequired() || !httpContext.Request.IsExternalRequest())
            {
                return true;
            }

            return httpContext.Request.IsSystemAdmin();
        }
    }
}