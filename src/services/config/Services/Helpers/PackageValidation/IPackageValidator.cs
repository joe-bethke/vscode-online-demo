// <copyright file="IPackageValidator.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Config.Services.Helpers.PackageValidation
{
    public interface IPackageValidator
    {
        JObject ParsePackageContent(string package);

        bool Validate();
    }
}