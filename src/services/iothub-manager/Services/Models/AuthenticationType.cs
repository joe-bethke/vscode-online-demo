// <copyright file="AuthenticationType.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.IoTHubManager.Services.Models
{
    public enum AuthenticationType
    {
        Sas = 0,

        SelfSigned = 1,

        CertificateAuthority = 2,
    }
}