// <copyright file="LowercaseContractResolver.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json.Serialization;

namespace Mmm.Iot.IdentityGateway.WebService.Controllers
{
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}