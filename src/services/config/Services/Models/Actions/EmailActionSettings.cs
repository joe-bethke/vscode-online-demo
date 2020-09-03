// <copyright file="EmailActionSettings.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Config.Services.External;

namespace Mmm.Iot.Config.Services.Models.Actions
{
    public class EmailActionSettings : IActionSettings
    {
        private const string IsEnabledKey = "IsEnabled";
        private const string Office365ConnectorUrlKey = "Office365ConnectorUrl";
        private const string ApplicationPermissionsKey = "ApplicationPermissionsAssigned";
        private readonly IAzureResourceManagerClient resourceManagerClient;
        private readonly AppConfig config;
        private readonly ILogger logger;

        public EmailActionSettings(
            IAzureResourceManagerClient resourceManagerClient,
            AppConfig config,
            ILogger<EmailActionSettings> logger)
        {
            this.resourceManagerClient = resourceManagerClient;
            this.config = config;
            this.logger = logger;

            this.Type = ActionType.Email;
            this.Settings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public ActionType Type { get; }

        public IDictionary<string, object> Settings { get; set; }

        public async Task InitializeAsync()
        {
            // Check signin status of Office 365 Logic App Connector
            var office365IsEnabled = false;
            var applicationPermissionsAssigned = true;
            try
            {
                office365IsEnabled = await this.resourceManagerClient.IsOffice365EnabledAsync();
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                // If there is a 403 Not Authorized exception, it means the application has not
                // been given owner permissions to make the isEnabled check. This can be configured
                // by an owner in the Azure Portal.
                applicationPermissionsAssigned = false;
                this.logger.LogError(notAuthorizedException, "The application is not authorized and has not been assigned owner permissions for the subscription. Go to the Azure portal and assign the application as an owner in order to retrieve the token.");
            }

            this.Settings.Add(IsEnabledKey, office365IsEnabled);
            this.Settings.Add(ApplicationPermissionsKey, applicationPermissionsAssigned);

            // Get Url for Office 365 Logic App Connector setup in portal
            // for display on the webui for one-time setup.
            this.Settings.Add(Office365ConnectorUrlKey, this.config.ConfigService.ConfigServiceActions.Office365ConnectionUrl);

            this.logger.LogDebug("Email action settings retrieved: {settings}. Email setup status: {status}", office365IsEnabled, this.Settings);
        }
    }
}