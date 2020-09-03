// <copyright file="PackageApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Mmm.Iot.Common.Services.Models;
using Mmm.Iot.Config.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mmm.Iot.Config.WebService.Models
{
    public class PackageApiModel : AuditApiModel
    {
        public PackageApiModel(PackageServiceModel model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.PackageType = model.PackageType;
            this.DateCreated = model.DateCreated;
            this.Content = model.Content;
            this.ConfigType = model.ConfigType;
            this.Tags = model.Tags;
            this.Version = model.Version;
            this.CreatedDate = model.CreatedDate;
            this.CreatedBy = model.CreatedBy;
            this.ModifiedBy = model.ModifiedBy;
            this.ModifiedDate = model.ModifiedDate;
        }

        public PackageApiModel(
                string content,
                string name,
                PackageType type,
                string version,
                string configType,
                IList<string> tags)
        {
            this.Content = content;
            this.Name = name;
            this.PackageType = type;
            this.ConfigType = configType;
            this.Tags = tags;
            this.Version = version;
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("PackageType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType PackageType { get; set; }

        [JsonProperty("ConfigType")]
        public string ConfigType { get; set; }

        [JsonProperty(PropertyName = "DateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Tags")]
        public IList<string> Tags { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        public PackageServiceModel ToServiceModel()
        {
            return new PackageServiceModel()
            {
                Content = this.Content,
                Name = this.Name,
                PackageType = this.PackageType,
                ConfigType = this.ConfigType,
                Tags = this.Tags,
                Version = this.Version,
            };
        }
    }
}