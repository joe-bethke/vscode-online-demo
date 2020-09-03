// <copyright file="Factory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Autofac;

namespace Mmm.Iot.Common.Services
{
    public class Factory : IFactory
    {
        private static IContainer container;

        public static void RegisterContainer(IContainer c)
        {
            container = c;
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}