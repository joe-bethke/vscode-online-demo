// <copyright file="CorsSetup.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Auth
{
    public class CorsSetup : ICorsSetup
    {
        private readonly AppConfig config;
        private readonly ILogger logger;

        public CorsSetup(AppConfig config, ILogger<CorsSetup> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public void UseMiddleware(IApplicationBuilder app)
        {
            if (this.config.Global.ClientAuth.CorsEnabled)
            {
                this.logger.LogWarning("CORS is enabled");
                app.UseCors(this.BuildCorsPolicy);
            }
            else
            {
                this.logger.LogInformation("CORS is disabled");
            }
        }

        private void BuildCorsPolicy(CorsPolicyBuilder builder)
        {
            CorsWhitelistModel model;
            string errorMessage = $"Ignoring invalid CORS whitelist: '{this.config.Global.ClientAuth.CorsWhitelist}'";
            try
            {
                model = JsonConvert.DeserializeObject<CorsWhitelistModel>(this.config.Global.ClientAuth.CorsWhitelist);
                if (model == null)
                {
                    this.logger.LogError(new Exception(errorMessage), errorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, errorMessage);
                return;
            }

            if (model.Origins == null)
            {
                this.logger.LogInformation("No setting for CORS origin policy was found, ignore");
            }
            else if (model.Origins.Contains("*"))
            {
                this.logger.LogInformation("CORS policy allowed any origin");
                builder.AllowAnyOrigin();
            }
            else
            {
                this.logger.LogInformation("Add origins '{origins}' to CORS policy", model.Origins);
                builder.WithOrigins(model.Origins);
            }

            if (model.Origins == null)
            {
                this.logger.LogInformation("No setting for CORS method policy was found, ignore");
            }
            else if (model.Methods.Contains("*"))
            {
                this.logger.LogInformation("CORS policy allowed any method");
                builder.AllowAnyMethod();
            }
            else
            {
                this.logger.LogInformation("Add methods '{methods}' to CORS policy", model.Methods);
                builder.WithMethods(model.Methods);
            }

            if (model.Origins == null)
            {
                this.logger.LogInformation("No setting for CORS header policy was found, ignore");
            }
            else if (model.Headers.Contains("*"))
            {
                this.logger.LogInformation("CORS policy allowed any header");
                builder.AllowAnyHeader();
            }
            else
            {
                this.logger.LogInformation("Add headers '{headers}' to CORS policy", model.Headers);
                builder.WithHeaders(model.Headers);
            }
        }
    }
}