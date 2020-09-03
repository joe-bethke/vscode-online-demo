// <copyright file="JTokenExtension.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.IoTHubManager.Services.Extensions
{
    public static class JTokenExtension
    {
        public static IEnumerable<string> GetAllLeavesPath(this JToken root)
        {
            if (root is JValue)
            {
                yield return root.Path;
            }
            else
            {
                foreach (var child in root.Values())
                {
                    foreach (var name in child.GetAllLeavesPath())
                    {
                        yield return name;
                    }
                }
            }
        }
    }
}