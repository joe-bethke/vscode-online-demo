// <copyright file="AuthMiddleware.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.External.UserManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.Auth
{
    public class AuthMiddleware
    {
        private const string AuthHeaderPrefix = "Bearer ";
        private const string AuthHeader = "Authorization";
        private const string ExternalResourcesHeader = "X-Source";
        private const string Error401NotAuthenticated = @"{""Error"":""Authentication required""}";
        private const string Error503AuthenticationServiceNotAvailable = @"{""Error"":""Authentication service not available""}";
        private readonly RequestDelegate requestDelegate;
        private readonly IConfigurationManager<OpenIdConnectConfiguration> openIdCfgMan;
        private readonly AppConfig config;
        private readonly ILogger<AuthMiddleware> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly bool authRequired;
        private readonly RolesConfig rolesConfig;
        private readonly IUserManagementClient userManagementClient;
        private readonly List<string> allowedUrls = new List<string>() { "/v1/status", "/api/status", "/.well-known/openid-configuration", "/connect" };
        private IReadOnlyDictionary<string, IEnumerable<string>> permissions;
        private TokenValidationParameters tokenValidationParams;
        private bool tokenValidationInitialized;

        public AuthMiddleware(
            RequestDelegate requestDelegate,
            IConfigurationManager<OpenIdConnectConfiguration> openIdCfgMan,
            AppConfig config,
            IUserManagementClient userManagementClient,
            ILogger<AuthMiddleware> logger,
            ILoggerFactory loggerFactory)
        {
            this.requestDelegate = requestDelegate;
            this.openIdCfgMan = openIdCfgMan;
            this.config = config;
            this.logger = logger;
            this.loggerFactory = loggerFactory;
            this.authRequired = this.config.Global.AuthRequired;
            this.rolesConfig = this.config.Global.ClientAuth.Roles;
            this.tokenValidationInitialized = false;
            this.userManagementClient = userManagementClient;
            this.permissions = this.GetPermissions();

            // This will show in development mode, or in case auth is turned off
            if (!this.authRequired)
            {
                this.logger.LogWarning("### AUTHENTICATION IS DISABLED! ###");
                this.logger.LogWarning("### AUTHENTICATION IS DISABLED! ###");
                this.logger.LogWarning("### AUTHENTICATION IS DISABLED! ###");
            }
            else
            {
                this.logger.LogInformation("Auth config is {config}", this.config);
                this.InitializeTokenValidationAsync(CancellationToken.None).Wait();
            }

            // TODO ~devis: this is a temporary solution for public preview only
            // TODO ~devis: remove this approach and use the service to service authentication
            // https://github.com/Azure/pcs-auth-dotnet/issues/18
            // https://github.com/Azure/azure-iot-pcs-remote-monitoring-dotnet/issues/11
            this.logger.LogWarning("### Service to service authentication is not available in public preview ###");
            this.logger.LogWarning("### Service to service authentication is not available in public preview ###");
            this.logger.LogWarning("### Service to service authentication is not available in public preview ###");
        }

        public Task Invoke(HttpContext context)
        {
            var header = string.Empty;
            var token = string.Empty;

            // Store this setting to skip validating authorization in the controller if enabled
            context.Request.SetAuthRequired(this.config.Global.AuthRequired);

            context.Request.SetExternalRequest(true);

            // Skip Authentication on certain URLS
            if (this.allowedUrls.Where(s => context.Request.Path.StartsWithSegments(s)).Count() > 0)
            {
                return this.requestDelegate(context);
            }

            if (!context.Request.Headers.ContainsKey(ExternalResourcesHeader))
            {
                // This is a service to service request running in the private
                // network, so we skip the auth required for user requests
                // Note: this is a temporary solution for public preview
                // https://github.com/Azure/pcs-auth-dotnet/issues/18
                // https://github.com/Azure/azure-iot-pcs-remote-monitoring-dotnet/issues/11

                // Call the next delegate/middleware in the pipeline
                this.logger.LogDebug("Skipping auth for service to service request");
                context.Request.SetExternalRequest(false);
                context.Request.SetTenant(this.loggerFactory.CreateLogger(typeof(RequestExtension)));
                return this.requestDelegate(context);
            }

            if (!this.authRequired)
            {
                // Call the next delegate/middleware in the pipeline
                this.logger.LogDebug("Skipping auth (auth disabled)");
                return this.requestDelegate(context);
            }

            if (!this.InitializeTokenValidationAsync(context.RequestAborted).Result)
            {
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.Headers["Content-Type"] = "application/json";
                context.Response.WriteAsync(Error503AuthenticationServiceNotAvailable);
                return Task.CompletedTask;
            }

            if (context.Request.Headers.ContainsKey(AuthHeader))
            {
                header = context.Request.Headers[AuthHeader].SingleOrDefault();
            }
            else
            {
                string errorMessage = "Authorization header not found";
                this.logger.LogError(new Exception(errorMessage), errorMessage);
            }

            if (header != null && header.StartsWith(AuthHeaderPrefix))
            {
                token = header.Substring(AuthHeaderPrefix.Length).Trim();
            }
            else
            {
                string errorMessage = "Authorization header prefix not found";
                this.logger.LogError(new Exception(errorMessage), errorMessage);
            }

            if (this.ValidateToken(token, context) || !this.authRequired)
            {
                // Call the next delegate/middleware in the pipeline
                return this.requestDelegate(context);
            }

            this.logger.LogWarning("Authentication required");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers["Content-Type"] = "application/json";
            context.Response.WriteAsync(Error401NotAuthenticated);

            return Task.CompletedTask;
        }

        private IReadOnlyDictionary<string, IEnumerable<string>> GetPermissions()
        {
            Dictionary<string, IEnumerable<string>> permissions = new Dictionary<string, IEnumerable<string>>();
            try
            {
                Type type = this.rolesConfig.GetType();
                PropertyInfo[] properties = type.GetProperties();

                List<string> list;
                foreach (PropertyInfo property in properties)
                {
                    var role = JObject.Parse((string)property.GetValue(this.rolesConfig, null));
                    var entry = role[property.Name];
                    list = new List<string>();

                    foreach (var result in role[property.Name])
                    {
                            list.Add(result.ToObject<string>());
                    }

                    permissions.Add(property.Name.ToLower(), list);
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Failed to load in permissions from AppConfig.");
            }

            return permissions;
        }

        private bool ValidateToken(string token, HttpContext context)
        {
            if (string.IsNullOrEmpty(token))
            {
                this.logger.LogDebug("Token is not valid because it is null or empty");
                return false;
            }

            try
            {
                SecurityToken validatedToken;
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, this.tokenValidationParams, out validatedToken);
                var jwtToken = new JwtSecurityToken(token);

                // Validate the signature algorithm
                if (this.config.Global.ClientAuth.Jwt.AllowedAlgorithms.Contains(jwtToken.SignatureAlgorithm))
                {
                    // Store the user info in the request context, so the authorization
                    // header doesn't need to be parse again later in the User controller.
                    context.Request.SetCurrentUserClaims(jwtToken.Claims);

                    this.AddAllowedActionsToRequestContext(context);

                    // Set Tenant Information
                    context.Request.SetTenant(this.loggerFactory.CreateLogger(typeof(RequestExtension)));

                    return true;
                }

                string errorMessage = $"JWT token signature algorithm '{jwtToken.SignatureAlgorithm}' is not allowed.";
                this.logger.LogError(new Exception(errorMessage), errorMessage);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Failed to validate JWT token");
            }

            return false;
        }

        private void AddAllowedActionsToRequestContext(HttpContext context)
        {
            var roles = context.Request.GetCurrentUserRoleClaim().ToList();
            if (!roles.Any())
            {
                this.logger.LogWarning("JWT token doesn't include any role claims.");
                context.Request.SetCurrentUserAllowedActions(new string[] { });
                return;
            }

            var allowedActions = new List<string>();
            foreach (string role in roles)
            {
                if (!this.permissions.ContainsKey(role.ToLower()))
                {
                    this.logger.LogWarning("Role claim specifies a role '{role}' that does not exist", role);
                    continue;
                }

                allowedActions.AddRange(this.permissions[role.ToLower()]);
            }

            context.Request.SetCurrentUserAllowedActions(allowedActions);
        }

        private async Task<bool> InitializeTokenValidationAsync(CancellationToken token)
        {
            if (this.tokenValidationInitialized)
            {
                return true;
            }

            try
            {
                this.logger.LogInformation("Initializing OpenID configuration");
                var openIdConfig = await this.openIdCfgMan.GetConfigurationAsync(token);
                this.tokenValidationParams = new TokenValidationParameters
                {
                    // Validate the token signature
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = openIdConfig.SigningKeys,

                    // Validate the token issuer
                    ValidateIssuer = true,
                    ValidIssuer = this.config.Global.ClientAuth.Jwt.AuthIssuer,

                    // Validate the token audience
                    ValidateAudience = false,
                    ValidAudience = this.config.Global.ClientAuth.Jwt.Audience,

                    // Validate token lifetime
                    ValidateLifetime = true,
                    ClockSkew = new TimeSpan(0, 0, this.config.Global.ClientAuth.Jwt.ClockSkewSeconds),
                };

                this.logger.LogDebug("Initializing token validation parameters: {tokenValidationParameters}", JsonConvert.SerializeObject(this.tokenValidationParams, Formatting.Indented));
                this.logger.LogDebug("Global__ClientAuth__Jwt__AuthIssuer is {issuer}", this.config.Global.ClientAuth.Jwt.AuthIssuer);
                this.tokenValidationInitialized = true;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Failed to setup OpenId Connect");
            }

            return this.tokenValidationInitialized;
        }
    }
}