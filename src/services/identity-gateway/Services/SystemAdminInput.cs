// <copyright file="SystemAdminInput.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.CodeAnalysis.Emit;

namespace Mmm.Iot.IdentityGateway.Services
{
    public class SystemAdminInput
    {
        public SystemAdminInput()
        {
        }

        public SystemAdminInput(string userId, string name)
        {
            this.UserId = userId;
            this.Name = name;
        }

        public string UserId { get; set; }

        public string Name { get; set; }
    }
}