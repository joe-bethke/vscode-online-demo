// <copyright file="GuidKeyGenerator.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;

namespace Mmm.Iot.Common.Services.Wrappers
{
    public class GuidKeyGenerator : IKeyGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}