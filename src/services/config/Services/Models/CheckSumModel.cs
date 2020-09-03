// <copyright file="CheckSumModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>
using Newtonsoft.Json;

namespace Mmm.Iot.Config.Services.Models
{
    public class CheckSumModel
    {
        [JsonProperty(PropertyName = "MD5")]
        public string MD5 { get; set; }

        [JsonProperty(PropertyName = "SHA1")]
        public string SHA1 { get; set; }
    }
}