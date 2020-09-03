// <copyright file="IConverter.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.AsaManager.Services.Models;

namespace Mmm.Iot.AsaManager.Services
{
    public interface IConverter
    {
        Task<ConversionApiModel> ConvertAsync(string tenantId, string operationId = null);

        string GetBlobFilePath();
    }
}