// <copyright file="Actions.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Config.Services.External;
using Mmm.Iot.Config.Services.Models.Actions;

namespace Mmm.Iot.Config.Services
{
    public class Actions : IActions
    {
        private readonly IAzureResourceManagerClient resourceManagerClient;
        private readonly ILogger logger;
        private readonly IActionSettings emailActionSettings;

        public Actions(
            IAzureResourceManagerClient resourceManagerClient,
            ILogger<Actions> logger,
            IActionSettings emailActionSettings)
        {
            this.resourceManagerClient = resourceManagerClient;
            this.logger = logger;
            this.emailActionSettings = emailActionSettings;
        }

        public async Task<List<IActionSettings>> GetListAsync()
        {
            var result = new List<IActionSettings>();
            await this.emailActionSettings.InitializeAsync();
            result.Add(this.emailActionSettings);

            return result;
        }
    }
}