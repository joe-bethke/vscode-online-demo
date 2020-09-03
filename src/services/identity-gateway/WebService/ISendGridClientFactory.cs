// <copyright file="ISendGridClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using SendGrid;

namespace Mmm.Iot.IdentityGateway.WebService
{
    public interface ISendGridClientFactory
    {
        ISendGridClient CreateSendGridClient();
    }
}