// <copyright file="AppConfig.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;

namespace Mmm.Iot.Common.Services.Config
{
    public partial class AppConfig
    {
        public AppConfig()
        {
        }

        public AppConfig(IConfiguration configuration)
        {
            configuration.Bind(this);
            this.Configuration = configuration;
        }

        public AppConfig(IConfigurationBuilder configurationBuilder)
            : this(configurationBuilder.Build())
        {
        }

        public IConfiguration Configuration { get; private set; }

        public virtual string AppConfigurationConnectionString { get; set; }

        public virtual string ASPNETCORE_URLS { get; set; }
    }
}