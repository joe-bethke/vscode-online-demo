// <copyright file="HashsetExtensions.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Mmm.Iot.Common.TestHelpers
{
    public static class HashsetExtensions
    {
        public static bool SetEquals<T>(this HashSet<T> me, IEnumerable<T> other)
        {
            return me.IsSubsetOf(other) && me.IsSupersetOf(other);
        }
    }
}