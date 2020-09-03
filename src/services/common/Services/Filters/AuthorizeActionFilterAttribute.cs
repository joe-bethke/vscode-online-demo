// <copyright file="AuthorizeActionFilterAttribute.cs" company="3M">
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
    public class AuthorizeActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string allowedAction;

        public AuthorizeActionFilterAttribute(string allowedAction)
        {
            this.allowedAction = allowedAction;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isAuthorized = this.IsValidAuthorization(context.HttpContext, this.allowedAction);

            if (!isAuthorized)
            {
                throw new NotAuthorizedException($"Current user is not authorized to perform this action: '{this.allowedAction}'");
            }
            else
            {
                await next();
            }
        }

        private bool IsValidAuthorization(HttpContext httpContext, string allowedAction)
        {
            if (!httpContext.Request.GetAuthRequired() || !httpContext.Request.IsExternalRequest())
            {
                return true;
            }

            if (allowedAction == null || !allowedAction.Any())
            {
                return true;
            }

            var userAllowedActions = httpContext.Request.GetCurrentUserAllowedActions();
            if (userAllowedActions == null || !userAllowedActions.Any())
            {
                return false;
            }

            // validation succeeds if any required action occurs in the current user's allowed allowedAction
            return userAllowedActions.Select(a => a.ToLowerInvariant())
               .Contains(this.allowedAction.ToLowerInvariant());
        }
    }
}