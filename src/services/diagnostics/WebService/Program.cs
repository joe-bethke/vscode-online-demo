// <copyright file="Program.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Hosting;
using Mmm.Iot.Common.Services;

namespace Mmm.Iot.Diagnostics.WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            builder.UseStartup<Startup>();
            var host = builder.Build();
            host.Run();
        }
    }
}