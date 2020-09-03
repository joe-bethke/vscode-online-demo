// <copyright file="ResourceExtensions.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Microsoft.Azure.Documents;

namespace Mmm.Iot.Common.TestHelpers
{
    public static class ResourceExtensions
    {
        public static void SetETag(this Resource resource, string etag)
        {
            resource.SetPropertyValue("_etag", etag);
        }

        public static void SetTimestamp(this Resource resource, DateTimeOffset timestamp)
        {
            resource.SetPropertyValue("_ts", timestamp.ToUnixTimeSeconds());
        }
    }
}