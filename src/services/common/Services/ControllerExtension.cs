// <copyright file="ControllerExtension.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Mvc;

namespace Mmm.Iot.Common.Services
{
    public static class ControllerExtension
    {
        public static string GetTenantId(this Controller controller)
        {
            try
            {
                string tenantId = controller.HttpContext.Request.GetTenant();
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("Request Tenant was null or empty.");
                }

                return tenantId;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to get the tenantId from the user claims or the request headers.", e);
            }
        }

        public static string GetClaimsUserId(this Controller controller)
        {
            try
            {
                return controller.HttpContext.Request.GetCurrentUserObjectId();
            }
            catch (Exception e)
            {
                throw new Exception("A request was sent to an API endpoint that requires a userId, but the userId was not passed through the url nor was it available in the user Claims.", e);
            }
        }

        public static string GetClaimsUserDetails(this Controller controller)
        {
            try
            {
                return controller.HttpContext.Request.GetCurrentUserDetails();
            }
            catch (Exception e)
            {
                throw new Exception("A request was sent to an API endpoint that requires an UserId or UserEmail, but the details were not passed through the url nor was it available in the user Claims.", e);
            }
        }
    }
}