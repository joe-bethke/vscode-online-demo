// <copyright file="IConfigurationClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Azure.Data.AppConfiguration;

namespace Mmm.Iot.Common.Services.External.AppConfiguration
{
    public interface IConfigurationClientFactory
    {
        ConfigurationClient Create();
    }
}