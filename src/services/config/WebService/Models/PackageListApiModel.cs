// <copyright file="PackageListApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Mmm.Iot.Config.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.WebService.Models
{
    public class PackageListApiModel
    {
        public PackageListApiModel(IEnumerable<PackageServiceModel> models)
        {
            this.Items = models.Select(m => new PackageApiModel(m));

            this.Metadata = new Dictionary<string, string>
            {
                { "$type", $"PackageList;1" },
                { "$url", $"/v1/packages" },
            };
        }

        public IEnumerable<PackageApiModel> Items { get; set; }

        [JsonProperty(PropertyName = "$metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}