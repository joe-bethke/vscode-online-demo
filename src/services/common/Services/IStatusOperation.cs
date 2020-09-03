// <copyright file="IStatusOperation.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services
{
    public interface IStatusOperation
    {
        Task<StatusResultServiceModel> StatusAsync();
    }
}