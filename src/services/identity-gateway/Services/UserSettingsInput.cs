// <copyright file="UserSettingsInput.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.IdentityGateway.Services.Models;

namespace Mmm.Iot.IdentityGateway.Services
{
    public class UserSettingsInput : IUserInput<UserSettingsModel>
    {
        public string UserId { get; set; }

        public string SettingKey { get; set; }

        public string Value { get; set; }
    }
}