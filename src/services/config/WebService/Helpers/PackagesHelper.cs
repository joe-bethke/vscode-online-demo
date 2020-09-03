// <copyright file="PackagesHelper.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Azure.Devices;
using Mmm.Iot.Config.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.Config.WebService.Helpers
{
    public class PackagesHelper
    {
        /**
         * This function is used to verify if the package type and package contents are
         * compatible. for eg:- if package type is DeviceConfiguration it should contain
         * "devicesContent" object.
         */
        public static bool VerifyPackageType(string packageContent, PackageType packageType)
        {
            if (packageType == PackageType.EdgeManifest &&
                IsEdgePackage(packageContent))
            {
                return true;
            }
            else if (packageType == PackageType.DeviceConfiguration && !IsEdgePackage(packageContent))
            {
                return true;
            }

            return false;
        }

        public static bool IsEdgePackage(string packageContent)
        {
            var package = JsonConvert.DeserializeObject<Configuration>(packageContent);

            if (package.Content?.ModulesContent != null &&
                package.Content?.DeviceContent == null)
            {
                return true;
            }

            return false;
        }
    }
}