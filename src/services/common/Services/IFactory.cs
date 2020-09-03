// <copyright file="IFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Common.Services
{
    public interface IFactory
    {
        T Resolve<T>();
    }
}