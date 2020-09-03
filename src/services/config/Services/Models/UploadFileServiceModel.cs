// <copyright file="UploadFileServiceModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Config.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Platform.IoT.Common.Services.Models
{
    public class UploadFileServiceModel
    {
        [JsonProperty(PropertyName = "SoftwarePackageURL")]
        public string SoftwarePackageURL { get; set; }

        [JsonProperty(PropertyName = "CheckSum")]
        public CheckSumModel CheckSum { get; set; }
    }
}