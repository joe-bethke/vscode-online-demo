// <copyright file="SendGridClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Common.Services.Config;
using SendGrid;

namespace Mmm.Iot.IdentityGateway.WebService
{
    public class SendGridClientFactory : ISendGridClientFactory
    {
        private readonly AppConfig config;

        public SendGridClientFactory(AppConfig config)
        {
            this.config = config;
        }

        public ISendGridClient CreateSendGridClient()
        {
            return new SendGridClient(this.config.IdentityGatewayService.SendGridApiKey);
        }
    }
}