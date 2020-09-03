// <copyright file="PackageValidator.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Mmm.Iot.Common.Services.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Config.Services.Helpers.PackageValidation
{
    public abstract class PackageValidator : IPackageValidator
    {
        JObject IPackageValidator.ParsePackageContent(string package)
        {
            try
            {
                return JObject.Parse(package);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidInputException($"Provided package is not a valid json. Error: {e.Message}.");
            }
        }

        public abstract bool Validate();
    }
}