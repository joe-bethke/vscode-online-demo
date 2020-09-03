// <copyright file="Program.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Hosting;
using Mmm.Iot.Common.Services;

namespace Mmm.Iot.DeviceTelemetry.WebService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
                BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}