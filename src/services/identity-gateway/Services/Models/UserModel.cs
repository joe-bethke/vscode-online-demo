// <copyright file="UserModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.Services.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }

        public UserModel(string userId, string name)
        {
            this.UserId = userId;
            this.Name = name;
        }

        public string UserId { get; set; }

        public string Name { get; set; }
    }
}