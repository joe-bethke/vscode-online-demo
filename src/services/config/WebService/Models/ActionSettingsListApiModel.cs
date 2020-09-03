// <copyright file="ActionSettingsListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Mmm.Iot.Config.Services.Models.Actions;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.WebService.Models
{
    public class ActionSettingsListApiModel
    {
        public ActionSettingsListApiModel(List<IActionSettings> actionSettingsList)
        {
            this.Items = new List<ActionSettingsApiModel>();

            foreach (var actionSettings in actionSettingsList)
            {
                this.Items.Add(new ActionSettingsApiModel(actionSettings));
            }

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"ActionSettingsList;1" },
                { "$url", $"/v1/solution-settings/actions" },
            };
        }

        [JsonProperty("Items")]
        public List<ActionSettingsApiModel> Items { get; set; }

        [JsonProperty("$metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}