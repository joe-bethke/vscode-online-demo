// <copyright file="CorsWhitelistModel.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Common.Services.Auth
{
    public class CorsWhitelistModel
    {
        public string[] Origins { get; set; }

        public string[] Methods { get; set; }

        public string[] Headers { get; set; }
    }
}