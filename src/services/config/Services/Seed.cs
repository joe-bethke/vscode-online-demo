// <copyright file="Seed.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.External.StorageAdapter;
using Mmm.Iot.Config.Services.External;
using Mmm.Iot.Config.Services.Helpers;
using Mmm.Iot.Config.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.Services
{
    public class Seed : ISeed
    {
        private const string SeedCollectionId = "solution-settings";
        private const string MutexKey = "seedMutex";
        private const string CompletedFlagKey = "seedCompleted";
        private readonly TimeSpan mutexTimeout = TimeSpan.FromMinutes(5);
        private readonly AppConfig config;
        private readonly IStorageMutex mutex;
        private readonly IStorage storage;
        private readonly IStorageAdapterClient storageClient;
        private readonly IDeviceSimulationClient simulationClient;
        private readonly IDeviceTelemetryClient telemetryClient;
        private readonly ILogger logger;

        public Seed(
            AppConfig config,
            IStorageMutex mutex,
            IStorage storage,
            IStorageAdapterClient storageClient,
            IDeviceSimulationClient simulationClient,
            IDeviceTelemetryClient telemetryClient,
            ILogger<Seed> logger)
        {
            this.config = config;
            this.mutex = mutex;
            this.storage = storage;
            this.storageClient = storageClient;
            this.simulationClient = simulationClient;
            this.telemetryClient = telemetryClient;
            this.logger = logger;
        }

        public async Task TrySeedAsync()
        {
            if (string.IsNullOrEmpty(this.config.ConfigService.SeedTemplate))
            {
                return;
            }

            if (!await this.mutex.EnterAsync(SeedCollectionId, MutexKey, this.mutexTimeout))
            {
                this.logger.LogInformation("Seed skipped (conflict)");
                return;
            }

            if (await this.CheckCompletedFlagAsync())
            {
                this.logger.LogInformation("Seed skipped (completed)");
                return;
            }

            this.logger.LogInformation("Seed begin");
            try
            {
                await this.SeedAsync();
                this.logger.LogInformation("Seed end");
                await this.SetCompletedFlagAsync();
                this.logger.LogInformation("Seed completed flag set");
            }
            finally
            {
                await this.mutex.LeaveAsync(SeedCollectionId, MutexKey);
            }
        }

        private async Task<bool> CheckCompletedFlagAsync()
        {
            try
            {
                await this.storageClient.GetAsync(SeedCollectionId, CompletedFlagKey);
                return true;
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }
        }

        private async Task SetCompletedFlagAsync()
        {
            await this.storageClient.UpdateAsync(SeedCollectionId, CompletedFlagKey, "true", "*");
        }

        private async Task SeedAsync()
        {
            if (this.config.ConfigService.SolutionType.StartsWith("devicesimulation", StringComparison.OrdinalIgnoreCase))
            {
                await this.SeedSimulationAsync();
            }
            else
            {
                await this.SeedSingleTemplateAsync();
            }
        }

        // Seed single template for Remote Monitoring solution
        private async Task SeedSingleTemplateAsync()
        {
            var template = this.GetSeedContent(this.config.ConfigService.SeedTemplate);

            if (template.Groups.Select(g => g.Id).Distinct().Count() != template.Groups.Count())
            {
                this.logger.LogWarning("Found duplicated group ID {groups}", template.Groups);
            }

            if (template.Rules.Select(r => r.Id).Distinct().Count() != template.Rules.Count())
            {
                this.logger.LogWarning("Found duplicated rule ID {rules}", template.Rules);
            }

            var groupIds = new HashSet<string>(template.Groups.Select(g => g.Id));
            var rulesWithInvalidGroupId = template.Rules.Where(r => !groupIds.Contains(r.GroupId));
            if (rulesWithInvalidGroupId.Any())
            {
                this.logger.LogWarning("Invalid group ID found in rules {rules}", rulesWithInvalidGroupId);
            }

            foreach (var group in template.Groups)
            {
                try
                {
                    await this.storage.UpdateDeviceGroupAsync(group.Id, group, "*");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Failed to seed default group {group}", group);
                    throw;
                }
            }

            foreach (var rule in template.Rules)
            {
                try
                {
                    await this.telemetryClient.UpdateRuleAsync(rule, "*");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Failed to seed default rule {rule}", rule);
                    throw;
                }
            }

            await this.SeedSimulationAsync();
        }

        // Seed single template for Device Simulation solution
        private async Task SeedSimulationAsync()
        {
            try
            {
                var simulationModel = await this.simulationClient.GetDefaultSimulationAsync();

                if (simulationModel != null)
                {
                    this.logger.LogInformation("Skip seed simulation since there is already one simulation {simulationModel}", simulationModel);
                }
                else
                {
                    var template = this.GetSeedContent(this.config.ConfigService.SeedTemplate);

                    foreach (var simulation in template.Simulations)
                    {
                        await this.simulationClient.UpdateSimulationAsync(simulation);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to seed default simulations");
                throw;
            }
        }

        private Template GetSeedContent(string templateName)
        {
            Template template;
            string content;
            var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var file = Path.Combine(root, "Data", $"{templateName}.json");
            if (!File.Exists(file))
            {
                // ToDo: Check if `template` is a valid URL and try to load the content
                throw new ResourceNotFoundException($"Template {templateName} does not exist");
            }
            else
            {
                content = File.ReadAllText(file);
            }

            try
            {
                template = JsonConvert.DeserializeObject<Template>(content);
            }
            catch (Exception ex)
            {
                throw new InvalidInputException("Failed to parse template", ex);
            }

            return template;
        }
    }
}