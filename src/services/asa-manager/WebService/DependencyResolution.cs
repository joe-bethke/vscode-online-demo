// <copyright file="DependencyResolution.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Reflection;
using Autofac;
using Mmm.Iot.AsaManager.Services;
using Mmm.Iot.Common.Services;

namespace Mmm.Iot.AsaManager.WebService
{
    public class DependencyResolution : DependencyResolutionBase
    {
        protected override void SetupCustomRules(ContainerBuilder builder)
        {
            builder.RegisterType<RulesConverter>().As<RulesConverter>();
            builder.RegisterType<DeviceGroupsConverter>().As<DeviceGroupsConverter>();
            var assembly = typeof(StatusService).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
        }
    }
}