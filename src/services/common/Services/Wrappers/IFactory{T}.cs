// <copyright file="IFactory{T}.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

namespace Mmm.Iot.Common.Services.Wrappers
{
    public interface IFactory<out T>
    {
        T Create();
    }
}