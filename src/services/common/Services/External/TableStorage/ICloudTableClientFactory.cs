// <copyright file="ICloudTableClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Cosmos.Table;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public interface ICloudTableClientFactory
    {
        CloudTableClient Create();
    }
}