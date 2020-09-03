// <copyright file="HttpRequestExtensions.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json;

namespace Mmm.Iot.Common.Services.Http
{
    /* <summary>
     * This class is a Extension of HttpRequest class which is used by StorageAdapterClientTest
     * to validate the URL and data model using overloaded "Check" methods
     * </summary>     */
    public static class HttpRequestExtensions
    {
        public static bool Check(this IHttpRequest request, string uri)
        {
            return request.Uri.ToString() == uri;
        }

        public static bool Check<T>(this IHttpRequest request, string uri, Func<T, bool> validator)
        {
            if (request.Uri.ToString() != uri)
            {
                return false;
            }

            if (validator == null)
            {
                return true;
            }

            var model = JsonConvert.DeserializeObject<T>(request.Content.ReadAsStringAsync().Result);
            return validator(model);
        }
    }
}