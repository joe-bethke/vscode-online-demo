// <copyright file="AuditApiModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Models
{
    public class AuditApiModel
    {
        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate { get; set; }

        [JsonProperty(PropertyName = "ModifiedDate")]
        public string ModifiedDate { get; set; }

        [JsonProperty(PropertyName = "CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "ModifiedBy")]
        public string ModifiedBy { get; set; }
    }
}