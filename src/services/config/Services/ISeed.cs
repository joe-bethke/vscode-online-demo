// <copyright file="ISeed.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Mmm.Iot.Config.Services
{
    public interface ISeed
    {
        Task TrySeedAsync();
    }
}