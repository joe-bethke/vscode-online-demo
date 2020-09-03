// <copyright file="AuthorizeAttribute.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace Mmm.Iot.Common.Services.Filters
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string allowedActions)
            : base(typeof(AuthorizeActionFilterAttribute))
        {
            this.Arguments = new object[] { allowedActions };
        }
    }
}