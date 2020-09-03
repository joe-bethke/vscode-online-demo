// <copyright file="ConfigurationClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Azure.Data.AppConfiguration;
using Mmm.Iot.Common.Services.Config;

namespace Mmm.Iot.Common.Services.External.AppConfiguration
{
    public class ConfigurationClientFactory : IConfigurationClientFactory
    {
        private readonly AppConfig config;

        public ConfigurationClientFactory(AppConfig config)
        {
            this.config = config;
        }

        public ConfigurationClient Create()
        {
            return new ConfigurationClient(this.config.AppConfigurationConnectionString);
        }
    }
}