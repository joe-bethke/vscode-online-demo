// <copyright file="FirmwareValidator.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Config.Services.Helpers.PackageValidation
{
    internal class FirmwareValidator : PackageValidator, IPackageValidator
    {
        // TODO: Implement validation for Firmware Update for MxChip packages
        public override bool Validate()
        {
            return true;
        }
    }
}