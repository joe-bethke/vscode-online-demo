// <copyright file="EdgePackageValidator.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Config.Services.Helpers.PackageValidation
{
    internal class EdgePackageValidator : PackageValidator, IPackageValidator
    {
        // TODO: Implement validation for Edge packages
        public override bool Validate()
        {
            return true;
        }
    }
}