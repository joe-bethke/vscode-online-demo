// <copyright file="IInviteHelpers.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>
using System.Threading.Tasks;
using Mmm.Iot.IdentityGateway.Services.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Mmm.Iot.IdentityGateway.Services.Helpers
{
    public interface IInviteHelpers
    {
        SendGridMessage CreateMessage(Invitation body, string forwardFor, string inviteToken, string requestHost);

        Task<UserSettingsModel> CreateUserSettings(Invitation body, string userId);
    }
}