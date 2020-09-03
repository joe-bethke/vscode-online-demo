// <copyright file="MockFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Mmm.Iot.Common.Services.Wrappers;
using Moq;

namespace Mmm.Iot.Common.TestHelpers
{
    public class MockFactory<T> : IFactory<T>
        where T : class
    {
        private readonly Mock<T> mock;

        public MockFactory(Mock<T> mock)
        {
            this.mock = mock;
        }

        public T Create()
        {
            return this.mock.Object;
        }
    }
}